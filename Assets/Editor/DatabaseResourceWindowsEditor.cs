using UnityEngine;
using UnityEditor;

public class DatabaseResourceWindowsEditor : EditorWindow
{
    [MenuItem("Tools/Window/DatabaseResource")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(DatabaseResourceWindowsEditor));
    }

    public DatabaseResourceWindowsEditor()
    {
        this.titleContent = new GUIContent("资源添加工具");
    }

    private void OnGUI()
    {
        //滚动布局
        GUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(500), GUILayout.Height(1000));
        GUILayout.BeginVertical();

        //NPC创建
        GUILayout.BeginHorizontal();
        GUILayout.Label("Item添加：", GUILayout.Width(100), GUILayout.Height(20));


        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}