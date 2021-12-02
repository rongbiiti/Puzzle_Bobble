using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // ボタンクリックから呼ばれる
    // ポーズ中か否かを見て、ポーズするか解除する
    public void ButtonPause()
    {
        if (GameManager.Instance.isBobbleFalloutGameOverZone) return;
        PauseManager.Instance.Pause();
        SoundManager.Instance.PlaySystemSE(SysSE.Pause);
    }

    // ポーズ解除してもらう
    public void ButtonUnPause()
    {
        PauseManager.Instance.Resume();
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameEnd()
    {
        ButtonUnPause();
        SoundManager.Instance.BGMFadeChange(BGM.Result, 0.5f);
        GameManager.Instance.GameOver();
    }
}
