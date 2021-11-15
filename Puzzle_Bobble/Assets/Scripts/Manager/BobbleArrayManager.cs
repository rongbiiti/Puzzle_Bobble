using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleArrayManager : SingletonMonoBehaviour<BobbleArrayManager>
{
    /// <summary>
    /// 泡グループのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] _bobbleGroupPrefab;

    /// <summary>
    /// 泡グループの最大数
    /// </summary>
    public const int BOBBLE_ROW_MAX = 15;

    /// <summary>
    /// 泡の偶数行の個数
    /// </summary>
    public const int BOBBLE_EVEN_SIZE = 10;

    // 泡を格納する配列
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();
    

    private float bobbleCreateStartPosY = 3.24f;        // 一番最初に生成する泡グループのY座標
    private float bobbleCreateIncreaseYPos = 0.56f;      // 2個目以降の泡グループ生成時にこの値分Y座標を上にずらして生成

    void Start()
    {
        // 一次元目を確保
        for (int i = 0; i < BOBBLE_ROW_MAX; i++) bobbles.Add(new List<BobbleColor>());

        // 配列を初期化
        for (int i = 0; i < BOBBLE_ROW_MAX; i++)
        {
            // 偶数と奇数でサイズが違うので分ける
            if (i % 2 == 0)
            {
                // 偶数
                for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
                {
                    // 配列に泡の情報を追加
                    bobbles[i].Add(BobbleColor.Blue);
                }
                
            }
            else
            {
                // 奇数
                for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
                {
                    bobbles[i].Add(BobbleColor.Gray);
                }
                
            }

            // 泡グループ生成処理
            CreateBobbleGroupObject(i);
        }

        
    }

    /// <summary>
    /// 泡グループのオブジェクト生成と諸々の処理
    /// </summary>
    /// <param name="rowNum">行番号</param>
    private void CreateBobbleGroupObject(int rowNum)
    {
        // 泡グループのオブジェクトを生成
        // 今はとりあえず偶数か奇数かだけ
        GameObject bobbleG = Instantiate(_bobbleGroupPrefab[rowNum % 2]);

        // 生成する位置のY座標を前に生成したものと重ならないように調整
        bobbleG.transform.position += new Vector3(0, bobbleCreateStartPosY + bobbleCreateIncreaseYPos * rowNum, 0);

        // 泡グループのBobbleGroupコンポーネントに行番号を設定
        bobbleG.GetComponent<BobbleGroup>().rowNum = rowNum;

        // オブジェクトをListに追加
        bobbleGroups.Add(bobbleG.GetComponent<BobbleGroup>());
    }

    /// <summary>
    /// 泡を消す事ができるかチェックする
    /// </summary>
    /// <param name="x">ヒットした泡の行</param>
    /// <param name="y">ヒットした泡の列</param>
    /// <param name="color">玉の色</param>
    /// <returns></returns>
    public bool BobbleDeleteCheck(int x, int y, BobbleColor color)
    {
        if(bobbles[y][x] == color)
        {
            Debug.Log(bobbles[y][x]);
            FloodFill(x, y, color);
            return true;
        }
        else
        {
            Debug.Log(bobbles[y][x]);
            return false;
        }
    }

    /// <summary>
    /// フラッドフィル
    /// ヒットした泡と隣接していて玉と同じ色である泡を探索する
    /// </summary>
    /// <param name="x">行</param>
    /// <param name="y">列</param>
    /// <param name="color">色</param>
    private void FloodFill(int x, int y, BobbleColor color)
    {
        
        // 左端が右端を超えたらリターン
        // 奇数列だった場合y % 2の答えが1になるので、互い違いを考慮することができる
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // 天辺か底辺を超えたらリターン
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        if(bobbleGroups[y].GetChildBobbleColor(x) == color)
        {
            // 動作確認用　色を赤にする
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // 泡を削除する
            bobbleGroups[y].DestroyChildBobble(x);

            // 再帰処理
            FloodFill(x + 1, y    , color);
            FloodFill(x    , y + 1, color);
            FloodFill(x - 1, y    , color);
            FloodFill(x    , y - 1, color);
        }
    }
}
