using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShapeType
{
    None,
    Ruler,
    Rectangle,
    Circle,
    Triangle
}

public abstract class Shape : Annotation
{
    protected LineRenderer shapeLine;

    protected List<Transform> vertices;
    protected ShapeType shapeType;
    protected Color shapeColor;
    protected float area;

    public abstract void Draw();
    public abstract void CalculateArea();
}