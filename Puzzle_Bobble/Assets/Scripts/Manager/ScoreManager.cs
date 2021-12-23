using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    /// <summary>
    /// スコア
    /// </summary>
    private int score;
    public int Score {
        get { return score; }
    }

    /// <summary>
    /// 基本となる得点
    /// </summary>
    [SerializeField] private int _basePoint = 100;

    /// <summary>
    /// 同色泡削除コンボ数の上限
    /// </summary>
    [SerializeField] private int _deleteComboMax = 20;

    /// <summary>
    /// 泡落下コンボ数の上限
    /// </summary>
    [SerializeField] private int _fallComboMax = 20;

    /// <summary>
    /// コンボ数に応じた倍率を設定するテーブル
    /// </summary>
    [SerializeField] private int[] _comboTable;

    /// <summary>
    /// デンジャータイム中のスコアボーナス倍率
    /// </summary>
    [SerializeField] private float _dangerTimeScoreBonusRate = 1.5f;
    public float DangerTimeScoreBonusRate {
        get { return _dangerTimeScoreBonusRate; }
    }

    /// <summary>
    /// コンボ数
    /// </summary>
    private int combo;
    public int Combo {
        set { combo = value; }
        get { return combo; }
    }

    /// <summary>
    /// 同色の泡を一度に消したコンボ数
    /// </summary>
    private int deleteCombo;
    public int DeleteCombo {
        set { deleteCombo = value; }
        get { return deleteCombo; }
    }

    /// <summary>
    /// 一度に落とした泡のコンボ数
    /// </summary>
    private int fallCombo;
    public int FallCombo {
        set { fallCombo = value; }
        get { return fallCombo; }
    }

    /// <summary>
    /// 直前の獲得ポイント
    /// </summary>
    private int nowDeletePoint;
    public int NowDeletePoint
    {
        
        get { return nowDeletePoint; }
    }

    /// <summary>
    /// 1ショットで獲得したポイントの合計
    /// </summary>
    private int nowTurnPoint;
    public int NowTurnPoint {
        set { nowTurnPoint = value; }
        get { return nowTurnPoint; }
    }

    

    /// <summary>
    /// スコアを加算
    /// </summary>
    /// <param name="isDelete">同色の削除中か</param>
    public void AddNowTurnPoint(bool isDelete)
    {
        if (isDelete)
        {
            deleteCombo++;

            if(_deleteComboMax < deleteCombo)
            {
                deleteCombo = _deleteComboMax;
            }

            // 繋がってる泡を削除したときはコンボ数が適用される
            nowDeletePoint = _basePoint * deleteCombo * _comboTable[Mathf.Clamp(combo, 0, _comboTable.Length - 1)];
            
        }
        else
        {
            fallCombo++;

            if (_fallComboMax < fallCombo)
            {
                fallCombo = _fallComboMax;
            }

            // 浮いた泡を削除したときはベースポイント × (2^fallCombo)
            nowDeletePoint = _basePoint * (int)Mathf.Pow(2, fallCombo);
        }

        nowTurnPoint += nowDeletePoint;

    }

    /// <summary>
    /// 1ショットで獲得したポイントをスコアに反映させる
    /// </summary>
    public void AddNowTurnPointToScore()
    {
        // デンジャータイム中ならスコア倍率適用
        if (GameManager.Instance.isDangerTime)
        {
            nowTurnPoint = (int)(nowTurnPoint * _dangerTimeScoreBonusRate);
        }

        score += nowTurnPoint;
    }

    /// <summary>
    /// ヒエラルキー上にあるCanvasを取得しておく
    /// </summary>
    private Canvas canvas;
    public Canvas GetCanvas()
    {
        return canvas;
    }

    private GUIStyle style; // OnGUIでデバッグ表示用

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        // デバッグ用
        style = new GUIStyle();
        style.fontSize = 50;        
    }


}
