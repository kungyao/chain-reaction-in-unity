using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierManager))]
[CanEditMultipleObjects]
public class BezierUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BezierManager myTarget = (BezierManager)target;
        if (GUILayout.Button("Generate Domino"))
        {
            myTarget.GenerateDominoByLength();
        }
        if (GUILayout.Button("Clear Last Gen Domino"))
        {
            myTarget.ClearTmp();
        }
    }
}