using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    // �|�[�Y��t�@�|�[�Y�����ۂ��ŏ����𕪂���
    public void Pause()
    {
        if (!Pauser.isCanPausing) return;

        if (Pauser.isPaused)
        {
            Resume();
        }
        else
        {
            Pauser.Pause();
        }

    }

    // �|�[�Y����
    public void Resume()
    {
        Pauser.Resume();
    }
}
