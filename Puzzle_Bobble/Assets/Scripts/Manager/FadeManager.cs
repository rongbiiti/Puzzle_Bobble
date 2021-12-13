using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    /// <summary>フェード色</summary>
    public Color fadeColor = Color.black;

    /// <summary>フェード中の透明度</summary>
	private float fadeAlpha = 0;

    /// <summary>フェード中かどうか</summary>
    private bool isFading = false;

    /// <summary>フェード関数が呼ばれたか</summary>
    private bool fadeFlg;

    /// <summary>
    /// シーン上にあるShaderScreenFader
    /// </summary>
    private ShaderScrennFader scrennFader;

    protected override void Awake()
    {
        base.Awake();

        // シーン遷移時にDestroyされないようにする
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ShaderScreenFaderを取得
        scrennFader = FindObjectOfType<ShaderScrennFader>();
        if(scrennFader)
        {
            // コンポーネントをOFFにしておく
            scrennFader.enabled = false;
        }
    }

    #region OnGUI

    private void OnGUI()
    {
        if(isFading)
        {
            //色と透明度を更新して白テクスチャを描画
            //fadeColor.a = fadeAlpha;
            //GUI.color = fadeColor;
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    #endregion

    /// <summary>
    /// 画面遷移
    /// </summary>
    /// <param name='scene'>シーンインデックス</param>
    /// <param name='outInterval'>暗転にかかる時間(秒)</param>
    public void LoadScene(int scene, float outInterval, float inInterval)
    {
        if (fadeFlg) return;
        fadeFlg = true;
        StartCoroutine(TransScene(scene, outInterval, inInterval));
    }

    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name='scene'>シーンインデックス</param>
    /// <param name='outInterval'>暗転にかかる時間(秒)</param>
    private IEnumerator TransScene(int scene, float outInterval, float inInterval)
    {
        //だんだん暗く
        isFading = true;

        // ShaderScreenFaderを確実に取得しておく
        if (!scrennFader)
        {
            scrennFader = FindObjectOfType<ShaderScrennFader>();
        }
        // ONにする
        scrennFader.enabled = true;

        float time = 0;
        while (time <= outInterval)
        {
            fadeAlpha = Mathf.Lerp(0f, 1f, time / outInterval);

            scrennFader.SetMatrialFloat("_Alpha", fadeAlpha);

            time += Time.deltaTime;
            yield return 0;
        }

        //シーン切替
        SceneManager.LoadScene(scene);
        //yield return 0;
        // シーンが切り替わったのでShaderScreenFaderの参照を外す
        scrennFader = null;

        // ShaderScreenFaderを取得できるまで繰り返す
        do
        {
            scrennFader = FindObjectOfType<ShaderScrennFader>();
            yield return 0;
        }
        while (!scrennFader);
        // ONにする
        scrennFader.enabled = true;

        Pauser.isPaused = false;
        Pauser.isCanPausing = true;

        //だんだん明るく
        time = 0;
        while (time <= inInterval)
        {
            fadeAlpha = Mathf.Lerp(1f, 0f, time / inInterval);

            scrennFader.SetMatrialFloat("_Alpha", fadeAlpha);

            time += Time.deltaTime;
            yield return 0;
        }

        isFading = false;
        fadeFlg = false;
        scrennFader.enabled = false;
    }
}
