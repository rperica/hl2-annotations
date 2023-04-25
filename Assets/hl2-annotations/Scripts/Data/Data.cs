using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<RulerData> rulerDataList;
    public List<RectangleData> rectangleDataList;
    public List<CircleData> circleDataList;
    public List<TriangleData> triangleDataList;
    public List<TextData> textDataList;
}

[System.Serializable]
public class Data 
{
    public string annotationID;
    public AnnotationType annotationType;
}

#region ShapesData

[System.Serializable]
public class RulerData : Data
{
    public ShapeType shapeType;
    public Color color;
    public float distance;

    public Vec3[] vertices;
}

[System.Serializable]
public class RectangleData : Data
{
    public ShapeType shapeType;
    public Color color;
    public float area;

    public Vec3[] vertices;
    public Vec3 position;
}

[System.Serializable]
public class CircleData : Data
{
    public ShapeType shapeType;
    public Color color;
    public float area;

    public float radius;

    public Vec3[] vertices;
    public Vec3 position;
}

[System.Serializable]
public class TriangleData : Data
{
    public ShapeType shapeType;
    public Color color;
    public float area;

    public Vec3[] vertices;
    public Vec3 position;
}

#endregion


[System.Serializable]
public class TextData : Data
{
    public string text;
    public Vec3 position;
}

[System.Serializable]
public class Vec3
{
    public double x;
    public double y;
    public double z;

    public Vec3(double _x, double _y, double _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}