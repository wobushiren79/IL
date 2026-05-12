using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
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
        listAllStoryInfoDetails = null;
        listOrderStoryInfoDetails = null;
        listStoryTextInfo = null;
        mapNpcInfo.Clear();
        var dicNpc = NpcInfoCfg.GetAllData();
        if (dicNpc != null)
            foreach (var item in dicNpc)
                mapNpcInfo.Add(item.Key, item.Value);
        GameItemsHandler.Instance.manager.Awake();
        StoryInfoHandler.Instance.manager.Awake();
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
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        if (EditorUI.GUIButton("刷新", 100, 20))
        {
            if (listStoryInfo != null) listStoryInfo.Clear();
            if (listAllStoryInfoDetails != null) listAllStoryInfoDetails.Clear();
            if (listOrderStoryInfoDetails != null) listOrderStoryInfoDetails.Clear();
            listStoryTextInfo = null;
        }
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("人物创建：", 100, 20);
        mNpcCreateIdStr = EditorUI.GUIEditorText(mNpcCreateIdStr, 100, 20);
        if (EditorUI.GUIButton("创建"))
            CreateNpc(mNpcCreateIdStr);
        GUILayout.EndHorizontal();

        GUICreateStory();
        GUIFindStoryInfo();
        GUIStoryInfo();
        GUIStoryInfoDetails();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private long inputId = 0;
    private void GUICreateStory()
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("故事数据生成 [只读，请编辑Excel文件]：", 250, 20);
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
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("故事数据查询：", 100, 20);
        mFindStoryId = EditorUI.GUIEditorText(mFindStoryId, 100, 20);
        if (EditorUI.GUIButton("查询", 100, 20))
            QueryStoryInfoData(mFindStoryId);
        if (EditorUI.GUIButton("查询全部", 100, 20))
            QueryStoryInfoData(-1);
        if (EditorUI.GUIButton("查询客栈故事", 100, 20))
            QueryStoryInfoDataByScene(ScenesEnum.GameInnScene);
        if (EditorUI.GUIButton("查询小镇故事", 100, 20))
            QueryStoryInfoDataByScene(ScenesEnum.GameTownScene);
        if (EditorUI.GUIButton("查询竞技场故事", 100, 20))
            QueryStoryInfoDataByScene(ScenesEnum.GameArenaScene);
        GUILayout.EndHorizontal();
    }

    private void GUIStoryInfo()
    {
        if (listStoryInfo == null) return;
        for (int i = 0; i < listStoryInfo.Count; i++)
            GUIStoryInfoItem(listStoryInfo[i]);
    }

    private void GUIStoryInfoItem(StoryInfoBean storyInfo)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));
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
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    private void GUIStoryInfoDetails()
    {
        if (listOrderStoryInfoDetails == null) return;
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("刷新", 100, 20))
            RefreshSceneData(listOrderStoryInfoDetails);
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

    public void GUIStoryInfoDetailsList()
    {
        for (int i = 0; i < listOrderStoryInfoDetails.Count; i++)
            UIForStoryInfoDetails(listOrderStoryInfoDetails[i]);
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
                UIForStoryInfoDetailsNpcPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
                UIForStoryInfoDetailsExpression(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                UIForStoryInfoDetailsNpcDestory(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip:
                UIForStoryInfoDetailsNpcEquip(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                UIForStoryInfoDetailsTalk(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                UIForStoryInfoDetailsAutoNext(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
                UIForStoryInfoDetailsPropPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition:
                UIForStoryInfoDetailsWorkerPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect:
                UIForStoryInfoDetailsEffect(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime:
                UIForStoryInfoSetTime(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                UIForStoryInfoDetailsCameraPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                UIForStoryInfoDetailsCameraFollowCharacter(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                UIForStoryInfoDetailsAudioSound(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic:
                UIForStoryInfoDetailsAudioMusic(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                UIForStoryInfoDetailsSceneInt(itemData); break;
        }
        GUILayout.EndHorizontal();
    }

    protected void UIForStoryInfoDetailsTalk(StoryInfoDetailsBean itemData)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("对话 [只读，请编辑Excel文件]", 200, 20);
        GUILayout.EndHorizontal();
        if (listStoryTextInfo != null)
        {
            foreach (TextInfoBean textInfo in listStoryTextInfo)
            {
                GUILayout.BeginHorizontal();
                EditorUI.GUIText("ID", 50, 20);
                EditorGUILayout.LabelField(textInfo.id + "", GUILayout.Width(120), GUILayout.Height(20));
                textInfo.type = (int)EditorUI.GUIEnum<TextInfoTypeEnum>("对话类型", textInfo.type, 300, 20);
                EditorUI.GUIText("对话顺序", 100, 20);
                EditorGUILayout.LabelField(textInfo.text_order + "", GUILayout.Width(100), GUILayout.Height(20));
                EditorUI.GUIText("下一对话", 100, 20);
                EditorGUILayout.LabelField(textInfo.next_order + "", GUILayout.Width(100), GUILayout.Height(20));
                if (textInfo.type == 0)
                {
                    EditorUI.GUIText("userID", 100, 20);
                    EditorGUILayout.LabelField(textInfo.user_id + "", GUILayout.Width(100), GUILayout.Height(20));
                    NpcInfoBean npcInfo;
                    if (textInfo.user_id == 0)
                    { npcInfo = new NpcInfoBean(); npcInfo.name_language = "玩家"; }
                    else
                        mapNpcInfo.TryGetValue(textInfo.user_id, out npcInfo);
                    if (npcInfo != null)
                        EditorUI.GUIText(npcInfo.title_name_language + "-" + npcInfo.name, 120, 20);
                    EditorUI.GUIText("指定的姓名", 120, 20);
                    EditorGUILayout.LabelField(textInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
                }
                else if (textInfo.type == 5)
                {
                    EditorUI.GUIText("黑幕时间", 120, 20);
                    EditorGUILayout.LabelField(textInfo.wait_time + "", GUILayout.Width(100), GUILayout.Height(20));
                }
                EditorUI.GUIText("对话内容", 120, 20);
                EditorGUILayout.LabelField(textInfo.content, GUILayout.Width(400), GUILayout.Height(20));
                GUILayout.EndHorizontal();
            }
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
        NpcInfoBean npcInfo;
        if (itemData.npc_id == 0) { npcInfo = new NpcInfoBean(); npcInfo.name_language = "玩家"; }
        else if (itemData.npc_id == -1) { npcInfo = new NpcInfoBean(); npcInfo.name_language = "妻子"; }
        else mapNpcInfo.TryGetValue(itemData.npc_id, out npcInfo);
        EditorUI.GUIText("NPCId(0:自己，-1：妻子):", 200, 20);
        itemData.npc_id = EditorUI.GUIEditorText(itemData.npc_id, 100, 20);
        if (npcInfo != null)
            EditorUI.GUIText("姓名：" + npcInfo.title_name_language + "-" + npcInfo.name, 200, 20);
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
        EditorUI.GUIText("延迟执行 ", 100, 20);
        EditorUI.GUIText("延迟时间s:", 120, 20);
        itemData.wait_time = float.Parse(EditorUI.GUIEditorText(itemData.wait_time + "", 100, 20));
    }

    protected void UIForStoryInfoDetailsPropPosition(StoryInfoDetailsBean itemData)
    {
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
        EditorUI.GUIText("道具位置X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("道具位置Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("道具朝向1左2右:", 120, 20);
        itemData.face = EditorUI.GUIEditorText(itemData.face, 50, 20);
    }

    protected void UIForStoryInfoDetailsWorkerPosition(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("员工站位", 100, 20);
        EditorUI.GUIText("员工站位X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("员工站位Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("员工朝向1左2右:", 120, 20);
        itemData.face = EditorUI.GUIEditorText(itemData.face, 50, 20);
        EditorUI.GUIText("员工间隔(x y):", 120, 20);
        itemData.offset_x = EditorUI.GUIEditorText(itemData.offset_x, 50, 20);
        itemData.offset_y = EditorUI.GUIEditorText(itemData.offset_y, 50, 20);
        EditorUI.GUIText("员工横竖排数(h v):", 120, 20);
        itemData.horizontal = EditorUI.GUIEditorText(itemData.horizontal, 50, 20);
        itemData.vertical = EditorUI.GUIEditorText(itemData.vertical, 50, 20);
    }

    protected void UIForStoryInfoDetailsEffect(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("粒子名称:", 50, 20);
        itemData.key_name = EditorUI.GUIEditorText(itemData.key_name, 100, 20);
        EditorUI.GUIText("粒子位置X:", 120, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("粒子位置Y:", 120, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
        EditorUI.GUIText("持续时间（-1为永久）:", 120, 20);
        itemData.wait_time = EditorUI.GUIEditorText(itemData.wait_time, 100, 20);
    }

    protected void UIForStoryInfoSetTime(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("设置时间:", 50, 20);
        EditorUI.GUIText("小时:", 50, 20);
        itemData.time_hour = EditorUI.GUIEditorText(itemData.time_hour, 100, 20);
        EditorUI.GUIText("分钟", 50, 20);
        itemData.time_minute = EditorUI.GUIEditorText(itemData.time_minute, 100, 20);
    }

    protected void UIForStoryInfoDetailsCameraPosition(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("摄像头位置 ", 150, 20);
        EditorUI.GUIText("x:", 50, 20);
        itemData.position_x = EditorUI.GUIEditorText(itemData.position_x, 100, 20);
        EditorUI.GUIText("y:", 50, 20);
        itemData.position_y = EditorUI.GUIEditorText(itemData.position_y, 100, 20);
    }

    protected void UIForStoryInfoDetailsCameraFollowCharacter(StoryInfoDetailsBean itemData)
    {
        EditorUI.GUIText("摄像头跟随角色序号 ", 200, 20);
        itemData.num = EditorUI.GUIEditorText(itemData.num, 100, 20);
    }

    protected void UIForStoryInfoDetailsAudioSound(StoryInfoDetailsBean itemData)
    {
        itemData.audio_sound = (int)EditorUI.GUIEnum<AudioSoundEnum>("音效类型：", itemData.audio_sound, 300, 20);
    }

    protected void UIForStoryInfoDetailsAudioMusic(StoryInfoDetailsBean itemData)
    {
        itemData.audio_music = (int)EditorUI.GUIEnum<AudioMusicEnum>("音乐类型：", itemData.audio_music, 300, 20);
    }

    protected void UIForStoryInfoDetailsButton()
    {
        GUILayout.Label("[只读，请编辑Excel文件]", GUILayout.Width(300), GUILayout.Height(20));
    }

    protected void RemoveStoryInfoDetailsItem(StoryInfoDetailsBean itemData)
    {
        listAllStoryInfoDetails.Remove(itemData);
        listOrderStoryInfoDetails.Remove(itemData);
        if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            RemoveSceneObjByName("character_" + itemData.num);
        else if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition)
            RemoveSceneObjByName("prop_" + itemData.num);
    }

    public GameObject CreateNpc(string idStr)
    {
        if (long.TryParse(idStr, out long createNpcId))
            return CreateNpc(createNpcId, Vector3.zero, 0);
        else
            LogUtil.LogError("创建人物ID不规范");
        return null;
    }

    public GameObject CreateNpc(long createNpcId, Vector3 position, int number)
    {
        CharacterBean characterData = null;
        if (createNpcId == 0)
            characterData = new CharacterBean();
        else if (mapNpcInfo.TryGetValue(createNpcId, out NpcInfoBean npcInfo))
            characterData = new CharacterBean(npcInfo);
        if (characterData == null)
        {
            LogUtil.LogError("没有找到id为" + createNpcId + "的NPC");
            return null;
        }
        GameObject objNpc = Instantiate(StoryInfoHandler.Instance.manager.objNpcModel, StoryInfoHandler.Instance.builderForStory.transform);
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

    public void QueryStoryInfoData(long findStoryId)
    {
        listStoryInfo = new List<StoryInfoBean>();
        var dic = StoryInfoCfg.GetAllData();
        if (dic == null) return;
        if (findStoryId == -1)
        {
            foreach (var item in dic) listStoryInfo.Add(item.Value);
        }
        else
        {
            if (dic.TryGetValue(findStoryId, out StoryInfoBean story))
                listStoryInfo.Add(story);
        }
    }

    public void QueryStoryInfoDataByScene(ScenesEnum scenesEnum)
    {
        listStoryInfo = new List<StoryInfoBean>();
        var dic = StoryInfoCfg.GetAllData();
        if (dic == null) return;
        foreach (var item in dic)
            if (item.Value.story_scene == (int)scenesEnum)
                listStoryInfo.Add(item.Value);
    }

    public void QueryStoryDetailsData(long findStoryId)
    {
        for (int i = 0; i < StoryInfoHandler.Instance.builderForStory.transform.childCount; i++)
        {
            if (StoryInfoHandler.Instance.builderForStory.transform.GetChild(i).gameObject.activeSelf)
                GameObject.DestroyImmediate(StoryInfoHandler.Instance.builderForStory.transform.GetChild(i).gameObject);
        }
        listAllStoryInfoDetails = new List<StoryInfoDetailsBean>();
        StoryInfoDetailsBean[] array = StoryInfoDetailsCfg.GetAllArrayData();
        if (array != null)
            foreach (StoryInfoDetailsBean item in array)
                if (item.id == findStoryId)
                    listAllStoryInfoDetails.Add(item);
        listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(mFindStroyOrder);
        RefreshSceneData(listOrderStoryInfoDetails);
    }

    public List<StoryInfoDetailsBean> GetStoryInfoDetailsByOrder(int order)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        if (listAllStoryInfoDetails == null) return listData;
        foreach (StoryInfoDetailsBean itemData in listAllStoryInfoDetails)
            if (itemData.story_order == order)
                listData.Add(itemData);
        return listData;
    }

    private static TextInfoBean ConvertStoryToTextInfo(TextStoryBean src)
    {
        if (src == null) return null;
        TextInfoBean bean = new TextInfoBean();
        bean.id = src.id;
        bean.valid = src.valid;
        bean.mark_id = src.mark_id;
        bean.type = src.type;
        bean.text_order = src.text_order;
        bean.next_order = src.next_order;
        bean.talk_type = src.talk_type;
        bean.user_id = src.user_id;
        bean.condition_min_favorability = src.condition_min_favorability;
        bean.condition_max_favorability = src.condition_max_favorability;
        bean.select_type = src.select_type;
        bean.add_favorability = src.add_favorability;
        bean.pre_data_minigame = src.pre_data_minigame;
        bean.reward_data = src.reward_data;
        bean.wait_time = src.wait_time;
        bean.is_stoptime = src.is_stoptime;
        bean.scene_expression = src.scene_expression;
        bean.pre_data = src.pre_data;
        bean.name = src.name_language;
        bean.content = src.content_language;
        return bean;
    }

    public void RefreshSceneData(List<StoryInfoDetailsBean> listData)
    {
        if (listData == null) return;
        if (listStoryTextInfo != null) listStoryTextInfo.Clear();
        listStoryTextInfo = null;
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            StoryInfoDetailsBean.StoryInfoDetailsTypeEnum storyInfoDetailsType = itemData.GetStoryInfoDetailsType();
            if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            {
                GameObject objNpc = GetSceneObjByName("character_" + itemData.num);
                BaseNpcAI npcAI = objNpc != null ? objNpc.GetComponent<BaseNpcAI>() : null;
                if (npcAI == null)
                {
                    NpcInfoBean npcInfoBean;
                    if (itemData.npc_id == 0 || itemData.npc_id == -1)
                        npcInfoBean = new NpcInfoBean();
                    else if (!mapNpcInfo.TryGetValue(itemData.npc_id, out npcInfoBean))
                    {
                        npcInfoBean = new NpcInfoBean();
                        LogUtil.LogError("创建NPC失败 找不到ID为" + itemData.npc_id + "的NPC信息");
                    }
                    objNpc = CreateNpc(npcInfoBean.id, new Vector3(itemData.position_x, itemData.position_y), itemData.num);
                    npcAI = objNpc.GetComponent<BaseNpcAI>();
                }
                npcAI.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
                npcAI.SetCharacterFace(itemData.face);
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip)
            {
                GameObject objNpc = GetSceneObjByName("character_" + itemData.num);
                if (objNpc == null) continue;
                BaseNpcAI npcAI = objNpc.GetComponent<BaseNpcAI>();
                if (npcAI == null) continue;
                SexEnum sex = npcAI.characterData.body.GetSex();
                itemData.GetNpcEquip(sex, out long hatId, out long clothesId, out long shoesId);
                if (hatId != -1) npcAI.characterData.equips.hatTFId = hatId;
                if (clothesId != -1) npcAI.characterData.equips.clothesTFId = clothesId;
                if (shoesId != -1) npcAI.characterData.equips.shoesTFId = shoesId;
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
                objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
                Vector3 bodyScale = objProp.transform.localScale;
                if (itemData.face == 1) bodyScale.x = -1;
                else if (itemData.face == 2) bodyScale.x = 1;
                objProp.transform.localScale = bodyScale;
            }
            else if (storyInfoDetailsType == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                listStoryTextInfo = new List<TextInfoBean>();
                TextStoryBean[] array = TextStoryCfg.GetAllArrayData();
                if (array != null)
                    foreach (TextStoryBean item in array)
                        if (item.mark_id == itemData.text_mark_id)
                            listStoryTextInfo.Add(ConvertStoryToTextInfo(item));
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
                BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(StoryInfoHandler.Instance.builderForStory.gameObject, "character_" + itemData.num);
                CameraHandler.Instance.manager.camera2D.Follow = npcAI.transform;
            }
        }
    }

    public void RemoveSceneObjByName(string name)
    {
        GameObject objTarget = GetSceneObjByName(name);
        if (objTarget != null) GameObject.DestroyImmediate(objTarget);
    }

    public GameObject GetSceneObjByName(string name)
    {
        GameObject objBuilderForStory = StoryInfoHandler.Instance.builderForStory.gameObject;
        for (int i = 0; i < objBuilderForStory.transform.childCount; i++)
        {
            if (objBuilderForStory.transform.GetChild(i).gameObject.activeSelf)
                if (objBuilderForStory.transform.GetChild(i).gameObject.name.Equals(name))
                    return objBuilderForStory.transform.GetChild(i).gameObject;
        }
        return null;
    }

    private void GUITriggerCondition(StoryInfoBean storyInfo)
    {
        EditorGUILayout.BeginVertical();
        EditorUI.GUIText("触发条件：", 100, 20);
        if (EditorUI.GUIButton("添加条件", 100, 20))
            storyInfo.trigger_condition += ("|" + EventTriggerEnum.Year.GetEnumName() + ":" + "1|");
        List<string> listTriggerData = storyInfo.trigger_condition.SplitForListStr('|');
        storyInfo.trigger_condition = "";
        for (int i = 0; i < listTriggerData.Count; i++)
        {
            string itemTriggerData = listTriggerData[i];
            if (itemTriggerData.IsNull()) continue;
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
