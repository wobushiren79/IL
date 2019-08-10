using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//场景内弹窗测试
//[CustomEditor(typeof(StoryCreate))]
public class StoryCreateEditor : Editor
{
    public StoryCreate storyCreate;

    private void Awake()
    {
        storyCreate = target as StoryCreate;
    }

    private void OnSceneGUI()
    {

        Handles.BeginGUI();

        GUILayout.BeginVertical("剧情创建工具", "window", new[] { GUILayout.Height(500), GUILayout.Width(120) });

        var buttonStyle = new[] { GUILayout.Height(30), GUILayout.Width(100) };
        	

        GUILayout.Space(10);

        if (GUILayout.Button("生成数据", buttonStyle))
        {
            LogUtil.Log("生成数据");
        }
        GUILayout.EndVertical();

        Handles.EndGUI();
    }
}
