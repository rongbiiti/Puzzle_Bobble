using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePointText : MonoBehaviour
{
    /// <summary>
    /// �A��V�䂩�痎�Ƃ��ăX�R�A�𓾂��Ƃ��ɕ\������ۂ̕����F
    /// </summary>
    [SerializeField] private Color _falledPointOutlineColor;

    private RectTransform rectTransform;
    private Outline outline;

    /// <summary>
    /// ���_�e�L�X�g�I�u�W�F�N�g�̃p�����[�^�ݒ�
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pos"></param>
    /// <param name="isFall"></param>
    public void SetDeletePointText(string text, Vector3 pos, bool isFall, bool addRandomPos = false)
    {
        GetComponent<Text>().text = text;
        outline = GetComponent<Outline>();
        rectTransform = GetComponent<RectTransform>();

        // ������A�̓��_�������ꍇ�A�A�E�g���C���J���[��ς���
        if (isFall)
        {
            outline.effectColor = _falledPointOutlineColor;
        }

        // Canvas�̎q�ɂȂ�
        transform.SetParent(ScoreManager.Instance.GetCanvas().transform);

        // �����̈ʒu�Ɉړ�
        transform.position = pos;

        // ���[�J��Scale��1,1,1�ɂ���
        transform.localScale = Vector3.one;

        // �ʒu�𑽏������_���ɂ��炷
        if (addRandomPos)
        {
            Vector3 randPos = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            rectTransform.position += randPos;
        }

        // ��ʂ̏�ɂ͂ݏo���ꍇ�A��ʓ��ɂ����܂�悤�Ɉʒu�𒲐�

        if (rectTransform.position.y >= 1860f)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 1860f, 0);

        }
    }

}
