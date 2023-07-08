using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T inst;
    public static T Inst
    {
        get => inst;
    }

    protected void SetInst()
    {
        inst = FindObjectOfType<T>();
    }
}
