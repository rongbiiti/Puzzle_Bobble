using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private GUIStyle style;
    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 120;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �A���G�ꂽ��Q�[���I�[�o�[�t���O�𗧂Ă�
        if (collision.CompareTag("Bobble"))
        {
            GameManager.Instance.isBobbleFalloutGameOverZone = true;
        }
    }

    private void OnGUI()
    {
        if (GameManager.Instance.isBobbleFalloutGameOverZone)
        {
            GUI.Label(new Rect(100, 960, 1080, 60), "Game Over", style);
        }
    }
}
