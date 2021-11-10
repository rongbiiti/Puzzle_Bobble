using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // �ʂ̑��x
    [HideInInspector] public Vector3 moveDirection;   // �ʂ̐i�s����
    private bool moveFlg;           // ���ꂪtrue�̂Ƃ��Ɉړ�������������

    void Start()
    {
        
    }

    void Update()
    {
        // �t���O�������Ă��鎞�����ړ�������������
        if (moveFlg)
        {
            transform.Translate(moveDirection * moveSpeed);
        }
        
    }

    /// <summary>
    /// �ʂ𔭎˂���B
    /// </summary>
    /// <param name="speed">�ʂ̑��x</param>
    /// <param name="direction">�ʂ̐i�s����</param>
    public void ShotBubble(float speed, Vector3 direction)
    {
        moveSpeed = speed;
        moveDirection = direction;
        moveFlg = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ǂɓ��������琳���˂�����
        if (collision.gameObject.CompareTag("Wall"))
        {

        }
        else
        {
            // ��~������
            moveFlg = false;
            enabled = false;
        }
        
    }
}
