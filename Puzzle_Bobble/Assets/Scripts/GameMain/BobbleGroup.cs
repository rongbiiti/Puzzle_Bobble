using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleGroup : MonoBehaviour
{
    /// <summary>
    /// 泡のプレハブ
    /// </summary>
    [SerializeField] private GameObject _bobblePrefab;

    public int rowNum; // 行番号
    public List<Bobble> bobbles = new List<Bobble>();    // 子要素のBobbleスクリプト
    public List<BobbleColor> bobbleColors = new List<BobbleColor>();    // 子要素の泡の色

    private int childBobbleCount;   // 子要素の泡の数
    private float[] _firstBobblesPosX = { -2.504494f, -2.224494f };  // 最初の生成する泡のX座標。左端
    private float _bobblesSizeX = 0.56f;

    void Start()
    {
        // 子要素の泡の設定
        //CreateChiledBobbles();

        // オブジェクトの名前に行番号を追加する
        gameObject.name = rowNum + "Row";
    }
    
    // 行を変える
    public void ChangeRowNum(int newRowNum)
    {
        rowNum = newRowNum;
        gameObject.name = rowNum + "Row";

        foreach (var b in bobbles)
        {
            if (b == null) continue;
            b.SetNumber(b.bobbleNumber.x, newRowNum);
        }
    }

    // 泡をリセットする
    public void ClearBobbleColors()
    {
        // 一次元目を確保
        for (int i = 0; i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (rowNum % 2); i++)
        {
            bobbles.Add(null);
            bobbleColors.Add(BobbleColor.None);
        }
    }

    /// <summary>
    /// 泡を生成する
    /// </summary>
    public void CreateChiledBobbles()
    {
        int i;

        // 泡を生成
        for (i = 0; i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (rowNum % 2); i++)
        {
            GameObject b = Instantiate(_bobblePrefab, new Vector3(_firstBobblesPosX[rowNum % 2] + _bobblesSizeX * i, transform.position.y, 0), Quaternion.identity);
            b.transform.parent = transform;
            bobbles.Insert(i, b.GetComponent<Bobble>());
        }

        // 子要素の泡たちに行と列番号を設定
        i = 0;
        foreach (var b in bobbles)
        {
            b.SetNumber(i, rowNum);       // 行と列番号設定


            if (Probability.Lottery(BobbleArrayManager.Instance.PROB_SAME_COL_BOBBLE_COLOR) && // 一定の確率で
                0 < rowNum &&                                                                         // 1列目以上で
                i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (1 + rowNum % 2) &&                // 右端でなく                                                               
                BobbleColor.Blue <= BobbleArrayManager.Instance.bobbles[rowNum + 1][i] &&         // 生成可能な色の範囲なら
                BobbleArrayManager.Instance.bobbles[rowNum + 1][i] <= BobbleColor.Yellow)
            {
                
                // 同じ列で一つ前の行の泡と同じ色の泡を生成する
                b._BobbleColor = BobbleArrayManager.Instance.bobbles[rowNum + 1][i];
            }
            else if(Probability.Lottery(BobbleArrayManager.Instance.PROB_SAME_ROW_BOBBLE_COLOR) && 0 < i)
            {
                // 一定の確率で、1個左の泡と同じ色の泡を生成する
                // 1個目だったらこれは行わない
                b._BobbleColor = bobbleColors[i - 1];
            }
            else
            {
                // 泡の色をランダムに変更
                b._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);
            }
            
            bobbleColors.Add(b._BobbleColor);// 泡の色を取得しておく
            childBobbleCount++;             // 泡の数を記録

            i++;
        }
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
            return bobbles[x]._BobbleColor;
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
        // 正しく設定できる色の範囲外が指定されたら処理をスキップ
        if (color <= BobbleColor.None || BobbleColor.Max <= color) return;

        bobbles[x]._BobbleColor = color;
        bobbleColors[x] = color;
    }

    /// <summary>
    /// 泡を削除
    /// </summary>
    /// <param name="x"></param>
    public void DestroyChildBobble(int x, bool isFall, float delay)
    {
        //Destroy(bobbles[x].gameObject);     // オブジェクト破壊
        bobbles[x].BobbleDestroy(isFall, delay);
        //bobbles.Insert(x, null);             // 配列の中もnullにする
        bobbles[x] = null;
        bobbleColors[x] = BobbleColor.None; // 色を記憶してる配列の中もNoneに変える
        //bobbleColors.Insert(x, BobbleColor.None);
        childBobbleCount--;

        
    }
}
