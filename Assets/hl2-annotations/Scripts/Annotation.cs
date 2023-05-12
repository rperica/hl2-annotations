using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnnotationType
{
    None,
    Shape,
    Text,
    Paint
}

public abstract class Annotation : MonoBehaviour
{
    protected string annotationID;
    protected AnnotationType annotationType;
    protected bool isSelected;

    public string AnnotationID { get { return annotationID; } }
    public AnnotationType AnnotationType { get { return annotationType; } }

    public bool IsSelected 
    { 
        get { return isSelected; } 
        set { isSelected = value; } 
    }

    protected abstract void GenerateID();
    public abstract void Delete();

    public abstract T Save<T>() where T : Data;
}
