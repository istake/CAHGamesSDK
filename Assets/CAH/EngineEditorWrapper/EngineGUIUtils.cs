using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EngineGUIUtils
{
    public static bool IsSelectedInHierarchy(GameObject obj)
    {
#if UNITY_EDITOR
        if (UnityEditor.Selection.activeObject == obj)
            return true; 
#endif 
        return false;
    }
    
    public static void SelectObjectInHierarchy(GameObject obj)
    {
#if UNITY_EDITOR
        UnityEditor.Selection.activeObject = obj;
#endif 
    }
}