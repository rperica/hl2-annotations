using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class Ruler : Shape
{
    [SerializeField] private Transform verticesTransform;

    public void LoadRuler(RulerData data)
    {
        foreach(Transform vertice in verticesTransform)
        {
            vertice.GetComponent<ObjectManipulator>().OnManipulationStarted.RemoveAllListeners();
            vertice.GetComponent<ObjectManipulator>().OnManipulationEnded.RemoveAllListeners();
            Destroy(vertice.gameObject);
        }
        vertices.Clear();

        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Shape;

        shapeType = ShapeType.Ruler;
        shapeColor = data.color;
        area = data.distance;

        for(int pos=0; pos<data.vertices.Length; pos++)
        {
            AddPoint(Utility.ConvertToVector3(data.vertices[pos]));
        }
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
            vertice.gameObject.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(data => SelectAnnotation());
            vertice.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(data => CalibrateRuler());
            
            vertices.Add(vertice);
        }

        annotationType = AnnotationType.Shape;
        shapeType = ShapeType.Ruler;
        shapeColor = Color.black;
        shapeLine.material.color = shapeColor;
    }

    public void AddPoint()
    { 
        GameObject vertice = Instantiate(AnnotationsManager.Instance.verticePrefab, verticesTransform);
        vertice.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(data => SelectAnnotation());
        vertice.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(data => CalibrateRuler());
        vertice.GetComponent<TapToPlace>().enabled = true;

        vertices.Add(vertice.transform);
    }
    
    public void AddPoint(Vector3 position)
    {
        GameObject vertice = Instantiate(AnnotationsManager.Instance.verticePrefab, verticesTransform);
        vertice.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(data => SelectAnnotation());
        vertice.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(data => CalibrateRuler());

        vertice.transform.localPosition = position;

        vertices.Add(vertice.transform);
    }

    public void RemoveLastPoint()
    {
        if(vertices.Count > 0)
        {
            Destroy(vertices.Last().gameObject);
            vertices.RemoveAt(vertices.Count - 1);
        }
        else
        {
            Delete();
        }
    }

    public override void Delete()
    {
        Debug.Log("this object is " + gameObject.name);
        Destroy(gameObject);
    
        AnnotationsManager.Instance.RemoveAnnotation();
    }

    public override void CalculateArea()
    {
        area = 0.0f;
        for(int pos=0; pos < vertices.Count - 1; pos++)
        {
            area += Vector3.Distance(vertices.ElementAt(pos).localPosition, vertices.ElementAt(pos + 1).localPosition);
        }
    }

    public override void Draw()
    {
        shapeLine.material.color = isSelected ? Color.cyan : shapeColor;

        shapeLine.positionCount = vertices.Count;

        for(int pos=0; pos<vertices.Count; pos++)
        {
            shapeLine.SetPosition(pos, vertices.ElementAt(pos).position);
        }
    }

    protected override void GenerateID()
    {
        annotationID = "ruler" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    public void SelectAnnotation()
    {
        isSelected = true;
        AnnotationsManager.Instance.SelectShapeAnnotation(annotationID, shapeType);
    }

    // Test version
    private void CalibrateRuler()
    {
        float max = 0.5f;
        float min = -0.5f;

        for(int pos=0; pos < vertices.Count; pos++)
        {
            float x = 1.0f;
            float y = vertices.ElementAt(pos).localPosition.y;
            float z = vertices.ElementAt(pos).localPosition.z;

            if(y>max)
            {
                y = max;
            }
            else if(y<min)
            {
                y = min;
            }

            if (z > max)
            {
                z = max;
            }
            else if (z < min)
            {
                z = min;
            }

            vertices.ElementAt(pos).localPosition = new Vector3(x, y, z);
        }

        CalculateArea();
    }

    public override T Save<T>()
    {
        RulerData data = new RulerData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Shape;
        data.shapeType = ShapeType.Ruler;

        data.color = shapeColor;
        data.distance = area;

        data.vertices = new Vec3[vertices.Count];

        for (int pos = 0; pos < vertices.Count; pos++)
        {
            data.vertices[pos] = Utility.ConvertToVec3(vertices.ElementAt(pos).localPosition);
        }

        return data as T;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        Draw();

        //Test
        if(Input.GetKeyDown(KeyCode.L))
        {
            CalculateArea();
            Debug.Log(area.ToString());
        }
    }
}