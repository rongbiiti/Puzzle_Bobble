using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// �J�ڂ���V�[����BuildIndex
    /// </summary>
    [SerializeField] private int _sceneIndex;

    /// <summary>
    /// �t�F�[�h�A�E�g�ɂ����鎞��
    /// </summary>
    [SerializeField] private float _fadeOutInterval = 2f;

    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    [SerializeField] private float _fadeInInterval = 0.5f;

    /// <summary>
    /// �V�[���J��
    /// </summary>
    public void LoadScene()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(_sceneIndex);
        FadeManager.Instance.LoadScene(_sceneIndex, _fadeOutInterval, _fadeInInterval);
        
    }
}
