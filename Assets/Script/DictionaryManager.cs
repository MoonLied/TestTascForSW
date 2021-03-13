using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HelpJson;

public class DictionaryManager
{
    public static DictionaryManager Instance => _instance;
    private static DictionaryManager _instance;

    public const float ShapeDist = 3f;
    public List<Color> ColorsList { get; private set; }
    public Dictionary<string, ShapeData> DefaultShapesDict { get; private set; }

    public static void Initialize()
    {
        if (_instance == null) _instance = new DictionaryManager();
    }
    public DictionaryManager()
    {
        GameDict data = GetGameDict();
        ColorsList = data.ColorList;
        DefaultShapesDict = new Dictionary<string, ShapeData>();
        if (data.DefaultStart != null)
        {
            for (int i = 0; i < data.DefaultStart.Count; i++)
            {
                ShapeData shapeData = data.DefaultStart[i];
                DefaultShapesDict[shapeData.ShapeId] = shapeData;
            }
        }

    }


}
