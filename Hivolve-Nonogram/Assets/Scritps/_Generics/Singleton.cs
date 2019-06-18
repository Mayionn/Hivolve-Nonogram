using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.Log("Multiple Objects " + typeof(T).Name);
            Destroy(this.gameObject);
            return;
        }
    }
}
