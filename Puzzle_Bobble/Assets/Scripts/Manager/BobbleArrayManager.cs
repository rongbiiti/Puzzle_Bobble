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
    public static int BOBBLE_ROW_MAX = 27;

    /// <summary>
    /// �A�̋����s�̌�
    /// </summary>
    public int BOBBLE_EVEN_SIZE = 10;

    /// <summary>
    /// Start���ɖA�O���[�v�����s���������邩
    /// </summary>
    [SerializeField] private int _onStartCreateBobbleRow = 23;

    // �A���i�[����z��
    public List<List<BobbleColor>> bobbles = new List<List<BobbleColor>>();

    // ��Ɨp�z��
    private List<List<BobbleColor>> workBobbles = new List<List<BobbleColor>>();

    // �A�O���[�v�I�u�W�F�N�g�̔z��
    private List<BobbleGroup> bobbleGroups = new List<BobbleGroup>();

    private float bobbleCreateStartPosY = 9.52f;        // ��ԍŏ��ɐ�������A�O���[�v��Y���W
    private float bobbleCreateIncreaseYPos = 0.52f;      // 2�ڈȍ~�̖A�O���[�v�������ɂ��̒l��Y���W����ɂ��炵�Đ���
    private float nextBobbleGroupCreateWaitTime = 1060 * 0.0167f; // ���̖A�O���[�v�����܂ł̑ҋ@����
    private float waitTime;


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

    private void Update()
    {
        // �Q�[���I�[�o�[�ɂȂ��Ă��珈�����Ȃ�
        if (GameManager.Instance.isBobbleFalloutGameOverZone) return;

        // �A�̍폜���o��
        if (GameManager.Instance.isBobbleDeleting)
        {
            

            return;
        }

        // �A�����ҋ@���Ԃ����Z
        waitTime += Time.deltaTime;

        // �ҋ@���Ԃ��z������A�𐶐�
        if(nextBobbleGroupCreateWaitTime <= waitTime)
        {
            AddNewBobbleGroup();
            waitTime = 0;
        }
    }


    private void AddNewBobbleGroup()
    {
        foreach (var bg in bobbleGroups)
        {
            bg.ChangeRowNum(bg.rowNum + 2);
        }

        Destroy(bobbleGroups[bobbleGroups.Count - 1].gameObject);
        Destroy(bobbleGroups[bobbleGroups.Count - 2].gameObject);

        //bobbles.RemoveRange(bobbles.Count - 2, 2);

        bobbles.RemoveAt(bobbles.Count - 1);
        bobbles.RemoveAt(bobbles.Count - 1);

        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);
        bobbleGroups.RemoveAt(bobbleGroups.Count - 1);
        //bobbleGroups.RemoveRange(bobbleGroups.Count - 2, 2);

        bobbles.Insert(0, new List<BobbleColor>());
        bobbles.Insert(0, new List<BobbleColor>());

        CreateBobbleGroupObject(1);
        CreateBobbleGroupObject(0);

        int y = 0;
        foreach(var b in bobbles)
        {
            Debug.Log(y++ + string.Join(", ", b.Select(obj => obj.ToString())));
        }

        Debug.Log("bobbles�̐� : " + y);
        Debug.Log("bobbleGroups�̐� : " + bobbleGroups.Count);

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

        //bobbles[rowNum].Clear();

        // �����Ɗ�ŃT�C�Y���Ⴄ�̂ŕ�����
        if (rowNum % 2 == 0)
        {
            // ����
            for (int j = 0; j < BOBBLE_EVEN_SIZE; j++)
            {
                // �z��ɖA�̏���ǉ�
                //bobbles[rowNum] = bobbleGroup.bobbleColors;
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);

            }

        }
        else
        {
            // �
            for (int j = 0; j < BOBBLE_EVEN_SIZE - 1; j++)
            {
                //bobbles[rowNum] = bobbleGroup.bobbleColors;
                bobbles[rowNum].Insert(j, bobbleGroup.bobbleColors[j]);
            }

        }

        //bobbles.Insert(rowNum, bobbleGroup.bobbleColors);

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
                // �Ǘ��z��̏����X�V
                g.bobbles[x] = bobbleComponent;
                g.bobbleColors[x] = color;
                //g.bobbleColors.Insert(x, color);
                //bobbles[y] = g.bobbleColors;
                BobbleColor tmp = g.bobbleColors[x];
                bobbles[y][x] = tmp;

                //bobbles[y].RemoveAt(x);
                //bobbles[y].Insert(x, g.bobbleColors[x]);
                //bobbles.Insert(y, g.bobbleColors);

                Debug.Log(bobbleComponent.name + "��" + g.name + "�̎q�v�f�ɂȂ����B �F : " + color);

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

        Debug.Log(string.Join(", ", bobbles[y - 1].Select(obj => obj.ToString())));

        // ���˂����ʂƃq�b�g�����A�̐F��������
        if (bobbles[y][x] == color)
        {
            Debug.Log("�q�b�g�����A�̐F : " + bobbles[y][x]);

            // �ʂƓ����F�������̂ŁA�q�����Ă�����̂��}�[�N���Ă����B
            FloodFill(x, y, color, ref deleteCount);

            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            

            if(3 <= deleteCount)
            {
                // 3�ȏ�q�����Ă����̂ŁA�폜���
                GameManager.Instance.isBobbleDeleting = true;
                StartCoroutine(BobbleDeleteCoroutine(x, y));
            }
            else
            {
                // 3�q�����ĂȂ������̂ŁA�z����}�[�N��Ƃ���O�̏�Ԃɖ߂�
                
                //Debug.Log("�A��3�ȏ�q�����Ă��Ȃ�����");
                //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
                //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
                //bobbles = workBobbles.DeepCopy();
            }
            
        }
        else
        {
            Debug.Log("�q�b�g�����A�̐F : " + bobbles[y][x]);
            return false;
        }

        if(deleteCount < 3)
        {
            Debug.Log("�A��3�ȏ�q�����Ă��Ȃ�����");
            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
            bobbles = workBobbles.DeepCopy();
            //Debug.Log(string.Join(", ", workBobbles[y].Select(obj => obj.ToString())));
            //Debug.Log(string.Join(", ", bobbles[y].Select(obj => obj.ToString())));
        }

        return true;
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
        if ((y < 0) || (y > BOBBLE_ROW_MAX)) return;

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
                FloodFill(x + 1, y - 1, color, ref count);

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
            ScoreManager.Instance.AddScore(true);
            bobbleGroups[y].DestroyChildBobble(x);
            bobbles[y][x] = BobbleColor.None;
            
            Debug.Log("�폜�I�I Y : " + y + " X : " + x);

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
                BobbleDelete(x + 1, y - 1);

                BobbleDelete(x + 1, y    );
                BobbleDelete(x + 1, y + 1);
                BobbleDelete(x    , y + 1);
            }
        }
    }

    /// <summary>
    /// �V��ƌq�����Ă���A���}�[�N����
    /// </summary>
    /// <param name="x">��</param>
    /// <param name="y">�s</param>
    private void MarkNotConnectedCeilBobbles(int x, int y)
    {
        // ���[���E�[�𒴂����烊�^�[��
        // ��񂾂����ꍇy % 2�̓�����1�ɂȂ�̂ŁA�݂��Ⴂ���l�����邱�Ƃ��ł���
        if ((x < 0) || (x >= BOBBLE_EVEN_SIZE - (y % 2))) return;

        // �V�ӂ���ӂ𒴂����烊�^�[��
        if ((y < 0) || (y > BOBBLE_ROW_MAX)) return;

        if (BobbleColor.Blue <= bobbles[y][x] && bobbles[y][x] < BobbleColor.Max)
        {
            // ����m�F�p�@�F��Ԃɂ���
            //bobbleGroups[y].SetChildBobbleColor(x, BobbleColor.Red);

            // �V��ƂȂ����Ă���A�Ƃ��ă}�[�N����
            bobbles[y][x] = BobbleColor.NotConnectedCeil;

            // �ċA����
            // �����s����s���ŃA�N�Z�X����A�̈ʒu��ς���
            if (y % 2 == 0)
            {
                // �����s
                MarkNotConnectedCeilBobbles(x - 1, y);
                MarkNotConnectedCeilBobbles(x - 1, y - 1);
                MarkNotConnectedCeilBobbles(x, y - 1);

                MarkNotConnectedCeilBobbles(x + 1, y);
                MarkNotConnectedCeilBobbles(x, y + 1);
                MarkNotConnectedCeilBobbles(x - 1, y + 1);
            }
            else
            {
                // ��s
                MarkNotConnectedCeilBobbles(x - 1, y);
                MarkNotConnectedCeilBobbles(x, y - 1);
                MarkNotConnectedCeilBobbles(x + 1, y - 1);

                MarkNotConnectedCeilBobbles(x + 1, y);
                MarkNotConnectedCeilBobbles(x + 1, y + 1);
                MarkNotConnectedCeilBobbles(x, y + 1);
            }
        }
    }

    /// <summary>
    /// �V�䂩��t���b�h�t�B���ŐF������Z�����}�[�N����
    /// </summary>
    private void DeleteNotConnectedCeillingBobbles()
    {
        // �}�[�N�O�̏�Ԃ��R�s�[
        workBobbles = bobbles.DeepCopy();

        // �V��̍s�̖A���J�n��Ƃ��ă}�[�N���Ă���
        for (int i = 0; i < BOBBLE_EVEN_SIZE; i++)
        {
            // 0�si��̖A���F�t���i�}�[�N�ς݂�None�ł���j�łȂ���Ώ����X�L�b�v
            if(bobbles[0][i] <= BobbleColor.None || BobbleColor.Max <= bobbles[0][i])
            {
                continue;
            }

            // �V��ƌq�����Ă�����}�[�N
            MarkNotConnectedCeilBobbles(i, 0);
        }

        // �}�[�N��A��S�ă`�F�b�N����
        for( int y = 0; y < BOBBLE_ROW_MAX; y++)
        {
            for(int x = 0; x < BOBBLE_EVEN_SIZE - (y % 2); x++)
            {
                // �A���F�t���Ȃ�}�[�N����Ă��Ȃ��A�V��ƌq�����Ă��Ȃ��̂ō폜����
                if (BobbleColor.Blue <= bobbles[y][x] && bobbles[y][x] < BobbleColor.Max)
                {
                    ScoreManager.Instance.AddScore(false);
                    bobbleGroups[y].DestroyChildBobble(x);
                    bobbles[y][x] = BobbleColor.None;
                    
                    Debug.Log("�����Ă�A���폜�I�I Y : " + y + " X : " + x);
                }
                else
                {
                    // �}�[�N����Ă�����A�}�[�N�O�̏�Ԃɖ߂�
                    //bobbles[y][x] = workBobbles[y][x];
                    BobbleColor tmp = workBobbles[y][x];
                    bobbles[y][x] = tmp;
                }
            }
        }        
    }   

    private IEnumerator BobbleDeleteCoroutine(int x, int y)
    {
        // 1f�҂�
        yield return new WaitForEndOfFrame();

        // �R���{�����Z�b�g
        ScoreManager.Instance.Combo = 0;

        // �A���폜
        BobbleDelete(x, y);

        // �폜�A�j���N���b�v�̒������҂�
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        // �������A���폜        
        DeleteNotConnectedCeillingBobbles();

        // �폜�A�j���N���b�v�̒������҂�
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        // �폜���o���t���O��܂�
        GameManager.Instance.isBobbleDeleting = false;

    }
}
