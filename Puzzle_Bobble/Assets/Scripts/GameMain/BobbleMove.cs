using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // 玉の速度
    [HideInInspector] public Vector3 moveDirection;   // 玉の進行方向
    private Vector3 prePosition;                      // 前フレームでの位置
    private bool moveFlg;           // これがtrueのときに移動処理をさせる
    private bool hitFlg;            // これがfalseのときだけヒット後の処理
    private float bobbleHeightSize = 0.56f;           // 泡１個の縦幅
    private float bobbleWidthSize = 0.56f;            // 泡１個の横幅
    private Bobble myBobble;

    void Start()
    {
        myBobble = GetComponent<Bobble>();
    }

    void Update()
    {
        // フラグが立っている時だけ移動処理をさせる
        if (moveFlg)
        {
            transform.Translate(moveDirection * moveSpeed);
        }

        prePosition = transform.position;
    }

    /// <summary>
    /// 玉を発射する。
    /// </summary>
    /// <param name="speed">玉の速度</param>
    /// <param name="direction">玉の進行方向</param>
    public void ShotBubble(float speed, Vector3 direction)
    {
        moveSpeed = speed;
        moveDirection = direction;
        moveFlg = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁に当たったら正反射させる
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
        }
        else
        {
            // １フレームで２回処理させないように
            if (hitFlg) return;
            hitFlg = true;

            // 当たった泡のBobbleスクリプトを取得
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitX = hitBobble.GetNumber().x;
            int hitY = hitBobble.GetNumber().y;

            Debug.Log("X : " + hitX + " Y : " + hitY);

            float newY = 0;
            int newIY = hitY;

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
            int newIX = hitX;

            // 泡の左右どちら側に当たったか見て、X座標を泡の横幅の半分のサイズ分ずらす
            if (transform.position.x <= collision.transform.position.x)
            {
                // 左側
                newX = -bobbleWidthSize / 2;

                // ヒットした泡が偶数行か、同じ行なら左にずれる
                if (hitY % 2 == 0 || hitY == newIY)
                {
                    newIX--;
                }

            }
            else
            {
                // 右側
                newX = bobbleWidthSize / 2;

                // ヒットした泡が奇数行か、同じ行なら右にずれる
                if (hitY % 2 == 1 || hitY == newIY)
                {
                    newIX++;
                }

            }

            // 泡と同じ行だった場合
            if(hitY == newIY)
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

            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble.BobbleColor, myBobble);

            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble.BobbleColor);

            // 停止させる
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
