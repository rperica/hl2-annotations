// https://gist.github.com/Eumathes/0aac2c243d1d1ef4d87589a702728305
// Create a new script TouchEvent using the code below, and attach a newly created script to the desired GameObject

using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class TouchScript : MonoBehaviour, IMixedRealityTouchHandler
{
    [SerializeField]
    private Vector3 gameObjectPosition;
    void Start()
    {
        gameObject.AddComponent<NearInteractionTouchable>();
    }
    void IMixedRealityTouchHandler.OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        gameObjectPosition = gameObject.transform.localPosition;
        Debug.Log("OnTouchCompleted: " + gameObject.transform.localPosition.ToString());
        Debug.Log(gameObject.name.ToString());
    }

    void IMixedRealityTouchHandler.OnTouchStarted(HandTrackingInputEventData eventData) { /*Debug.Log("Started");*/ }
    void IMixedRealityTouchHandler.OnTouchUpdated(HandTrackingInputEventData eventData) { /*Debug.Log("Update");*/ }
}