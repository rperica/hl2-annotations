using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance==null)
            {
                SetupInstance();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null) 
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        _instance = (T)FindObjectOfType(typeof(T));
        if (_instance == null)
        {
            Debug.LogError("Missing " + typeof(T).ToString());
        }
    }
}
