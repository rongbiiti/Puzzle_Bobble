using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private enum ReflectMode
    {
        Score,
        Combo
    }

    [SerializeField] private ReflectMode _reflectMode;
    private Text text;
    private ScoreManager scoreM;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        scoreM = ScoreManager.Instance;
    }

    private void LateUpdate()
    {
        switch (_reflectMode)
        {
            case ReflectMode.Score:
                text.text = scoreM.Score.ToString("N0");
                break;
            case ReflectMode.Combo:
                text.text = scoreM.Combo.ToString("N0");
                break;
            default:
                break;
        }
        
    }
}
