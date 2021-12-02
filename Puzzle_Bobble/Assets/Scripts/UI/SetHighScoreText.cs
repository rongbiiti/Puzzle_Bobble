using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScoreText : MonoBehaviour
{
    // ハイスコアテキストを入れるText
    private Text[] highScoreTexts;

    void Start()
    {
        int updateIndex = SaveDataManager.Instance.ScoreCompare(ScoreManager.Instance.Score);

        // 子要素のTextコンポーネント取得
        highScoreTexts = GetComponentsInChildren<Text>();

        // ハイスコアをUIにセットする
        int i = 0;
        foreach(var hst in highScoreTexts)
        {
            // UIにハイスコアを反映
            hst.text = i + 1 + " : " + SaveDataManager.Instance.saveData._highScore[i].ToString("N0");

            // ハイスコア更新時、そのスコアを1680万色に光らせる
            if (updateIndex != -1 && i == updateIndex)
            {
                hst.gameObject.AddComponent<GamingColorChangeAnim>();
                SoundManager.Instance.PlaySystemSE(SysSE.HighScoreUpdate);
            }

            i++;
        }
    }    
}
