using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryCreateWindowsEditor : EditorWindow
{
    private GameObject mObjContent;
    private GameObject mObjNpcModel;

    [MenuItem("Tools/Window/StoryCreate")]
    static void CreateTestWindows()
    {
        EditorWindow.GetWindow(typeof(StoryCreateWindowsEditor));
    }

    public StoryCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("剧情创建辅助工具");
    }

    private string npcCreateIdStr = "人物ID";

    private void OnGUI()
    {
        //滚动布局
        GUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(500), GUILayout.Height(1000));
        GUILayout.BeginVertical();
        //父对象
        mObjContent = EditorGUILayout.ObjectField(new GUIContent("剧情容器", ""), mObjContent, typeof(GameObject), true) as GameObject;
        mObjNpcModel = EditorGUILayout.ObjectField(new GUIContent("NPC模型", ""), mObjNpcModel, typeof(GameObject), true) as GameObject;

        //NPC创建
        GUILayout.BeginHorizontal();
        GUILayout.Label("人物创建：", GUILayout.Width(100), GUILayout.Height(20));
        npcCreateIdStr = EditorGUILayout.TextArea(npcCreateIdStr, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("创建"))
        {
            CreateNpc(npcCreateIdStr);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 创建一个NPC
    /// </summary>
    /// <param name="idStr"></param>
    public void CreateNpc(string idStr)
    {
        if (mObjContent == null)
        {
            LogUtil.LogError("还没有定义剧情容器");
            return;
        }
        if (mObjNpcModel == null)
        {
            LogUtil.LogError("还没有NPC模型");
            return;
        } 
        if (long.TryParse(idStr, out long createNpcId))
        {
            NpcInfoService npcInfoService = new NpcInfoService();
            GameItemsManager gameItemsManager = new GameItemsManager();
            gameItemsManager.Awake();
            gameItemsManager.itemsInfoController.GetAllItemsInfo();

            List<NpcInfoBean> listData = npcInfoService.QueryDataById(createNpcId);
            if (CheckUtil.ListIsNull(listData))
            {
                LogUtil.LogError("没有查询到对应创建NPC ID 数据");
                return;
            }
            CharacterBean characterData=  NpcInfoBean.NpcInfoToCharacterData(listData[0]);
            GameObject objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
            BaseNpcAI baseNpcAI= objNpc.GetComponent<BaseNpcAI>();
            baseNpcAI.gameItemsManager = gameItemsManager;
            baseNpcAI.SetCharacterData(characterData);
            objNpc.SetActive(true);
        }
        else
            LogUtil.LogError("创建人物ID不规范");
    }
}