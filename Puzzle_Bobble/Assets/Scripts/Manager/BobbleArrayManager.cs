using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BobbleArrayManager : SingletonMonoBehaviour<BobbleArrayManager>
{
    /// <summary>
    /// �A�O���[�v�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject[] _bobbleGroupPrefab;

    /// <summary>
    /// �A�O���[�v�̍ő吔
    /// </summary>
    public int BOBBLE_ROW_MAX = 25;

    /// <summary>
    /// �A�̋����s�̌�
    /// </summary>
    public int BOBBLE_EVEN_SIZE = 10;

    /// <summary>
    /// Start���ɖA�O���[�v�����s���������邩
    /// </summary>
    [SerializeField] private int _onStartCreateBobbleRow = 15;

    // �A���i�[����z��
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();

    // ��Ɨp�z��
    private List<List<BobbleColor>> workBobbles = new List<List<BobbleColor>>();

    // �A�O���[�v�I�u�W�F�N�g�̔z��
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();

    private float bobbleCreateStartPosY = 11.08f;        // ��ԍŏ��ɐ�������A�O���[�v��Y���W
    private float bobbleCreateIncreaseYPos = 0.56f;      // 2�ڈȍ~�̖A�O���[�v�������ɂ��̒l��Y���W����ɂ��炵�Đ���

    void Start()
    {
        // �ꎟ���ڂ��m��
        for (int i = 0; i < BOBBLE_ROW_MAX; i++)
        {
            bobbles.Add(new List<BobbleColor>());
            workBobbles.Add(new List<BobbleColor>());
        }

        // �z���������
        for (int i = BOBBLE_ROW_MAX - 1; 0 <= i; i--)
        {
            // �A�O���[�v��������
            CreateBobbleGroupObject(i);
        }

    }

    /// <summary>
    /// �A�O���[�v�̃I�u�W�F�N�g����
    /// </summary>
    /// <param name="rowNum">�s�ԍ�</param>
    private void CreateBobbleGroupObject(int rowNum)
    {
        // �A�O���[�v�̃I�u�W�F�N�g�𐶐�
        // ���͂Ƃ肠�����������������
        GameObject bobbleG = Instantiate(_bobbleGroupPrefab[rowNum % 2]);

        // ��������ʒu��Y���W��O�ɐ����������̂Əd�Ȃ�Ȃ��悤�ɒ���
        bobbleG.transform.position += new Vector3(0, bobbleCreateStartPosY - bobbleCreateIncreaseYPos * rowNum, 0);

        // �A�O���[�v��BobbleGroup�R���|�[�l���g�ɍs�ԍ���ݒ�
        BobbleGroup bobbleGroup = bobbleG.GetComponent<BobbleGroup>();

        bobbleGroup.rowNum = rowNum;

        if(rowNum <= _onStartCreateBobbleRow - 1)
        {
            bobbleGroup.CreateChiledBobbles();
        }
        else
        {
            bobbleGroup.ClearBobbleColors();
        }

        // �I�u�W�F�N�g��List�ɒǉ�
        bobbleGroups.Insert(0, bobbleG.GetComponent<BobbleGroup>());

        // �����Ɗ�ŃT�C�Y���Ⴄ�̂ŕ�����
        if (rowNum % 2 == 0)
        {
            // ����
            for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
            {
                // �z��ɖA�̏���ǉ�
                bobbles[rowNum] = bobbleGroup.bobbleColors;
                
            }

        }
        else
        {
            // �
            for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
            {
                bobbles[rowNum] = bobbleGroup.bobbleColors;
            }

        }

        // ��Ɨp�z��ɋL�^���Ă���
        //workBobbles = bobbles;
        Debug.Log(string.Join(", ", bobbles[rowNum].Select(obj => obj.ToString())));
        //workBobbles = bobbles.DeepCopy();
        
    }

    /// <summary>
    /// �w�肵���s�Ɠ����s�ԍ������A�O���[�v��Transform���擾
    /// </summary>
    /// <param name="y">�s�ԍ�</param>
    /// <returns>�A�O���[�v��Transform</returns>
    public Transform GetSameRowBobbleGroup(int x, int y, BobbleColor color, Bobble bobbleComponent)
    {
        foreach(var g in bobbleGroups)
        {
            if(g.rowNum == y)
            {
                g.bobbles[x] = bobbleComponent;
                g.bobbleColors[x] = color;
                bobbles[y] = g.bobbleColors;
                return g.transform;
            }
        }

        return null;
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
        // �Q�Ɠn���ŖA�̏��������J�E���g����p
        int deleteCount = 0;

        // 3�ȏ�q�����ĂȂ������Ƃ��Ƀ}�[�N�����Z�b�g�ł���悤�ɁA�z���ۑ����Ă���
        workBobbles = bobbles.DeepCopy();

        // ���˂����ʂƃq�b�g�����A�̐F��������
        if (bobbles[y][x] == color)
        {
            Debug.Log("�q�b�g�����A�̐F : " + bobbles[y][x]);

            // �ʂƓ����F�������̂ŁA�q�����Ă�����̂��}�[�N���Ă����B
            FloodFill(x, y, color, ref deleteCount);

            Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));

            if(3 <= deleteCount)
            {
                // 3�ȏ�q�����Ă����̂ŁA�폜���
                BobbleDelete(x, y);
            }
            else
            {
                // 3�q�����ĂȂ������̂ŁA�z����}�[�N��Ƃ���O�̏�Ԃɖ߂�
                bobbles = workBobbles.DeepCopy();
                Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
            }
            return true;
        }
        else
        {
            Debug.Log("�q�b�g�����A�̐F : " + bobbles[y][x]);
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
    private void FloodFill(int x, int y, BobbleColor color, ref int count)
    {
        
        // ���[���E�[�𒴂����烊�^�[��
        // ��񂾂����ꍇy % 2�̓�����1�ɂȂ�̂ŁA�݂��Ⴂ���l�����邱�Ƃ��ł���
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // �V�ӂ���ӂ𒴂����烊�^�[��
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        if(bobbles[y][x] == color)
        {
            // ����m�F�p�@�F��Ԃɂ���
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // �폜�\�Ȃ̂ŁA�J�E���g���C���N�������g
            count++;
            // �폜����A�Ƃ��ă}�[�N����
            bobbles[y][x] = BobbleColor.Delete;
            Debug.Log("�J�E���g" + count);

            // �ċA����
            // �����s����s���ŃA�N�Z�X����A�̈ʒu��ς���
            if(y % 2 == 0)
            {
                // �����s
                FloodFill(x - 1, y    , color, ref count);
                FloodFill(x - 1, y - 1, color, ref count);
                FloodFill(x    , y - 1, color, ref count);

                FloodFill(x + 1, y    , color, ref count);
                FloodFill(x    , y + 1, color, ref count);
                FloodFill(x - 1, y + 1, color, ref count);
            }
            else
            {
                // ��s
                FloodFill(x - 1, y    , color, ref count);
                FloodFill(x    , y - 1, color, ref count);
                FloodFill(x + 1, y + 1, color, ref count);

                FloodFill(x + 1, y    , color, ref count);
                FloodFill(x + 1, y + 1, color, ref count);
                FloodFill(x    , y + 1, color, ref count);
            }            
        }
    }

    /// <summary>
    /// �}�[�N�����A���폜
    /// </summary>
    /// <param name="x">�s</param>
    /// <param name="y">��</param>
    private void BobbleDelete(int x, int y)
    {
        // ���[���E�[�𒴂����烊�^�[��
        // ��񂾂����ꍇy % 2�̓�����1�ɂȂ�̂ŁA�݂��Ⴂ���l�����邱�Ƃ��ł���
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // �V�ӂ���ӂ𒴂����烊�^�[��
        if ((y < 0) || (y >= BOBBLE_ROW_MAX)) return;

        // �}�[�N���ꂽ���̂Ȃ�A�폜
        if (bobbles[y][x] == BobbleColor.Delete)
        {
            bobbleGroups[y].DestroyChildBobble(x);
            bobbles[y][x] = BobbleColor.None;
            Debug.Log("�폜�I�I");

            // �ċA����
            // �����s����s���ŃA�N�Z�X����A�̈ʒu��ς���
            if (y % 2 == 0)
            {
                // �����s
                BobbleDelete(x - 1, y    );
                BobbleDelete(x - 1, y - 1);
                BobbleDelete(x    , y - 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x    , y + 1);
                BobbleDelete(x - 1, y + 1);
            }
            else
            {
                // ��s
                BobbleDelete(x - 1, y    );
                BobbleDelete(x    , y - 1);
                BobbleDelete(x + 1, y + 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x + 1, y + 1);
                BobbleDelete(x    , y + 1);
            }
        }
    }
}
