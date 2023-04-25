using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

public class Triangle : Shape
{
    #region fields
    [SerializeField] private Transform verticesTransform;
    private Vector3[] initialPositions;
    private const int NUM_OF_VERTICES = 3;
    #endregion

    public override void CalculateArea()
    {
        Vector3 verticeA = vertices.ElementAt(0).localPosition;
        Vector3 verticeB = vertices.ElementAt(1).localPosition;
        Vector3 verticeC = vertices.ElementAt(2).localPosition;

        float a = Vector3.Distance(verticeB, verticeC);
        float b = Vector3.Distance(verticeA, verticeC);
        float c = Vector3.Distance(verticeA, verticeB);

        float s = (a + b + c) / 2.0f;
            
        area = Mathf.Sqrt(s*(s-a)*(s-b)*(s-c));
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

        TriangleData data = new TriangleData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Shape;
        data.shapeType = ShapeType.Triangle;

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
        annotationID = "triangle" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    public void LoadTriangle(TriangleData data)
    {
        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Shape;

        shapeType = ShapeType.Triangle;
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

    private void Initialize()
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

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            vertices.ElementAt(pos).GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(data => SelectAnnotation());
        }

        initialPositions = new Vector3[NUM_OF_VERTICES];
        for (int pos = 0; pos < NUM_OF_VERTICES; pos++)
        {
            initialPositions[pos] = vertices.ElementAt(pos).localPosition;
        }

        annotationType = AnnotationType.Shape;
        shapeType = ShapeType.Triangle;
        shapeColor = Color.black;
        shapeLine.material.color = shapeColor;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        Draw();
    }
}
