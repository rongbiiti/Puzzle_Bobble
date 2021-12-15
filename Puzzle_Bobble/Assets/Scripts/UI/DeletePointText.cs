using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePointText : MonoBehaviour
{
    /// <summary>
    /// 泡を天井から落としてスコアを得たときに表示する際の文字色
    /// </summary>
    [SerializeField] private Color _falledPointOutlineColor;

    private RectTransform rectTransform;
    private Outline outline;

    /// <summary>
    /// 得点テキストオブジェクトのパラメータ設定
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pos"></param>
    /// <param name="isFall"></param>
    public void SetDeletePointText(string text, Vector3 pos, bool isFall, bool addRandomPos = false)
    {
        GetComponent<Text>().text = text;
        outline = GetComponent<Outline>();
        rectTransform = GetComponent<RectTransform>();

        // 落ちる泡の得点だった場合、アウトラインカラーを変える
        if (isFall)
        {
            outline.effectColor = _falledPointOutlineColor;
        }

        // Canvasの子になる
        transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        // 引数の位置に移動
        transform.position = pos;

        // ローカルScaleを1,1,1にする
        transform.localScale = Vector3.one;

        // 位置を多少ランダムにずらす
        if (addRandomPos)
        {
            Vector3 randPos = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            rectTransform.position += randPos;
        }

        // 画面の上にはみ出た場合、画面内におさまるように位置を調整

        if (rectTransform.position.y >= 1860f)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 1860f, 0);

        }
    }

}
