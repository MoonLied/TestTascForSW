using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HelpJson;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance => _instance;
    private static UIControl _instance;

    [SerializeField] private GameObject _backButton;
    [SerializeField] private Transform _panelA;
    [SerializeField] private Transform _panelB;

    private Dictionary<string, GameObject> _shapeDict;
    private Dictionary<Color, GameObject> _colorTextDict;
    private Dictionary<string, Color> _shapeColorDict;

    private string _targetShapeId;
    private GameObject _targetColor;

    private Transform _shapesPool;

    void Awake() {
        if (_instance == null) _instance = this;
        _backButton.GetComponent<Button>().onClick.AddListener(() => EscFindShape());
        _backButton.SetActive(false);
    }

    void Start() {
        Transform _shapesPool = new GameObject("ShapesPool").transform;

        List<ShapeData> shepesList = LoadGameSave();
        _shapeDict = new Dictionary<string, GameObject>();
        _colorTextDict = new Dictionary<Color, GameObject>();
        _shapeColorDict = new Dictionary<string, Color>();
        Debug.Log(shepesList.Count);
        if (shepesList.Count == 0)
        {
            DefaultRespawn();
        }
        else {
            SaveDataRespawn(shepesList);
        }
        CreateColorPanel();
        _panelB.gameObject.SetActive(false);
    }


    private void DefaultRespawn() {
        int x = 0;

        Dictionary<string, GameObject> shapePrefDict = PrefabManager.Instance.ShapesDict;
        foreach (KeyValuePair<string, ShapeData> item in DictionaryManager.Instance.DefaultShapesDict) {
            CreateShape(item.Key, x, item.Value.ShapeColor);
            x++;
        }

        //проверяем, что если в словаре дефотных настроек фигур меньше чем, добавленных в игру.
        if (_shapeDict.Count < shapePrefDict.Count) {
            foreach (KeyValuePair<string, GameObject> itemPrefab in shapePrefDict)
            {
                if (_shapeDict.ContainsKey(itemPrefab.Key)) continue;
                CreateShape(itemPrefab.Key, x, Color.clear);
                x++;
            }
        }
    }

    private void SaveDataRespawn(List<ShapeData> shepesList)
    {
        int x = 0;
        for(int i = 0; i < shepesList.Count;i++)
        {
            ShapeData shape = shepesList[i];
            CreateShape(shape.ShapeId, x, shape.ShapeColor);
            x++;
        }

        Dictionary<string, GameObject> shapePrefDict = PrefabManager.Instance.ShapesDict;
        //проверяем, что если в сохранение фигур меньше чем, добавленных в игру.
        if (_shapeDict.Count < shapePrefDict.Count)
        {
            foreach (KeyValuePair<string, GameObject> itemPrefab in shapePrefDict)
            {
                if (_shapeDict.ContainsKey(itemPrefab.Key)) continue;
                CreateShape(itemPrefab.Key, x, Color.clear);
                x++;
            }
        }
    }


    private void CreateShape(string shapeId, int x, Color color)
    {
        if (!PrefabManager.Instance.ShapesDict.ContainsKey(shapeId)) return; //если по каким либо причинам в сохранение окажется больше фигур чем добавленно

        GameObject obj = Instantiate(PrefabManager.Instance.ShapesDict[shapeId], _shapesPool);
        obj.transform.position = new Vector3(x * DictionaryManager.ShapeDist, 0, 0);

        if (color == Color.clear)
        {
            color = new Color(Random.value, Random.value, Random.value); //Т.к. эта фигура небыла указана в дефолтных настройках, присваиваем ей рандомный цвет.
        }
        obj.GetComponent<MeshRenderer>().material.color = color;
        _shapeDict[shapeId] = obj;
        _shapeColorDict[shapeId] = color;
        x++;
        
        GameObject buttonObj = Instantiate(PrefabManager.Instance.UIDict["ShapeBtt"], _panelA);
        buttonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shapeId;
        buttonObj.GetComponent<Button>().onClick.AddListener(() => FindShape(shapeId));
    }

    private void CreateColorPanel() {
        List<Color> colorsList =  DictionaryManager.Instance.ColorsList;
        for (int i = 0; i < colorsList.Count; i++) {
            GameObject colorBttObj = Instantiate(PrefabManager.Instance.UIDict["ColorBtt"], _panelB);
            Color color = colorsList[i];
            colorBttObj.GetComponent<Image>().color = color;
            GameObject text = colorBttObj.transform.GetChild(0).gameObject;
            colorBttObj.GetComponent<Button>().onClick.AddListener(()=> ChangeShapeColor(color, text));
            _colorTextDict[color] = text;
        }
       
    }

    private void ChangeShapeColor(Color color, GameObject text) {
        if (_targetColor != null) _targetColor.gameObject.SetActive(false);
        text.SetActive(true);
        _targetColor = text;
        _shapeDict[_targetShapeId].GetComponent<MeshRenderer>().material.color = color;
        _shapeColorDict[_targetShapeId] = color;
    }

    private void FindShape(string shapeId)
    {
        GameObject obj = _shapeDict[shapeId];
        _targetShapeId = shapeId;
        Vector3 pos = obj.transform.position;
        CamControl.Instance.GoShowShape(new Vector3(pos.x, pos.y + 1, pos.z - 2), pos);

        for (int i = 0; i < _panelA.childCount; i++)
        {
            _panelA.GetChild(i).gameObject.SetActive(false);
        }
        _backButton.SetActive(true);
        _panelB.gameObject.SetActive(true);

        if (_targetColor != null) _targetColor.SetActive(false);
        Color color = obj.GetComponent<MeshRenderer>().material.color;
        if (_colorTextDict.ContainsKey(color)){
            GameObject newTargetColor = _colorTextDict[color];
            newTargetColor.SetActive(true);
            _targetColor = newTargetColor;
        }

    }

    private void EscFindShape()
    {
        CamControl.Instance.ExitShowShape();

        for (int i = 0; i < _panelA.childCount; i++)
        {
            _panelA.GetChild(i).gameObject.SetActive(true);
        }
        _backButton.SetActive(false);
        _panelB.gameObject.SetActive(false);
    }


    void OnDestroy() {
        List<ShapeData> ShapesList = new List<ShapeData>();
        foreach (KeyValuePair<string, Color> shape in _shapeColorDict) {
            ShapesList.Add(new ShapeData(shape.Key, shape.Value));
        }
        SaveGame(ShapesList);
    }

}
