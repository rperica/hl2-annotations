using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
    None,
    ToolBarMenu,
    ShapeMenu,
    TextMenu,
}

public class Menu : MonoBehaviour
{
    [SerializeField] private MenuType menuType;
    [SerializeField] private GameObject menuObject;
    [SerializeField] private bool isActive;

    public MenuType MenuTypeProp { get { return menuType; } }
    public bool IsActive { get { return isActive; } }

    public void Toggle()
    {
        isActive = !isActive;
        menuObject.SetActive(isActive);
    }

    public void ToggleOff()
    {
        isActive = false;
        menuObject.SetActive(isActive);
    }
}