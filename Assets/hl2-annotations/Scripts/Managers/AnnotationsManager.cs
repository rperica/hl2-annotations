using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;

public class AnnotationsManager : Singleton<AnnotationsManager>
{
    #region Field
    private string selectedAnnotation;
    private string previousSelectedAnnotation;

    private List<Annotation> annotationsList;

    #region Prefabs
    public GameObject verticePrefab;

    [Header("ShapePrefabs")]
    [SerializeField] private GameObject rulerPrefab;
    [SerializeField] private GameObject rectanglePrefab;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject trianglePrefab;

    [Header("OtherPrefabs")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject paintManagerObject;
    [SerializeField] private GameObject paintPrefab;

    [Header("PaintSettings")]
    public Material drawingMaterial;
    public Color32 drawingColor;

    public float startWidth = 0.04f;
    public float endWidth = 0.04f;

    #endregion
    #endregion

    #region Properties
    public string SelectedAnnotation
    {
        get { return selectedAnnotation; }
        set { selectedAnnotation = value; }
    }

    public string PreviousSelectedAnnotation
    {
        get { return previousSelectedAnnotation; }
        set { previousSelectedAnnotation = value; }
    }
    #endregion

    private void Awake()
    {
        annotationsList = new List<Annotation>();
    }

    #region Save/Load
    public void Save()
    {
        SaveData saveData = new SaveData();

        saveData.rulerDataList = new List<RulerData>();
        saveData.rectangleDataList = new List<RectangleData>();
        saveData.circleDataList = new List<CircleData>();
        saveData.triangleDataList = new List<TriangleData>();
        saveData.textDataList = new List<TextData>();
        saveData.paintDataList = new List<PaintData>();

        foreach (Annotation annotation in annotationsList)
        {
            if (annotation is Ruler)
            {
                Data data = annotation.Save<RulerData>();
                saveData.rulerDataList.Add(data as RulerData);
            }
            else if (annotation is Rectangle)
            {
                Data data = annotation.Save<RectangleData>();
                saveData.rectangleDataList.Add(data as RectangleData);
            }
            else if (annotation is Circle)
            {
                Data data = annotation.Save<CircleData>();
                saveData.circleDataList.Add(data as CircleData);
            }
            else if (annotation is Triangle)
            {
                Data data = annotation.Save<TriangleData>();
                saveData.triangleDataList.Add(data as TriangleData);
            }
            else if (annotation is Text)
            {
                Data data = annotation.Save<TextData>();
                saveData.textDataList.Add(data as TextData);
            }
            else if (annotation is Paint)
            {
                Data data = annotation.Save<PaintData>();
                saveData.paintDataList.Add(data as PaintData);
            }
        }

        SaveSystem.Save(saveData);
    }

    public void Load()
    {
        SaveData saveData = SaveSystem.Load();

        foreach (RulerData rulerData in saveData.rulerDataList)
        {
            Ruler rulerShape = CreateAnnotation(ShapeType.Ruler) as Ruler;
            rulerShape.LoadRuler(rulerData);
            annotationsList.Add(rulerShape);
        }

        foreach (RectangleData rectangleData in saveData.rectangleDataList)
        {
            Rectangle rectangleShape = CreateAnnotation(ShapeType.Rectangle) as Rectangle;
            rectangleShape.LoadRectangle(rectangleData);
            annotationsList.Add(rectangleShape);
        }

        foreach (CircleData circleData in saveData.circleDataList)
        {
            Circle circleShape = CreateAnnotation(ShapeType.Circle) as Circle;
            circleShape.LoadCircle(circleData);
            annotationsList.Add(circleShape);
        }

        foreach (TriangleData triangleData in saveData.triangleDataList)
        {
            Triangle triangleShape = CreateAnnotation(ShapeType.Triangle) as Triangle;
            triangleShape.LoadTriangle(triangleData);
            annotationsList.Add(triangleShape);
        }

        foreach (TextData textData in saveData.textDataList)
        {
            Text text = CreateShape<Text>(textPrefab); // not convenient
            text.LoadText(textData);
            annotationsList.Add(text);
        }

        foreach(PaintData paintData in saveData.paintDataList)
        {
            Paint paint = CreateShape<Paint>(paintPrefab); // not convenient
            paint.LoadPaint(paintData);
            annotationsList.Add(paint);
        }
    }
    #endregion

    public void DeleteAllAnnotations()
    {
        foreach (Annotation annotation in annotationsList.ToList())
        {
            annotation.Delete();
        }

        selectedAnnotation = null;
        previousSelectedAnnotation = null;

        annotationsList.Clear();
    }

    #region ShapeAnnotations

    public void CreateAndAddShapeAnnotation(ShapeType type)
    {
        Annotation shapeAnnotation = CreateAnnotation(type);

        if (shapeAnnotation != null)
        {
            previousSelectedAnnotation = selectedAnnotation;
            selectedAnnotation = shapeAnnotation.AnnotationID;

            if (previousSelectedAnnotation != null)
            {
                UnSelectAnnotation(previousSelectedAnnotation);
            }

            shapeAnnotation.IsSelected = true;
            annotationsList.Add(shapeAnnotation);
        }
    }

    public void RemoveAnnotation()
    {
        annotationsList.Remove(annotationsList.Find(a => a.AnnotationID == selectedAnnotation));
        selectedAnnotation = null;
    }

    public void SelectShapeAnnotation(string selected, ShapeType shapeType)
    {
        previousSelectedAnnotation = selectedAnnotation;
        selectedAnnotation = selected;

        if (selectedAnnotation != previousSelectedAnnotation)
        {
            UnSelectAnnotation(previousSelectedAnnotation);
        }

        ViewerUI_Manager.Instance.ToggleSelectedShapeAnnotation(shapeType);
    }

    public void UnSelectAnnotation(string selected)
    {
        if (selected != null)
        {
            annotationsList.Find(a => a.AnnotationID == selected).IsSelected = false;
        }
    }

    #region Ruler

    public void RulerAddPoint()
    {
        if (selectedAnnotation != null)
        {
            Ruler ruler = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Ruler;
            ruler.AddPoint();
        }
    }

    public void RulerRemoveLastPoint()
    {
        if (selectedAnnotation != null)
        {
            Ruler ruler = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Ruler;
            ruler.RemoveLastPoint();
        }
    }

    public void RulerDelete()
    {
        if (selectedAnnotation != null)
        {
            Ruler ruler = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Ruler;
            ruler.Delete();
        }
    }

    #endregion

    #region Rectangle

    public void RectangleReset()
    {
        if (selectedAnnotation != null)
        {
            Rectangle rectangle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Rectangle;
            rectangle.Reset();
        }
    }

    public void RectangleDelete()
    {
        if (selectedAnnotation != null)
        {
            Rectangle rectangle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Rectangle;
            rectangle.Delete();
        }
    }

    #endregion

    #region Circle

    public void CircleReset()
    {
        if (selectedAnnotation != null)
        {
            Circle circle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Circle;
            circle.Reset();
        }
    }

    public void CircleDelete()
    {
        if (selectedAnnotation != null)
        {
            Circle circle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Circle;
            circle.Delete();
        }
    }

    #endregion

    #region Triangle

    public void TriangleReset()
    {
        if (selectedAnnotation != null)
        {
            Triangle triangle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Triangle;
            triangle.Reset();
        }
    }

    public void TriangleDelete()
    {
        if (selectedAnnotation != null)
        {
            Triangle triangle = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Triangle;
            triangle.Delete();
        }
    }

    #endregion

    private Annotation CreateAnnotation(ShapeType type)
    {
        switch (type)
        {
            case ShapeType.Ruler:
                return CreateShape<Ruler>(rulerPrefab);

            case ShapeType.Rectangle:
                return CreateShape<Rectangle>(rectanglePrefab);

            case ShapeType.Circle:
                return CreateShape<Circle>(circlePrefab);

            case ShapeType.Triangle:
                return CreateShape<Triangle>(trianglePrefab);

            default:
                return null;
        }
    }

    private T CreateShape<T>(GameObject shapePrefab) where T : Annotation
    {
        GameObject shapeObjectInstance = GameObject.Instantiate(shapePrefab, gameObject.transform);
        shapeObjectInstance.AddComponent<T>();

        return shapeObjectInstance.GetComponent<T>();
    }

    #endregion

    #region Text
    public void CreateAndAddTextAnnotation()
    {
        Annotation textAnnotation = CreateShape<Text>(textPrefab); // CreateShape --> not convenient name

        if (textAnnotation != null)
        {
            previousSelectedAnnotation = selectedAnnotation;
            selectedAnnotation = textAnnotation.AnnotationID;

            if (previousSelectedAnnotation != null)
            {
                UnSelectAnnotation(previousSelectedAnnotation);
            }

            textAnnotation.IsSelected = true;
            annotationsList.Add(textAnnotation);
        }
    }

    public void SelectTextAnnotation(string selected)
    {
        previousSelectedAnnotation = selectedAnnotation;
        selectedAnnotation = selected;

        if (selectedAnnotation != previousSelectedAnnotation)
        {
            UnSelectAnnotation(previousSelectedAnnotation);
        }

        ViewerUI_Manager.Instance.ToggleSelectedTextAnnotation();
    }

    public void DeleteText()
    {
        if (selectedAnnotation != null)
        {
            Text text = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Text;
            text.Delete();
        }
    }

    public void AddText()
    {
        if (selectedAnnotation != null)
        {
            Text text = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Text;
            text.ToggleKeyBoard();

            Microsoft.MixedReality.Toolkit.Experimental.UI.NonNativeKeyboard.Instance.OnTextSubmitted += SubmittText;
        }
    }

    public void SubmittText(System.Object sender, System.EventArgs args)
    {
        Text text = annotationsList.Find(a => a.AnnotationID == selectedAnnotation) as Text;
        text.SubmitText(sender);
    }

    public void UpdateText(string newText)
    {
        ViewerUI_Manager.Instance.UpdateText(newText);
    }
    #endregion

    #region Paint
    public void EnablePainting()
    {
        paintManagerObject.SetActive(true);
    }

    public void CreateAndAddPaintAnnotation()
    {
        Annotation paintAnnotation = CreateShape<Paint>(paintPrefab); // CreateShape --> not convenient name
        annotationsList.Add(paintAnnotation);
    }

    public void StartDrawing(MixedRealityPointerEventData eventData)
    {
        Paint painter = annotationsList.Find(a => (a.AnnotationType == AnnotationType.Paint) && (a.IsSelected)) as Paint;
        painter.FreeDraw(eventData);
    }

    public void StopDrawing()
    {
        Paint painter = annotationsList.Find(a => (a.AnnotationType == AnnotationType.Paint) && (a.IsSelected)) as Paint;
        painter.UnSelectAndSavePositions();
    }

    public void ClearPaintings()
    {
        List<Annotation> list = annotationsList.FindAll(a => a.AnnotationType == AnnotationType.Paint);
        foreach(Annotation annotation in list)
        {
            annotationsList.Remove(annotation);

            Paint paint = annotation as Paint;
            paint.Delete();
        }
    }
    #endregion
}