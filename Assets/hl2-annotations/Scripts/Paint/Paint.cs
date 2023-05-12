using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;

public class Paint : Annotation
{
    private LineRenderer lineRenderer;

    private Vector3[] positions;

    public override void Delete()
    {
        Debug.Log("this object is " + gameObject.name);
        Destroy(gameObject);
    }

    public override T Save<T>()
    {
        PaintData data = new PaintData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Paint;

        data.positions = new Vec3[positions.Length];

        for (int pos = 0; pos < positions.Length; pos++)
        {
            data.positions[pos] = Utility.ConvertToVec3(positions[pos]);
        }

        return data as T;
    }

    public void LoadPaint(PaintData data)
    {
        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Paint;

        positions = new Vector3[data.positions.Length];

        for (int pos = 0; pos < positions.Length; pos++)
        {
            positions[pos] = Utility.ConvertToVector3(data.positions[pos]);
        }

        DrawFromLoad();
    }

    protected override void GenerateID()
    {
        annotationID = "paint" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    private void Initialize()
    {
        if (string.IsNullOrEmpty(annotationID))
        {
            GenerateID();
        }

        annotationType = AnnotationType.Paint;
        IsSelected = true;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = AnnotationsManager.Instance.startWidth;
        lineRenderer.endWidth = AnnotationsManager.Instance.endWidth;
    }

    public void UnSelectAndSavePositions()
    {
        IsSelected = false;

        positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        Material lineMaterial = Instantiate(AnnotationsManager.Instance.drawingMaterial);
        lineRenderer.material = lineMaterial;
        lineMaterial.color = AnnotationsManager.Instance.drawingColor;
    }

    public void FreeDraw(MixedRealityPointerEventData eventData)
    {
        var handPos = eventData.Pointer.Position;
        Vector3 mousePos = new Vector3(handPos.x, handPos.y, handPos.z);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
    }

    private void DrawFromLoad()
    {
        Material lineMaterial = Instantiate(AnnotationsManager.Instance.drawingMaterial);
        lineRenderer.material = lineMaterial;
        lineMaterial.color = AnnotationsManager.Instance.drawingColor;

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void Awake()
    {
        Initialize();
    }
}