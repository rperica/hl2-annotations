using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

public class Rectangle : Shape
{
    #region Fields
    [SerializeField] private Transform verticesTransform;
    private Vector3[] initialPositions;
    private const int NUM_OF_VERTICES = 4;
    #endregion
    
    public override void CalculateArea()
    {
        Vector3 verticeA = vertices.ElementAt(0).localPosition;
        Vector3 verticeB = vertices.ElementAt(1).localPosition;
        Vector3 verticeC = vertices.ElementAt(2).localPosition;

        float a = Vector3.Distance(verticeA, verticeB);
        float b = Vector3.Distance(verticeB, verticeC);

        area = a * b;
    }

    public override void Delete()
    {
        Debug.Log("this object is " + gameObject.name);
        Destroy(gameObject);

        AnnotationsManager.Instance.RemoveAnnotation();
    }

    public override void Draw()
    {
        shapeLine.material.color = isSelected ? Color.cyan : shapeColor;

        shapeLine.positionCount = vertices.Count;

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            shapeLine.SetPosition(pos, vertices.ElementAt(pos).position);
        }
    }

    public override T Save<T>()
    {
        CalculateArea();

        RectangleData data = new RectangleData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Shape;
        data.shapeType = ShapeType.Rectangle;

        data.color = shapeColor;
        data.area = area;

        data.vertices = new Vec3[vertices.Count];

        data.position = Utility.ConvertToVec3(transform.localPosition);

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            data.vertices[pos] = Utility.ConvertToVec3(vertices.ElementAt(pos).localPosition);
        }

        return data as T;
    }

    protected override void GenerateID()
    {
        annotationID = "rectangle" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    public void LoadRectangle(RectangleData data)
    {
        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Shape;

        shapeType = ShapeType.Rectangle;
        shapeColor = data.color;
        area = data.area;

        transform.localPosition = Utility.ConvertToVector3(data.position);

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            vertices.ElementAt(pos).localPosition = Utility.ConvertToVector3(data.vertices[pos]);
        }
    }

    public void Reset()
    {
        for (int pos = 0; pos < NUM_OF_VERTICES; pos++)
        {
            vertices.ElementAt(pos).localPosition = initialPositions[pos];
        }
    }

    public void SelectAnnotation()
    {
        isSelected = true;
        AnnotationsManager.Instance.SelectShapeAnnotation(annotationID, shapeType);
    }

    public void Initialize()
    {
        if (string.IsNullOrEmpty(annotationID))
        {
            GenerateID();
        }

        shapeLine = gameObject.GetComponent<LineRenderer>();

        vertices = new List<Transform>();

        verticesTransform = transform.GetChild(0).transform;
        foreach (Transform vertice in verticesTransform)
        {
            vertices.Add(vertice);
        }

        for(int pos=0; pos < vertices.Count; pos++)
        {
            vertices.ElementAt(pos).GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(data => SelectAnnotation());
        }

        initialPositions = new Vector3[NUM_OF_VERTICES];
        for (int pos = 0; pos < NUM_OF_VERTICES; pos++)
        {
            initialPositions[pos] = vertices.ElementAt(pos).localPosition;
        }

        annotationType = AnnotationType.Shape;
        shapeType = ShapeType.Rectangle;
        shapeColor = Color.black;
        shapeLine.material.color = shapeColor;
    }

    private void Resize(int verticeIndex)
    {
        int nextVertice;
        int prevVertice;

        Vector3 prevVerticePos = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 nextVerticePos = new Vector3(0.0f, 0.0f, 0.0f);

        if(verticeIndex==0)
        {
            nextVertice = verticeIndex + 1;
            prevVertice = vertices.Count - 1;

            prevVerticePos.Set(vertices.ElementAt(verticeIndex).localPosition.x, vertices.ElementAt(prevVertice).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);
            nextVerticePos.Set(vertices.ElementAt(nextVertice).localPosition.x, vertices.ElementAt(verticeIndex).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);

            vertices.ElementAt(prevVertice).localPosition = prevVerticePos;
            vertices.ElementAt(nextVertice).localPosition = nextVerticePos;
        }
        else if(verticeIndex==1)
        {
            nextVertice = verticeIndex + 1;
            prevVertice = verticeIndex - 1;

            prevVerticePos.Set(vertices.ElementAt(prevVertice).localPosition.x, vertices.ElementAt(verticeIndex).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);
            nextVerticePos.Set(vertices.ElementAt(verticeIndex).localPosition.x, vertices.ElementAt(nextVertice).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);

            vertices.ElementAt(prevVertice).localPosition = prevVerticePos;
            vertices.ElementAt(nextVertice).localPosition = nextVerticePos;
        }
        else if(verticeIndex==2)
        {
            nextVertice = verticeIndex + 1;
            prevVertice = verticeIndex - 1;

            prevVerticePos.Set(vertices.ElementAt(verticeIndex).localPosition.x, vertices.ElementAt(prevVertice).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);
            nextVerticePos.Set(vertices.ElementAt(nextVertice).localPosition.x, vertices.ElementAt(verticeIndex).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);

            vertices.ElementAt(prevVertice).localPosition = prevVerticePos;
            vertices.ElementAt(nextVertice).localPosition = nextVerticePos;
        }
        else if(verticeIndex==3)
        {
            nextVertice = 0;
            prevVertice = verticeIndex - 1;

            prevVerticePos.Set(vertices.ElementAt(prevVertice).localPosition.x, vertices.ElementAt(verticeIndex).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);
            nextVerticePos.Set(vertices.ElementAt(verticeIndex).localPosition.x, vertices.ElementAt(nextVertice).localPosition.y, vertices.ElementAt(verticeIndex).localPosition.z);

            vertices.ElementAt(prevVertice).localPosition = prevVerticePos;
            vertices.ElementAt(nextVertice).localPosition = nextVerticePos;
        }

        CalculateArea();
    }

    private void ResizeCheck()
    {
        foreach (Transform vertice in vertices)
        {
            int verticeIndex = vertices.IndexOf(vertice);
            if(vertice.hasChanged)
            {
                Resize(verticeIndex);
                vertice.hasChanged = false;
            }
        } 
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        Draw();
        ResizeCheck();
    }
}
