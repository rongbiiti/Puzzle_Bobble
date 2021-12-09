using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    /// ���F�A�폜�R���{���̏��
    /// </summary>
    [SerializeField] private int _deleteComboMax = 20;

    /// <summary>
    /// �A�����R���{���̏��
    /// </summary>
    [SerializeField] private int _fallComboMax = 20;

    /// <summary>
    /// �R���{���ɉ������{����ݒ肷��e�[�u��
    /// </summary>
    [SerializeField] private int[] _comboTable;

    /// <summary>
    /// �R���{��
    /// </summary>
    private int combo;
    public int Combo {
        set { combo = value;
            if (combo > _comboTable.Length - 1)
            {
                // �R���{�{���e�[�u���̗v�f�����z���Ȃ��悤�ɂ���
                combo = _comboTable.Length - 1;
            }
        }
        get { return combo; }
    }

    /// <summary>
    /// ���F�̖A����x�ɏ������R���{��
    /// </summary>
    private int deleteCombo;
    public int DeleteCombo {
        set { deleteCombo = value; }
        get { return deleteCombo; }
    }

    /// <summary>
    /// ��x�ɗ��Ƃ����A�̃R���{��
    /// </summary>
    private int fallCombo;
    public int FallCombo {
        set { fallCombo = value; }
        get { return fallCombo; }
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
    /// 1�V���b�g�Ŋl�������|�C���g�̍��v
    /// </summary>
    private int nowTurnPoint;
    public int NowTurnPoint {
        set { nowTurnPoint = value; }
        get { return nowTurnPoint; }
    }

    /// <summary>
    /// �X�R�A�����Z
    /// </summary>
    /// <param name="isDelete">���F�̍폜����</param>
    public void AddNowTurnPoint(bool isDelete)
    {
        if (isDelete)
        {
            deleteCombo++;

            if(_deleteComboMax < deleteCombo)
            {
                deleteCombo = _deleteComboMax;
            }

            // �q�����Ă�A���폜�����Ƃ��̓R���{�����K�p�����
            nowDeletePoint = _basePoint * deleteCombo * _comboTable[combo];
            
        }
        else
        {
            fallCombo++;

            if (_fallComboMax < fallCombo)
            {
                fallCombo = _fallComboMax;
            }

            // �������A���폜�����Ƃ��̓x�[�X�|�C���g �~ (2^fallCombo)
            nowDeletePoint = _basePoint * (int)Mathf.Pow(2, fallCombo);
        }

        nowTurnPoint += nowDeletePoint;

    }

    /// <summary>
    /// 1�V���b�g�Ŋl�������|�C���g���X�R�A�ɔ��f������
    /// </summary>
    public void AddNowTurnPointToScore()
    {
        score += nowTurnPoint;
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

    
}
