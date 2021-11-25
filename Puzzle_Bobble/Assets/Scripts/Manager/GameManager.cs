using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
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

    private void Start()
    {
        // 参照が空だったら検索して記憶しておく。
        if(_gameOverPanel == null)
        {
            _gameOverPanel = GameObject.Find("GameOverPanel");
        }
    }
}