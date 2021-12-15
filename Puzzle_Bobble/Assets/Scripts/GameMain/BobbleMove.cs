using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 2f;    // �ʂ̑��x
    [HideInInspector] public Vector3 moveDirection;   // �ʂ̐i�s����
    private Vector3 prePosition;                      // �O�t���[���ł̈ʒu
    private bool moveFlg;           // ���ꂪtrue�̂Ƃ��Ɉړ�������������
    private bool hitFlg;            // ���ꂪfalse�̂Ƃ������q�b�g��̏���
    private float bobbleHeightSize = 0.52f;           // �A�P�̏c��
    private float bobbleWidthSize = 0.56f;            // �A�P�̉���
    private Vector3 lastVelocity;
    private Bobble myBobble;
    private Rigidbody2D rb;

    void Start()
    {
        myBobble = GetComponent<Bobble>();
    }

    private void FixedUpdate()
    {
        // �t���O�������Ă��鎞�����ړ�������������
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
    /// �ʂ𔭎˂���B
    /// </summary>
    /// <param name="speed">�ʂ̑��x</param>
    /// <param name="direction">�ʂ̐i�s����</param>
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
        // �ǂɓ��������琳���˂�����
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
            SoundManager.Instance.PlaySE(SE.WallReflect);
        }
        else
        {
            // �P�t���[���łQ�񏈗������Ȃ��悤��
            if (hitFlg) return;
            hitFlg = true;

            rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.simulated = false;
            rb.velocity = Vector3.zero;

            // ���������A��Bobble�X�N���v�g���擾
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitIX = hitBobble.GetNumber().x;
            int hitIY = hitBobble.GetNumber().y;

            Debug.Log("X : " + hitIX + " Y : " + hitIY);

            float newY = 0;
            int newIY = hitIY;

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
            int newIX = hitIX;

            // �A�̍��E�ǂ��瑤�ɓ������������āAX���W��A�̉����̔����̃T�C�Y�����炷
            if (transform.position.x <= collision.transform.position.x)
            {
                // ����
                newX = -bobbleWidthSize / 2;

                // �q�b�g�����A�������s���A�����s�Ȃ獶�ɂ����
                if (hitIY % 2 == 0 || hitIY == newIY)
                {
                    newIX--;
                }

                // �Œ肳�ꂽ�C���f�b�N�X��0�����ɂȂ��Ă���0�ɂȂ�悤�C������
                if(newIX < 0)
                {
                    newIX = 0;
                    newX += bobbleWidthSize;
                }

            }
            else
            {
                // �E��
                newX = bobbleWidthSize / 2;

                // �q�b�g�����A����s���A�����s�Ȃ�E�ɂ����
                if (hitIY % 2 == 1 || hitIY == newIY)
                {
                    newIX++;
                }

                // �Œ肳�ꂽ�C���f�b�N�X���z��̗v�f�����z���Ă���v�f���Ɏ��܂�悤�C������
                if (BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (newIY % 2) <= newIX)
                {
                    newIX = BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - 1 - (newIY % 2);
                    newX -= bobbleWidthSize;
                }

            }

            // �A�Ɠ����s�������ꍇ
            if(hitIY == newIY)
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

            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble._BobbleColor, myBobble);

            SoundManager.Instance.PlaySE(SE.BobbleSeted);

            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble._BobbleColor);

            // ��~������
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
