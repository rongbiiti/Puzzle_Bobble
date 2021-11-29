using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    /// <summary>�t�F�[�h�F</summary>
    public Color fadeColor = Color.black;

    /// <summary>�t�F�[�h���̓����x</summary>
	private float fadeAlpha = 0;

    /// <summary>�t�F�[�h�����ǂ���</summary>
    private bool isFading = false;

    /// <summary>�t�F�[�h�֐����Ă΂ꂽ��</summary>
    private bool fadeFlg;

    protected override void Awake()
    {
        base.Awake();

        // �V�[���J�ڎ���Destroy����Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    private void OnGUI()
    {
        if(isFading)
        {
            //�F�Ɠ����x���X�V���Ĕ��e�N�X�`����`��
            fadeColor.a = fadeAlpha;
            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    /// <summary>
	/// ��ʑJ��
	/// </summary>
	/// <param name='scene'>�V�[���C���f�b�N�X</param>
	/// <param name='outInterval'>�Ó]�ɂ����鎞��(�b)</param>
	public void LoadScene(int scene, float outInterval, float inInterval)
    {
        if (fadeFlg) return;
        fadeFlg = true;
        StartCoroutine(TransScene(scene, outInterval, inInterval));
    }

    /// <summary>
    /// �V�[���J�ڗp�R���[�`��
    /// </summary>
    /// <param name='scene'>�V�[���C���f�b�N�X</param>
    /// <param name='outInterval'>�Ó]�ɂ����鎞��(�b)</param>
    private IEnumerator TransScene(int scene, float outInterval, float inInterval)
    {
        //���񂾂�Â�
        isFading = true;
        float time = 0;
        while (time <= outInterval)
        {
            fadeAlpha = Mathf.Lerp(0f, 1f, time / outInterval);
            time += Time.deltaTime;
            yield return 0;
        }

        //�V�[���ؑ�
        SceneManager.LoadScene(scene);

        Pauser.isPaused = false;
        Pauser.isCanPausing = true;

        //���񂾂񖾂邭
        time = 0;
        while (time <= inInterval)
        {
            fadeAlpha = Mathf.Lerp(1f, 0f, time / inInterval);
            time += Time.deltaTime;
            yield return 0;
        }

        isFading = false;
        fadeFlg = false;
    }
}
