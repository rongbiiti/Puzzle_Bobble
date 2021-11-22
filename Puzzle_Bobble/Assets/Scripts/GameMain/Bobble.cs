using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 泡の色
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

// 泡の行と列番号が入った構造体
[Serializable]
public struct BobbleNumber
{
    public int x, y;
}

public class Bobble : MonoBehaviour
{
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
            sprRenderer.sprite = _bobbleSprites._sprite[(int)_bobbleColor];
            gameObject.name = _bobbleColor.ToString();
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
        [SerializeField] public Sprite[] _sprite;   // Assetのスプライト指定
        
    }
    [SerializeField] BobbleSprite _bobbleSprites;

    private SpriteRenderer sprRenderer;      // 自身にアタッチされているSpriteRendererコンポーネント

    public BobbleNumber bobbleNumber = new BobbleNumber(); // 行と列番号

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();       // SpriteRenderer取得

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
        sprRenderer.sprite = _bobbleSprites._sprite[(int)color];
        gameObject.name = _bobbleColor.ToString();
    }

}
