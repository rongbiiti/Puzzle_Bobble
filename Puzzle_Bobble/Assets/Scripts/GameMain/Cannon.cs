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

    private GameObject haveBobble;      // ��C�ɃZ�b�g����Ă����

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

        // ���˂���ʂ𐶐�
        haveBobble = Instantiate(_bobblePrefab, transform.position, Quaternion.identity) as GameObject;

        // �f�o�b�O�p
        style = new GUIStyle();
        style.fontSize = 50;
    }

    void Update()
    {
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
        _guideCircle.SetActiveGuideCircle(true);    // ���˃K�C�h�\��

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
        _guideCircle.SetActiveGuideCircle(false);   // ���˃K�C�h��\��

        touchPosition.z = transform.position.z;     // Z���W�𑵂���
        Vector3 dir = (touchPosition - transform.position).normalized;       // �x�N�g����������������o��

        // �ʂ������Ă���Ƃ��������ˏ���
        if (haveBobble)
        {
            haveBobble.AddComponent<BobbleMove>().ShotBubble(_bobbleMoveSpeed, dir);    // �ʂ𔭎�
            StartCoroutine(nameof(ReloadBobble));
        }
        
        haveBobble = null;

        // �ȉ��f�o�b�O�p
        pos = touchPosition;
        dbgStr = "�^�b�`�I��";
    }

    /// <summary>
    /// �^�b�`�����ςȂ��̂Ƃ��̏���
    /// </summary>
    private void OnTouchHold(Vector3 touchPosition)
    {
        touchPosition.z = transform.position.z; // Z���W�𑵂���

        Vector3 dir = (touchPosition - transform.position).normalized;  // �x�N�g����������������o��
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);      // ��������p�x���쐬

        // 0�x����180�x�������ꍇ�̂݉�]
        // ���̒l�i-1�x �` -180�x�j�A�܂萅����艺���������ƂɂȂ�Ȃ��]�����Ȃ�
        // ����ɁA�C���X�y�N�^�[�Ŏw�肵���ŏ��p�x�ƍő�p�x�͈͓̔��ŉ�]������
        if(0 <= angle && _minAngleDelta <= angle && angle <= _maxAngleDelta)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);         // ���̕����ɑ�C��������
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
        haveBobble = Instantiate(_bobblePrefab, transform.position, Quaternion.identity) as GameObject;
        haveBobble.GetComponent<Bobble>().BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow);
    }

    /// <summary>
    /// �����Ԃ������ċʂ𐶐�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadBobble()
    {
        yield return new WaitForSeconds(0.5f);
        Reload();
    }

    private void OnGUI()
    {
        var sw = Screen.width;
        var sh = Screen.height;

        GUI.Label(new Rect(50, sh - 80, 500, 500), "���W : " + pos + " " + dbgStr + " " + dbgTouchCount, style);
    }

}