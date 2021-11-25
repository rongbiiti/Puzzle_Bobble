using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePointText : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        // 画面の上にはみ出た場合、画面内におさまるように位置を調整
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform.position.y >= 1860f)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 1860f, 0);
            
        }
    }

    void Update()
    {
        
    }
}
