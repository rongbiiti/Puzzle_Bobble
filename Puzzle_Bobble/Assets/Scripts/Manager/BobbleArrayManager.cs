using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BobbleArrayManager : SingletonMonoBehaviour<BobbleArrayManager>
{
    /// <summary>
    /// 泡グループのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] _bobbleGroupPrefab;

    /// <summary>
    /// 泡グループの最大数
    /// </summary>
    public int BOBBLE_ROW_MAX = 27;

    /// <summary>
    /// 泡の偶数行の個数
    /// </summary>
    public int BOBBLE_EVEN_SIZE = 10;

    /// <summary>
    /// 同じ行で、同じ色の泡が連続で生成される確率
    /// </summary>
    public float PROB_SAME_ROW_BOBBLE_COLOR = 25.0f;

    /// <summary>
    /// 同じ列で、同じ色の泡が連続で生成される確率
    /// </summary>
    public float PROB_SAME_COL_BOBBLE_COLOR = 25.0f;

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

    /// <summary>
    /// ポイントUIプレハブ
    /// </summary>
    [SerializeField] private GameObject _pointTextUIPrefab;

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

        SoundManager.Instance.PlayBGM(BGM.Main);

    }

    private void Update()
    {
        // ゲームオーバーになってたら処理しない
        if (GameManager.Instance.isBobbleFalloutGameOverZone) return;

        // 泡の削除演出中
        if (GameManager.Instance.isBobbleDeleting) return;

        // 泡生成待機時間を加算
        waitTime += Time.deltaTime * GameManager.Instance.gameSpeed;

        // 待機時間を越えたら泡を生成
        if(nextBobbleGroupCreateWaitTime <= waitTime)
        {
            AddNewBobbleGroup();
            waitTime = 0;
        }
    }

    /// <summary>
    /// 新しい泡グループを0列目と1列目に追加する
    /// </summary>
    private void AddNewBobbleGroup()
    {
        foreach (var bg in bobbleGroups)
        {
            bg.ChangeRowNum(bg.rowNum + 2);
        }

        // 一番下の列の泡グループオブジェクトを削除
        Destroy(bobbleGroups[bobbleGroups.Count - 1].gameObject);
        Destroy(bobbleGroups[bobbleGroups.Count - 2].gameObject);

        // Listからも削除
        bobbles.RemoveAt(bobbles.Count - 1);
        bobbles.RemoveAt(bobbles.Count - 1);
        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);
        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);

        // 新しい空のListを追加
        bobbles.Insert(0, new List<BobbleColor>());
        bobbles.Insert(0, new List<BobbleColor>());

        // 新しい泡グループを生成！
        CreateBobbleGroupObject(1);
        CreateBobbleGroupObject(0);

        // Listの状態をDebug.Log出力
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

        // 泡グループの列が画面下半分に当たるものなら、泡を生成させない（Start時に生成する用）
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
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);
            }

        }
        else
        {
            // 奇数
            for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
            {
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);
            }

        }

        // 作業用配列に記録しておく
        Debug.Log(string.Join(", ", bobbles[rowNum].Select(obj => obj.ToString())));
        
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
                BobbleColor tmp = g.bobbleColors[x];
                bobbles[y][x] = tmp;

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

        // ポーズできないようにする
        Pauser.isCanPausing = false;

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

            // ポーズできるようにする
            Pauser.isCanPausing = true;

            return false;
        }

        if(deleteCount < 3)
        {
            Debug.Log("泡は3つ以上繋がっていなかった");
            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));

            bobbles = workBobbles.DeepCopy();
            ScoreManager.Instance.Combo = 0;

            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));

            // ポーズできるようにする
            Pauser.isCanPausing = true;
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
            ScoreManager.Instance.AddNowTurnPoint(true);
            bobbleGroups[y].DestroyChildBobble(x, false, 0.0334f * ScoreManager.Instance.DeleteCombo);
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
    /// 天井と繋がらなくなった泡を削除する
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

        // 1個でも泡を消すことができたか
        bool isDeleted = false;

        // マーク後泡を全てチェックする
        for ( int y = 0; y < BOBBLE_ROW_MAX; y++)
        {
            for(int x = 0; x < BOBBLE_EVEN_SIZE - (y % 2); x++)
            {
                // 泡が色付きならマークされていない、天井と繋がっていないので削除する
                if (BobbleColor.Blue <= bobbles[y][x] && bobbles[y][x] < BobbleColor.Max)
                {
                    ScoreManager.Instance.AddNowTurnPoint(false);
                    bobbleGroups[y].DestroyChildBobble(x, true, 0);
                    bobbles[y][x] = BobbleColor.None;
                    isDeleted = true;
                    Debug.Log("浮いてる泡を削除！！ Y : " + y + " X : " + x);
                }
                else
                {
                    // マークされていたら、マーク前の状態に戻す
                    BobbleColor tmp = workBobbles[y][x];
                    bobbles[y][x] = tmp;
                }
            }
        }      
        
        // 泡を消していたら、SE再生
        if(isDeleted)
        {
            SoundManager.Instance.PlaySE(SE.BobbleFall);
        }
    }   

    private IEnumerator BobbleDeleteCoroutine(int x, int y)
    {
        // 1f待つ
        yield return new WaitForEndOfFrame();

        // コンボ数リセット
        ScoreManager.Instance.DeleteCombo = 0;
        ScoreManager.Instance.FallCombo = 0;
        ScoreManager.Instance.Combo += 1;
        ScoreManager.Instance.NowTurnPoint = 0;

        // 泡を削除
        BobbleDelete(x, y);

        // 削除アニメクリップの長さ分待つ
        yield return new WaitForSeconds(0.0334f * (ScoreManager.Instance.DeleteCombo + 5));
        yield return new WaitForEndOfFrame();

        // 浮いた泡を削除        
        DeleteNotConnectedCeillingBobbles();

        // カメラを揺らす
        if (ScoreManager.Instance.FallCombo >= 6)
        {
            // 一瞬timeScaleを下げて、ヒットストップみたいな演出をさせる
            Time.timeScale = 0.4f;
            FindObjectOfType<CameraShake>().Shake(0.25f, 0.07f);

            // timeScale元に戻す
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
        }        

        // 削除アニメクリップの長さ分待つ
        yield return new WaitForSeconds(0.25f + 0.125f);
        yield return new WaitForEndOfFrame();

        // スクリーン座標計算
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // 表示するテキスト
        string scoreText = "+" + ScoreManager.Instance.NowTurnPoint.ToString("N0");

        // デンジャータイム中なら、ボーナスが適用されたこと伝える文章を追加する
        if (GameManager.Instance.isDangerTime)
        {
            scoreText = scoreText + " × DangerBonus " + ScoreManager.Instance.DangerTimeScoreBonusRate + " = " + (ScoreManager.Instance.NowTurnPoint * ScoreManager.Instance.DangerTimeScoreBonusRate).ToString("N0") + "!!";
        }

        // ポイントの合計をスコア表示の下に +〇〇 と表示
        GameObject text = Instantiate(_pointTextUIPrefab, screenPos, Quaternion.identity) as GameObject;
        text.GetComponent<DeletePointText>().SetDeletePointText(scoreText, screenPos, false);

        // 1ショットで稼いだポイントの合計をスコアに加算
        ScoreManager.Instance.AddNowTurnPointToScore();

        if (GameManager.Instance.isDangerTime)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // 少し待つ
        yield return new WaitForSeconds(0.125f);
        yield return new WaitForEndOfFrame();

        // 削除演出中フラグを折る
        GameManager.Instance.isBobbleDeleting = false;

        // ポーズできるようにする
        Pauser.isCanPausing = true;

    }
}
