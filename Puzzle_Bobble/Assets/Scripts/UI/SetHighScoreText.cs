using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScoreText : MonoBehaviour
{
    private Text[] highScoreTexts;

    void Start()
    {
        int updateIndex = SaveDataManager.Instance.ScoreCompare(ScoreManager.Instance.Score);

        // �q�v�f��Text�R���|�[�l���g�擾
        highScoreTexts = GetComponentsInChildren<Text>();

        // �n�C�X�R�A��UI�ɃZ�b�g����
        int i = 0;
        foreach(var hst in highScoreTexts)
        {
            // UI�Ƀn�C�X�R�A�𔽉f
            hst.text = i + 1 + " : " + SaveDataManager.Instance.saveData._highScore[i].ToString();

            // �n�C�X�R�A�X�V���A���̃X�R�A��1680���F�Ɍ��点��
            if (updateIndex != -1 && i == updateIndex)
            {
                hst.gameObject.AddComponent<GamingColorChangeAnim>();
            }

            i++;
        }
    }    
}
