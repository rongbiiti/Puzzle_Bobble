using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerZone : MonoBehaviour
{
    // 画面を赤くするパネル
    [SerializeField] private Image _dangerPanel;

    private GameManager gm;     // GameManagerのインスタンス
    private Color startColor;   // パネルの初期色

    void Start()
    {
        // インスタンス取得しておく
        gm = GameManager.Instance;

        // パネルの色を取得
        startColor = _dangerPanel.color;

        // 透明にしておく
        _dangerPanel.color = Color.clear;
    }

    private void FixedUpdate()
    {
        // ゲームが終了したら、画面を赤く光らせないようにする
        if (GameManager.Instance.isBobbleFalloutGameOverZone)
        {
            _dangerPanel.color = Color.clear;
            this.enabled = false;
            return;
        }

        // 削除演出中なら早期リターン
        if (GameManager.Instance.isBobbleDeleting) return;

        // 泡がトリガーに入っていたら、画面を赤く光らせる
        if(0 < gm.dangerZoneContactCount)
        {
            // パネルの色を赤く点滅させる（透明度を変えている）
            _dangerPanel.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Sin(Time.time * 7) / 4 + 0.05f);
            GameManager.Instance.isDangerTime = true;
        }
        else
        {
            // パネルを透明に戻す
            _dangerPanel.color = Color.clear;
            StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            gm.dangerZoneContactCount++;
            GameManager.Instance.isDangerTime = true;
        }
    }

    // 次の玉が撃てるようになってからデンジャータイムフラグを折る
    private IEnumerator LiftDangerTimeFlagCoroutine()
    {
        // 玉の削除演出中なら処理を止める
        while (GameManager.Instance.isBobbleDeleting)
        {
            yield return new WaitForFixedUpdate();
        }

        // 一拍置く
        yield return new WaitForSeconds(0.5f);

        // デンジャーゾーンに泡が入っていなければ、デンジャータイムフラグを折る
        if(gm.dangerZoneContactCount <= 0)
        {
            GameManager.Instance.isDangerTime = false;
        }
        
    }
}
