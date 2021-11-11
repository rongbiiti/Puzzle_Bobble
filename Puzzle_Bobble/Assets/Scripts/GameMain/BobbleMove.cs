using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // �ʂ̑��x
    [HideInInspector] public Vector3 moveDirection;   // �ʂ̐i�s����
    private Vector3 prePosition;                      // �O�t���[���ł̈ʒu
    private bool moveFlg;           // ���ꂪtrue�̂Ƃ��Ɉړ�������������
    private float bobbleHeightSize = 0.56f;           // �A�P�̏c��
    private float bobbleWidthSize = 0.56f;            // �A�P�̉���

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

        prePosition = transform.position;
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
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
        }
        else
        {
            float newX = 0;

            // �A�̍��E�ǂ��瑤�ɓ������������āAX���W��A�̉����̔����̃T�C�Y�����炷
            if(transform.position.x <= collision.transform.position.x)
            {
                // ����
                newX = -bobbleWidthSize / 2;
            }
            else
            {
                // �E��
                newX = bobbleWidthSize / 2;
            }

            float newY = 0;

            // �A�̏㉺�^�񒆂ǂ�ɓ������������āAY���W��A�̏c�������炷
            if(collision.transform.position.y + bobbleHeightSize / 2 <= transform.position.y)
            {
                // �㑤
                newY = bobbleHeightSize;
            }
            else if (transform.position.y <= collision.transform.position.y - bobbleHeightSize / 4)
            {
                // ����
                newY = -bobbleHeightSize;
            }
            else
            {
                // �^�񒆂̏ꍇ��X���W������ɂ��炷
                newY = 0;
                newX *= 2;
            }

            Vector3 newPosition = new Vector3(collision.transform.position.x + newX, collision.transform.position.y + newY);
            transform.position = newPosition;
            GameManager.Instance.shootedBobbleMoving = false;
            GetComponent<Bobble>().enabled = true;  // �Œ肵�����ƁA�ق��̖A�Ɠ��l�ɂ��킶�퉺�ɗ�����悤�ɂ���

            // ��~������
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
