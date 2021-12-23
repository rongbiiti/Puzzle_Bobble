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

    // 落下速度上昇ゾーンに入った泡の数
    public int fallSpeedUpZoneContactCount;

    // デンジャーゾーンに入った泡の数
    public int dangerZoneContactCount;

    // 撃った玉が移動中か
    public bool shootedBobbleMoving;

    // 泡の削除演出中か
    public bool isBobbleDeleting;

    // 泡がゲームオーバーゾーンまで落ち切ったか
    public bool isBobbleFalloutGameOverZone;

    // 泡がもうすぐゲームオーバーゾーンに達しそうな「デンジャータイム中」か
    public bool isDangerTime;

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
        isBobbleFalloutGameOverZone = true;
        FindObjectOfType<PauseButton>().gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        // ゲームオーバーのUIをアクティブにする
        _gameOverPanel.SetActive(true);
    }
}