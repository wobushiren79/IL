using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryCreateWindowsEditor : EditorWindow
{
    private GameObject mObjContent;
    private GameObject mObjNpcModel;

    [MenuItem("Tools/Window/StoryCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(StoryCreateWindowsEditor));
    }

    public StoryCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("剧情创建辅助工具");
    }

    private string mNpcCreateIdStr = "人物ID";
    private string mStoryScene = "故事场景";
    private string mStoryId = "故事ID";
    private string mStroyOrder = "故事顺序序号";
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
        mNpcCreateIdStr = EditorGUILayout.TextArea(mNpcCreateIdStr, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("创建"))
        {
            CreateNpc(mNpcCreateIdStr);
        }
        GUILayout.EndHorizontal();

        //故事数据生成
        GUILayout.BeginHorizontal();
        GUILayout.Label("故事数据生成：", GUILayout.Width(100), GUILayout.Height(20));
        mStoryScene = EditorGUILayout.TextArea(mStoryScene, GUILayout.Width(100), GUILayout.Height(20));
        mStoryId = EditorGUILayout.TextArea(mStoryId, GUILayout.Width(100), GUILayout.Height(20));
        mStroyOrder = EditorGUILayout.TextArea(mStroyOrder, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("生成数据"))
        {
            CreateStoryData(mStoryScene,mStoryId, mStroyOrder);
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

            CharacterBean characterData = null;

            if (createNpcId == 0)
            {
                characterData = new CharacterBean();
            }
            else
            {
                List<NpcInfoBean> listData = npcInfoService.QueryDataById(createNpcId);
                if (CheckUtil.ListIsNull(listData))
                {
                    LogUtil.LogError("没有查询到对应创建NPC ID 数据");
                    return;
                }
                characterData = NpcInfoBean.NpcInfoToCharacterData(listData[0]);
            }

            GameObject objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
            BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
            baseNpcAI.gameItemsManager = gameItemsManager;
            baseNpcAI.SetCharacterData(characterData);
            objNpc.SetActive(true);
        }
        else
            LogUtil.LogError("创建人物ID不规范");
    }

    /// <summary>
    /// 创建故事数据
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="storyId"></param>
    /// <param name="order"></param>
    public void CreateStoryData(string scene,string storyId, string order)
    {
        if (mObjContent == null)
        {
            LogUtil.LogError("还没有定义剧情容器");
            return;
        }
        float storyPositionX = mObjContent.transform.position.x;
        float storyPositionY = mObjContent.transform.position.y;
        LogUtil.Log("story_info:" + storyId + "	1	" + scene + "	" + storyPositionX + "	" + storyPositionY);

        string detailsStr = "";
        for (int i = 0; i < mObjContent.transform.childCount; i++)
        {
            GameObject itemObj = mObjContent.transform.GetChild(i).gameObject;
            if (itemObj.activeSelf)
            {
                BaseNpcAI baseNpc = itemObj.GetComponent<BaseNpcAI>();
                if (baseNpc == null)
                    continue;
                CharacterBean characterData = baseNpc.characterData;
                if (characterData == null)
                    continue;
                detailsStr += (storyId + "	" + order + "	1	");
                //NPC_ID
                detailsStr += (baseNpc.characterData.baseInfo.characterId + "	");
                //npc位置
                detailsStr += (itemObj.transform.localPosition.x + "	" + itemObj.transform.localPosition.y + "	");
                //npc编号
                detailsStr += (itemObj.name + "	");

                detailsStr += ("\n");
            }
        }
        LogUtil.Log("story_info_details:" + detailsStr);
    }
}