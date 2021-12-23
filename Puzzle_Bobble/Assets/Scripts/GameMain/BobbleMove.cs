using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleMove : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection;   // �ʂ̐i�s����
    private Vector3 prePosition;                      // �O�t���[���ł̈ʒu
    private bool hitFlg;            // ���ꂪfalse�̂Ƃ������q�b�g��̏���
    private float bobbleHeightSize = 0.52f;           // �A�P�̏c��
    private float bobbleWidthSize = 0.56f;            // �A�P�̉���
    private Vector3 lastVelocity;                     // �O�t���[����velocity
    private Bobble myBobble;                          // Bobble�R���|�[�l���g
    private Rigidbody2D rb;                           // Rigidbody2D�R���|�[�l���g
    private CircleCollider2D circleCollider;          // CircleCollider�R���|�[�l���g
    private float startColliderRadius;                // �������̃R���C�_�[��Radius
    private float destroyWaitTime;                    // ���̕b���𒴂��đ��݂��Ă�����폜

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
    /// �ʂ𔭎˂���B
    /// </summary>
    /// <param name="speed">�ʂ̑��x</param>
    /// <param name="direction">�ʂ̐i�s����</param>
    public void ShotBubble(float speed, Vector3 direction)
    {
        // ���������œ�����
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        moveDirection = direction;
        
        circleCollider = GetComponent<CircleCollider2D>();

        // �ړ����͌��Ԃ�ʂ��悤�����蔻�������������
        startColliderRadius = circleCollider.radius;
        circleCollider.radius = 0.4f;

        // Bobble�R���|�[�l���g�擾
        myBobble = GetComponent<Bobble>();

        // �ړ����̓^�O��ς��Ă���
        transform.tag = "MovingBobble";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            // �ǂɓ��������琳���˂�����
            moveDirection = Vector3.Reflect(moveDirection, transform.right);
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
            SoundManager.Instance.PlaySE(SE.WallReflect);
        }
        else
        {
            // �A�ɓ��������Ƃ�

            // �P�t���[���łQ�񏈗������Ȃ��悤��
            if (hitFlg) return;
            hitFlg = true;

            // Rigidbody�^�C�v���L�l�}�e�B�b�N�ɕύX�i���̓_�C�i�~�b�N�j�A���������������Ȃ��悤�ɂ���
            rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.simulated = false;
            rb.velocity = Vector3.zero;

            // ���������A��Bobble�X�N���v�g���擾
            Bobble hitBobble = collision.gameObject.GetComponent<Bobble>();
            int hitIX = hitBobble.GetNumber().x;
            int hitIY = hitBobble.GetNumber().y;

            //Debug.Log("X : " + hitIX + " Y : " + hitIY);

            float newY = 0;     // Y���W
            int newIY = hitIY;  // �s�ԍ�

            if (prePosition.y <= collision.transform.position.y - bobbleHeightSize / 2)
            {
                // ����
                newY = -bobbleHeightSize;
                newIY++;
            }

            float newX = 0;     // X���W
            int newIX = hitIX;  // ��ԍ�

            // �A�̍��E�ǂ��瑤�ɓ������������āAX���W��A�̉����̔����̃T�C�Y�����炷
            if (prePosition.x <= collision.transform.position.x)
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

            // �s�Ɨ�ԍ��m��
            myBobble.bobbleNumber.x = newIX;
            myBobble.bobbleNumber.y = newIY;

            // �A�O���[�v�̎q�ɂȂ�
            transform.parent = BobbleArrayManager.Instance.GetSameRowBobbleGroup(newIX, newIY, myBobble._BobbleColor, myBobble);

            // ���Đ�
            SoundManager.Instance.PlaySE(SE.BobbleSeted);

            // �A�������邩�`�F�b�N������
            BobbleArrayManager.Instance.BobbleDeleteCheck(newIX, newIY, myBobble._BobbleColor);

            // �����蔻������ɖ߂�
            circleCollider.radius = startColliderRadius;

            lastVelocity = rb.velocity;

            // �^�O��߂�
            transform.tag = "Bobble";

            // ��~������
            Destroy(GetComponent<BobbleMove>());
        }
        
    }
}
