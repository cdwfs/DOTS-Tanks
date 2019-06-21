using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowEntityAuthoring))]
[CanEditMultipleObjects]
public class FollowEntityEditor : Editor
{
    private void OnSceneGUI()
    {
        var t = (target as MonoBehaviour).transform;

        Handles.matrix = t.localToWorldMatrix;

        Handles.color = Handles.xAxisColor;
        Handles.DrawLine(Vector3.zero, Vector3.right);
        Handles.color = Handles.yAxisColor;
        Handles.DrawLine(Vector3.zero, Vector3.up);
        Handles.color = Handles.zAxisColor;
        Handles.DrawLine(Vector3.zero, Vector3.forward);

        Handles.color = Color.white;
    }
}
