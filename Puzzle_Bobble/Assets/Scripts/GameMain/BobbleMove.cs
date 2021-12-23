using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection;   // 玉の進行方向
    private Vector3 prePosition;                      // 前フレームでの位置
    private bool hitFlg;            // これがfalseのときだけヒット後の処理
    private float bobbleHeightSize = 0.52f;           // 泡１個の縦幅
    private float bobbleWidthSize = 0.56f;            // 泡１個の横幅
    private Vector3 lastVelocity;                     // 前フレームのvelocity
    private Bobble myBobble;                          // Bobbleコンポーネント
    private Rigidbody2D rb;                           // Rigidbody2Dコンポーネント
    private CircleCollider2D circleCollider;          // CircleColliderコンポーネント
    private float startColliderRadius;                // 生成時のコライダーのRadius
    private float destroyWaitTime;                    // この秒数を超えて存在していたら削除

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
        prePosition = transform.position;

        destroyWaitTime += Time.deltaTime;
        if(10f < destroyWaitTime)
        {
            GameManager.Instance.shootedBobbleMoving = false;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 玉を発射する。
    /// </summary>
    /// <param name="speed">玉の速度</param>
    /// <param name="direction">玉の進行方向</param>
    public void ShotBubble(float speed, Vector3 direction)
    {
        // 物理挙動で動かす
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        moveDirection = direction;
        
        circleCollider = GetComponent<CircleCollider2D>();

        // 移動中は隙間を通れるよう当たり判定を小さくする
        startColliderRadius = circleCollider.radius;
        circleCollider.radius = 0.4f;

        // Bobbleコンポーネント取得
        myBobble = GetComponent<Bobble>();

        // 移動中はタグを変えておく
        transform.tag = "MovingBobble";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 壁に当たったら正反射させる
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
            SoundManager.Instance.PlaySE(SE.WallReflect);
        }
        else
        {
            // 泡に当たったとき

            // １フレームで２回処理させないように
            if (hitFlg) return;
            hitFlg = true;

            // Rigidbodyタイプをキネマティックに変更（元はダイナミック）、物理挙動をさせないようにする
            rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.simulated = false;
            rb.velocity = Vector3.zero;

            // 当たった泡のBobbleスクリプトを取得
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitIX = hitBobble.GetNumber().x;
            int hitIY = hitBobble.GetNumber().y;

            //Debug.Log("X : " + hitIX + " Y : " + hitIY);

            float newY = 0;     // Y座標
            int newIY = hitIY;  // 行番号

            if (prePosition.y <= collision.transform.position.y - bobbleHeightSize / 2)
            {
                // 下側
                newY = -bobbleHeightSize;
                newIY++;
            }

            float newX = 0;     // X座標
            int newIX = hitIX;  // 列番号

            // 泡の左右どちら側に当たったか見て、X座標を泡の横幅の半分のサイズ分ずらす
            if (prePosition.x <= collision.transform.position.x)
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

            // 行と列番号確定
            myBobble.bobbleNumber.x = newIX;
            myBobble.bobbleNumber.y = newIY;

            // 泡グループの子になる
            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble._BobbleColor, myBobble);

            // 音再生
            SoundManager.Instance.PlaySE(SE.BobbleSeted);

            // 泡が消せるかチェックさせる
            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble._BobbleColor);

            // 当たり判定を元に戻す
            circleCollider.radius = startColliderRadius;

            lastVelocity = rb.velocity;

            // タグを戻す
            transform.tag = "Bobble";

            // 停止させる
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
