using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    // タイトル画面BGM
    [SerializeField] private BGM _titleSceneBGM;

    // タイトル画面に遷移したときに実行する処理
    void Start()
    {
        // タイトル画面BGMを再生する
        SoundManager.Instance.PlayBGM(_titleSceneBGM);
    }

    // タイトル画面BGMをフェードアウト
    public void TitleBGMFadeOut()
    {
        SoundManager.Instance.BGMFadeOut(1.9f);
    }
    
}