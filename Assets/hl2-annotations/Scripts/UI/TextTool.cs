using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTool : MonoBehaviour
{
    [SerializeField] private GameObject textToolObject;
    [SerializeField] private bool isActive;

    public bool IsActive { get { return isActive; } }

    public void Toggle()
    {
        isActive = !isActive;
        textToolObject.SetActive(isActive);
    }

    public void ToggleOff()
    {
        isActive = false;
        textToolObject.SetActive(isActive);
    }
}