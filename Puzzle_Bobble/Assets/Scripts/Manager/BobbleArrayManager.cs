using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleArrayManager : SingletonMonoBehaviour<BobbleArrayManager>
{
    /// <summary>
    /// �A�O���[�v�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject[] _bobbleGroupPrefab;

    /// <summary>
    /// �A�O���[�v�̍ő吔
    /// </summary>
    public const int BOBBLE_ROW_MAX = 15;

    /// <summary>
    /// �A�̋����s�̌�
    /// </summary>
    public const int BOBBLE_EVEN_SIZE = 10;

    // �A���i�[����z��
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();
    

    private float bobbleCreateStartPosY = 3.24f;        // ��ԍŏ��ɐ�������A�O���[�v��Y���W
    private float bobbleCreateIncreaseYPos = 0.56f;      // 2�ڈȍ~�̖A�O���[�v�������ɂ��̒l��Y���W����ɂ��炵�Đ���

    void Start()
    {
        // �ꎟ���ڂ��m��
        for (int i = 0; i < BOBBLE_ROW_MAX; i++) bobbles.Add(new List<BobbleColor>());

        // �z���������
        for (int i = 0; i < BOBBLE_ROW_MAX; i++)
        {
            // �����Ɗ�ŃT�C�Y���Ⴄ�̂ŕ�����
            if (i % 2 == 0)
            {
                // ����
                for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
                {
                    // �z��ɖA�̏���ǉ�
                    bobbles[i].Add(BobbleColor.Blue);
                }
                
            }
            else
            {
                // �
                for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
                {
                    bobbles[i].Add(BobbleColor.Gray);
                }
                
            }

            // �A�O���[�v��������
            CreateBobbleGroupObject(i);
        }

        
    }

    /// <summary>
    /// �A�O���[�v�̃I�u�W�F�N�g�����Ə��X�̏���
    /// </summary>
    /// <param name="rowNum">�s�ԍ�</param>
    private void CreateBobbleGroupObject(int rowNum)
    {
        // �A�O���[�v�̃I�u�W�F�N�g�𐶐�
        // ���͂Ƃ肠�����������������
        GameObject bobbleG = Instantiate(_bobbleGroupPrefab[rowNum % 2]);

        // ��������ʒu��Y���W��O�ɐ����������̂Əd�Ȃ�Ȃ��悤�ɒ���
        bobbleG.transform.position += new Vector3(0, bobbleCreateStartPosY + bobbleCreateIncreaseYPos * rowNum, 0);

        // �A�O���[�v��BobbleGroup�R���|�[�l���g�ɍs�ԍ���ݒ�
        bobbleG.GetComponent<BobbleGroup>().rowNum = rowNum;

        // �I�u�W�F�N�g��List�ɒǉ�
        bobbleGroups.Add(bobbleG.GetComponent<BobbleGroup>());
    }

    /// <summary>
    /// �A�����������ł��邩�`�F�b�N����
    /// </summary>
    /// <param name="x">�q�b�g�����A�̍s</param>
    /// <param name="y">�q�b�g�����A�̗�</param>
    /// <param name="color">�ʂ̐F</param>
    /// <returns></returns>
    public bool BobbleDeleteCheck(int x, int y, BobbleColor color)
    {
        if(bobbles[y][x] == color)
        {
            Debug.Log(bobbles[y][x]);
            FloodFill(x, y, color);
            return true;
        }
        else
        {
            Debug.Log(bobbles[y][x]);
            return false;
        }
    }

    /// <summary>
    /// �t���b�h�t�B��
    /// �q�b�g�����A�Ɨאڂ��Ă��ċʂƓ����F�ł���A��T������
    /// </summary>
    /// <param name="x">�s</param>
    /// <param name="y">��</param>
    /// <param name="color">�F</param>
    private void FloodFill(int x, int y, BobbleColor color)
    {
        
        // ���[���E�[�𒴂����烊�^�[��
        // ��񂾂����ꍇy % 2�̓�����1�ɂȂ�̂ŁA�݂��Ⴂ���l�����邱�Ƃ��ł���
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // �V�ӂ���ӂ𒴂����烊�^�[��
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        if(bobbleGroups[y].GetChildBobbleColor(x) == color)
        {
            // ����m�F�p�@�F��Ԃɂ���
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // �A���폜����
            bobbleGroups[y].DestroyChildBobble(x);

            // �ċA����
            FloodFill(x + 1, y    , color);
            FloodFill(x    , y + 1, color);
            FloodFill(x - 1, y    , color);
            FloodFill(x    , y - 1, color);
        }
    }
}
