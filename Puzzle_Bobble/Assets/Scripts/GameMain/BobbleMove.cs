using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 2f;    // 玉の速度
    [HideInInspector] public Vector3 moveDirection;   // 玉の進行方向
    private Vector3 prePosition;                      // 前フレームでの位置
    private bool moveFlg;           // これがtrueのときに移動処理をさせる
    private bool hitFlg;            // これがfalseのときだけヒット後の処理
    private float bobbleHeightSize = 0.52f;           // 泡１個の縦幅
    private float bobbleWidthSize = 0.56f;            // 泡１個の横幅
    private Vector3 lastVelocity;
    private Bobble myBobble;
    private Rigidbody2D rb;

    void Start()
    {
        myBobble = GetComponent<Bobble>();
    }

    private void FixedUpdate()
    {
        // フラグが立っている時だけ移動処理をさせる
        if (moveFlg)
        {
            //transform.Translate(moveDirection * moveSpeed);
            
        }
        lastVelocity = rb.velocity;
        prePosition = transform.position;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 玉を発射する。
    /// </summary>
    /// <param name="speed">玉の速度</param>
    /// <param name="direction">玉の進行方向</param>
    public void ShotBubble(float speed, Vector3 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        moveSpeed = speed;
        moveDirection = direction;
        rb.AddForce(moveDirection * moveSpeed, ForceMode2D.Impulse);
        moveFlg = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁に当たったら正反射させる
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
            SoundManager.Instance.PlaySE(SE.WallReflect);
        }
        else
        {
            // １フレームで２回処理させないように
            if (hitFlg) return;
            hitFlg = true;

            rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.simulated = false;
            rb.velocity = Vector3.zero;

            // 当たった泡のBobbleスクリプトを取得
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitIX = hitBobble.GetNumber().x;
            int hitIY = hitBobble.GetNumber().y;

            Debug.Log("X : " + hitIX + " Y : " + hitIY);

            float newY = 0;
            int newIY = hitIY;

            // 泡の上下真ん中どれに当たったか見て、Y座標を泡の縦幅分ずらす
            if(collision.transform.position.y + bobbleHeightSize / 2 <= transform.position.y)
            {
                // 上側
                newY = bobbleHeightSize;
                newIY--;
            }
            else if (transform.position.y <= collision.transform.position.y - bobbleHeightSize / 4)
            {
                // 下側
                newY = -bobbleHeightSize;
                newIY++;
            }

            float newX = 0;
            int newIX = hitIX;

            // 泡の左右どちら側に当たったか見て、X座標を泡の横幅の半分のサイズ分ずらす
            if (transform.position.x <= collision.transform.position.x)
            {
                // 左側
                newX = -bobbleWidthSize / 2;

                // ヒットした泡が偶数行か、同じ行なら左にずれる
                if (hitIY % 2 == 0 || hitIY == newIY)
                {
                    newIX--;
                }

                // 固定されたインデックスが0未満になってたら0になるよう修正する
                if(newIX < 0)
                {
                    newIX = 0;
                    newX += bobbleWidthSize;
                }

            }
            else
            {
                // 右側
                newX = bobbleWidthSize / 2;

                // ヒットした泡が奇数行か、同じ行なら右にずれる
                if (hitIY % 2 == 1 || hitIY == newIY)
                {
                    newIX++;
                }

                // 固定されたインデックスが配列の要素数を越えてたら要素数に収まるよう修正する
                if (BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (newIY % 2) <= newIX)
                {
                    newIX = BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - 1 - (newIY % 2);
                    newX -= bobbleWidthSize;
                }

            }

            // 泡と同じ行だった場合
            if(hitIY == newIY)
            {
                // ワールドX座標をさらにずらす
                newX *= 2;
            }            

            // 玉を、ヒットした泡の位置とヒットした方向をもとに修正して、互い違いの位置に設置
            Vector3 newPosition = new Vector3(collision.transform.position.x + newX, collision.transform.position.y + newY);
            transform.position = newPosition;

            // 固定したあと、ほかの泡と同様にじわじわ下に落ちるようにする
            GameManager.Instance.shootedBobbleMoving = false;
            myBobble.enabled = true;

            myBobble.bobbleNumber.x = newIX;
            myBobble.bobbleNumber.y = newIY;

            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble._BobbleColor, myBobble);

            SoundManager.Instance.PlaySE(SE.BobbleSeted);

            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble._BobbleColor);

            // 停止させる
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
