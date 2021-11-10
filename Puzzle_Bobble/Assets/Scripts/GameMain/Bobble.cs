using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// �A�̐F
/// </summary>
public enum BobbleColor
{
    Blue,
    Gray,
    Green,
    Purple,
    Red,
    Yellow
}

public class Bobble : MonoBehaviour
{
    /// <summary>
    /// �A�̐F
    /// </summary>
    [SerializeField]
    private BobbleColor _bobbleColor = BobbleColor.Blue;

    /// <summary>
    /// ���I�u�W�F�N�g����F��ύX������
    /// </summary>
    public BobbleColor BobbleColor {
        set { _bobbleColor = value;
            sprRenderer.sprite = _bobbleSprites._sprite[(int)_bobbleColor];
        }
    }

    /// <summary>
    /// Awake����_bobbleColor�Ŏw�肵���F�̃X�v���C�g�ɕύX���邩
    /// </summary>
    [SerializeField]
    private bool _onAwakeColorChange = false;

    /// <summary>
    /// �A�̃X�v���C�g�̎Q��
    /// </summary>
    [Serializable]
    private class BobbleSprite
    {
        [SerializeField] public Sprite[] _sprite;   // Asset�̃X�v���C�g�w��
        
    }
    [SerializeField] BobbleSprite _bobbleSprites;

    private SpriteRenderer sprRenderer;      // ���g�ɃA�^�b�`����Ă���SpriteRenderer�R���|�[�l���g

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();       // SpriteRenderer�擾

        // inspector�Ŏw�肵��_bobbleColor�ɑΉ�����X�v���C�g�ɕύX����
        if (_onAwakeColorChange)
        {
            ColorChange(_bobbleColor);  // �X�v���C�g�ύX
        }
        
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    /// <summary>
    /// �w�肵���F�̃X�v���C�g�ɕύX����
    /// </summary>
    /// <param name="color">�F</param>
    private void ColorChange(BobbleColor color)
    {
        sprRenderer.sprite = _bobbleSprites._sprite[(int)color];
    }

}
