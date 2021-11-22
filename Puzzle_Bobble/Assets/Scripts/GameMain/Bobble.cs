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
    None = -1,
    Blue,
    Gray,
    Green,
    Purple,
    Red,
    Yellow,
    Max,

    Delete = 99,
    NotConnectedCeil = 100
}

// �A�̍s�Ɨ�ԍ����������\����
[Serializable]
public struct BobbleNumber
{
    public int x, y;
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
    public BobbleColor _BobbleColor {
        set { _bobbleColor = value;
            sprRenderer.sprite = _bobbleSprites._sprite[(int)_bobbleColor];
            gameObject.name = _bobbleColor.ToString();
        }

        get { return _bobbleColor; }
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

    public BobbleNumber bobbleNumber = new BobbleNumber(); // �s�Ɨ�ԍ�

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
        // �A���܂��Q�[���I�[�o�[�]�[���ɒB���Ă��Ȃ���΁A���킶��Ɨ����Ă���
        if (!GameManager.Instance.isBobbleFalloutGameOverZone)
        {
            transform.Translate(-transform.up * 0.001f);

            if (transform.position.y <= -3.06f)
            {
                GameManager.Instance.isBobbleFalloutGameOverZone = true;
                Debug.Log(gameObject.name);
            }
        }
        
    }

    /// <summary>
    /// �s�Ɨ�ԍ���ݒ�
    /// </summary>
    /// <param name="row">�s</param>
    /// <param name="col">��</param>
    public void SetNumber(int row, int col)
    {
        bobbleNumber.x = row;
        bobbleNumber.y = col;
    }

    public BobbleNumber GetNumber()
    {
        return bobbleNumber;
    }

    /// <summary>
    /// �w�肵���F�̃X�v���C�g�ɕύX����
    /// </summary>
    /// <param name="color">�F</param>
    private void ColorChange(BobbleColor color)
    {
        sprRenderer.sprite = _bobbleSprites._sprite[(int)color];
        gameObject.name = _bobbleColor.ToString();
    }

}
