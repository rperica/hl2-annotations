using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

public class Circle : Shape
{
    #region Fields
    [SerializeField] private Transform verticesTransform;
    private Vector3[] initialPositions;
    private int NUM_OF_VERTICES = 4;

    public int steps = 100;
    public float radius = 0.3f;
    #endregion

    public override void CalculateArea()
    {
        area = radius * 2 * Mathf.PI;
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

        shapeLine.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0.0f);

            shapeLine.SetPosition(currentStep, currentPosition);
        }
    }

    public override T Save<T>()
    {
        CalculateArea();

        CircleData data = new CircleData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Shape;
        data.shapeType = ShapeType.Circle;

        data.color = shapeColor;
        data.area = area;

        data.radius = radius;

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
        annotationID = "circle" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    public void LoadCircle(CircleData data)
    {
        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Shape;

        shapeType = ShapeType.Circle;
        shapeColor = data.color;
        area = data.area;

        radius = data.radius;

        transform.localPosition = Utility.ConvertToVector3(data.position);

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            vertices.ElementAt(pos).localPosition = Utility.ConvertToVector3(data.vertices[pos]);
        }
    }

    public void Reset()
    {
        radius = 0.3f;

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
        shapeType = ShapeType.Circle;
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