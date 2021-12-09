using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePointText : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        
    }

    /// <summary>
    /// 得点テキストオブジェクトのパラメータ設定
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pos"></param>
    public void SetDeletePointText(string text, Vector3 pos)
    {
        GetComponent<Text>().text = text;

        // Canvasの子になる
        transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        // 引数の位置に移動
        transform.position = pos;

        // ローカルScaleを1,1,1にする
        transform.localScale = Vector3.one;

        // 画面の上にはみ出た場合、画面内におさまるように位置を調整
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform.position.y >= 1860f)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 1860f, 0);

        }
    }

}
