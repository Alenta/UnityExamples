using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurveTool))]
public class BezierCurvesToolEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BezierCurveTool bezierTool = (BezierCurveTool)target;
        bezierTool._bezierCurveOutput = EditorGUILayout.Vector3Field("Bezier curve point", bezierTool._bezierCurveOutput);
    }
}
