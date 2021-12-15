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
    
    // �s��ς���
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

    // �A�����Z�b�g����
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
            b.SetNumber(i, rowNum);       // �s�Ɨ�ԍ��ݒ�


            if (Probability.Lottery(BobbleArrayManager.Instance.PROB_SAME_COL_BOBBLE_COLOR) && // ���̊m����
                0 < rowNum &&                                                                         // 1��ڈȏ��
                i < BobbleArrayManager.Instance.BOBBLE_EVEN_SIZE - (1 + rowNum % 2) &&                // �E�[�łȂ�                                                               
                BobbleColor.Blue <= BobbleArrayManager.Instance.bobbles[rowNum + 1][i] &&         // �����\�ȐF�͈̔͂Ȃ�
                BobbleArrayManager.Instance.bobbles[rowNum + 1][i] <= BobbleColor.Yellow)
            {
                
                // ������ň�O�̍s�̖A�Ɠ����F�̖A�𐶐�����
                b._BobbleColor = BobbleArrayManager.Instance.bobbles[rowNum + 1][i];
            }
            else if(Probability.Lottery(BobbleArrayManager.Instance.PROB_SAME_ROW_BOBBLE_COLOR) && 0 < i)
            {
                // ���̊m���ŁA1���̖A�Ɠ����F�̖A�𐶐�����
                // 1�ڂ������炱��͍s��Ȃ�
                b._BobbleColor = bobbleColors[i - 1];
            }
            else
            {
                // �A�̐F�������_���ɕύX
                b._BobbleColor = (BobbleColor)Random.Range((int)BobbleColor.Blue, (int)BobbleColor.Yellow + 1);
            }
            
            bobbleColors.Add(b._BobbleColor);// �A�̐F���擾���Ă���
            childBobbleCount++;             // �A�̐����L�^

            i++;
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
    public void DestroyChildBobble(int x, bool isFall, float delay)
    {
        //Destroy(bobbles[x].gameObject);     // �I�u�W�F�N�g�j��
        bobbles[x].BobbleDestroy(isFall, delay);
        //bobbles.Insert(x, null);             // �z��̒���null�ɂ���
        bobbles[x] = null;
        bobbleColors[x] = BobbleColor.None; // �F���L�����Ă�z��̒���None�ɕς���
        //bobbleColors.Insert(x, BobbleColor.None);
        childBobbleCount--;

        
    }
}
