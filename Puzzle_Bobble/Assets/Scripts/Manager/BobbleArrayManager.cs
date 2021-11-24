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
    public static int BOBBLE_ROW_MAX = 27;

    /// <summary>
    /// 泡の偶数行の個数
    /// </summary>
    public int BOBBLE_EVEN_SIZE = 10;

    /// <summary>
    /// Start時に泡グループを何行分生成するか
    /// </summary>
    [SerializeField] private int _onStartCreateBobbleRow = 23;

    // 泡を格納する配列
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();

    // 作業用配列
    private List<List<BobbleColor>> workBobbles = new List<List<BobbleColor>>();

    // 泡グループオブジェクトの配列
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();

    private float bobbleCreateStartPosY = 9.52f;        // 一番最初に生成する泡グループのY座標
    private float bobbleCreateIncreaseYPos = 0.52f;      // 2個目以降の泡グループ生成時にこの値分Y座標を上にずらして生成
    private float nextBobbleGroupCreateWaitTime = 1060 * 0.0167f; // 次の泡グループ生成までの待機時間
    private float waitTime;


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

    private void Update()
    {
        // ゲームオーバーになってたら処理しない
        if (GameManager.Instance.isBobbleFalloutGameOverZone) return;

        // 泡の削除演出中
        if (GameManager.Instance.isBobbleDeleting)
        {
            

            return;
        }

        // 泡生成待機時間を加算
        waitTime += Time.deltaTime;

        // 待機時間を越えたら泡を生成
        if(nextBobbleGroupCreateWaitTime <= waitTime)
        {
            AddNewBobbleGroup();
            waitTime = 0;
        }
    }


    private void AddNewBobbleGroup()
    {
        foreach (var bg in bobbleGroups)
        {
            bg.ChangeRowNum(bg.rowNum + 2);
        }

        Destroy(bobbleGroups[bobbleGroups.Count - 1].gameObject);
        Destroy(bobbleGroups[bobbleGroups.Count - 2].gameObject);

        //bobbles.RemoveRange(bobbles.Count - 2, 2);

        bobbles.RemoveAt(bobbles.Count - 1);
        bobbles.RemoveAt(bobbles.Count - 1);

        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);
        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);
        //bobbleGroups.RemoveRange(bobbleGroups.Count - 2, 2);

        bobbles.Insert(0, new List<BobbleColor>());
        bobbles.Insert(0, new List<BobbleColor>());

        CreateBobbleGroupObject(1);
        CreateBobbleGroupObject(0);

        int y = 0;
        foreach(var b in bobbles)
        {
            Debug.Log(y++ + string.Join(", ", b.Select(obj => obj.ToString())));
        }

        Debug.Log("bobblesの数 : " + y);
        Debug.Log("bobbleGroupsの数 : " + bobbleGroups.Count);

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

        //bobbles[rowNum].Clear();

        // 偶数と奇数でサイズが違うので分ける
        if (rowNum % 2 == 0)
        {
            // 偶数
            for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
            {
                // 配列に泡の情報を追加
                //bobbles[rowNum] = bobbleGroup.bobbleColors;
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);

            }

        }
        else
        {
            // 奇数
            for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
            {
                //bobbles[rowNum] = bobbleGroup.bobbleColors;
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);
            }

        }

        //bobbles.Insert(rowNum, bobbleGroup.bobbleColors);

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
                // 管理配列の情報を更新
                g.bobbles[x] = bobbleComponent;
                g.bobbleColors[x] = color;
                //g.bobbleColors.Insert(x, color);
                //bobbles[y] = g.bobbleColors;
                BobbleColor tmp = g.bobbleColors[x];
                bobbles[y][x] = tmp;

                //bobbles[y].RemoveAt(x);
                //bobbles[y].Insert(x, g.bobbleColors[x]);
                //bobbles.Insert(y, g.bobbleColors);

                Debug.Log(bobbleComponent.name + "は" + g.name + "の子要素になった。 色 : " + color);

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

        Debug.Log(string.Join(", ", bobbles[y - 1].Select(obj => obj.ToString())));

        // 発射した玉とヒットした泡の色が同じか
        if (bobbles[y][x] == color)
        {
            Debug.Log("ヒットした泡の色 : " + bobbles[y][x]);

            // 玉と同じ色だったので、繋がっているものをマークしていく。
            FloodFill(x, y, color, ref deleteCount);

            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            

            if(3 <= deleteCount)
            {
                // 3つ以上繋がっていたので、削除作業
                GameManager.Instance.isBobbleDeleting = true;
                StartCoroutine(BobbleDeleteCoroutine(x, y));
            }
            else
            {
                // 3つ繋がってなかったので、配列をマーク作業する前の状態に戻す
                
                //Debug.Log("泡は3つ以上繋がっていなかった");
                //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
                //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
                //bobbles = workBobbles.DeepCopy();
            }
            
        }
        else
        {
            Debug.Log("ヒットした泡の色 : " + bobbles[y][x]);
            return false;
        }

        if(deleteCount < 3)
        {
            Debug.Log("泡は3つ以上繋がっていなかった");
            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
            bobbles = workBobbles.DeepCopy();
            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
        }

        return true;
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
        if ((y < 0) || (y > BOBBLE_ROW_MAX)) return;

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
                FloodFill(x + 1, y - 1, color, ref count);

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
            ScoreManager.Instance.AddScore(true);
            bobbleGroups[y].DestroyChildBobble(x);
            bobbles[y][x] = BobbleColor.None;
            
            Debug.Log("削除！！ Y : " + y + " X : " + x);

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
                BobbleDelete(x + 1, y - 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x + 1, y + 1);
                BobbleDelete(x    , y + 1);
            }
        }
    }

    /// <summary>
    /// 天井と繋がっている泡をマークする
    /// </summary>
    /// <param name="x">列</param>
    /// <param name="y">行</param>
    private void MarkNotConnectedCeilBobbles(int x, int y)
    {
        // 左端が右端を超えたらリターン
        // 奇数列だった場合y % 2の答えが1になるので、互い違いを考慮することができる
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // 天辺か底辺を超えたらリターン
        if ((y < 0) || (y > BOBBLE_ROW_MAX)) return;

        if (BobbleColor.Blue <= bobbles[y][x] && bobbles[y][x] < BobbleColor.Max)
        {
            // 動作確認用　色を赤にする
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // 天井とつながっている泡としてマークする
            bobbles[y][x] = BobbleColor.NotConnectedCeil;

            // 再帰処理
            // 偶数行か奇数行かでアクセスする泡の位置を変える
            if (y % 2 == 0)
            {
                // 偶数行
                MarkNotConnectedCeilBobbles(x - 1, y);
                MarkNotConnectedCeilBobbles(x - 1, y - 1);
                MarkNotConnectedCeilBobbles(x, y - 1);

                MarkNotConnectedCeilBobbles(x + 1, y);
                MarkNotConnectedCeilBobbles(x, y + 1);
                MarkNotConnectedCeilBobbles(x - 1, y + 1);
            }
            else
            {
                // 奇数行
                MarkNotConnectedCeilBobbles(x - 1, y);
                MarkNotConnectedCeilBobbles(x, y - 1);
                MarkNotConnectedCeilBobbles(x + 1, y - 1);

                MarkNotConnectedCeilBobbles(x + 1, y);
                MarkNotConnectedCeilBobbles(x + 1, y + 1);
                MarkNotConnectedCeilBobbles(x, y + 1);
            }
        }
    }

    /// <summary>
    /// 天井からフラッドフィルで色があるセルをマークする
    /// </summary>
    private void DeleteNotConnectedCeillingBobbles()
    {
        // マーク前の状態をコピー
        workBobbles = bobbles.DeepCopy();

        // 天井の行の泡を開始列としてマークしていく
        for (int i = 0; i < BOBBLE_EVEN_SIZE; i++)
        {
            // 0行i列の泡が色付き（マーク済みかNoneである）でなければ処理スキップ
            if(bobbles[0][i] <= BobbleColor.None || BobbleColor.Max <= bobbles[0][i])
            {
                continue;
            }

            // 天井と繋がっていたらマーク
            MarkNotConnectedCeilBobbles(i, 0);
        }

        // マーク後泡を全てチェックする
        for( int y = 0; y < BOBBLE_ROW_MAX; y++)
        {
            for(int x = 0; x < BOBBLE_EVEN_SIZE - (y % 2); x++)
            {
                // 泡が色付きならマークされていない、天井と繋がっていないので削除する
                if (BobbleColor.Blue <= bobbles[y][x] && bobbles[y][x] < BobbleColor.Max)
                {
                    ScoreManager.Instance.AddScore(false);
                    bobbleGroups[y].DestroyChildBobble(x);
                    bobbles[y][x] = BobbleColor.None;
                    
                    Debug.Log("浮いてる泡を削除！！ Y : " + y + " X : " + x);
                }
                else
                {
                    // マークされていたら、マーク前の状態に戻す
                    //bobbles[y][x] = workBobbles[y][x];
                    BobbleColor tmp = workBobbles[y][x];
                    bobbles[y][x] = tmp;
                }
            }
        }        
    }   

    private IEnumerator BobbleDeleteCoroutine(int x, int y)
    {
        // 1f待つ
        yield return new WaitForEndOfFrame();

        // コンボ数リセット
        ScoreManager.Instance.Combo = 0;

        // 泡を削除
        BobbleDelete(x, y);

        // 削除アニメクリップの長さ分待つ
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        // 浮いた泡を削除        
        DeleteNotConnectedCeillingBobbles();

        // 削除アニメクリップの長さ分待つ
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        // 削除演出中フラグを折る
        GameManager.Instance.isBobbleDeleting = false;

    }
}
