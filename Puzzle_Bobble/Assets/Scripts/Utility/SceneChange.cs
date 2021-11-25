using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// �J�ڂ���V�[����BuildIndex
    /// </summary>
    [SerializeField] private int _sceneIndex;

    /// <summary>
    /// �V�[���J��
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndex);
        Pauser.Resume();
    }
}
