using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidget
{
    public string GUID { get; private set; }
    public GameObject UIObject { get; private set; }
    public UIWidget(string guid, GameObject go)
    {
        GUID = guid;
        UIObject = go;
    }
}
