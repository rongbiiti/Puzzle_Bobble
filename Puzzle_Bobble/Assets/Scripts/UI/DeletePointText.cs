using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePointText : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        // ‰æ–Ê‚Ìã‚É‚Í‚İo‚½ê‡A‰æ–Ê“à‚É‚¨‚³‚Ü‚é‚æ‚¤‚ÉˆÊ’u‚ğ’²®
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
