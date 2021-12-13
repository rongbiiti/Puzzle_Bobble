using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button button;          // Buttonコンポーネント
    private bool preIsCanPausing;   // 前のフレームのPauserのポーズ可能状態

    private void Start()
    {
        button = GetComponent<Button>();
        preIsCanPausing = Pauser.isCanPausing;
    }

    private void Update()
    {
        // ポーズ可能状態が変わっていたら、処理をする
        if(preIsCanPausing != Pauser.isCanPausing)
        {
            
            if (Pauser.isCanPausing)
            {
                // ポーズできるようになっていたら、ポーズボタンを触れるようにする
                button.interactable = true;
            }
            else
            {
                // ポーズできないようになっていたら、ポーズボタンを触れないようにする
                button.interactable = false;
            }
        }

        preIsCanPausing = Pauser.isCanPausing;
    }

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
        Time.timeScale = 1f;
        SoundManager.Instance.BGMFadeChange(BGM.Result, 0.5f);
        GameManager.Instance.GameOver();
    }
}
