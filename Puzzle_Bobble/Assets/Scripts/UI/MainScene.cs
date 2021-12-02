using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    // メインに遷移したときに実行する処理
    void Start()
    {
        
    }

    // タイトル画面BGMをフェードアウト
    public void MainBGMFadeOut()
    {
        SoundManager.Instance.BGMFadeOut(1.9f);
    }
}
