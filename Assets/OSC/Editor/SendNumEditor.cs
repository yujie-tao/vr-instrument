using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SendNum))]
public class SendNumEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SendNum sendNum = (SendNum)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Send"))
        {
            sendNum.Send();
        }
        GUILayout.EndHorizontal();
    }
}