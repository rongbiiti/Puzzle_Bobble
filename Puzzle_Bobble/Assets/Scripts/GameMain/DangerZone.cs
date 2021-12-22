using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerZone : MonoBehaviour
{
    // 画面を赤くするパネル
    [SerializeField] private Image _dangerPanel;

    private GameManager gm;     // GameManagerのインスタンス
    private int preContactCount;    // 前のフレームのGameManagerのdangerZoneContactCount
    private Color startColor;
    private int contactCount;   // このオブジェクトのトリガー範囲と接触している泡の個数

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

        // 泡がトリガーに入っていたら、画面を赤く光らせる
        if(0 < gm.dangerZoneContactCount)
        {
            _dangerPanel.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Sin(Time.time * 7) / 4 + 0.05f);
            GameManager.Instance.isDangerTime = true;
        }
        else
        {
            _dangerPanel.color = Color.clear;
            if(preContactCount != gm.dangerZoneContactCount)
            {
                StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
            }            
        }

        preContactCount = gm.dangerZoneContactCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            //contactCount++;
            //GameManager.Instance.isDangerTime = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            //contactCount--;
        }

        if (contactCount <= 0)
        {
            //_dangerPanel.color = Color.clear;
            //StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
        }
    }

    // 次の玉が撃てるようになってからデンジャータイムフラグを折る
    private IEnumerator LiftDangerTimeFlagCoroutine()
    {
        while (GameManager.Instance.isBobbleDeleting)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.isDangerTime = false;
    }
}
