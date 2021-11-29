using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 泡の色
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

// 泡の行と列番号が入った構造体
[Serializable]
public struct BobbleNumber
{
    public int x, y;
}

public class Bobble : MonoBehaviour
{
    /// <summary>
    /// ポイントUIプレハブ
    /// </summary>
    [SerializeField] private GameObject _pointTextUIPrefab;

    /// <summary>
    /// 泡の色
    /// </summary>
    [SerializeField]
    private BobbleColor _bobbleColor = BobbleColor.Blue;

    /// <summary>
    /// 他オブジェクトから色を変更させる
    /// </summary>
    public BobbleColor _BobbleColor {
        set { _bobbleColor = value;
            ColorChange(_BobbleColor);
        }

        get { return _bobbleColor; }
    }

    /// <summary>
    /// Awake時に_bobbleColorで指定した色のスプライトに変更するか
    /// </summary>
    [SerializeField]
    private bool _onAwakeColorChange = false;

    /// <summary>
    /// 泡のスプライトの参照
    /// </summary>
    [Serializable]
    private class BobbleSprite
    {
        [SerializeField] public Texture[] _idleTexture;   // 通常アニメ用テクスチャ指定
        [SerializeField] public Texture[] _deleteTexture;   // 消滅アニメ用テクスチャ
    }
    [SerializeField] BobbleSprite _bobbleSprites;

    private SpriteRenderer sprRenderer;      // 自身にアタッチされているSpriteRendererコンポーネント
    private OverrideSprite overrideSprite;   // OverrideSpriteコンポーネント
    private Animator animator;
    private bool isDisconnectFall;           // 天井と繋がれなくなり、自然落下状態か

    public BobbleNumber bobbleNumber = new BobbleNumber(); // 行と列番号

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();       // SpriteRenderer取得
        overrideSprite = GetComponent<OverrideSprite>();    // OverrideSprite取得
        animator = GetComponent<Animator>();                // Animator取得

        // inspectorで指定した_bobbleColorに対応するスプライトに変更する
        if (_onAwakeColorChange)
        {
            ColorChange(_bobbleColor);  // スプライト変更
        }
        
    }

    void Start()
    {

    }
    
    void Update()
    {
        // 泡がまだゲームオーバーゾーンに達していなければ、じわじわと落ちていく
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
    /// 行と列番号を設定
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
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
    /// 指定した色のスプライトに変更する
    /// </summary>
    /// <param name="color">色</param>
    private void ColorChange(BobbleColor color)
    {
        overrideSprite.overrideTexture = _bobbleSprites._idleTexture[(int)color];
        gameObject.name = color.ToString();
    }

    /// <summary>
    /// 破壊処理
    /// </summary>
    public void BobbleDestroy(bool isFall)
    {
        StartCoroutine(SelfDestroyProcess(isFall));
    }

    // 破壊処理コルーチン
    private IEnumerator SelfDestroyProcess(bool isFall)
    {
        // スクリーン座標計算
        Vector3 scrennPos = Camera.main.WorldToScreenPoint(transform.position);

        // 泡の位置にポイント表示
        GameObject text = Instantiate(_pointTextUIPrefab, scrennPos, Quaternion.identity) as GameObject;
        text.GetComponent<Text>().text = ScoreManager.Instance.NowDeletePoint.ToString();
        text.transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        if (isFall)
        {
            isDisconnectFall = isFall;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;
            float vecX = 1;
            if(UnityEngine.Random.Range(0, 2) >= 1)
            {
                // 泡を右か左に飛ばすため、1が出たら左に飛ばすようにする
                vecX = -1;
            }
            rb.AddForce(new Vector2(vecX, 1.5f) * UnityEngine.Random.Range(0.75f, 1.5f), ForceMode2D.Impulse);

            GetComponent<CircleCollider2D>().enabled = false;
            yield return new WaitForSeconds(3f);  // アニメクリップの長さ分待つ
        }
        else
        {
            animator.SetBool("Delete", true);
            overrideSprite.overrideTexture = _bobbleSprites._deleteTexture[(int)_bobbleColor];
            yield return new WaitForSeconds(0.5f);  // アニメクリップの長さ分待つ
        }
       

        Destroy(gameObject);
        //Destroy(text);
    }

    /// <summary>
    /// ゲームオーバー後の処理を少し遅れて実行
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.GameOver();
    }

}
