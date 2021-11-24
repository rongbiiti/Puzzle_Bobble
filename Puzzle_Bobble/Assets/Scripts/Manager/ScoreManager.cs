using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    /// <summary>
    /// �X�R�A
    /// </summary>
    private int score;
    public int Score {
        get { return score; }
    }

    /// <summary>
    /// ��{�ƂȂ链�_
    /// </summary>
    [SerializeField] private int _basePoint = 100;

    /// <summary>
    /// �R���{���ɉ������{����ݒ肷��e�[�u��
    /// </summary>
    [SerializeField] private int[] _comboTable;

    /// <summary>
    /// �R���{��
    /// </summary>
    private int combo;
    public int Combo {
        set { combo = value; }
        get { return combo; }
    }

    /// <summary>
    /// ���O�̊l���|�C���g
    /// </summary>
    private int nowDeletePoint;
    public int NowDeletePoint
    {
        get { return nowDeletePoint; }
    }

    /// <summary>
    /// �X�R�A�����Z
    /// </summary>
    public void AddScore(bool iscombo)
    {
        if (iscombo)
        {
            if(combo <= _comboTable.Length - 1)
            {
                // �R���{�{���e�[�u���̗v�f�����z���Ȃ��悤�ɂ���
                combo++;
            }

            // �q�����Ă�A���폜�����Ƃ��̓R���{�����K�p�����
            score += _basePoint * _comboTable[combo];
            nowDeletePoint = _basePoint * _comboTable[combo];
        }
        else
        {
            // �������A���폜�����Ƃ��̓R���{�����K�p����Ȃ�
            score += _basePoint * _comboTable[0];
            nowDeletePoint = _basePoint * _comboTable[0];
        }
        
    }

    /// <summary>
    /// �q�G�����L�[��ɂ���Canvas���擾���Ă���
    /// </summary>
    private Canvas canvas;
    public Canvas GetCanvas()
    {
        return canvas;
    }

    private GUIStyle style; // OnGUI�Ńf�o�b�O�\���p

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        // �f�o�b�O�p
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
