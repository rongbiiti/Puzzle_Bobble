using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [SerializeField]
    private BobbleColor _bobbleColor = BobbleColor.Blue;

    [Serializable]
    private class BobbleSprite
    {
        [SerializeField] public Sprite[] _sprite;
        
    }
    [SerializeField] BobbleSprite _bobbleSprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _bobbleSprites._sprite[(int)_bobbleColor];
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
