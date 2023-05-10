using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;

public class PaintManager : Singleton<PaintManager>, IMixedRealityPointerHandler
{
    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public Material drawingMaterial;
    public Color32 drawingColor;
    public MeshRenderer resultColorMesh;
    public float startWidth = 0.04f;
    public float endWidth = 0.04f;

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        gameObject.SetActive(false);
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        AddDrawing();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        FreeDraw(eventData);
    }

    private void Start()
    {
        Material lineMaterial = Instantiate(drawingMaterial);
        lineMaterial.color = drawingColor;
        //resultColorMesh.material = lineMaterial;
    }

    private void AddDrawing()
    {
        GameObject drawing = Instantiate(drawingPrefab);
        lineRenderer = drawing.GetComponent<LineRenderer>();
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
    }

    private void FreeDraw(MixedRealityPointerEventData eventData)
    {
        Material lineMaterial = Instantiate(drawingMaterial);
        lineMaterial.color = drawingColor;
        lineRenderer.material = lineMaterial;
        var handPos = eventData.Pointer.Position;
        Vector3 mousePos = new Vector3(handPos.x, handPos.y, handPos.z);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
    }
}
