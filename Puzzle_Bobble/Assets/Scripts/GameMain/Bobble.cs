using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// �A�̐F
/// </summary>
public enum BobbleColor
{
    None = -1,
    Blue,
    Red,
    Green,
    Yellow,
    Purple,
    Gray,
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
    /// �|�C���gUI�v���n�u
    /// </summary>
    [SerializeField] private GameObject _pointTextUIPrefab;

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
            ColorChange(_BobbleColor);
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
        [SerializeField] public Texture[] _idleTexture;   // �ʏ�A�j���p�e�N�X�`���w��
        [SerializeField] public Texture[] _deleteTexture;   // ���ŃA�j���p�e�N�X�`��
    }
    [SerializeField] BobbleSprite _bobbleSprites;

    private SpriteRenderer sprRenderer;      // ���g�ɃA�^�b�`����Ă���SpriteRenderer�R���|�[�l���g
    private OverrideSprite overrideSprite;   // OverrideSprite�R���|�[�l���g
    private Animator animator;

    public BobbleNumber bobbleNumber = new BobbleNumber(); // �s�Ɨ�ԍ�

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();       // SpriteRenderer�擾
        overrideSprite = GetComponent<OverrideSprite>();    // OverrideSprite�擾
        animator = GetComponent<Animator>();                // Animator�擾

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
        if (!GameManager.Instance.isBobbleFalloutGameOverZone && !GameManager.Instance.isBobbleDeleting)
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
        overrideSprite.overrideTexture = _bobbleSprites._idleTexture[(int)color];
        gameObject.name = color.ToString();
    }

    /// <summary>
    /// �j�󏈗�
    /// </summary>
    public void BobbleDestroy()
    {
        StartCoroutine(nameof(SelfDestroyProcess));
    }

    // �j�󏈗��R���[�`��
    private IEnumerator SelfDestroyProcess()
    {
        Vector3 scrennPos = Camera.main.WorldToScreenPoint(transform.position);

        GameObject text = Instantiate(_pointTextUIPrefab, scrennPos, Quaternion.identity) as GameObject;
        text.GetComponent<Text>().text = ScoreManager.Instance.NowDeletePoint.ToString();
        text.transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        animator.SetBool("Delete", true);
        overrideSprite.overrideTexture = _bobbleSprites._deleteTexture[(int)_bobbleColor];
        yield return new WaitForSeconds(0.5f);  // �A�j���N���b�v�̒������҂�

        Destroy(gameObject);
        //Destroy(text);
    }

}
