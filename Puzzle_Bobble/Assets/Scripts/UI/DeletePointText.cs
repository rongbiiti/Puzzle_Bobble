using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePointText : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        // ��ʂ̏�ɂ͂ݏo���ꍇ�A��ʓ��ɂ����܂�悤�Ɉʒu�𒲐�
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
