using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    private List<int> openedIslands = new List<int>();

    private void Start()
    {
        LoadData();
    }

    public void AddIsland(int islandId)
    {
        if (!openedIslands.Contains(islandId))
        {
            openedIslands.Add(islandId);
            SaveData();
        }
    }

    private void SaveData()
    {
        // List'i string olarak kaydetmek için JSON formatı kullanıyoruz
        string jsonData = JsonUtility.ToJson(new IslandData(openedIslands));
        PlayerPrefs.SetString("OpenedIslands", jsonData);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("OpenedIslands"))
        {
            string jsonData = PlayerPrefs.GetString("OpenedIslands");
            IslandData data = JsonUtility.FromJson<IslandData>(jsonData);
            openedIslands = data.openedIslands;
        }
    }

    public bool IsIslandOpened(int islandId)
    {
        return openedIslands.Contains(islandId);
    }
    
    public void PrintSavedIslands()
    {
        if (openedIslands.Count > 0)
        {
            Debug.Log("Açılan Adalar: ");
            foreach (int islandId in openedIslands)
            {
                Debug.Log("Ada ID: " + islandId);
            }
        }
        else
        {
            Debug.Log("Henüz açılan ada yok.");
        }
    }
}

[System.Serializable]
public class IslandData
{
    public List<int> openedIslands;

    public IslandData(List<int> openedIslands)
    {
        this.openedIslands = openedIslands;
    }
}