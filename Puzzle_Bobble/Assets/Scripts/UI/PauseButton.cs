using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // ボタンクリックから呼ばれる
    // ポーズ中か否かを見て、ポーズするか解除する
    public void ButtonPause()
    {
        PauseManager.Instance.Pause();
    }

    // ポーズ解除してもらう
    public void ButtonUnPause()
    {
        PauseManager.Instance.Resume();
    }
}
