using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    public SaveData saveData;   // �Z�[�u�f�[�^�N���X

    private string filePath;    // �Z�[�u�f�[�^�̃t�@�C���p�X

    protected override void Awake()
    {
        base.Awake();

        // �V�[���J�ڎ���Destroy����Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);

        // �t�@�C���p�X��Unity�̃f�t�H���g
        filePath = Application.persistentDataPath + "/" + ".savedata.json";

        saveData = new SaveData();
        Debug.Log(filePath);
        Load();
    } 

    /// <summary>
    /// �v���C��̃X�R�A��S�Ẵn�C�X�R�A�Ɣ�r
    /// </summary>
    /// <param name="score"></param>
    /// <returns>�}�������C���f�b�N�X�A�X�V�Ȃ��Ȃ�-1</returns>
    public int ScoreCompare(int score)
    {
        // List�ɔz����R�s�[
        List<int> scores = new List<int>();
        scores.AddRange(saveData._highScore);

        for(int i = 0; i < 10; i++)
        {
            if(scores[i] < score)
            {
                // �n�C�X�R�A��荂�������̂ōX�V
                // ���������ꏊ�ɑ}�����A���ӂꂽ�������폜
                scores.Insert(i, score);
                scores.Remove(scores.Count - 1);

                // ���̔z��ɃR�s�[���ĕۑ����A�����𔲂���
                saveData._highScore = scores.ToArray();
                Save();
            
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// �Z�[�u
    /// </summary>
    public void Save()
    {
        JsonSave();
    }

    /// <summary>
    /// ���[�h
    /// </summary>
    public void Load()
    {
        if (!File.Exists(filePath)) return;
        var streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        saveData = JsonUtility.FromJson<SaveData>(data);
    }

    /// <summary>
    /// JSON�`���ŃZ�[�u����
    /// </summary>
    private void JsonSave()
    {
        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    /// <summary>
    /// �Z�[�u�f�[�^�N���X
    /// </summary>
    [Serializable]
    public class SaveData
    {
        [SerializeField] public int[] _highScore = new int[10];
    }
}
