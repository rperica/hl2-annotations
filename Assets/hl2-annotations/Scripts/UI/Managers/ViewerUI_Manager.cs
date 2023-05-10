using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Input;

public class ViewerUI_Manager : Singleton<ViewerUI_Manager>
{
    #region Fields
    [SerializeField] private List<Menu> menuList;
    [SerializeField] private List<ShapeTool> shapeToolsList;
    [SerializeField] private TextTool textTool;

    [SerializeField] private GameObject annotationViewerObject;
    [SerializeField] private GameObject keyboardObject; 

    [SerializeField] private TextMesh textUI;
    [SerializeField] private GazeProvider gazePos;

    private MenuType activeMenu;
    private ShapeType activeShapeTool;

    #endregion

    #region Properties
    public MenuType activeMenuInstance { get { return activeMenu; } }
    public ShapeType activeShapeToolInstance { get { return activeShapeTool; } }

    #endregion

    private void Awake()
    {
        activeMenu = MenuType.None;
        activeShapeTool = ShapeType.None;
    }

    #region ToolBarMenu

    public void ToggleMenu(string menuType)
    {
        MenuType type = (MenuType)Enum.Parse(typeof(MenuType), menuType);

        if (activeMenu != MenuType.None && activeMenu != type)
        {
            menuList.Find(menu => menu.MenuTypeProp == activeMenu).Toggle();
        }

        if(activeMenu==type)
        {
            menuList.Find(menu => menu.MenuTypeProp == type).Toggle();
            activeMenu = MenuType.None;
        }
        else
        {
            menuList.Find(menu => menu.MenuTypeProp == type).Toggle();
            activeMenu = type;
        }
    }

    public void Save()
    {
        AnnotationsManager.Instance.Save();
    }

    public void Load()
    {
        DeleteAll();
        AnnotationsManager.Instance.Load();
    }

    public void DeleteAll()
    {
        AnnotationsManager.Instance.DeleteAllAnnotations();

        if (activeMenu != MenuType.None && activeShapeTool != ShapeType.None) 
        {
            menuList.Find(menu => menu.MenuTypeProp == activeMenu).ToggleOff();
            shapeToolsList.Find(shapeTool => shapeTool.ShapeTypeProp == activeShapeTool).ToggleOff();

            activeMenu = MenuType.None;
            activeShapeTool = ShapeType.None;
        }
        else if(activeMenu != MenuType.None)
        {
            menuList.Find(menu => menu.MenuTypeProp == activeMenu).ToggleOff();
            textTool.ToggleOff();

            activeMenu = MenuType.None;
        }
    }

    #endregion

    #region ShapeMenu

    public void ToggleShapeTool(ShapeType shapeType)
    {
        if (activeShapeTool != ShapeType.None)
        {
            shapeToolsList.Find(shapeTool => shapeTool.ShapeTypeProp == activeShapeTool).Toggle();
        }

        shapeToolsList.Find(shapeTool => shapeTool.ShapeTypeProp == shapeType).Toggle();
        activeShapeTool = shapeType;
    }

    public void ToggleShapeToolAndCreateAnnotation(string shapeType)
    {
        ShapeType type = (ShapeType)Enum.Parse(typeof(ShapeType), shapeType);
        ToggleShapeTool(type);

        AnnotationsManager.Instance.CreateAndAddShapeAnnotation(type);
    }

    public void ToggleSelectedShapeAnnotation(ShapeType shapeType)
    {
        if (activeMenu == MenuType.ShapeMenu && activeShapeTool != shapeType) 
        {
            ToggleShapeTool(shapeType);
        }
        else if(activeMenu != MenuType.ShapeMenu && activeShapeTool != shapeType)
        {
            ToggleMenu(MenuType.ShapeMenu.ToString());
            ToggleShapeTool(shapeType);
        }   
        else if(activeMenu!=MenuType.ShapeMenu && activeShapeTool==shapeType)
        {
            ToggleMenu(MenuType.ShapeMenu.ToString());
        }
    }

    #region RulerTools

    public void AddPoint()
    {
        AnnotationsManager.Instance.RulerAddPoint(); 
    }

    public void RemoveLastPoint()
    {
        AnnotationsManager.Instance.RulerRemoveLastPoint();
    }

    public void DeleteRuler()
    {
        AnnotationsManager.Instance.RulerDelete();

        shapeToolsList.Find(tool=>tool.ShapeTypeProp==ShapeType.Ruler).Toggle();
        activeShapeTool = ShapeType.None;
    }

    #endregion

    #region RectangleTools

    public void RectangleReset()
    {
        AnnotationsManager.Instance.RectangleReset();
    }

    public void RectangleDelete()
    {
        AnnotationsManager.Instance.RectangleDelete();

        shapeToolsList.Find(tool => tool.ShapeTypeProp == ShapeType.Rectangle).Toggle();
        activeShapeTool = ShapeType.None;
    }

    #endregion

    #region CircleTools

    public void CircleReset()
    {
        AnnotationsManager.Instance.CircleReset();
    }

    public void CircleDelete()
    {
        AnnotationsManager.Instance.CircleDelete();

        shapeToolsList.Find(tool => tool.ShapeTypeProp == ShapeType.Circle).Toggle();
        activeShapeTool = ShapeType.None;
    }

    #endregion

    #region TriangleTools

    public void TriangleReset()
    {
        AnnotationsManager.Instance.TriangleReset();
    }

    public void TriangleDelete()
    {
        AnnotationsManager.Instance.TriangleDelete();

        shapeToolsList.Find(tool => tool.ShapeTypeProp == ShapeType.Triangle).Toggle();
        activeShapeTool = ShapeType.None;
    }

    #endregion

    #endregion

    #region TextMenu

    public void CreateTextAnnotation()
    {
        ToggleTextTools();
        AnnotationsManager.Instance.CreateAndAddTextAnnotation();
    }

    public void ToggleTextTools()
    {
        if (!textTool.IsActive)
        {
            textTool.Toggle();
        }
    }

    public void ToggleSelectedTextAnnotation()
    {
        if (activeMenu != MenuType.TextMenu) 
        {
            ToggleMenu(MenuType.TextMenu.ToString());
        }
        
        if(!textTool.IsActive)
        {
            textTool.Toggle();
        }
    }

    #region TextTools

    public void DeleteText()
    {
        AnnotationsManager.Instance.DeleteText();

        textTool.ToggleOff();
    }

    public void AddText()
    {
        AnnotationsManager.Instance.AddText();
    }

    public void UpdateText(string newText)
    {
        textUI.text=newText;
    }

    private void KeyboardLook()
    {
        // Test
        float distance = Vector3.Distance(gazePos.GazeOrigin, keyboardObject.transform.position);
        Vector3 posToLook = gazePos.GazeDirection.normalized * distance * 2 + gazePos.GazeOrigin;
        keyboardObject.transform.LookAt(posToLook);
    }

    #endregion

    #endregion

    public void ToggleRadialView()
    {
        RadialView radialView = annotationViewerObject.GetComponent<RadialView>();

        if(radialView.isActiveAndEnabled)
        {
            radialView.enabled = false;    
        }
        else
        {
            radialView.enabled = true;
        }
    }

    private void Update()
    {
        KeyboardLook();
        // var image=new Dicom.Imaging.DicomImage()
    }
}