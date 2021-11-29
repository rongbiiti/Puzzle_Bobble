using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // �{�^���N���b�N����Ă΂��
    // �|�[�Y�����ۂ������āA�|�[�Y���邩��������
    public void ButtonPause()
    {
        PauseManager.Instance.Pause();
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
        ButtonUnPause();
        GameManager.Instance.GameOver();
    }
}
