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

    private void OnProjectChange()
    {
        LogUtil.Log("OnProjectChange");
        //查询所有NPC数据
        NpcInfoService npcInfoService = new NpcInfoService();
        mapNpcInfo.Clear();
        List<NpcInfoBean> listNpcInfo = npcInfoService.QueryAllData();
        foreach (NpcInfoBean itemInfo in listNpcInfo)
        {
            mapNpcInfo.Add(itemInfo.id, itemInfo);
        }
        gameItemsManager = new GameItemsManager();
        gameItemsManager.Awake();
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
    }

    GameItemsManager gameItemsManager;

    private string mNpcCreateIdStr = "人物ID";
    private string mStoryScene = "故事场景";
    private string mStoryId = "故事ID";
    private string mStroyOrder = "故事顺序序号";

    List<StoryInfoDetailsBean> listAllStoryInfoDetails;
    List<StoryInfoDetailsBean> listOrderStoryInfoDetails;

    Dictionary<long, NpcInfoBean> mapNpcInfo = new Dictionary<long, NpcInfoBean>();

    private void OnGUI()
    {
        //滚动布局
        GUILayout.BeginScrollView(Vector2.zero);
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
            CreateStoryData(mStoryScene, mStoryId, mStroyOrder);
        }
        GUILayout.EndHorizontal();
        //故事查询
        GUILayout.BeginHorizontal();
        GUILayout.Label("故事数据查询：", GUILayout.Width(100), GUILayout.Height(20));
        mStoryScene = EditorGUILayout.TextArea(mStoryScene, GUILayout.Width(100), GUILayout.Height(20));
        mStoryId = EditorGUILayout.TextArea(mStoryId, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询"))
        {
            QueryStoryData(mStoryScene, mStoryId);
        }
        GUILayout.EndHorizontal();

        //故事内容
        if (listOrderStoryInfoDetails != null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
            {
                mStroyOrder = "" + (int.Parse(mStroyOrder) - 1);
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(int.Parse(mStroyOrder));
            }
            mStroyOrder = EditorGUILayout.TextArea(mStroyOrder, GUILayout.Width(100), GUILayout.Height(20));
            if (GUILayout.Button(">"))
            {
                mStroyOrder = "" + (int.Parse(mStroyOrder) + 1);
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(int.Parse(mStroyOrder));
            }
            GUILayout.EndHorizontal();

            ShowStoryInfoDetails();
        }


        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 创建一个NPC
    /// </summary>
    /// <param name="idStr"></param>
    public GameObject CreateNpc(string idStr)
    {
  
        if (mObjContent == null)
        {
            LogUtil.LogError("还没有定义剧情容器");
            return null;
        }
        if (mObjNpcModel == null)
        {
            LogUtil.LogError("还没有NPC模型");
            return null;
        }
        if (long.TryParse(idStr, out long createNpcId))
        {
            return CreateNpc(createNpcId, Vector3.zero,0);
        }
        else
            LogUtil.LogError("创建人物ID不规范");
        return null; 
    }

    public GameObject CreateNpc(long createNpcId,Vector3 position,int number)
    {
        CharacterBean characterData = null;
        GameObject objNpc = null;
        if (createNpcId == 0)
        {
            characterData = new CharacterBean();
        }
        else
        {
            characterData = NpcInfoBean.NpcInfoToCharacterData(mapNpcInfo[createNpcId]);
        }

        objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        baseNpcAI.gameItemsManager = gameItemsManager;
        baseNpcAI.transform.localPosition = position;
        baseNpcAI.SetCharacterData(characterData);
        baseNpcAI.name = "" + number;
        objNpc.SetActive(true);
    
        return objNpc;
    }
    /// <summary>
    /// 创建故事数据
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="storyId"></param>
    /// <param name="order"></param>
    public void CreateStoryData(string scene, string storyId, string order)
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

    /// <summary>
    /// 查询故事详情数据
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="storyId"></param>
    public void QueryStoryData(string scene, string storyId)
    {
        if (mObjContent == null)
        {
            LogUtil.LogError("还没有定义剧情容器");
            return;
        }
        StoryInfoService storyInfoService = new StoryInfoService();
        listAllStoryInfoDetails = storyInfoService.QueryStoryDetailsById(long.Parse(storyId));
        listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(int.Parse(mStroyOrder));
    }

    /// <summary>
    /// 根据序号查询数据详情
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<StoryInfoDetailsBean> GetStoryInfoDetailsByOrder(int order)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        if (listAllStoryInfoDetails == null)
            return listData;
        foreach (StoryInfoDetailsBean itemData in listAllStoryInfoDetails)
        {
            if (itemData.order == order)
            {
                listData.Add(itemData);
                CreateSceneData(itemData);
            }
        }
        return listData;
    }
   
    /// <summary>
    /// 创建场景数据
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateSceneData(StoryInfoDetailsBean itemData)
    {
        if (itemData.type == 1)
        {
            BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(mObjContent, itemData.npc_num + "");
            if (npcAI == null)
            {
                NpcInfoBean npcInfoBean = mapNpcInfo[itemData.npc_id];
                CreateNpc(npcInfoBean.npc_id, new Vector3(itemData.npc_position_x, itemData.npc_position_y), itemData.npc_num);
            }
            else
            {
                npcAI.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
            }
           
        }
    }

    /// <summary>
    /// 展示指定顺序的故事详情
    /// </summary>
    public void ShowStoryInfoDetails()
    {
        foreach (StoryInfoDetailsBean itemData in listOrderStoryInfoDetails)
        {
            GUILayout.BeginHorizontal();
            if (itemData.type == 1)
            {
                GUILayout.Label("NPC站位 ");
                NpcInfoBean npcInfoBean = mapNpcInfo[itemData.npc_id];
                GUILayout.Label("姓名：" + npcInfoBean.title_name + "-" + npcInfoBean.name);
                GUILayout.Label("NPCId:");
                itemData.npc_id = long.Parse(EditorGUILayout.TextArea(itemData.npc_id + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC序号:" + itemData.npc_num);
                GUILayout.Label("NPC位置X:");
                itemData.npc_position_x = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC位置Y:");
                itemData.npc_position_y = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
                if (GUILayout.Button("更新数据"))
                {

                }
            }
            else if (itemData.type == 2)
            {
                GUILayout.Label("指定NPC展现表情 ");
            }
            else if (itemData.type == 12)
            {
                GUILayout.Label("延迟执行 ");
                GUILayout.Label("延迟时间s:");
                itemData.wait_time = float.Parse(EditorGUILayout.TextArea(itemData.wait_time + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            GUILayout.EndHorizontal();
        }
    }


}