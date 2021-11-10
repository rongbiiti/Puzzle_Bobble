using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // 玉の速度
    [HideInInspector] public Vector3 moveDirection;   // 玉の進行方向
    private bool moveFlg;           // これがtrueのときに移動処理をさせる

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

        }
        else
        {
            // 停止させる
            moveFlg = false;
            enabled = false;
        }
        
    }
}
