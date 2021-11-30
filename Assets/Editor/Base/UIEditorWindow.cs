using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIEditorWindow : EditorWindow
{
    [MenuItem("Custom/UI/��������")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(UIEditorWindow));
    }

    public Font SelectOldFont;
    static Font OldFont;

    public Font SelectNewFont;
    static Font NewFont;

    static float NewLineSpacing;

    private void OnGUI()
    {
        SelectOldFont = (Font)EditorGUILayout.ObjectField("��ѡ�������������", SelectOldFont, typeof(Font), true, GUILayout.MinWidth(100));
        OldFont = SelectOldFont;
        SelectNewFont = (Font)EditorGUILayout.ObjectField("��ѡ���µ�����", SelectNewFont, typeof(Font), true, GUILayout.MinWidth(100));
        NewFont = SelectNewFont;
        NewLineSpacing = 1;
        NewLineSpacing = EditorGUILayout.FloatField("���м��", NewLineSpacing);

        if (GUILayout.Button("����ѡ�е�Ԥ����"))
        {
            if (SelectOldFont == null || SelectNewFont == null)
            {
                Debug.LogError("��ѡ�����壡");
            }
            else
            {
                Change();
            }
        }
        if (GUILayout.Button("�����ļ��������е�Ԥ����"))
        {
            if (SelectOldFont == null || SelectNewFont == null)
            {
                Debug.LogError("��ѡ�����壡");
            }
            else
            {
                ChangeSelectFloud();
            }
        }
    }

    public static void Change()
    {
        Object[] Texts = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
        Debug.Log("�ҵ�" + Texts.Length + "��Text����������");
        int count = 0;
        foreach (Object text in Texts)
        {
            if (text)
            {
                Text AimText = (Text)text;
                Undo.RecordObject(AimText, AimText.gameObject.name);
                if (AimText.font == OldFont)
                {
                    AimText.font = NewFont;
                    AimText.lineSpacing = NewLineSpacing;
                    //Debug.Log(AimText.name + ":" + AimText.text);
                    EditorUtility.SetDirty(AimText);
                    count++;
                }
            }
        }
        Debug.Log("���������ϣ�������" + count + "��");
    }

    public static void ChangeSelectFloud()
    {

        object[] objs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            string ext = System.IO.Path.GetExtension(objs[i].ToString());
            if (!ext.Contains(".GameObject"))
            {
                continue;
            }
            GameObject go = (GameObject)objs[i];
            var Texts = go.GetComponentsInChildren<Text>(true);
            int count = 0;
            foreach (Text text in Texts)
            {
                Undo.RecordObject(text, text.gameObject.name);
                if (text.font == OldFont)
                {
                    text.font = NewFont;
                    text.lineSpacing = NewLineSpacing;
                    EditorUtility.SetDirty(text);
                    count++;
                }
            }
            if (count > 0)
            {
                AssetDatabase.SaveAssets();
                Debug.Log(go.name + "������:" + count + "��Arial����");
            }

        }
    }
}
