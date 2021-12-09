using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePointText : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        
    }

    /// <summary>
    /// ���_�e�L�X�g�I�u�W�F�N�g�̃p�����[�^�ݒ�
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pos"></param>
    public void SetDeletePointText(string text, Vector3 pos)
    {
        GetComponent<Text>().text = text;

        // Canvas�̎q�ɂȂ�
        transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        // �����̈ʒu�Ɉړ�
        transform.position = pos;

        // ���[�J��Scale��1,1,1�ɂ���
        transform.localScale = Vector3.one;

        // ��ʂ̏�ɂ͂ݏo���ꍇ�A��ʓ��ɂ����܂�悤�Ɉʒu�𒲐�
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform.position.y >= 1860f)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 1860f, 0);

        }
    }

}
