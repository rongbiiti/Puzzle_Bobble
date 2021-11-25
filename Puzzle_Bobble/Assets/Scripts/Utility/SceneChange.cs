using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// 遷移するシーンのBuildIndex
    /// </summary>
    [SerializeField] private int _sceneIndex;

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndex);
        Pauser.Resume();
    }
}
