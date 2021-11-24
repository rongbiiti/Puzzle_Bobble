using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// コンボ数に応じた倍率を設定するテーブル
    /// </summary>
    [SerializeField] private int[] _comboTable;

    /// <summary>
    /// コンボ数
    /// </summary>
    private int combo;
    public int Combo {
        set { combo = value; }
        get { return combo; }
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
    /// スコアを加算
    /// </summary>
    public void AddScore(bool iscombo)
    {
        if (iscombo)
        {
            if(combo <= _comboTable.Length - 1)
            {
                // コンボ倍率テーブルの要素数を越えないようにする
                combo++;
            }

            // 繋がってる泡を削除したときはコンボ数が適用される
            score += _basePoint * _comboTable[combo];
            nowDeletePoint = _basePoint * _comboTable[combo];
        }
        else
        {
            // 浮いた泡を削除したときはコンボ数が適用されない
            score += _basePoint * _comboTable[0];
            nowDeletePoint = _basePoint * _comboTable[0];
        }
        
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

    private void OnGUI()
    {
        var sw = Screen.width;
        var sh = Screen.height;

        GUI.Label(new Rect(50, sh - 80, 500, 500), "SCORE : " + score + "  COMBO : " + combo, style);
    }
}
