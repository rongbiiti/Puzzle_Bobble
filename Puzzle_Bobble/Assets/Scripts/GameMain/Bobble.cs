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
    /// ���Ŏ��G�t�F�N�g
    /// </summary>
    [SerializeField] private GameObject _deleteEffect;

    /// <summary>
    /// �������G�t�F�N�g
    /// </summary>
    [SerializeField] private GameObject _fallEffect;

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
    private bool isDisconnectFall;           // �V��ƌq����Ȃ��Ȃ�A���R������Ԃ�

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
    
    void Update()
    {
        // �A���܂��Q�[���I�[�o�[�]�[���ɒB���Ă��Ȃ���΁A���킶��Ɨ����Ă���
        if (!GameManager.Instance.isBobbleFalloutGameOverZone && !GameManager.Instance.isBobbleDeleting && !isDisconnectFall)
        {
            transform.Translate(-transform.up * 0.001f * GameManager.Instance.gameSpeed);

            if (transform.position.y <= -3.06f)
            {
                GameManager.Instance.isBobbleFalloutGameOverZone = true;
                StartCoroutine(nameof(GameOverCoroutine));
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
    public void BobbleDestroy(bool isFall, float delay)
    {
        StartCoroutine(SelfDestroyProcess(isFall, delay));
    }

    // �j�󏈗��R���[�`��
    private IEnumerator SelfDestroyProcess(bool isFall, float delay)
    {
        // �X�N���[�����W�v�Z
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // ���̖A�̓��_
        string scoreText = ScoreManager.Instance.NowDeletePoint.ToString("N0");

        yield return new WaitForSeconds(delay);

        // �A�̈ʒu�Ƀ|�C���g�\��
        GameObject text = Instantiate(_pointTextUIPrefab, screenPos, Quaternion.identity) as GameObject;
        text.GetComponent<DeletePointText>().SetDeletePointText(scoreText, screenPos);

        if (isFall)
        {
            // �A���V��ƌq����Ȃ��Ȃ�A�������Ă����Ƃ�
            isDisconnectFall = isFall;

            // Rigidbody�擾�ADynamic�ɐ؂�ւ��ďd�͂�������悤�ɂ���
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1.2f;

            float vecX = 1;

            if(UnityEngine.Random.Range(0, 2) >= 1)
            {
                // �A���E�����ɔ�΂����߁A1���o���獶�ɔ�΂��悤�ɂ���
                vecX = -1;
            }

            // �E�ォ����ɗ͂�������
            rb.AddForce(new Vector2(vecX, 1.5f) * UnityEngine.Random.Range(0.75f, 1.5f), ForceMode2D.Impulse);

            //�����蔻��I�t
            GetComponent<CircleCollider2D>().enabled = false;

            // �G�t�F�N�g
            Instantiate(_fallEffect, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(3f);  // �A�j���N���b�v�̒������҂�
        }
        else
        {
            // ���ŃA�j���[�V����������
            animator.SetBool("Delete", true);
            overrideSprite.overrideTexture = _bobbleSprites._deleteTexture[(int)_bobbleColor];
            // ���Đ�
            SoundManager.Instance.PlaySE(SE.BobbleDelete);
            // �G�t�F�N�g
            Instantiate(_deleteEffect, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);  // �A�j���N���b�v�̒������҂�
        }

        Destroy(gameObject);
        //Destroy(text);
    }

    /// <summary>
    /// �Q�[���I�[�o�[��̏����������x��Ď��s
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverCoroutine()
    {
        SoundManager.Instance.BGMFadeChange(BGM.Result, 3f);
        SoundManager.Instance.PlaySE(SE.GameOver);
        GameManager.Instance.InstantiateGameOverText();

        yield return new WaitForSeconds(3f);
        GameManager.Instance.GameOver();
    }

}
