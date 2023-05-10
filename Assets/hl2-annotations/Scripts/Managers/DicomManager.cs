using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dicom.Imaging;

public class DicomManager : MonoBehaviour
{
    private Texture2D texture;
    [SerializeField] private GameObject dicomPlane;

    public string dicomImagePath;

    private void Start()
    {
#if !WINDOWS_UWP
        string path = Path.Combine(Application.dataPath + "/Resources", "case1_008.dcm");
#elif !UNITY_EDITOR && UNITY_WSA
     string path = Path.Combine(Application.streamingAssetsPath, "case1_008.dcm");
#endif
        var image = new DicomImage(@path);
        texture = image.RenderImage().AsTexture2D();

        dicomPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }
}
