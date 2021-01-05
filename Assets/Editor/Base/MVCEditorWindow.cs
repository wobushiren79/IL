using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MVCEditorWindow : EditorWindow
{
    public string mvcClassName = "";

    protected readonly string scrpitsTemplatesPath = "/Editor/ScrpitsTemplates/";

    protected int saveType = 1;
    protected string mvcBeanPath = "Assets/Scrpits/Bean/MVC";
    protected string mvcViewPath = "Assets/Scrpits/MVC/View";
    protected string mvcModelPath = "Assets/Scrpits/MVC/Model";
    protected string mvcControllerPath = "Assets/Scrpits/MVC/Controller";
    protected string mvcServicePath = "Assets/Scrpits/MVC/Service";

    [MenuItem("MVC/创建")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(MVCEditorWindow));
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorUI.GUIText("创建MVC,输入MVC名称:", 200);
        mvcClassName = EditorUI.GUIEditorText(mvcClassName, 100);
        EditorUI.GUIText("保存类型（1.SQLite 2.FileJson）:", 200);
        saveType = EditorUI.GUIEditorText(saveType, 50);
        EditorGUILayout.EndHorizontal();

        EditorUI.GUIText("Bean路径:");
        mvcBeanPath = EditorUI.GUIEditorText(mvcBeanPath, 500);
        EditorUI.GUIText("View路径:");
        mvcViewPath = EditorUI.GUIEditorText(mvcViewPath, 500);
        EditorUI.GUIText("Model路径:");
        mvcModelPath = EditorUI.GUIEditorText(mvcModelPath, 500);
        EditorUI.GUIText("Controller路径:");
        mvcControllerPath = EditorUI.GUIEditorText(mvcControllerPath, 500);
        EditorUI.GUIText("Service路径:");
        mvcServicePath = EditorUI.GUIEditorText(mvcServicePath, 500);

        if (EditorUI.GUIButton("创建"))
        {
            CreateMVCClass(mvcClassName, saveType);
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 创建MVC
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="saveType">1.sqlite 2.filejson</param>
    public void CreateMVCClass(string fileName, int saveType)
    {
        //注意，Application.datapath会根据使用平台不同而不同  
        string beanPath = Application.dataPath + scrpitsTemplatesPath + "MVC_Bean.txt";
        string viewPath = Application.dataPath + scrpitsTemplatesPath + "MVC_IView.txt";
        string modelPath = Application.dataPath + scrpitsTemplatesPath + "MVC_Model.txt";
        string controllerPath = Application.dataPath + scrpitsTemplatesPath + "MVC_Controller.txt";
        string servicePath = "";

        switch (saveType)
        {
            case 1:
                servicePath = Application.dataPath + scrpitsTemplatesPath + "MVC_Service_SQLite.txt";
                break;
            case 2:
                servicePath = Application.dataPath + scrpitsTemplatesPath + "MVC_Service_FileJson.txt";
                break;
        }

        //创建.CS文件
        CreateClass(beanPath, fileName + "Bean", fileName, mvcBeanPath);
        CreateClass(viewPath, "I" + fileName + "View", fileName, mvcViewPath);
        CreateClass(modelPath, fileName + "Model", fileName, mvcModelPath);
        CreateClass(controllerPath, fileName + "Controller", fileName, mvcControllerPath);
        CreateClass(servicePath, fileName + "Service", fileName, mvcServicePath);
        //刷新资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 创建.cs文件
    /// </summary>
    /// <param name="templatesPath"></param>
    /// <param name="fileName"></param>
    /// <param name="className"></param>
    /// <param name="createPath"></param>
    protected void CreateClass(string templatesPath, string fileName,string className, string createPath)
    {
        if (CheckUtil.StringIsNull(templatesPath))
        {
            LogUtil.LogError("模板路径为空");
            return;
        }
        if (CheckUtil.StringIsNull(fileName))
        {
            LogUtil.LogError("文件名为空");
            return;
        }
        if (CheckUtil.StringIsNull(createPath))
        {
            LogUtil.LogError("生成路径为空");
            return;
        }
        //读取模板
        string viewScriptContent = File.ReadAllText(templatesPath);
        //替换规则
        viewScriptContent = ReplaceRole(viewScriptContent, className);
        //创建文件
        FileUtil.CreateTextFile(createPath, fileName + ".cs", viewScriptContent);
    }

    /// <summary>
    /// 替换规则
    /// </summary>
    /// <param name="scripteContent"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    protected string ReplaceRole(string scripteContent, string fileName)
    {
        //这里实现自定义的一些规则  
        scripteContent = scripteContent.Replace("#ScriptName#", fileName);
        scripteContent = scripteContent.Replace("#Author#", "AppleCoffee");
        scripteContent = scripteContent.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
        return scripteContent;
    }
}
