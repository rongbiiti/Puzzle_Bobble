using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // �Q�[���I��
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
