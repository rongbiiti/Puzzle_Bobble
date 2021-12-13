using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button button;          // Button�R���|�[�l���g
    private bool preIsCanPausing;   // �O�̃t���[����Pauser�̃|�[�Y�\���

    private void Start()
    {
        button = GetComponent<Button>();
        preIsCanPausing = Pauser.isCanPausing;
    }

    private void Update()
    {
        // �|�[�Y�\��Ԃ��ς���Ă�����A����������
        if(preIsCanPausing != Pauser.isCanPausing)
        {
            
            if (Pauser.isCanPausing)
            {
                // �|�[�Y�ł���悤�ɂȂ��Ă�����A�|�[�Y�{�^����G���悤�ɂ���
                button.interactable = true;
            }
            else
            {
                // �|�[�Y�ł��Ȃ��悤�ɂȂ��Ă�����A�|�[�Y�{�^����G��Ȃ��悤�ɂ���
                button.interactable = false;
            }
        }

        preIsCanPausing = Pauser.isCanPausing;
    }

    // �{�^���N���b�N����Ă΂��
    // �|�[�Y�����ۂ������āA�|�[�Y���邩��������
    public void ButtonPause()
    {
        if (GameManager.Instance.isBobbleFalloutGameOverZone) return;
        PauseManager.Instance.Pause();
        SoundManager.Instance.PlaySystemSE(SysSE.Pause);
    }

    // �|�[�Y�������Ă��炤
    public void ButtonUnPause()
    {
        PauseManager.Instance.Resume();
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameEnd()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.BGMFadeChange(BGM.Result, 0.5f);
        GameManager.Instance.GameOver();
    }
}
