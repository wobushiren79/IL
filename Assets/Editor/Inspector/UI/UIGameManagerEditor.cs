using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIGameManager))]
public class UIGameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        base.DrawDefaultInspector();

        UIGameManager uiGameManager = (UIGameManager)target;

        if (EditorUI.GUIButton("初始化子UI"))
        {
            uiGameManager.InitListUI();
        }
    }
}
