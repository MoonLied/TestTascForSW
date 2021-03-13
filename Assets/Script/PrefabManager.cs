using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance => _instance;
    private static PrefabManager _instance;

    [SerializeField] private ListStringGameObject _shapesLists;
    [SerializeField] private ListStringGameObject _uiLists;

    public Dictionary<string, GameObject> ShapesDict { get; private set; }
    public Dictionary<string, GameObject> UIDict { get; private set; }

    void Awake()
    {
        if (_instance == null) _instance = this;

        ShapesDict = new Dictionary<string, GameObject>();
        for (int shapeNum = 0; shapeNum < _shapesLists.PrefabKeys.Count; shapeNum++)
        {
            ShapesDict[_shapesLists.PrefabKeys[shapeNum]] = _shapesLists.GameObjects[shapeNum];
        }

        UIDict = new Dictionary<string, GameObject>();
        for (int uiNum = 0; uiNum < _uiLists.PrefabKeys.Count; uiNum++)
        {
            UIDict[_uiLists.PrefabKeys[uiNum]] = _uiLists.GameObjects[uiNum];
        }
        Instantiate(UIDict["UI"]);
    }
}
