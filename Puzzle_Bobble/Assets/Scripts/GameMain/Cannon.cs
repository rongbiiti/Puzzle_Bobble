using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    /// <summary>
    /// �A�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject _bobblePrefab;

    /// <summary>
    /// ���˂����ʂ̑��x
    /// </summary>
    [SerializeField] private float _bobbleMoveSpeed = 1f;    

    /// <summary>
    /// �K�C�h�T�[�N���Ǘ��X�N���v�g
    /// </summary>
    [SerializeField] private GuideCircle _guideCircle;

    /// <summary>
    /// ��C�̉�]�\�ŏ��p�x
    /// </summary>
    [SerializeField] private float _minAngleDelta = 10f;

    /// <summary>
    /// ��C�̉�]�\�ő�p�x
    /// </summary>
    [SerializeField] private float _maxAngleDelta = 170f;

    /// <summary>
    /// ���ɑ�C�ɃZ�b�g����ʂ̐������ʒu
    /// </summary>
    [SerializeField] private Transform _nextBobblePosition;

    /// <summary>
    /// ���˃G�t�F�N�g
    /// </summary>
    [SerializeField] private GameObject _fireEffect;

    private GameObject haveBobbleObj;      // ��C�ɃZ�b�g����Ă����
    private bool shotFlg;               // ���ˉ\�t���O
    private bool isShotBobbleChanging;  // �ʌ����A�j������
    private bool isShotGuideActive;     // ���˃K�C�h�\������
    private Vector3 shotDirection;      // �ʂ𔭎˂������

    private GameObject nextBobbleObj;      // ���ɑ�C�ɃZ�b�g�����

    private GUIStyle style; // OnGUI�Ńf�o�b�O�\���p
    private Vector3 pos;    // OnGUI�Ńf�o�b�O�\���p
    private string dbgStr;  // OnGUI�Ńf�o�b�O�\���p
    private int dbgTouchCount;     // OnGUI�Ńf�o�b�O�\���p

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        // ��C����Ɍ�����
        transform.rotation = Quaternion.Euler(0, 0, 90.0f);

        // ���˃K�C�h���\���ɂ��Ă���
        _guideCircle.SetActiveGuideCircle(false);

        nextBobbleObj = Instantiate(_bobblePrefab, transform.position, Quaternion.identity) as GameObject;
        nextBobbleObj.GetComponent<Bobble>()._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);
        nextBobbleObj.GetComponent<Bobble>().enabled = false;

        // ���˂���ʂ𐶐�
        Reload();

        // �f�o�b�O�p
        style = new GUIStyle();
        style.fontSize = 50;
    }

    void Update()
    {
        if (GameManager.Instance.isBobbleFalloutGameOverZone || GameManager.Instance.isBobbleDeleting) return;

        if (Application.isEditor)
        {
            // �G�f�B�^�Ŏ��s��

            // �N���b�N�����u��
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegan(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            // �������u��
            if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnded(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            // �N���b�N�����ςȂ�
            if (Input.GetMouseButton(0))
            {
                OnTouchHold(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else
        {
            // ���@�Ŏ��s��

            // �^�b�`����Ă��邩�`�F�b�N
            if (Input.touchCount > 0)
            {
                // �^�b�`���̎擾
                Touch touch = Input.GetTouch(0);

                // �^�b�`�����u��
                if (touch.phase == TouchPhase.Began)
                {
                    OnTouchBegan(Camera.main.ScreenToWorldPoint(touch.position));
                }

                // �������u��
                if (touch.phase == TouchPhase.Ended)
                {
                    OnTouchEnded(Camera.main.ScreenToWorldPoint(touch.position));
                }

                // �^�b�`�����ςȂ�
                if (touch.phase == TouchPhase.Moved)
                {
                    OnTouchHold(Camera.main.ScreenToWorldPoint(touch.position));
                }
            }
        }
    }

    /// <summary>
    /// �^�b�`�����u�Ԃ̏���
    /// </summary>
    private void OnTouchBegan(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        _guideCircle.SetActiveGuideCircle(true);    // ���˃K�C�h�\��
        isShotGuideActive = true;
        SoundManager.Instance.PlaySE(SE.CannonAiming);

        // �ȉ��f�o�b�O�p
        pos = touchPosition;
        dbgStr = "�^�b�`�J�n";
        dbgTouchCount++;
    }

    /// <summary>
    /// �������u�Ԃ̏���
    /// </summary>
    private void OnTouchEnded(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        _guideCircle.SetActiveGuideCircle(false);   // ���˃K�C�h��\��
        isShotGuideActive = false;

        // �ʂ������Ă���Ƃ��������ˏ���
        if (haveBobbleObj && shotFlg)
        {
            haveBobbleObj.AddComponent<BobbleMove>().ShotBubble(_bobbleMoveSpeed, shotDirection);    // �ʂ𔭎�
            haveBobbleObj = null;
            GameManager.Instance.shootedBobbleMoving = true;
            SoundManager.Instance.PlaySE(SE.CannonFire);
            StartCoroutine(nameof(ReloadBobble));
            Instantiate(_fireEffect, transform.position, Quaternion.identity);
        }        

        // �ȉ��f�o�b�O�p
        pos = touchPosition;
        dbgStr = "�^�b�`�I��";
    }

    /// <summary>
    /// �^�b�`�����ςȂ��̂Ƃ��̏���
    /// </summary>
    private void OnTouchHold(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        // ���˃K�C�h���\������ĂȂ�������A�\������
        if (!isShotGuideActive)
        {
            _guideCircle.SetActiveGuideCircle(true);    // ���˃K�C�h�\��
            isShotGuideActive = true;
        }

        touchPosition.z = transform.position.z; // Z���W�𑵂���

        Vector3 dir = (touchPosition - transform.position).normalized;  // �x�N�g����������������o��
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);      // ��������p�x���쐬

        // 0�x����180�x�������ꍇ�̂݉�]
        // ���̒l�i-1�x �` -180�x�j�A�܂萅����艺���������ƂɂȂ�Ȃ��]�����Ȃ�
        // ����ɁA�C���X�y�N�^�[�Ŏw�肵���ŏ��p�x�ƍő�p�x�͈͓̔��ŉ�]������
        if (0 <= angle && _minAngleDelta <= angle && angle <= _maxAngleDelta)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);         // ���̕����ɑ�C��������
            shotDirection = dir;            // ���˂���������Z�b�g
            shotFlg = true;
        }
        else if (0 <= angle && angle <= 180)
        {
            // �������ŏ��p�x�ƍő�p�x�łȂ��Ă��A�����ȏ�Ȃ甭�˕����͏㏑�������ɔ��ˉ\�ɂ���
            shotFlg = true;
        }
        else
        {
            // ���ˋ֎~
            shotFlg = false;
        }

        // �ȉ��f�o�b�O�p
        pos = touchPosition;
        dbgStr = "�^�b�`��";
    }

    /// <summary>
    /// �ʂ������_���Ő�������
    /// </summary>
    private void Reload()
    {
        // �O�ɐ������Ă������A�𑕓U����
        haveBobbleObj = nextBobbleObj;
        haveBobbleObj.transform.position = transform.position;

        // ���e����
        nextBobbleObj = Instantiate(_bobblePrefab, _nextBobblePosition.position, Quaternion.identity) as GameObject;

        // �������Ă�A�Ɛ��������A�̃R���|�[�l���g�擾�i�����Get������߂�ǂ��j
        Bobble haveBobbleComp = haveBobbleObj.GetComponent<Bobble>();
        Bobble nextBobbleComp = nextBobbleObj.GetComponent<Bobble>();
        
        // �F�������_���Ő�������
        // �������U�����ʂƓ����F�Ȃ�Ē��I����
        do
        {
            nextBobbleComp._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);

        } while (nextBobbleComp._BobbleColor == haveBobbleComp._BobbleColor);

        // Bobble�R���|�[�l���g���A�N�e�B�u�ɂ��Ă����i�����������Ȃ��Ȃ�j
        nextBobbleComp.enabled = false;
    }

    /// <summary>
    /// ���ɑ�C�ɃZ�b�g����ʂƑ�C�ɃZ�b�g����Ă���ʂ�����
    /// </summary>
    public void ShotBobbleChange()
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        // �ʒu�����ւ�
        haveBobbleObj.transform.position = _nextBobblePosition.transform.position;
        nextBobbleObj.transform.position = transform.position;

        // �ʂ����ւ�
        GameObject tmp = nextBobbleObj;
        nextBobbleObj = haveBobbleObj;
        haveBobbleObj = tmp;        

        Debug.Log("�����ꂽ");
    }


    /// <summary>
    /// �����Ԃ������ċʂ𐶐�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadBobble()
    {
        while (GameManager.Instance.shootedBobbleMoving)
        {
            yield return new WaitForFixedUpdate();
        }
        
        Reload();
    }

}