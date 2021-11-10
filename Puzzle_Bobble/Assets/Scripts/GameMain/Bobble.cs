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
    /// 泡の色
    /// </summary>
    [SerializeField]
    private BobbleColor _bobbleColor = BobbleColor.Blue;

    /// <summary>
    /// 他オブジェクトから色を変更させる
    /// </summary>
    public BobbleColor BobbleColor {
        set { _bobbleColor = value;
            sprRenderer.sprite = _bobbleSprites._sprite[(int)_bobbleColor];
        }
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
        
    }

    /// <summary>
    /// 指定した色のスプライトに変更する
    /// </summary>
    /// <param name="color">色</param>
    private void ColorChange(BobbleColor color)
    {
        sprRenderer.sprite = _bobbleSprites._sprite[(int)color];
    }

}
