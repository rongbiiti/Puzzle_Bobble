using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    /// <summary>
    /// 泡のプレハブ
    /// </summary>
    [SerializeField] private GameObject _bobblePrefab;

    /// <summary>
    /// 発射した玉の速度
    /// </summary>
    [SerializeField] private float _bobbleMoveSpeed = 1f;    

    /// <summary>
    /// ガイドサークル管理スクリプト
    /// </summary>
    [SerializeField] private GuideCircle _guideCircle;

    /// <summary>
    /// 大砲の回転可能最小角度
    /// </summary>
    [SerializeField] private float _minAngleDelta = 10f;

    /// <summary>
    /// 大砲の回転可能最大角度
    /// </summary>
    [SerializeField] private float _maxAngleDelta = 170f;

    /// <summary>
    /// 次に大砲にセットする玉の生成時位置
    /// </summary>
    [SerializeField] private Transform _nextBobblePosition;

    /// <summary>
    /// 発射エフェクト
    /// </summary>
    [SerializeField] private GameObject _fireEffect;

    private GameObject haveBobbleObj;      // 大砲にセットされている玉
    private bool shotFlg;               // 発射可能フラグ
    private bool isShotBobbleChanging;  // 玉交換アニメ中か
    private bool isShotGuideActive;     // 発射ガイド表示中か
    private Vector3 shotDirection;      // 玉を発射する方向

    private GameObject nextBobbleObj;      // 次に大砲にセットする玉

    private GUIStyle style; // OnGUIでデバッグ表示用
    private Vector3 pos;    // OnGUIでデバッグ表示用
    private string dbgStr;  // OnGUIでデバッグ表示用
    private int dbgTouchCount;     // OnGUIでデバッグ表示用

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        // 大砲を上に向ける
        transform.rotation = Quaternion.Euler(0, 0, 90.0f);

        // 発射ガイドを非表示にしておく
        _guideCircle.SetActiveGuideCircle(false);

        nextBobbleObj = Instantiate(_bobblePrefab, transform.position, Quaternion.identity) as GameObject;
        nextBobbleObj.GetComponent<Bobble>()._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);
        nextBobbleObj.GetComponent<Bobble>().enabled = false;

        // 発射する玉を生成
        Reload();

        // デバッグ用
        style = new GUIStyle();
        style.fontSize = 50;
    }

    void Update()
    {
        if (GameManager.Instance.isBobbleFalloutGameOverZone || GameManager.Instance.isBobbleDeleting) return;

        if (Application.isEditor)
        {
            // エディタで実行中

            // クリックした瞬間
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegan(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            // 離した瞬間
            if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnded(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            // クリックしっぱなし
            if (Input.GetMouseButton(0))
            {
                OnTouchHold(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else
        {
            // 実機で実行中

            // タッチされているかチェック
            if (Input.touchCount > 0)
            {
                // タッチ情報の取得
                Touch touch = Input.GetTouch(0);

                // タッチした瞬間
                if (touch.phase == TouchPhase.Began)
                {
                    OnTouchBegan(Camera.main.ScreenToWorldPoint(touch.position));
                }

                // 離した瞬間
                if (touch.phase == TouchPhase.Ended)
                {
                    OnTouchEnded(Camera.main.ScreenToWorldPoint(touch.position));
                }

                // タッチしっぱなし
                if (touch.phase == TouchPhase.Moved)
                {
                    OnTouchHold(Camera.main.ScreenToWorldPoint(touch.position));
                }
            }
        }
    }

    /// <summary>
    /// タッチした瞬間の処理
    /// </summary>
    private void OnTouchBegan(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        _guideCircle.SetActiveGuideCircle(true);    // 発射ガイド表示
        isShotGuideActive = true;
        SoundManager.Instance.PlaySE(SE.CannonAiming);

        // 以下デバッグ用
        pos = touchPosition;
        dbgStr = "タッチ開始";
        dbgTouchCount++;
    }

    /// <summary>
    /// 離した瞬間の処理
    /// </summary>
    private void OnTouchEnded(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        _guideCircle.SetActiveGuideCircle(false);   // 発射ガイド非表示
        isShotGuideActive = false;

        // 玉を持っているときだけ発射処理
        if (haveBobbleObj && shotFlg)
        {
            haveBobbleObj.AddComponent<BobbleMove>().ShotBubble(_bobbleMoveSpeed, shotDirection);    // 玉を発射
            haveBobbleObj = null;
            GameManager.Instance.shootedBobbleMoving = true;
            SoundManager.Instance.PlaySE(SE.CannonFire);
            StartCoroutine(nameof(ReloadBobble));
            Instantiate(_fireEffect, transform.position, Quaternion.identity);
        }        

        // 以下デバッグ用
        pos = touchPosition;
        dbgStr = "タッチ終了";
    }

    /// <summary>
    /// タッチしっぱなしのときの処理
    /// </summary>
    private void OnTouchHold(Vector3 touchPosition)
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        // 発射ガイドが表示されてなかったら、表示する
        if (!isShotGuideActive)
        {
            _guideCircle.SetActiveGuideCircle(true);    // 発射ガイド表示
            isShotGuideActive = true;
        }

        touchPosition.z = transform.position.z; // Z座標を揃える

        Vector3 dir = (touchPosition - transform.position).normalized;  // ベクトルから方向情報を取り出す
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);      // 方向から角度を作成

        // 0度から180度だった場合のみ回転
        // 負の値（-1度 ～ -180度）、つまり水平より下を向くことになるなら回転させない
        // さらに、インスペクターで指定した最小角度と最大角度の範囲内で回転させる
        if (0 <= angle && _minAngleDelta <= angle && angle <= _maxAngleDelta)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);         // その方向に大砲を向ける
            shotDirection = dir;            // 発射する方向をセット
            shotFlg = true;
        }
        else if (0 <= angle && angle <= 180)
        {
            // 方向が最小角度と最大角度でなくても、水平以上なら発射方向は上書きせずに発射可能にする
            shotFlg = true;
        }
        else
        {
            // 発射禁止
            shotFlg = false;
        }

        // 以下デバッグ用
        pos = touchPosition;
        dbgStr = "タッチ中";
    }

    /// <summary>
    /// 玉をランダムで生成する
    /// </summary>
    private void Reload()
    {
        // 前に生成しておいた泡を装填する
        haveBobbleObj = nextBobbleObj;
        haveBobbleObj.transform.position = transform.position;

        // 次弾生成
        nextBobbleObj = Instantiate(_bobblePrefab, _nextBobblePosition.position, Quaternion.identity) as GameObject;

        // 今持ってる泡と生成した泡のコンポーネント取得（何回もGetしたらめんどい）
        Bobble haveBobbleComp = haveBobbleObj.GetComponent<Bobble>();
        Bobble nextBobbleComp = nextBobbleObj.GetComponent<Bobble>();
        
        // 色をランダムで生成する
        // もし装填した玉と同じ色なら再抽選する
        do
        {
            nextBobbleComp._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);

        } while (nextBobbleComp._BobbleColor == haveBobbleComp._BobbleColor);

        // Bobbleコンポーネントを非アクティブにしておく（自動落下しなくなる）
        nextBobbleComp.enabled = false;
    }

    /// <summary>
    /// 次に大砲にセットする玉と大砲にセットされている玉を交換
    /// </summary>
    public void ShotBobbleChange()
    {
        if (GameManager.Instance.shootedBobbleMoving) return;

        // 位置を入れ替え
        haveBobbleObj.transform.position = _nextBobblePosition.transform.position;
        nextBobbleObj.transform.position = transform.position;

        // 玉を入れ替え
        GameObject tmp = nextBobbleObj;
        nextBobbleObj = haveBobbleObj;
        haveBobbleObj = tmp;        

        Debug.Log("おされた");
    }


    /// <summary>
    /// 少し間をおいて玉を生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadBobble()
    {
        while (GameManager.Instance.shootedBobbleMoving)
        {
            yield return new WaitForFixedUpdate();
        }
        
        Reload();
    }

}