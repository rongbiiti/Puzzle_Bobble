using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleFallSpeedControll : MonoBehaviour
{
    /// <summary>
    /// ���̊ԁA�A�̗������x�����܂�
    /// </summary>
    [SerializeField] private float _bobbleFallSpeedUpTime = 5f;

    /// <summary>
    /// �������x���͂܂��Ă���ԁA���̑��x�ŗ�������
    /// </summary>
    [SerializeField] private float _SpeedUpFallSpeed = 10f;

    /// <summary>
    /// �g���K�[�ɖA���G��Ă��Ȃ�������A���̕b���҂��Ă���X�s�[�h�A�b�v������
    /// �������̊ԂɖA���G�ꂽ��A�X�s�[�h�A�b�v�����Ȃ�
    /// </summary>
    [SerializeField] private float _speedUpDelay = 2f;

    private GameManager gm;     // GameManager�̃C���X�^���X
    private bool isStartedSpeedUpCoroutine; // �R���[�`�����Ăяo������
    private float startFallSpeed;   // �J�n���̗������x

    private void Start()
    {   
        gm = GameManager.Instance;      // �C���X�^���X�擾���Ă���
        startFallSpeed = gm.gameSpeed;  // �J�n���̃Q�[���X�s�[�h�o���Ă���
    }

    private void LateUpdate()
    {
        // gm.fallSpeedUpZoneContactCount��0�ȉ��ŁA
        // �R���[�`�����܂��Ăяo���Ă��Ȃ���
        // �폜���o���I����Ă�����
        if (gm.fallSpeedUpZoneContactCount <= 0 && !isStartedSpeedUpCoroutine && !GameManager.Instance.isBobbleDeleting)
        {
            // �������x�㏸�̃R���[�`�����Ă�
            isStartedSpeedUpCoroutine = true;
            StartCoroutine(nameof(FallSpeedUp));
        }

    }

    // ���̃g���K�[�ɖA����������J�E���g�𑝂₷
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            gm.fallSpeedUpZoneContactCount++;

            // ���J�E���g�́A�A�̍폜������0�Ƀ��Z�b�g�����B
        }
    }

    // �A�̗������x���グ��
    private IEnumerator FallSpeedUp()
    {
        // �A�������o�����u�Ԃ���ꔏ�u��
        yield return new WaitForSeconds(_speedUpDelay);

        // �������x�㏸����]�[���ɖA�������Ă��Ȃ���΁A�������x���グ��
        if(gm.fallSpeedUpZoneContactCount <= 0)
        {
            GameManager.Instance.gameSpeed = _SpeedUpFallSpeed;
            yield return new WaitForSeconds(_bobbleFallSpeedUpTime);
        }
        
        // �������x��߂�
        GameManager.Instance.gameSpeed = startFallSpeed;

        // ���̃R���[�`�����ĂьĂׂ�悤�ɂ���
        isStartedSpeedUpCoroutine = false;
    }



    
}
