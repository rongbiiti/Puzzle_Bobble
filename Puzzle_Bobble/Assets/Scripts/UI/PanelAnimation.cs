using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        ShowWindow();
    }

    void Start()
    {
        
    }

    void ShowWindow()
    {
        transform.DOScale(1f, 0.08f).SetEase(Ease.Linear);
    }

    
    
}
