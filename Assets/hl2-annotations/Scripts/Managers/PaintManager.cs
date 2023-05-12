using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;

public class PaintManager : Singleton<PaintManager>, IMixedRealityPointerHandler
{
    public void OnPointerClicked(MixedRealityPointerEventData eventData) {}

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        gameObject.SetActive(false);
        AnnotationsManager.Instance.StopDrawing();
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        AnnotationsManager.Instance.CreateAndAddPaintAnnotation();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        AnnotationsManager.Instance.StartDrawing(eventData);
    }
}
