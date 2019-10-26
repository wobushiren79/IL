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

    private void OnEnable()
    {
        LogUtil.Log("OnEnable");
        //查询所有NPC数据
        listAllStoryInfoDetails = null;
        listOrderStoryInfoDetails = null;
        listStoryTextInfo = null;
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
        textInfoService = new TextInfoService();
        storyInfoService = new StoryInfoService();
    }

    GameItemsManager gameItemsManager;
    TextInfoService textInfoService;
    StoryInfoService storyInfoService;

    private string mNpcCreateIdStr = "人物ID";
    private string mStoryScene = "故事场景";
    private string mStoryId = "故事ID";
    private string mStroyOrder = "故事顺序序号";

    List<StoryInfoDetailsBean> listAllStoryInfoDetails;
    List<StoryInfoDetailsBean> listOrderStoryInfoDetails;
    List<TextInfoBean> listStoryTextInfo;
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
                CreateSceneData(listOrderStoryInfoDetails);
            }
            mStroyOrder = EditorGUILayout.TextArea(mStroyOrder, GUILayout.Width(100), GUILayout.Height(20));
            if (GUILayout.Button(">"))
            {
                mStroyOrder = "" + (int.Parse(mStroyOrder) + 1);
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(int.Parse(mStroyOrder));
                CreateSceneData(listOrderStoryInfoDetails);
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
            return CreateNpc(createNpcId, Vector3.zero, 0);
        }
        else
            LogUtil.LogError("创建人物ID不规范");
        return null;
    }

    public GameObject CreateNpc(long createNpcId, Vector3 position, int number)
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
        //清空容器
        for (int i = 0; i < mObjContent.transform.childCount; i++)
        {
            if (mObjContent.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject.DestroyImmediate(mObjContent.transform.GetChild(i).gameObject);
            }
        }
        listAllStoryInfoDetails = storyInfoService.QueryStoryDetailsById(long.Parse(storyId));
        listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(int.Parse(mStroyOrder));
        CreateSceneData(listOrderStoryInfoDetails);
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
            if (itemData.story_order == order)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建场景数据
    /// </summary>
    /// <param name="listData"></param>
    public void CreateSceneData(List<StoryInfoDetailsBean> listData)
    {
        if (listData == null)
            return;
        listStoryTextInfo = null;
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
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
            //如果是对话 查询对话数据
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                listStoryTextInfo = textInfoService.QueryDataByMarkId(TextEnum.Story, itemData.text_mark_id);
            }
        }
    }

    /// <summary>
    /// 展示指定顺序的故事详情
    /// </summary>
    public void ShowStoryInfoDetails()
    {
        StoryInfoDetailsBean removeTempData = null;
        foreach (StoryInfoDetailsBean itemData in listOrderStoryInfoDetails)
        {
            GUILayout.BeginHorizontal();
            if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            {
                GUILayout.Label("NPC站位 ");
                NpcInfoBean npcInfo;
                if (itemData.npc_id == 0)
                {
                    npcInfo = new NpcInfoBean();
                    npcInfo.name = "玩家";
                }
                else
                {
                    npcInfo = mapNpcInfo[itemData.npc_id];
                }
                GUILayout.Label("姓名：" + npcInfo.title_name + "-" + npcInfo.name);
                GUILayout.Label("NPCId:");
                itemData.npc_id = long.Parse(EditorGUILayout.TextArea(itemData.npc_id + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC序号:");
                itemData.npc_num = int.Parse(EditorGUILayout.TextArea(itemData.npc_num + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("NPC位置X:");
                itemData.npc_position_x = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC位置Y:");
                itemData.npc_position_y = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
                if (GUILayout.Button("更新显示"))
                {
                    RemoveSceneCharacterByName(itemData.npc_num + "");
                    CreateSceneData(listOrderStoryInfoDetails);
                }
                if (GUILayout.Button("获取显示坐标"))
                {
                    GameObject objItem = GetSceneObjByName(itemData.npc_num + "");
                    itemData.npc_position_x = objItem.transform.localPosition.x;
                    itemData.npc_position_y = objItem.transform.localPosition.y;
                }
                if (GUILayout.Button("删除"))
                {
                    removeTempData = itemData;
                }
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                GUILayout.Label("对话 ");
                GUILayout.BeginVertical();
                if (listStoryTextInfo != null)
                {
                    TextInfoBean removeTempText = null;
                    foreach (TextInfoBean textInfo in listStoryTextInfo)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("ID");
                        textInfo.id = long.Parse(EditorGUILayout.TextArea(textInfo.id + "", GUILayout.Width(120), GUILayout.Height(20)));
                        GUILayout.Label("userID");
                        textInfo.user_id = long.Parse(EditorGUILayout.TextArea(textInfo.user_id + "", GUILayout.Width(100), GUILayout.Height(20)));
                        GUILayout.Label("姓名");
                        NpcInfoBean npcInfo;
                        if (textInfo.user_id == 0)
                        {
                            npcInfo = new NpcInfoBean();
                            npcInfo.name = "玩家";
                        }
                        else
                        {
                            npcInfo = mapNpcInfo[textInfo.user_id];
                        }
                        GUILayout.Label(npcInfo.title_name + "-" + npcInfo.name);
                        GUILayout.Label("指定的姓名");
                        textInfo.name = EditorGUILayout.TextArea(textInfo.name, GUILayout.Width(100), GUILayout.Height(20));
                        GUILayout.Label("对话内容");
                        textInfo.content = EditorGUILayout.TextArea(textInfo.content, GUILayout.Width(400), GUILayout.Height(20));

                        if (GUILayout.Button("删除子对话"))
                        {
                            removeTempText = textInfo;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (removeTempText != null)
                        listStoryTextInfo.Remove(removeTempText);
                }
                if (GUILayout.Button("添加子对话"))
                {
                    if (listStoryTextInfo == null)
                        listStoryTextInfo = new List<TextInfoBean>();
                    TextInfoBean textInfo = new TextInfoBean();
                    textInfo.id = itemData.text_mark_id * 1000 + listStoryTextInfo.Count + 1;
                    textInfo.type = 0;
                    textInfo.mark_id = itemData.text_mark_id;
                    textInfo.user_id = 1;
                    textInfo.text_order = listStoryTextInfo.Count + 1;
                    listStoryTextInfo.Add(textInfo);
                }
                GUILayout.EndVertical();
                if (GUILayout.Button("删除所有对话"))
                {
                    removeTempData = itemData;
                }
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Expression)
            {
                GUILayout.Label("指定NPC展现表情 ");

            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext)
            {
                GUILayout.Label("延迟执行 ");
                GUILayout.Label("延迟时间s:");
                itemData.wait_time = float.Parse(EditorGUILayout.TextArea(itemData.wait_time + "", GUILayout.Width(100), GUILayout.Height(20)));
                if (GUILayout.Button("删除"))
                {
                    removeTempData = itemData;
                }
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition)
            {
                GUILayout.Label("摄像头位置 ");
                GUILayout.Label("x:");
                itemData.camera_position_x = float.Parse(EditorGUILayout.TextArea(itemData.camera_position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("y:");
                itemData.camera_position_y = float.Parse(EditorGUILayout.TextArea(itemData.camera_position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter)
            {
                GUILayout.Label("摄像头跟随角色序号 ");
                itemData.camera_follow_character = int.Parse(EditorGUILayout.TextArea(itemData.camera_follow_character + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            GUILayout.EndHorizontal();
        }
        if (removeTempData != null)
        {
            listOrderStoryInfoDetails.Remove(removeTempData);
            RemoveSceneCharacterByName(removeTempData.npc_num + "");
        }
        if (GUILayout.Button("添加站位"))
        {
            CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition);
        }
        if (GUILayout.Button("添加对话"))
        {
            CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk);
        }
        if (GUILayout.Button("添加延迟"))
        {
            CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext);
        }
        if (GUILayout.Button("添加摄像头坐标"))
        {
            CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition);
        }
        if (GUILayout.Button("添加摄像头跟随角色"))
        {
            CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter);
        }
        if (GUILayout.Button("保存该序号下的故事数据"))
        {
            //先处理对话
            if (listStoryTextInfo != null)
                textInfoService.UpdateDataByMarkIdFor(TextEnum.Story, long.Parse(mStoryId) * 10000 + int.Parse(mStroyOrder), listStoryTextInfo);

            storyInfoService.UpdateStoryDetailsByIdAndOrder(long.Parse(mStoryId), int.Parse(mStroyOrder), listOrderStoryInfoDetails);
            
            //刷新数据
            QueryStoryData(mStoryScene, mStoryId);
        }
    }

    public void CreateStoryInfoDetailsDataByType(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum type)
    {
        StoryInfoDetailsBean itemPositionInfo = new StoryInfoDetailsBean();

        itemPositionInfo.type = (int)type;
        switch (type)
        {
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                itemPositionInfo.npc_id = 1;
                itemPositionInfo.npc_num = listOrderStoryInfoDetails.Count + 1;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                itemPositionInfo.text_mark_id = long.Parse(mStoryId)*10000+ int.Parse(mStroyOrder);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                itemPositionInfo.wait_time = 1;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                break;
        }

        itemPositionInfo.story_id = long.Parse(mStoryId);
        itemPositionInfo.story_order = int.Parse(mStroyOrder);

        listOrderStoryInfoDetails.Add(itemPositionInfo);
        CreateSceneData(listOrderStoryInfoDetails);
    }

    /// <summary>
    /// 根据名字删除角色
    /// </summary>
    /// <param name="name"></param>
    public void RemoveSceneCharacterByName(string name)
    {
        GameObject objTarget = GetSceneObjByName(name);
        if (objTarget != null)
            GameObject.DestroyImmediate(objTarget);
    }

    /// <summary>
    /// 获取场景物体
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetSceneObjByName(string name)
    {
        for (int i = 0; i < mObjContent.transform.childCount; i++)
        {
            if (mObjContent.transform.GetChild(i).gameObject.activeSelf)
            {
                if (mObjContent.transform.GetChild(i).gameObject.name.Equals(name))
                {
                    return mObjContent.transform.GetChild(i).gameObject;
                }
            }
        }
        return null;
    }
}