using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleFallSpeedControll : MonoBehaviour
{
    /// <summary>
    /// この間、泡の落下速度が早まる
    /// </summary>
    [SerializeField] private float _bobbleFallSpeedUpTime = 5f;

    /// <summary>
    /// 落下速度がはまっている間、この速度で落下する
    /// </summary>
    [SerializeField] private float _SpeedUpFallSpeed = 10f;

    /// <summary>
    /// トリガーに泡が触れていなかったら、この秒数待ってからスピードアップをする
    /// もしこの間に泡が触れたら、スピードアップをしない
    /// </summary>
    [SerializeField] private float _speedUpDelay = 2f;

    private GameManager gm;     // GameManagerのインスタンス
    private bool isStartedSpeedUpCoroutine; // コルーチンを呼び出したか
    private float startFallSpeed;   // 開始時の落下速度

    private void Start()
    {   
        gm = GameManager.Instance;      // インスタンス取得しておく
        startFallSpeed = gm.gameSpeed;  // 開始時のゲームスピード覚えておく
    }

    private void LateUpdate()
    {
        // gm.fallSpeedUpZoneContactCountが0以下で、
        // コルーチンをまだ呼び出していなくて
        // 削除演出が終わっていたら
        if (gm.fallSpeedUpZoneContactCount <= 0 && !isStartedSpeedUpCoroutine && !GameManager.Instance.isBobbleDeleting)
        {
            // 落下速度上昇のコルーチンを呼ぶ
            isStartedSpeedUpCoroutine = true;
            StartCoroutine(nameof(FallSpeedUp));
        }

    }

    // このトリガーに泡が入ったらカウントを増やす
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            gm.fallSpeedUpZoneContactCount++;

            // ※カウントは、泡の削除処理で0にリセットされる。
        }
    }

    // 泡の落下速度を上げる
    private IEnumerator FallSpeedUp()
    {
        // 泡が動き出した瞬間から一拍置く
        yield return new WaitForSeconds(_speedUpDelay);

        // 落下速度上昇判定ゾーンに泡が入っていなければ、落下速度を上げる
        if(gm.fallSpeedUpZoneContactCount <= 0)
        {
            GameManager.Instance.gameSpeed = _SpeedUpFallSpeed;
            yield return new WaitForSeconds(_bobbleFallSpeedUpTime);
        }
        
        // 落下速度を戻す
        GameManager.Instance.gameSpeed = startFallSpeed;

        // このコルーチンが再び呼べるようにする
        isStartedSpeedUpCoroutine = false;
    }



    
}
