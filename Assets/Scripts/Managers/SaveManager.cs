using System;
using UnityEngine;


[Serializable]
public class GameData
{
}

public class SaveManager : Singleton<SaveManager>
{
    public string prefsName = "Trickcal_Fan";

    private GameData gameData;

    public GameData GameData
    {
        get
        {
            if (gameData == null)
                LoadGameData();
            return gameData;
        }
    }

    protected override void OnCreated()
    {
        LoadGameData();
    }

    public void ResetSaveFile()
    {
        gameData = new GameData();
        SaveGameData();
        LoadGameData();
    }

    private void LoadGameData()
    {
        var s = PlayerPrefs.GetString(prefsName, "null");
        gameData = s.Equals("null") || string.IsNullOrEmpty(s) ? new GameData() : JsonUtility.FromJson<GameData>(s);
    }


    private void SaveGameData()
    {
        PlayerPrefs.SetString(prefsName, JsonUtility.ToJson(gameData));
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGameData();
    }
}