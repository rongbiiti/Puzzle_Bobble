using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// ポーズボタン
    /// </summary>
    [SerializeField] private GameObject _pauseButton;

    /// <summary>
    /// スコアテキスト
    /// </summary>
    [SerializeField] private Text scoreText;

    void Start()
    {
        _pauseButton.SetActive(false);
        scoreText.text = ScoreManager.Instance.Score.ToString("N0");
    }

    
}
