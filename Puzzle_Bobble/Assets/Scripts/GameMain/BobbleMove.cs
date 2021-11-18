using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 1f;    // �ʂ̑��x
    [HideInInspector] public Vector3 moveDirection;   // �ʂ̐i�s����
    private Vector3 prePosition;                      // �O�t���[���ł̈ʒu
    private bool moveFlg;           // ���ꂪtrue�̂Ƃ��Ɉړ�������������
    private bool hitFlg;            // ���ꂪfalse�̂Ƃ������q�b�g��̏���
    private float bobbleHeightSize = 0.56f;           // �A�P�̏c��
    private float bobbleWidthSize = 0.56f;            // �A�P�̉���
    private Bobble myBobble;

    void Start()
    {
        myBobble = GetComponent<Bobble>();
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
            // �P�t���[���łQ�񏈗������Ȃ��悤��
            if (hitFlg) return;
            hitFlg = true;

            // ���������A��Bobble�X�N���v�g���擾
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitX = hitBobble.GetNumber().x;
            int hitY = hitBobble.GetNumber().y;

            Debug.Log("X : " + hitX + " Y : " + hitY);

            float newY = 0;
            int newIY = hitY;

            // �A�̏㉺�^�񒆂ǂ�ɓ������������āAY���W��A�̏c�������炷
            if(collision.transform.position.y + bobbleHeightSize / 2 <= transform.position.y)
            {
                // �㑤
                newY = bobbleHeightSize;
                newIY--;
            }
            else if (transform.position.y <= collision.transform.position.y - bobbleHeightSize / 4)
            {
                // ����
                newY = -bobbleHeightSize;
                newIY++;
            }

            float newX = 0;
            int newIX = hitX;

            // �A�̍��E�ǂ��瑤�ɓ������������āAX���W��A�̉����̔����̃T�C�Y�����炷
            if (transform.position.x <= collision.transform.position.x)
            {
                // ����
                newX = -bobbleWidthSize / 2;

                // �q�b�g�����A�������s���A�����s�Ȃ獶�ɂ����
                if (hitY % 2 == 0 || hitY == newIY)
                {
                    newIX--;
                }

            }
            else
            {
                // �E��
                newX = bobbleWidthSize / 2;

                // �q�b�g�����A����s���A�����s�Ȃ�E�ɂ����
                if (hitY % 2 == 1 || hitY == newIY)
                {
                    newIX++;
                }

            }

            // �A�Ɠ����s�������ꍇ
            if(hitY == newIY)
            {
                // ���[���hX���W������ɂ��炷
                newX *= 2;
            }

            

            // �ʂ��A�q�b�g�����A�̈ʒu�ƃq�b�g�������������ƂɏC�����āA�݂��Ⴂ�̈ʒu�ɐݒu
            Vector3 newPosition = new Vector3(collision.transform.position.x + newX, collision.transform.position.y + newY);
            transform.position = newPosition;

            // �Œ肵�����ƁA�ق��̖A�Ɠ��l�ɂ��킶�퉺�ɗ�����悤�ɂ���
            GameManager.Instance.shootedBobbleMoving = false;
            myBobble.enabled = true;

            myBobble.bobbleNumber.x = newIX;
            myBobble.bobbleNumber.y = newIY;

            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble.BobbleColor, myBobble);

            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble.BobbleColor);

            // ��~������
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
