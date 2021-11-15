using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleGroup : MonoBehaviour
{
    public int rowNum; // 行番号
    public Bobble[] bobbles;    // 子要素のBobbleスクリプト

    void Start()
    {
        // 子要素の泡たちに行と列番号を設定
        bobbles = GetComponentsInChildren<Bobble>();

        int i = 0;
        foreach(var b in bobbles)
        {
            b.SetNumber(i++, rowNum);
        }
    }
    
    void Update()
    {
        
    }

    /// <summary>
    /// 子要素の泡の色を取得
    /// </summary>
    /// <param name="x">何個目の泡か</param>
    /// <returns></returns>
    public BobbleColor GetChildBobbleColor(int x)
    {
        if (bobbles[x] != null)
        {
            return bobbles[x].BobbleColor;
        }
        else
        {
            return BobbleColor.None;
        }
        
    }

    /// <summary>
    /// 子要素の泡の色を変更する
    /// </summary>
    /// <param name="x">何個目の泡か</param>
    /// <param name="color">変更後の色</param>
    public void SetChildBobbleColor(int x, BobbleColor color)
    {
        bobbles[x].BobbleColor = color;
    }

    /// <summary>
    /// 泡を削除
    /// </summary>
    /// <param name="x"></param>
    public void DestroyChildBobble(int x)
    {
        Destroy(bobbles[x].gameObject);
        bobbles[x] = null;
    }
}
