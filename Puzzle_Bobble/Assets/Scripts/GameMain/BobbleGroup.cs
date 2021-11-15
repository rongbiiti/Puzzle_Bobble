using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleGroup : MonoBehaviour
{
    public int rowNum; // �s�ԍ�
    public Bobble[] bobbles;    // �q�v�f��Bobble�X�N���v�g

    void Start()
    {
        // �q�v�f�̖A�����ɍs�Ɨ�ԍ���ݒ�
        bobbles = GetComponentsInChildren<Bobble>();

        int i = 0;
        foreach(var b in bobbles)
        {
            b.SetNumber(i++, rowNum);
        }
    }
    
    void Update()
    {
        
    }

    /// <summary>
    /// �q�v�f�̖A�̐F���擾
    /// </summary>
    /// <param name="x">���ڂ̖A��</param>
    /// <returns></returns>
    public BobbleColor GetChildBobbleColor(int x)
    {
        if (bobbles[x] != null)
        {
            return bobbles[x].BobbleColor;
        }
        else
        {
            return BobbleColor.None;
        }
        
    }

    /// <summary>
    /// �q�v�f�̖A�̐F��ύX����
    /// </summary>
    /// <param name="x">���ڂ̖A��</param>
    /// <param name="color">�ύX��̐F</param>
    public void SetChildBobbleColor(int x, BobbleColor color)
    {
        bobbles[x].BobbleColor = color;
    }

    /// <summary>
    /// �A���폜
    /// </summary>
    /// <param name="x"></param>
    public void DestroyChildBobble(int x)
    {
        Destroy(bobbles[x].gameObject);
        bobbles[x] = null;
    }
}
