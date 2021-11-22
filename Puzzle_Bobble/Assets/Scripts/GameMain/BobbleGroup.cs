using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleGroup : MonoBehaviour
{
    /// <summary>
    /// �A�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject _bobblePrefab;

    public int rowNum; // �s�ԍ�
    public List<Bobble> bobbles = new List<Bobble>();    // �q�v�f��Bobble�X�N���v�g
    public List<BobbleColor> bobbleColors = new List<BobbleColor>();    // �q�v�f�̖A�̐F

    private int childBobbleCount;   // �q�v�f�̖A�̐�
    private float[] _firstBobblesPosX = { -2.504494f, -2.224494f };  // �ŏ��̐�������A��X���W�B���[
    private float _bobblesSizeX = 0.56f;

    void Start()
    {
        // �q�v�f�̖A�̐ݒ�
        //CreateChiledBobbles();

        // �I�u�W�F�N�g�̖��O�ɍs�ԍ���ǉ�����
        gameObject.name = rowNum + "Row";
    }
    
    void Update()
    {
        
    }

    public void ChangeRowNum(int newRowNum)
    {
        rowNum = newRowNum;
        gameObject.name = rowNum + "Row";

        foreach (var b in bobbles)
        {
            if (b == null) continue;
            b.SetNumber(b.bobbleNumber.x, newRowNum);
        }
    }

    public void ClearBobbleColors()
    {
        // �ꎟ���ڂ��m��
        for (int i = 0; i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (rowNum % 2); i++)
        {
            bobbles.Add(null);
            bobbleColors.Add(BobbleColor.None);
        }
    }

    /// <summary>
    /// �A�𐶐�����
    /// </summary>
    public void CreateChiledBobbles()
    {
        int i;

        // �A�𐶐�
        for (i = 0; i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (rowNum % 2); i++)
        {
            GameObject b = Instantiate(_bobblePrefab, new Vector3(_firstBobblesPosX[rowNum % 2] + _bobblesSizeX * i, transform.position.y, 0), Quaternion.identity);
            b.transform.parent = transform;
            bobbles.Insert(i, b.GetComponent<Bobble>());
        }

        // �q�v�f�̖A�����ɍs�Ɨ�ԍ���ݒ�
        i = 0;
        foreach (var b in bobbles)
        {
            b.SetNumber(i++, rowNum);       // �s�Ɨ�ԍ��ݒ�

            // �A�̐F�������_���ɕύX
            b._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Purple + 1);

            bobbleColors.Add(b._BobbleColor);// �A�̐F���擾���Ă���
            childBobbleCount++;             // �A�̐����L�^
        }
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
            return bobbles[x]._BobbleColor;
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
        // �������ݒ�ł���F�͈̔͊O���w�肳�ꂽ�珈�����X�L�b�v
        if (color <= BobbleColor.None || BobbleColor.Max <= color) return;

        bobbles[x]._BobbleColor = color;
        bobbleColors[x] = color;
    }

    /// <summary>
    /// �A���폜
    /// </summary>
    /// <param name="x"></param>
    public void DestroyChildBobble(int x)
    {
        Destroy(bobbles[x].gameObject);     // �I�u�W�F�N�g�j��
        //bobbles.Insert(x, null);             // �z��̒���null�ɂ���
        bobbles[x] = null;
        bobbleColors[x] = BobbleColor.None; // �F���L�����Ă�z��̒���None�ɕς���
        //bobbleColors.Insert(x, BobbleColor.None);
        childBobbleCount--;

        // �q�v�f�̖A�����ׂď�������e�ł��邱�̃I�u�W�F�N�g���폜...
        if(childBobbleCount <= 0)
        {
            //Destroy(gameObject);
            //Debug.Log(gameObject.name + "���q�����Ȃ��Ȃ����̂ō폜���ꂽ");
        }
    }
}
