using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BobbleArrayManager : SingletonMonoBehaviour<BobbleArrayManager>
{
    /// <summary>
    /// 泡グループのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] _bobbleGroupPrefab;

    /// <summary>
    /// 泡グループの最大数
    /// </summary>
    public int BOBBLE_ROW_MAX = 25;

    /// <summary>
    /// 泡の偶数行の個数
    /// </summary>
    public int BOBBLE_EVEN_SIZE = 10;

    /// <summary>
    /// Start時に泡グループを何行分生成するか
    /// </summary>
    [SerializeField] private int _onStartCreateBobbleRow = 15;

    // 泡を格納する配列
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();

    // 作業用配列
    private List<List<BobbleColor>> workBobbles = new List<List<BobbleColor>>();

    // 泡グループオブジェクトの配列
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();

    private float bobbleCreateStartPosY = 11.08f;        // 一番最初に生成する泡グループのY座標
    private float bobbleCreateIncreaseYPos = 0.56f;      // 2個目以降の泡グループ生成時にこの値分Y座標を上にずらして生成

    void Start()
    {
        // 一次元目を確保
        for (int i = 0; i < BOBBLE_ROW_MAX; i++)
        {
            bobbles.Add(new List<BobbleColor>());
            workBobbles.Add(new List<BobbleColor>());
        }

        // 配列を初期化
        for (int i = BOBBLE_ROW_MAX - 1; 0 <= i; i--)
        {
            // 泡グループ生成処理
            CreateBobbleGroupObject(i);
        }

    }

    /// <summary>
    /// 泡グループのオブジェクト生成
    /// </summary>
    /// <param name="rowNum">行番号</param>
    private void CreateBobbleGroupObject(int rowNum)
    {
        // 泡グループのオブジェクトを生成
        // 今はとりあえず偶数か奇数かだけ
        GameObject bobbleG = Instantiate(_bobbleGroupPrefab[rowNum % 2]);

        // 生成する位置のY座標を前に生成したものと重ならないように調整
        bobbleG.transform.position += new Vector3(0, bobbleCreateStartPosY - bobbleCreateIncreaseYPos * rowNum, 0);

        // 泡グループのBobbleGroupコンポーネントに行番号を設定
        BobbleGroup bobbleGroup = bobbleG.GetComponent<BobbleGroup>();

        bobbleGroup.rowNum = rowNum;

        if(rowNum <= _onStartCreateBobbleRow - 1)
        {
            bobbleGroup.CreateChiledBobbles();
        }
        else
        {
            bobbleGroup.ClearBobbleColors();
        }

        // オブジェクトをListに追加
        bobbleGroups.Insert(0, bobbleG.GetComponent<BobbleGroup>());

        // 偶数と奇数でサイズが違うので分ける
        if (rowNum % 2 == 0)
        {
            // 偶数
            for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
            {
                // 配列に泡の情報を追加
                bobbles[rowNum] = bobbleGroup.bobbleColors;
                
            }

        }
        else
        {
            // 奇数
            for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
            {
                bobbles[rowNum] = bobbleGroup.bobbleColors;
            }

        }

        // 作業用配列に記録しておく
        //workBobbles = bobbles;
        Debug.Log(string.Join(", ", bobbles[rowNum].Select(obj => obj.ToString())));
        //workBobbles = bobbles.DeepCopy();
        
    }

    /// <summary>
    /// 指定した行と同じ行番号を持つ泡グループのTransformを取得
    /// </summary>
    /// <param name="y">行番号</param>
    /// <returns>泡グループのTransform</returns>
    public Transform GetSameRowBobbleGroup(int x, int y, BobbleColor color, Bobble bobbleComponent)
    {
        foreach(var g in bobbleGroups)
        {
            if(g.rowNum == y)
            {
                g.bobbles[x] = bobbleComponent;
                g.bobbleColors[x] = color;
                bobbles[y] = g.bobbleColors;
                return g.transform;
            }
        }

        return null;
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
        // 参照渡しで泡の消す数をカウントする用
        int deleteCount = 0;

        // 3つ以上繋がってなかったときにマークをリセットできるように、配列を保存しておく
        workBobbles = bobbles.DeepCopy();

        // 発射した玉とヒットした泡の色が同じか
        if (bobbles[y][x] == color)
        {
            Debug.Log("ヒットした泡の色 : " + bobbles[y][x]);

            // 玉と同じ色だったので、繋がっているものをマークしていく。
            FloodFill(x, y, color, ref deleteCount);

            Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));

            if(3 <= deleteCount)
            {
                // 3つ以上繋がっていたので、削除作業
                BobbleDelete(x, y);
            }
            else
            {
                // 3つ繋がってなかったので、配列をマーク作業する前の状態に戻す
                bobbles = workBobbles.DeepCopy();
                Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
            }
            return true;
        }
        else
        {
            Debug.Log("ヒットした泡の色 : " + bobbles[y][x]);
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
    private void FloodFill(int x, int y, BobbleColor color, ref int count)
    {
        
        // 左端が右端を超えたらリターン
        // 奇数列だった場合y % 2の答えが1になるので、互い違いを考慮することができる
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // 天辺か底辺を超えたらリターン
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        if(bobbles[y][x] == color)
        {
            // 動作確認用　色を赤にする
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // 削除可能なので、カウントをインクリメント
            count++;
            // 削除する泡としてマークする
            bobbles[y][x] = BobbleColor.Delete;
            Debug.Log("カウント" + count);

            // 再帰処理
            // 偶数行か奇数行かでアクセスする泡の位置を変える
            if(y % 2 == 0)
            {
                // 偶数行
                FloodFill(x - 1, y    , color, ref count);
                FloodFill(x - 1, y - 1, color, ref count);
                FloodFill(x    , y - 1, color, ref count);

                FloodFill(x + 1, y    , color, ref count);
                FloodFill(x    , y + 1, color, ref count);
                FloodFill(x - 1, y + 1, color, ref count);
            }
            else
            {
                // 奇数行
                FloodFill(x - 1, y    , color, ref count);
                FloodFill(x    , y - 1, color, ref count);
                FloodFill(x + 1, y + 1, color, ref count);

                FloodFill(x + 1, y    , color, ref count);
                FloodFill(x + 1, y + 1, color, ref count);
                FloodFill(x    , y + 1, color, ref count);
            }            
        }
    }

    /// <summary>
    /// マークつけた泡を削除
    /// </summary>
    /// <param name="x">行</param>
    /// <param name="y">列</param>
    private void BobbleDelete(int x, int y)
    {
        // 左端が右端を超えたらリターン
        // 奇数列だった場合y % 2の答えが1になるので、互い違いを考慮することができる
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // 天辺か底辺を超えたらリターン
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        // マークされたものなら、削除
        if (bobbles[y][x] == BobbleColor.Delete)
        {
            bobbleGroups[y].DestroyChildBobble(x);
            bobbles[y][x] = BobbleColor.None;
            Debug.Log("削除！！");

            // 再帰処理
            // 偶数行か奇数行かでアクセスする泡の位置を変える
            if (y % 2 == 0)
            {
                // 偶数行
                BobbleDelete(x - 1, y    );
                BobbleDelete(x - 1, y - 1);
                BobbleDelete(x    , y - 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x    , y + 1);
                BobbleDelete(x - 1, y + 1);
            }
            else
            {
                // 奇数行
                BobbleDelete(x - 1, y    );
                BobbleDelete(x    , y - 1);
                BobbleDelete(x + 1, y + 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x + 1, y + 1);
                BobbleDelete(x    , y + 1);
            }
        }
    }
}
