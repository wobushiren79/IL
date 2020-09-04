﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/////////////////////////////////////////////////////////////////////////////
//查找节点及所有子节点中,是否有指定的脚本组件
/////////////////////////////////////////////////////////////////////////////
public class MonoFinderWindowEditor  : EditorWindow
{
    Transform root = null;
    MonoScript scriptObj = null;
    int loopCount = 0;

    List<Transform> results = new List<Transform>();

    [MenuItem("Tools/Finder/MonoFinder")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(MonoFinderWindowEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("节点:");
        root = (Transform)EditorGUILayout.ObjectField(root, typeof(Transform), true);
        GUILayout.Label("脚本类型:");
        scriptObj = (MonoScript)EditorGUILayout.ObjectField(scriptObj, typeof(MonoScript), true);
        if (GUILayout.Button("Find"))
        {
            results.Clear();
            loopCount = 0;
            Debug.Log("开始查找.");
            FindScript(root);
        }
        if (results.Count > 0)
        {
            foreach (Transform t in results)
            {
                EditorGUILayout.ObjectField(t, typeof(Transform), false);
            }
        }
        else
        {
            GUILayout.Label("未找到数据");
        }
    }

    void FindScript(Transform root)
    {
        if (root != null && scriptObj != null)
        {
            loopCount++;
            Debug.Log(".." + loopCount + ":" + root.gameObject.name);
            if (root.GetComponent(scriptObj.GetClass()) != null)
            {
                results.Add(root);
            }
            foreach (Transform t in root)
            {
                FindScript(t);
            }
        }
    }
}