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

    private int contactCount;   // ���̃I�u�W�F�N�g�̃g���K�[�͈͂ƐڐG���Ă���A�̌�
    private bool isStartedSpeedUpCoroutine; // �R���[�`�����Ăяo������
    private float startFallSpeed;

    private void Start()
    {
        startFallSpeed = GameManager.Instance.gameSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            contactCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            contactCount--;
        }

        if(contactCount <= 0 && !isStartedSpeedUpCoroutine)
        {
            isStartedSpeedUpCoroutine = true;
            StartCoroutine(nameof(FallSpeedUp));
        }
    }

    private IEnumerator FallSpeedUp()
    {
        yield return new WaitForSeconds(_speedUpDelay);

        if(contactCount <= 0)
        {
            GameManager.Instance.gameSpeed = _SpeedUpFallSpeed;
            yield return new WaitForSeconds(_bobbleFallSpeedUpTime);
        }
        

        GameManager.Instance.gameSpeed = startFallSpeed;
        isStartedSpeedUpCoroutine = false;
    }

    
}
