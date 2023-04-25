using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class Text : Annotation
{
    private string annotationText;

    private TouchScreenKeyboard keyboard;

    public override void Delete()
    {
        Debug.Log("this object is " + gameObject.name);
        Destroy(gameObject);

        AnnotationsManager.Instance.RemoveAnnotation();
    }

    public override T Save<T>()
    {
        TextData data = new TextData();

        data.annotationID = annotationID;
        data.annotationType = AnnotationType.Text;

        data.text = annotationText;

        data.position = Utility.ConvertToVec3(gameObject.transform.localPosition);

        return data as T;
    }

    protected override void GenerateID()
    {
        annotationID = "text" + Random.Range(1, 1000).ToString();
        gameObject.name = annotationID;
    }

    public void LoadText(TextData data)
    {
        isSelected = false;

        annotationID = data.annotationID;
        gameObject.name = annotationID;

        annotationType = AnnotationType.Text;

        annotationText = data.text;

        gameObject.transform.localPosition = Utility.ConvertToVector3(data.position);
    }

    public void SelectAnnotation()
    {
        isSelected = true;
        AnnotationsManager.Instance.SelectTextAnnotation(annotationID);
    }

    public void ToggleKeyBoard()
    {
        if (keyboard != null)
        {
            keyboard = TouchScreenKeyboard.Open("text");
        }   
    }

    private void Initialize()
    {
        if (string.IsNullOrEmpty(annotationID))
        {
            GenerateID();
        }

        annotationType = AnnotationType.Text;

        gameObject.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(Data => SelectAnnotation());
    }

    private void Awake()
    {
        Initialize();       
    }

    private void Update()
    {
        if(isSelected)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

        if(keyboard != null)
        {
            annotationText = keyboard.text;
        }    
    }
}
// https://github.com/Ayfel/MRTK-Keyboard 

// https://github.com/MicrosoftDocs/mixed-reality/blob/docs/mixed-reality-docs/mr-dev-docs/develop/unity/keyboard-input-in-unity.md
// https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/ux-building-blocks/system-keyboard?view=mrtkunity-2022-05