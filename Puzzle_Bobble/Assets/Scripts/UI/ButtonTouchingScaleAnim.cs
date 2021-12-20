using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTouchingScaleAnim : MonoBehaviour
{
    private float _endValue = 0.8f;           // 目的のScale
    private float _duration = 0.05f;        // 処理にかける時間
    private RectTransform rectTransform;    // RectTransformコンポーネント

    private Vector3 startScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startScale = rectTransform.localScale;
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        rectTransform.localScale = startScale;
    }

    public void ScaleUp()
    {
        StartCoroutine(nameof(ScaleUpCoroutine));
    }

    public void ScaleDown()
    {
        StartCoroutine(nameof(ScaleDownCoroutine));
    }

    private IEnumerator ScaleUpCoroutine()
    {
        float elapsedTime = 0f;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        while (true)
        {
            elapsedTime += Time.unscaledDeltaTime;

            rectTransform.localScale = Vector3.Lerp(new Vector3(_endValue, _endValue, _endValue), startScale, elapsedTime / _duration);

            if (_duration < elapsedTime)
            {

                yield break;
            }

            yield return null;
        }


    }

    private IEnumerator ScaleDownCoroutine()
    {
        float elapsedTime = 0f;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        while (true)
        {
            elapsedTime += Time.unscaledDeltaTime;

            rectTransform.localScale = Vector3.Lerp(startScale, new Vector3(_endValue, _endValue, _endValue), elapsedTime / _duration);

            if (_duration < elapsedTime)
            {

                yield break;
            }

            yield return null;
        }


    }
}
