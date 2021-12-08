using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// アニメーションするゲームオーバーテキストのPrefab
    /// </summary>
    [SerializeField] private GameObject _gameOverTextPrefab;

    /// <summary>
    /// ゲームオーバーUI
    /// </summary>
    [SerializeField] private GameObject _gameOverPanel;
    public GameObject GetGameOverPanel()
    {
        return _gameOverPanel;
    }

    // 撃った玉が移動中か
    public bool shootedBobbleMoving;

    // 泡の削除演出中か
    public bool isBobbleDeleting;

    // 泡がゲームオーバーゾーンまで落ち切ったか
    public bool isBobbleFalloutGameOverZone;

    // ゲームスピード
    public float gameSpeed = 1f;

    private void Start()
    {
        // 参照が空だったら検索して記憶しておく。
        if(_gameOverPanel == null)
        {
            _gameOverPanel = GameObject.Find("GameOverPanel");
        }
    }

    /// <summary>
    /// アニメーションするゲームオーバーテキストのPrefabを出す
    /// </summary>
    public void InstantiateGameOverText()
    {
        Instantiate(_gameOverTextPrefab, ScoreManager.Instance.GetCanvas().transform);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        isBobbleFalloutGameOverZone = true;
        FindObjectOfType<GameOverZone>().enabled = false;
        
        // ゲームオーバーのUIをアクティブにする
        _gameOverPanel.SetActive(true);
    }
}