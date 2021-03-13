using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class HelpJson
{

    public static GameDict GetGameDict()
    {
         GameDict wrapper = JsonUtility.FromJson<GameDict>(Resources.Load<TextAsset>("GameDict").text);
         return wrapper;
    }

    public static List<ShapeData> LoadGameSave()
    {
        if (!Resources.Load<TextAsset>("GameSave")) return new List<ShapeData>();
        string str = File.ReadAllText("Assets/Resources/GameSave.json");
        List<ShapeData> shapeList = JsonUtility.FromJson<GameSaveData>(str).ShapeList;
        return shapeList;
    }

    public static void SaveGame(List<ShapeData> ShapeList)
    {
        GameSaveData data = new GameSaveData();
        data.ShapeList = ShapeList;
        string str = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/GameSave.json", str);
    }

    [Serializable]
    public class GameDict
    {
        public List<Color> ColorList;
        public List<ShapeData> DefaultStart;
    }


    [Serializable]
    public class GameSaveData
    {
        public List<ShapeData> ShapeList;
    }

}
