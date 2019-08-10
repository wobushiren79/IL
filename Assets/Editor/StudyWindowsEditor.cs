using UnityEngine;
using UnityEditor;

public class StudyWindowsEditor : EditorWindow
{
    public enum StudyEnum
    {
        A,
        B,
        C,
        D
    }

    private StudyEnum mStudyEnum = StudyEnum.A;
    private GameObject mGameObject ;
    [MenuItem("Tools/Window/Study")]
    static void CreateTestWindows()
    {
        EditorWindow.GetWindow(typeof(StudyWindowsEditor));
    }

    public StudyWindowsEditor()
    {
        this.titleContent = new GUIContent("学习编辑器窗口");
    }

    private void OnGUI()
    {
        //滚动布局
        GUILayout.BeginScrollView(Vector2.zero,GUILayout.Width(500),GUILayout.Height(1000));
        //横向布局
        GUILayout.BeginHorizontal();
        GUILayout.Label("标签：横向布局");
        GUILayout.Button("按钮");
        GUILayout.Button("按钮");
        GUILayout.Button("按钮");
        GUILayout.EndHorizontal();

        //竖向布局
        GUILayout.BeginVertical();
        GUILayout.Label("标签：竖向布局");
        GUILayout.Button("按钮");

        GUILayout.Box("区域");

        EditorGUILayout.LabelField("文本信息");

        EditorGUILayout.HelpBox("帮助信息", MessageType.None, true);
        EditorGUILayout.HelpBox("帮助信息", MessageType.Info, true);
        EditorGUILayout.HelpBox("帮助信息", MessageType.Warning, false);
        EditorGUILayout.HelpBox("帮助信息", MessageType.Error, false);

        EditorGUILayout.TextArea("文本输入框");
        GUILayout.TextArea("文本输入框");

     //   EditorGUILayout.Toggle(false, "单选框");

        GUILayout.Toggle(true,"单选框");
        GUILayout.Toggle(false, "单选框");

        EditorGUILayout.Slider(0.5f,0,1);

        mStudyEnum = (StudyEnum)EditorGUILayout.EnumPopup(new GUIContent("枚举值:", "详细信息"), mStudyEnum);

        mGameObject= EditorGUILayout.ObjectField(new GUIContent("序列化Object","提示信息"), mGameObject, typeof(GameObject),true) as GameObject;

        GUILayout.EndVertical();

        GUILayout.EndScrollView();
    }
}