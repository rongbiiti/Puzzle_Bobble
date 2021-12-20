using System;
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
    private SlideNumberEffectController sliderNumberEffectController;
    private int preNum;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        scoreM = ScoreManager.Instance;
        text.text = "0";
        if (_reflectMode == ReflectMode.Score)
        {
            sliderNumberEffectController = GetComponent<SlideNumberEffectController>();
        }
    }

    private void LateUpdate()
    {
        switch (_reflectMode)
        {
            case ReflectMode.Score:
                //text.text = scoreM.Score.ToString("N0");

                if(preNum != scoreM.Score)
                {
                    sliderNumberEffectController.SlideToNumber(scoreM.Score, 2f);
                }

                preNum = scoreM.Score;
                break;
            case ReflectMode.Combo:
                text.text = scoreM.Combo.ToString("N0");
                preNum = scoreM.Combo;
                break;
            default:
                break;
        }
        
    }
}
