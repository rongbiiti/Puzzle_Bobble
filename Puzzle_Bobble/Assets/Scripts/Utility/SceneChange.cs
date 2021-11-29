using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// 遷移するシーンのBuildIndex
    /// </summary>
    [SerializeField] private int _sceneIndex;

    /// <summary>
    /// フェードアウトにかける時間
    /// </summary>
    [SerializeField] private float _fadeOutInterval = 2f;

    /// <summary>
    /// フェードインにかける時間
    /// </summary>
    [SerializeField] private float _fadeInInterval = 0.5f;

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void LoadScene()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(_sceneIndex);
        FadeManager.Instance.LoadScene(_sceneIndex, _fadeOutInterval, _fadeInInterval);
        
    }
}
