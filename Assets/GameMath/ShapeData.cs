using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShapeData 
{
    public string ShapeId;
    public Color ShapeColor;

    public ShapeData(string shapeId, Color shapeColor) {
        ShapeId = shapeId;
        ShapeColor = shapeColor;
    }
}
