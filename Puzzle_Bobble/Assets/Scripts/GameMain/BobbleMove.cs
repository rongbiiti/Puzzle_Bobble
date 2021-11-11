using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // 玉の速度
    [HideInInspector] public Vector3 moveDirection;   // 玉の進行方向
    private Vector3 prePosition;                      // 前フレームでの位置
    private bool moveFlg;           // これがtrueのときに移動処理をさせる
    private float bobbleHeightSize = 0.56f;           // 泡１個の縦幅
    private float bobbleWidthSize = 0.56f;            // 泡１個の横幅

    void Start()
    {
        
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
            float newX = 0;

            // 泡の左右どちら側に当たったか見て、X座標を泡の横幅の半分のサイズ分ずらす
            if(transform.position.x <= collision.transform.position.x)
            {
                // 左側
                newX = -bobbleWidthSize / 2;
            }
            else
            {
                // 右側
                newX = bobbleWidthSize / 2;
            }

            float newY = 0;

            // 泡の上下真ん中どれに当たったか見て、Y座標を泡の縦幅分ずらす
            if(collision.transform.position.y + bobbleHeightSize / 2 <= transform.position.y)
            {
                // 上側
                newY = bobbleHeightSize;
            }
            else if (transform.position.y <= collision.transform.position.y - bobbleHeightSize / 4)
            {
                // 下側
                newY = -bobbleHeightSize;
            }
            else
            {
                // 真ん中の場合はX座標をさらにずらす
                newY = 0;
                newX *= 2;
            }

            Vector3 newPosition = new Vector3(collision.transform.position.x + newX, collision.transform.position.y + newY);
            transform.position = newPosition;
            GameManager.Instance.shootedBobbleMoving = false;
            GetComponent<Bobble>().enabled = true;  // 固定したあと、ほかの泡と同様にじわじわ下に落ちるようにする

            // 停止させる
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
