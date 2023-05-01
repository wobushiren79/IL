using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cinemachine;
using static CharacterExpressionCpt;

public class StoryInfoCreateWindowsEditor : EditorWindow
{

    [MenuItem("Tools/Window/StoryCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(StoryInfoCreateWindowsEditor));
    }

    public StoryInfoCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("剧情创建辅助工具");
    }

    

    private void OnEnable()
    {
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
        GameItemsHandler.Instance.manager.Awake();
        StoryInfoHandler.Instance.manager.Awake();
        textInfoService = new TextInfoService();
        storyInfoService = new StoryInfoService();
    }

    public void OnDisable()
    {
        GameItemsHandler.Instance.DestorySelf(1);
        StoryInfoHandler.Instance.DestorySelf(1); 
        GameDataHandler.Instance.DestorySelf(1);
        IconHandler.Instance.DestorySelf(1);
        CameraHandler.Instance.DestorySelf(1);
        CharacterBodyHandler.Instance.DestorySelf(1);
        CharacterDressHandler.Instance.DestorySelf(1);
    }

    TextInfoService textInfoService;
    StoryInfoService storyInfoService;

    private string mNpcCreateIdStr = "人物ID";
    private StoryInfoBean mCreateStoryInfo = new StoryInfoBean();
    private long mFindStoryId = 0;
    private int mFindStroyOrder = 1;


    List<StoryInfoBean> listStoryInfo = new List<StoryInfoBean>();

    List<StoryInfoDetailsBean> listAllStoryInfoDetails = new List<StoryInfoDetailsBean>();
    List<StoryInfoDetailsBean> listOrderStoryInfoDetails = new List<StoryInfoDetailsBean>();
    List<TextInfoBean> listStoryTextInfo = new List<TextInfoBean>();
    Dictionary<long, NpcInfoBean> mapNpcInfo = new Dictionary<long, NpcInfoBean>();

    private Vector2 scrollPosition = Vector2.zero;
    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        //父对象
        // mObjContent = EditorGUILayout.ObjectField(new GUIContent("剧情容器", ""), mObjContent, typeof(GameObject), true) as GameObject;
        // mObjNpcModel = EditorGUILayout.ObjectField(new GUIContent("NPC模型", ""), mObjNpcModel, typeof(GameObject), true) as GameObject;
        if (EditorUI.GUIButton("刷新", 100, 20))
        {
            if(listStoryInfo!=null)
                listStoryInfo.Clear();
            if (listAllStoryInfoDetails != null)
                listAllStoryInfoDetails.Clear();
            if (listOrderStoryInfoDetails != null)
                listOrderStoryInfoDetails.Clear();
            listStoryTextInfo = null;
        }
        //NPC创建
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("人物创建：", 100, 20);
        mNpcCreateIdStr = EditorUI.GUIEditorText(mNpcCreateIdStr, 100, 20);
        if (EditorUI.GUIButton("创建"))
        {
            CreateNpc(mNpcCreateIdStr);
        }
        GUILayout.EndHorizontal();

        //故事数据生成
        GUICreateStory();
        //查询相关UI
        GUIFindStoryInfo();
        //故事信息UI
        GUIStoryInfo();
        //故事详情
        GUIStoryInfoDetails();


        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private long inputId = 0;
    private void GUICreateStory()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("生成数据", 100, 20))
        {
            CreateStoryData(mCreateStoryInfo);
        }
        EditorUI.GUIText("故事数据生成：", 100, 20);
        mCreateStoryInfo.story_scene = (int)EditorUI.GUIEnum<ScenesEnum>("场景：", mCreateStoryInfo.story_scene, 300, 20);
        mCreateStoryInfo.id = mCreateStoryInfo.story_scene * 10000000;
        if (mCreateStoryInfo.story_scene == (int)ScenesEnum.GameTownScene)
        {
            mCreateStoryInfo.location_type = (int)EditorUI.GUIEnum<TownBuildingEnum>("城镇建筑：", mCreateStoryInfo.location_type, 150, 20);
            mCreateStoryInfo.id += mCreateStoryInfo.location_type * 100000;
        }
        inputId = EditorUI.GUIEditorText(inputId, 100, 20);
        mCreateStoryInfo.id += (int)inputId;
        EditorUI.GUIText("id：" + mCreateStoryInfo.id, 150, 20);
        EditorUI.GUIText("备注：", 50, 20);
        mCreateStoryInfo.note = EditorUI.GUIEditorText(mCreateStoryInfo.note + "", 100, 20);
        GUILayout.EndHorizontal();
    }

    private void GUIFindStoryInfo()
    {
        //故事查询
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("故事数据查询：", 100, 20);
        mFindStoryId = EditorUI.GUIEditorText(mFindStoryId, 100, 20);
        if (EditorUI.GUIButton("查询", 100, 20))
        {
            QueryStoryInfoData(mFindStoryId);
        }
        if (EditorUI.GUIButton("查询全部", 100, 20))
        {
            QueryStoryInfoData(-1);
        }
        if (EditorUI.GUIButton("查询客栈故事", 100, 20))
        {
            QueryStoryInfoDataByScene(ScenesEnum.GameInnScene);
        }
        if (EditorUI.GUIButton("查询小镇故事", 100, 20))
        {
            QueryStoryInfoDataByScene(ScenesEnum.GameTownScene);
        }
        if (EditorUI.GUIButton("查询竞技场故事", 100, 20))
        {
            QueryStoryInfoDataByScene(ScenesEnum.GameArenaScene);
        }
        GUILayout.EndHorizontal();
    }

    private void GUIStoryInfo()
    {
        if (listStoryInfo == null)
            return;
        for (int i = 0; i < listStoryInfo.Count; i++)
        {
            StoryInfoBean storyInfo = listStoryInfo[i];
            GUIStoryInfoItem(storyInfo);
        }
    }

    private void GUIStoryInfoItem(StoryInfoBean storyInfo)
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("删除", 50, 20))
        {
            storyInfoService.DeleteDataById(storyInfo.id);
            listStoryInfo.Remove(storyInfo);
        }
        if (EditorUI.GUIButton("更新", 50, 20))
        {
            storyInfoService.UpdateStoryData(storyInfo);
        }
        if (EditorUI.GUIButton("显示详情", 100, 20))
        {
            mFindStoryId = storyInfo.id;
            StoryInfoHandler.Instance.builderForStory.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
            QueryStoryInfoData(mFindStoryId);
            QueryStoryDetailsData(mFindStoryId);
        }
        EditorUI.GUIText("注释：", 50, 20);
        storyInfo.note = EditorUI.GUIEditorText(storyInfo.note + "", 200, 20);

        EditorUI.GUIText("id：" + storyInfo.id, 150, 20);
        storyInfo.story_scene = (int)EditorUI.GUIEnum<ScenesEnum>("场景：", storyInfo.story_scene, 300, 20);
        if (storyInfo.story_scene == (int)ScenesEnum.GameTownScene)
        {
            EditorUI.GUIText("故事发生地点：", 150, 20);
            storyInfo.location_type = (int)EditorUI.GUIEnum<TownBuildingEnum>("", storyInfo.location_type, 150, 20);

            EditorUI.GUIText("0外 1里：", 150, 20);
            storyInfo.out_in = int.Parse(EditorUI.GUIEditorText(storyInfo.out_in + "", 50, 20));
        }
        EditorUI.GUIText("坐标：", 150, 20);
        if (EditorUI.GUIButton("获取容器坐标", 150, 20))
        {
            storyInfo.position_x = StoryInfoHandler.Instance.builderForStory.transform.position.x;
            storyInfo.position_y = StoryInfoHandler.Instance.builderForStory.transform.position.y;
        }
        storyInfo.position_x = float.Parse(EditorUI.GUIEditorText(storyInfo.position_x + "", 100, 20));
        storyInfo.position_y = float.Parse(EditorUI.GUIEditorText(storyInfo.position_y + "", 100, 20));

        GUITriggerCondition(storyInfo);

        if (EditorUI.GUIButton("更新", 50, 20))
        {
            storyInfoService.UpdateStoryData(storyInfo);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    private void GUIStoryInfoDetails()
    {
        //故事内容
        if (listOrderStoryInfoDetails != null)
        {
            GUILayout.BeginHorizontal();
            if (EditorUI.GUIButton("刷新", 100, 20))
            {
                RefreshSceneData(listOrderStoryInfoDetails);
            }
            if (EditorUI.GUIButton("保存", 100, 20))
            {
                storyInfoService.UpdateStoryDetailsByIdAndOrder(mFindStoryId, mFindStroyOrder, listOrderStoryInfoDetails);
            }
            if (EditorUI.GUIButton("<", 100, 20))
            {
                mFindStroyOrder = (mFindStroyOrder - 1);
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(mFindStroyOrder);
                RefreshSceneData(listOrderStoryInfoDetails);
            }
            mFindStroyOrder = int.Parse(EditorUI.GUIEditorText(mFindStroyOrder + "", 100, 20));
            if (EditorUI.GUIButton(">", 100, 20))
            {
                mFindStroyOrder = mFindStroyOrder + 1;
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(mFindStroyOrder);
                RefreshSceneData(listOrderStoryInfoDetails);
            }
            GUILayout.EndHorizontal();
            GUIStoryInfoDetailsList();
        }
    }

    /// <summary>
    /// 展示指定顺序的故事详情
    /// </summary>
    public void GUIStoryInfoDetailsList()
    {
        for (int i = 0; i < listOrderStoryInfoDetails.Count; i++)
        {
            StoryInfoDetailsBean itemData = listOrderStoryInfoDetails[i];
            UIForStoryInfoDetails(itemData);
        }

        GUILayout.Space(50);
        UIForStoryInfoDetailsButton();
    }

    protected void UIForStoryInfoDetails(StoryInfoDetailsBean itemData)
    {
        GUILayout.BeginHorizontal();
        StoryInfoDetailsBean.StoryInfoDetailsTypeEnum storyInfoDetailsType = itemData.GetStoryInfoDetailsType();
        switch (storyInfoDetailsType)
        {
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                UIForStoryInfoDetailsNpcPosition(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
                UIForStoryInfoDetailsExpression(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                UIForStoryInfoDetailsNpcDestory(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip:
                UIForStoryInfoDetailsNpcEquip(itemData);
                break;

            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                UIForStoryInfoDetailsTalk(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                UIForStoryInfoDetailsAutoNext(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
                UIForStoryInfoDetailsPropPosition(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition:
                UIForStoryInfoDetailsWorkerPosition(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect:
                UIForStoryInfoDetailsEffect(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime:
                UIForStoryInfoSetTime(itemData);
                break;

            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                UIForStoryInfoDetailsCameraPosition(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                UIForStoryInfoDetailsCameraFollowCharacter(itemData);
                break;


            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                UIForStoryInfoDetailsAudioSound(itemData);
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic:
                UIForStoryInfoDetailsAudioMusic(itemData);
                break;



            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                UIForStoryInfoDetailsSceneInt(itemData);
                break;
        }
        GUILayout.EndHorizontal();
    }

    protected void UIForStoryInfoDetailsTalk(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除", 200, 20))
        {
            storyInfoService.DeleteDetailsDataByIdOrderType(itemData.story_id, itemData.story_order, itemData.type);
            listOrderStoryInfoDetails.Remove(itemData);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("对话 ", 120, 20);
        if (EditorUI.GUIButton("添加子对话", 120, 20))
        {
            if (listStoryTextInfo == null)
                listStoryTextInfo = new List<TextInfoBean>();
            TextInfoBean textInfo = new TextInfoBean();
            textInfo.id = (int)itemData.text_mark_id * 1000 + listStoryTextInfo.Count + 1;
            textInfo.text_id = textInfo.id;
            textInfo.type = 0;
            textInfo.mark_id = itemData.text_mark_id;
            textInfo.user_id = 0;
            textInfo.text_order = listStoryTextInfo.Count + 1;
            listStoryTextInfo.Add(textInfo);
        }
        if (EditorUI.GUIButton("删除所有对话", 120, 20))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        GUILayout.EndHorizontal();
        if (listStoryTextInfo != null)
        {
            TextInfoBean removeTempText = null;

            foreach (TextInfoBean textInfo in listStoryTextInfo)
            {

                GUILayout.BeginHorizontal();
                if (EditorUI.GUIButton("删除子对话", 120, 20))
                {
                    removeTempText = textInfo;
                    textInfoService.DeleteDataById(TextEnum.Story, textInfo.id);
                }
                if (EditorUI.GUIButton("更新", 120, 20))
                {
                    textInfoService.UpdateDataById(TextEnum.Story, textInfo.id, textInfo);
                }
                EditorUI.GUIText("ID", 50, 20);
                textInfo.id = EditorUI.GUIEditorText(textInfo.id, 120, 20);
                textInfo.type = (int)EditorUI.GUIEnum<TextInfoTypeEnum>("对话类型", textInfo.type, 300, 20);
                EditorUI.GUIText("对话顺序", 100, 20);
                textInfo.text_order = int.Parse(EditorUI.GUIEditorText(textInfo.text_order + "", 100, 20));
                EditorUI.GUIText("下一对话", 100, 20);
                textInfo.next_order = int.Parse(EditorUI.GUIEditorText(textInfo.next_order + "", 100, 20));
                if (textInfo.type == 0)
                {

                    EditorUI.GUIText("userID", 100, 20);
                    textInfo.user_id = EditorUI.GUIEditorText(textInfo.user_id, 100, 20);
                    EditorUI.GUIText("姓名", 100, 20);
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
                    EditorUI.GUIText(npcInfo.title_name + "-" + npcInfo.name, 120, 20);
                    EditorUI.GUIText("指定的姓名", 120, 20);
                    textInfo.name = EditorUI.GUIEditorText(textInfo.name, 100, 20);

                }
                else if (textInfo.type == 1)
                {
                    EditorUI.GUIText("select type", 120, 20);
                    textInfo.select_type = int.Parse(EditorUI.GUIEditorText(textInfo.select_type + "", 100, 20));
                    EditorUI.GUIText("分支选择", 120, 20);
                    if (textInfo.select_type == 0)
                    {
                        EditorUI.GUIText("默认对话", 120, 20);
                    }
                    else
                    {
                        EditorUI.GUIText("分支选项 下一句对话ID", 200, 20);
                        textInfo.next_order = int.Parse(EditorUI.GUIEditorText(textInfo.next_order + "", 100, 20));
                    }

                }
                else if (textInfo.type == 5)
                {
                    EditorUI.GUIText("黑幕时间", 120, 20);
                    textInfo.wait_time = float.Parse(EditorUI.GUIEditorText(textInfo.wait_time + "", 100, 20));
                }

                EditorUI.GUIText("对话内容", 120, 20);
                textInfo.content = EditorUI.GUIEditorText(textInfo.content, 400, 20);

                EditorUI.GUIText("增加的好感：", 120, 20);
                textInfo.add_favorability = int.Parse(EditorUI.GUIEditorText(textInfo.add_favorability + "", 50, 20));
                if (EditorUI.GUIButton("更新", 120, 20))
                {
                    textInfoService.UpdateDataById(TextEnum.Story, textInfo.id, textInfo);
                }

                textInfo.reward_data = EditorUI.GUIListData<RewardTypeEnum>("奖励", textInfo.reward_data);
                textInfo.pre_data = EditorUI.GUIListData<PreTypeEnum>("付出", textInfo.pre_data);
                GUILayout.EndHorizontal();
            }

            if (removeTempText != null)
                listStoryTextInfo.Remove(removeTempText);
        }
        GUILayout.EndVertical();
    }
    protected void UIForStoryInfoDetailsNpcPosition(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("更新显示", 100, 20))
        {
            RemoveSceneObjByName("character_" + itemData.num);
            RefreshSceneData(listOrderStoryInfoDetails);
        }
        if (EditorUI.GUIButton("获取显示坐标", 120, 20))
        {
            GameObject objItem = GetSceneObjByName("character_" + itemData.num);
            itemData.position_x = objItem.transform.localPosition.x;
            itemData.position_y = objItem.transform.localPosition.y;
        }
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        NpcInfoBean npcInfo;
        if (itemData.npc_id == 0)
        {
            npcInfo = new NpcInfoBean();
            npcInfo.name = "玩家";
        }
        else if (itemData.npc_id == -1)
        {
            npcInfo = new NpcInfoBean();
            npcInfo.name = "妻子";
        }
        else
        {
            npcInfo = mapNpcInfo[itemData.npc_id];
        }
        EditorUI.GUIText("NPCId(0:自己，-1：妻子):", 200, 20);
        itemData.npc_id = EditorUI.GUIEditorText(itemData.npc_id, 100, 20);
        EditorUI.GUIText("姓名：" + npcInfo.title_name + "-" + npcInfo.name, 200, 20);
        EditorUI.GUIText("NPC序号:", 100, 20);
        itemData.num = int.Parse(EditorUI.GUIEditorText(itemData.num + "", 50, 20));
        EditorUI.GUIText("NPC站位 ", 100, 20);
        EditorUI.GUIText("NPC位置X:", 120, 20);
        itemData.position_x = float.Parse(EditorUI.GUIEditorText(itemData.position_x + "", 100, 20));
        EditorUI.GUIText("NPC位置Y:", 120, 20);
        itemData.position_y = float.Parse(EditorUI.GUIEditorText(itemData.position_y + "", 100, 20));
        EditorUI.GUIText("NPC朝向1左2右:", 120, 20);
        itemData.face = int.Parse(EditorUI.GUIEditorText(itemData.face + "", 50, 20));
    }
    protected void UIForStoryInfoDetailsExpression(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("指定NPC展现表情 ", 150, 20);
        EditorUI.GUIText("NPC编号：", 120, 20);
        itemData.num = int.Parse(EditorUI.GUIEditorText(itemData.num + "", 200, 20));
        itemData.expression = (int)EditorUI.GUIEnum<CharacterExpressionEnum>("表情编号：", itemData.expression, 300, 20);
    }
    protected void UIForStoryInfoDetailsSceneInt(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("场景互动 ", 120, 20);
        EditorUI.GUIText("互动物体名称：", 120, 20);
        itemData.scene_intobj_name = EditorUI.GUIEditorText(itemData.scene_intobj_name, 200, 20);
        EditorUI.GUIText("互动类型名称：", 120, 20);
        itemData.scene_intcomponent_name = EditorUI.GUIEditorText(itemData.scene_intcomponent_name, 200, 20);
        EditorUI.GUIText("互动方法：", 120, 20);
        itemData.scene_intcomponent_method = EditorUI.GUIEditorText(itemData.scene_intcomponent_method, 200, 20);
        EditorUI.GUIText("互动方法参数：", 120, 20);
        itemData.scene_intcomponent_parameters = EditorUI.GUIEditorText(itemData.scene_intcomponent_parameters, 200, 20);
    }
    protected void UIForStoryInfoDetailsNpcDestory(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("删除角色(num,num)：", 120, 20);
        itemData.npc_destroy = EditorUI.GUIEditorText(itemData.npc_destroy, 200, 20);
        EditorUI.GUIText("延迟删除时间s：", 120, 20);
        itemData.wait_time = EditorUI.GUIEditorText(itemData.wait_time);
    }

    protected void UIForStoryInfoDetailsNpcEquip(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("指定NPC穿着（可以用，分割  前为男后为女）：", 300, 20);
        EditorUI.GUIText("NPC num：", 120, 20);
        itemData.num = EditorUI.GUIEditorText(itemData.num);
        EditorUI.GUIText("头ID(-1默认 0不穿)：", 150, 20);
        itemData.npc_hat = EditorUI.GUIEditorText(itemData.npc_hat);
        EditorUI.GUIText("衣ID(-1默认 0不穿)：", 150, 20);
        itemData.npc_clothes = EditorUI.GUIEditorText(itemData.npc_clothes);
        EditorUI.GUIText("鞋ID(-1默认 0不穿)：", 150, 20);
        itemData.npc_shoes = EditorUI.GUIEditorText(itemData.npc_shoes);
    }
    protected void UIForStoryInfoDetailsAutoNext(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("延迟执行 ", 100, 20);
        EditorUI.GUIText("延迟时间s:", 120, 20);
        itemData.wait_time = float.Parse(EditorUI.GUIEditorText(itemData.wait_time + "", 100, 20));
    }
    protected void UIForStoryInfoDetailsPropPosition(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        if (EditorUI.GUIButton("更新显示", 100, 20))
        {
            RemoveSceneObjByName("prop_" + itemData.num);
            RefreshSceneData(listOrderStoryInfoDetails);
        }
        if (EditorUI.GUIButton("获取显示坐标", 120, 20))
        {
            GameObject objItem = GetSceneObjByName("prop_" + itemData.num);
            itemData.position_x = objItem.transform.localPosition.x;
            itemData.position_y = objItem.transform.localPosition.y;
        }
        EditorUI.GUIText("道具名称:", 50, 20);
        itemData.key_name = EditorUI.GUIEditorText(itemData.key_name, 100, 20);
        EditorUI.GUIText("道具序号:", 100, 20);
        itemData.num = EditorUI.GUIEditorText(itemData.num, 50, 20);
        EditorUI.GUIText("道具站位 ", 100, 20);
        EditorUI.GUIText("道具位置X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("道具位置Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("道具朝向1左2右:", 120, 20);
        itemData.face = EditorUI.GUIEditorText(itemData.face, 50, 20);
    }

    protected void UIForStoryInfoDetailsWorkerPosition(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("员工站位", 100, 20);
        EditorUI.GUIText("员工站位X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("员工站位Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("员工朝向1左2右:", 120, 20);
        itemData.face = EditorUI.GUIEditorText(itemData.face , 50, 20);
        EditorUI.GUIText("员工间隔(x y):", 120, 20);
        itemData.offset_x = EditorUI.GUIEditorText(itemData.offset_x , 50, 20);
        itemData.offset_y = EditorUI.GUIEditorText(itemData.offset_y, 50, 20);
        EditorUI.GUIText("员工横竖排数(h v):", 120, 20);
        itemData.horizontal = EditorUI.GUIEditorText(itemData.horizontal, 50, 20);
        itemData.vertical = EditorUI.GUIEditorText(itemData.vertical, 50, 20);
    }

    protected  void UIForStoryInfoDetailsEffect(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("粒子名称:", 50, 20);
        itemData.key_name = EditorUI.GUIEditorText(itemData.key_name, 100, 20);
        EditorUI.GUIText("粒子位置X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("粒子位置Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("持续时间（-1为永久）:", 120, 20);
        itemData.wait_time= EditorUI.GUIEditorText(itemData.wait_time, 100, 20);
    }
    protected void UIForStoryInfoSetTime(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("设置时间:", 50, 20);
        EditorUI.GUIText("小时:", 50, 20);
        itemData.time_hour = EditorUI.GUIEditorText(itemData.time_hour, 100, 20);
        EditorUI.GUIText("分钟", 50, 20);
        itemData.time_minute = EditorUI.GUIEditorText(itemData.time_minute, 100, 20);
    }

    protected void UIForStoryInfoDetailsCameraPosition(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("摄像头位置 ", 150, 20);
        EditorUI.GUIText("x:", 50, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("y:", 50, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
    }


    protected void UIForStoryInfoDetailsCameraFollowCharacter(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        EditorUI.GUIText("摄像头跟随角色序号 ", 200, 20);
        itemData.num = EditorUI.GUIEditorText(itemData.num , 100, 20);
    }
    protected void UIForStoryInfoDetailsAudioSound(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        itemData.audio_sound = (int)EditorUI.GUIEnum<AudioSoundEnum>("音效类型：", itemData.audio_sound, 300, 20);
    }
    protected void UIForStoryInfoDetailsAudioMusic(StoryInfoDetailsBean itemData)
    {
        if (EditorUI.GUIButton("删除"))
        {
            RemoveStoryInfoDetailsItem(itemData);
            return;
        }
        itemData.audio_music = (int)EditorUI.GUIEnum<AudioMusicEnum>("音乐类型：", itemData.audio_music, 300, 20);
    }

    protected void UIForStoryInfoDetailsButton()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("添加站位", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition);
        }
        if (EditorUI.GUIButton("添加人物表情", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression);
        }
        if (EditorUI.GUIButton("删除人物", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory);
        }
        if (EditorUI.GUIButton("设置人物装备", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("添加对话", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk);
        }
        if (EditorUI.GUIButton("添加场景互动", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt);
        }
        if (EditorUI.GUIButton("添加延迟", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext);
        }
        if (EditorUI.GUIButton("添加道具位置", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition);
        }
        if (EditorUI.GUIButton("添加员工", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition);
        }
        if (EditorUI.GUIButton("添加粒子", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect);
        }
        if (EditorUI.GUIButton("设置时间", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("添加摄像头坐标", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition);
        }
        if (EditorUI.GUIButton("添加摄像头跟随角色", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter);
        }
        if (EditorUI.GUIButton("添加音效播放", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound);
        }
        if (EditorUI.GUIButton("添加音乐播放", 200, 20))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic);
        }

        GUILayout.EndHorizontal();

        if (EditorUI.GUIButton("保存该序号下的故事数据", 200, 20))
        {
            storyInfoService.UpdateStoryDetailsByIdAndOrder(mFindStoryId, mFindStroyOrder, listOrderStoryInfoDetails);
        }
    }


    protected void RemoveStoryInfoDetailsItem(StoryInfoDetailsBean itemData)
    {
        listAllStoryInfoDetails.Remove(itemData);
        listOrderStoryInfoDetails.Remove(itemData);
        if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
        {
            RemoveSceneObjByName("character_" + itemData.num);
        }
        else if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition)
        {
            RemoveSceneObjByName("prop_" + itemData.num);
        }

        return;
    }

    /// <summary>
    /// 创建一个NPC
    /// </summary>
    /// <param name="idStr"></param>
    public GameObject CreateNpc(string idStr)
    {
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
            characterData = new CharacterBean(mapNpcInfo[createNpcId]);
        }

        if (characterData == null)
        {
            LogUtil.LogError("没有找到id为" + createNpcId + "的NPC");
            return null;
        }

        objNpc = Instantiate(StoryInfoHandler.Instance.manager.objNpcModel, StoryInfoHandler.Instance.builderForStory.transform);
        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        baseNpcAI.Awake();

        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(baseNpcAI.gameObject, "Body");
        characterDress.Awake();

        baseNpcAI.transform.localPosition = position;
        baseNpcAI.SetCharacterData(characterData);
        baseNpcAI.name = "character_" + number;
        objNpc.SetActive(true);

        return objNpc;
    }

    /// <summary>
    /// 创建故事数据
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="positionType"></param>
    /// <param name="storyId"></param>
    public void CreateStoryData(StoryInfoBean storyInfo)
    {
        storyInfo.position_x = 0;
        storyInfo.position_y = 0;
        storyInfo.valid = 1;
        storyInfoService.CreateStoryInfo(storyInfo);
    }

    public void QueryStoryInfoData(long findStoryId)
    {
        if (findStoryId == -1)
        {
            listStoryInfo = storyInfoService.QueryAllStoryData();
        }
        else
        {
            listStoryInfo = storyInfoService.QueryStoryData(mFindStoryId);
        }
    }

    public void QueryStoryInfoDataByScene(ScenesEnum scenesEnum)
    {
        listStoryInfo = storyInfoService.QueryStoryData((int)scenesEnum);
    }

    /// <summary>
    /// 查询故事详细数据
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="storyId"></param>
    public void QueryStoryDetailsData(long mFindStoryId)
    {
        //清空容器
        for (int i = 0; i < StoryInfoHandler.Instance.builderForStory.transform.childCount; i++)
        {
            if (StoryInfoHandler.Instance.builderForStory.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject.DestroyImmediate(StoryInfoHandler.Instance.builderForStory.transform.GetChild(i).gameObject);
            }
        }
        listAllStoryInfoDetails = storyInfoService.QueryStoryDetailsById(mFindStoryId);
        listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(mFindStroyOrder);
        RefreshSceneData(listOrderStoryInfoDetails);
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
    /// 刷新场景数据
    /// </summary>
    /// <param name="listData"></param>
    public void RefreshSceneData(List<StoryInfoDetailsBean> listData)
    {
        if (listData == null)
            return;
        if (listStoryTextInfo != null)
            listStoryTextInfo.Clear();
        listStoryTextInfo = null;
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            StoryInfoDetailsBean.StoryInfoDetailsTypeEnum storyInfoDetailsType = itemData.GetStoryInfoDetailsType();
            if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            {
                GameObject objNpc = GetSceneObjByName("character_" + itemData.num);
                BaseNpcAI npcAI = null;
                if (objNpc!=null)
                     npcAI = objNpc.GetComponent<BaseNpcAI>();
     
                if (npcAI == null)
                {
                    NpcInfoBean npcInfoBean;
                    if (itemData.npc_id == 0)
                    {
                        npcInfoBean = new NpcInfoBean();
                    }
                    else if (itemData.npc_id == -1)
                    {
                        npcInfoBean = new NpcInfoBean();
                    }
                    else
                    {
                        if (mapNpcInfo.TryGetValue(itemData.npc_id, out NpcInfoBean npcInfo))
                            npcInfoBean = npcInfo;
                        else
                        {
                            npcInfoBean = new NpcInfoBean();
                            LogUtil.LogError("创建NPC失败 找不到ID为" + itemData.npc_id + "的NPC信息");
                        }
                    }
                    objNpc = CreateNpc(npcInfoBean.npc_id, new Vector3(itemData.position_x, itemData.position_y), itemData.num);
                    npcAI = objNpc.GetComponent<BaseNpcAI>();
                }
                npcAI.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
                //设置朝向
                npcAI.SetCharacterFace(itemData.face);
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip)
            {
                GameObject objNpc = GetSceneObjByName("character_" + itemData.num);
                if (objNpc == null)
                {
                    continue;
                }
                BaseNpcAI npcAI = objNpc.GetComponent<BaseNpcAI>();
                if (npcAI == null)
                {
                    continue;
                }
                SexEnum sex = npcAI.characterData.body.GetSex();
                itemData.GetNpcEquip(sex, out long hatId, out long clothesId, out long shoesId);
                if (hatId != -1)
                    npcAI.characterData.equips.hatTFId = hatId;
                if (clothesId != -1)
                    npcAI.characterData.equips.clothesTFId = clothesId;
                if (shoesId != -1)
                    npcAI.characterData.equips.shoesTFId = shoesId;
                npcAI.SetCharacterData(npcAI.characterData);
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition)
            {
                GameObject objProp = GetSceneObjByName("prop_" + itemData.num);
                if (objProp == null)
                {
                    GameObject objModel = StoryInfoHandler.Instance.manager.GetStoryPropModelByName(itemData.key_name);
                    objProp = Instantiate(objModel, StoryInfoHandler.Instance.builderForStory.transform);
                    objProp.name = "prop_" + itemData.num;
                }
                //设置位置和朝向
                objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
                Vector3 bodyScale = objProp.transform.localScale;
                switch (itemData.face)
                {
                    case 1:
                        bodyScale.x = -1;
                        break;

                    case 2:
                        bodyScale.x = 1;
                        break;
                }
                objProp.transform.localScale = bodyScale;
            }
            //如果是对话 查询对话数据
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                listStoryTextInfo = textInfoService.QueryDataByMarkId(TextEnum.Story, itemData.text_mark_id);
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory)
            {
                int[] numList = itemData.npc_destroy.SplitForArrayInt(',');
                foreach (int num in numList)
                {
                    BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(StoryInfoHandler.Instance.builderForStory.gameObject, num + "");
                    DestroyImmediate(npcAI.gameObject);
                }
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition)
            {
                Vector3 cameraWorldPosition = StoryInfoHandler.Instance.builderForStory.transform.TransformPoint(new Vector3(itemData.position_x, itemData.position_y, -10));

                CameraHandler.Instance.manager.camera2D.Follow = null;
                CameraHandler.Instance.manager.camera2D.transform.position = cameraWorldPosition;
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter)
            {
                BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(StoryInfoHandler.Instance.builderForStory.gameObject,"character_"+ itemData.num);
                CameraHandler.Instance.manager.camera2D.Follow = npcAI.transform;
            }
        }
    }

    public void CreateStoryInfoDetailsDataByType(long storyId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum type)
    {
        StoryInfoDetailsBean itemDetailsInfo = new StoryInfoDetailsBean();
        itemDetailsInfo.type = (int)type;
        switch (type)
        {
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                itemDetailsInfo.npc_id = 1;
                itemDetailsInfo.num = 1;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                itemDetailsInfo.text_mark_id = storyId * 10000 + mFindStroyOrder;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                itemDetailsInfo.wait_time = 1;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                itemDetailsInfo.audio_sound = (int)AudioSoundEnum.ButtonForNormal;
                break;
        }

        itemDetailsInfo.story_id = storyId;
        itemDetailsInfo.story_order = mFindStroyOrder;
        listAllStoryInfoDetails.Add(itemDetailsInfo);
        listOrderStoryInfoDetails.Add(itemDetailsInfo);
        storyInfoService.UpdateStoryDetailsByIdAndOrder(mFindStoryId, mFindStroyOrder, listOrderStoryInfoDetails);
    }

    /// <summary>
    /// 根据名字删除物品
    /// </summary>
    /// <param name="name"></param>
    public void RemoveSceneObjByName(string name)
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
        GameObject objBuilderForStory = StoryInfoHandler.Instance.builderForStory.gameObject;
        for (int i = 0; i < objBuilderForStory.transform.childCount; i++)
        {
            if (objBuilderForStory.transform.GetChild(i).gameObject.activeSelf)
            {
                if (objBuilderForStory.transform.GetChild(i).gameObject.name.Equals(name))
                {
                    return objBuilderForStory.transform.GetChild(i).gameObject;
                }
            }
        }
        return null;
    }


    /// <summary>
    /// 前置条件UI
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUITriggerCondition(StoryInfoBean storyInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        EditorUI.GUIText("触发条件：", 100, 20);
        if (EditorUI.GUIButton("添加条件", 100, 20))
        {
            storyInfo.trigger_condition += ("|" + EventTriggerEnum.Year.GetEnumName() + ":" + "1|");
        }
        List<string> listTriggerData = storyInfo.trigger_condition.SplitForListStr('|');
        storyInfo.trigger_condition = "";
        for (int i = 0; i < listTriggerData.Count; i++)
        {
            string itemTriggerData = listTriggerData[i];
            if (itemTriggerData.IsNull())
            {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            if (EditorUI.GUIButton("删除"))
            {
                listTriggerData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemTriggerData = itemTriggerData.SplitForListStr(':');
            listItemTriggerData[0] = EditorUI.GUIEnum<EventTriggerEnum>("触发条件", (int)listItemTriggerData[0].GetEnum<EventTriggerEnum>(), 300, 20).GetEnumName();
            listItemTriggerData[1] = EditorUI.GUIEditorText(listItemTriggerData[1] + "", 100, 20);
            EditorGUILayout.EndHorizontal();
            storyInfo.trigger_condition += (listItemTriggerData[0] + ":" + listItemTriggerData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }
}