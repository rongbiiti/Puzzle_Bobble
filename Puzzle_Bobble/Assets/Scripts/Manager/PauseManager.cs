using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    // ポーズ受付　ポーズ中か否かで処理を分ける
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

    // ポーズ解除
    public void Resume()
    {
        Pauser.Resume();
    }
}
