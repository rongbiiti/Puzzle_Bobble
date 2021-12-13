using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    public SaveData saveData;   // セーブデータクラス

    private string filePath;    // セーブデータのファイルパス

    protected override void Awake()
    {
        base.Awake();

        // シーン遷移時にDestroyされないようにする
        DontDestroyOnLoad(gameObject);

        // ファイルパスはUnityのデフォルト
        filePath = Application.persistentDataPath + "/" + ".savedata.json";

        saveData = new SaveData();
        Debug.Log(filePath);
        Load();
    } 

    /// <summary>
    /// プレイ後のスコアを全てのハイスコアと比較
    /// </summary>
    /// <param name="score"></param>
    /// <returns>挿入したインデックス、更新なしなら-1</returns>
    public int ScoreCompare(int score)
    {
        // Listに配列をコピー
        List<int> scores = new List<int>();
        scores.AddRange(saveData._highScore);

        for(int i = 0; i < 10; i++)
        {
            if(scores[i] < score)
            {
                // ハイスコアより高かったので更新
                // 高かった場所に挿入し、あふれた末尾を削除
                scores.Insert(i, score);
                scores.Remove(scores.Count - 1);

                // 元の配列にコピーして保存し、処理を抜ける
                saveData._highScore = scores.ToArray();
                Save();
            
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// セーブ
    /// </summary>
    public void Save()
    {
        JsonSave();
    }

    /// <summary>
    /// ロード
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
    /// JSON形式でセーブする
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
    /// セーブデータクラス
    /// </summary>
    [Serializable]
    public class SaveData
    {
        [SerializeField] public int[] _highScore = new int[10];
    }
}
