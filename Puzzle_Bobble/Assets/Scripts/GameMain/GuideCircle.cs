using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCircle : MonoBehaviour
{
    /// <summary>
    /// ���˃K�C�h�̎q�v�f��SpriteRenderer
    /// </summary>
    private SpriteRenderer[] _chiledSprRenderers;

    void Start()
    {
        // �q�v�f��SpriteRenderer��C�Ɏ擾
        _chiledSprRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// �q�v�f�̕\����Ԑ؂�ւ�
    /// </summary>
    /// <param name="value">true�ŕ\���Afalse�Ŕ�\��</param>
    public void SetActiveGuideCircle(bool value)
    {
        // �܂�SpriteRenderer�擾���ĂȂ�������擾������
        if(_chiledSprRenderers == null)
        {
            Start();
        }

        // �q�v�f�̕\����Ԃ����ׂĐ؂�ւ���
        foreach(var sp in _chiledSprRenderers)
        {
            sp.enabled = value;
        }
    }
}
