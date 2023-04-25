using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTool : MonoBehaviour
{
    [SerializeField] private ShapeType shapeType;
    [SerializeField] private GameObject shapeToolObject;
    [SerializeField] private bool isActive;

    public ShapeType ShapeTypeProp { get { return shapeType; } }
    public bool IsActive { get { return isActive; } }

    public void Toggle()
    {
        isActive = !isActive;
        shapeToolObject.SetActive(isActive);
    }

    public void ToggleOff()
    {
        isActive = false;
        shapeToolObject.SetActive(isActive);
    }
}

