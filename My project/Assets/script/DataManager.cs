using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if(!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public void StartGame()
    {
        string filepath = Application.persistentDataPath + "/" + GameDataFileName;

        if(File.Exists(filepath))
        {
            string FromJsonData = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print("불러오기 완료");
        }
    }

    public void EndGame()
    {
        for(int i = 0; i < data.TopScore.Length - 1; i++)
        {
            if (data.TopScore[i] < GameManager.Instance.Score)
            {
                for(int j = 4; j >= i; j--)
                {
                    data.TopScore[j + 1] = data.TopScore[j];
                    if (data.TopScore[j] <= 0) break;
                }
                data.TopScore[i] = GameManager.Instance.Score;

                break;
            }
        }

        for(int i = 0; i < 5; i++)
        {
            Debug.Log($"{i+1}. " +  data.TopScore[i]);
            if (data.TopScore[i] == 0) GameManager.Instance.text[i].GetComponent<Text>().text = $"{i + 1}. (Empty)";

            else GameManager.Instance.text[i].GetComponent<Text>().text = $"{i + 1}. " + data.TopScore[i];
        }

        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        print("저장 완료");
    }
  
}
