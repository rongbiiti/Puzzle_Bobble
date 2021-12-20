using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{
    private float _endValue = 1f;           // 終了時のScale
    private float _duration = 0.08f;        // 処理にかける時間
    private RectTransform rectTransform;    // RectTransformコンポーネント

    private void OnEnable()
    {
        // アクティブになったらScaleをゼロにする
        transform.localScale = Vector3.zero;
        StartCoroutine(nameof(OpenWindow));
    }

    void Start()
    {
        
    }

    private IEnumerator OpenWindow()
    {
        float elapsedTime = 0f;

        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        while (true)
        {
            elapsedTime += Time.unscaledDeltaTime;

            rectTransform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(_endValue, _endValue, _endValue), elapsedTime / _duration);

            if(_duration < elapsedTime)
            {

                yield break;
            }

            yield return null;
        }

       
    }

    
    
}
