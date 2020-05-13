using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cinemachine;
using static CharacterExpressionCpt;

public class StoryCreateWindowsEditor : EditorWindow
{
    private GameObject mObjContent;
    private GameObject mObjNpcModel;
    //镜头
    private GameObject mObjCarmera;
    private CinemachineVirtualCamera mCamera2D;

    [MenuItem("Tools/Window/StoryCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(StoryCreateWindowsEditor));
    }

    public StoryCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("剧情创建辅助工具");
    }

    private void OnDestroy()
    {
        CptUtil.RemoveChildsByActiveInEditor(mObjContent);
    }

    private void OnFocus()
    {
        mCamera2D = mObjCarmera.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        mObjContent = GameObject.Find("StoryBuilder");
        mObjNpcModel = mObjContent.transform.Find("CharacterForStory").gameObject;
        mObjCarmera = GameObject.Find("Camera2D");

        mCamera2D = mObjCarmera.GetComponent<CinemachineVirtualCamera>();
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
    private StoryInfoBean mCreateStoryInfo = new StoryInfoBean();
    private long mFindStoryId = 0;
    private int mFindStroyOrder = 1;


    List<StoryInfoBean> listStoryInfo = new List<StoryInfoBean>();

    List<StoryInfoDetailsBean> listAllStoryInfoDetails=new List<StoryInfoDetailsBean>();
    List<StoryInfoDetailsBean> listOrderStoryInfoDetails=new List<StoryInfoDetailsBean>();
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
        if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listStoryInfo.Clear();
            listAllStoryInfoDetails.Clear();
            listOrderStoryInfoDetails.Clear();
            listStoryTextInfo = null;
        }
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
        if (GUILayout.Button("生成数据", GUILayout.Width(100), GUILayout.Height(20)))
        {
            CreateStoryData(mCreateStoryInfo);
        }
        GUILayout.Label("故事数据生成：", GUILayout.Width(100), GUILayout.Height(20));
        mCreateStoryInfo.story_scene = (int)(ScenesEnum)EditorGUILayout.EnumPopup("场景：", (ScenesEnum)mCreateStoryInfo.story_scene, GUILayout.Width(300), GUILayout.Height(20));
        mCreateStoryInfo.id = mCreateStoryInfo.story_scene * 10000000;
        if (mCreateStoryInfo.story_scene == (int)ScenesEnum.GameTownScene)
        {
            GUILayout.Label("城镇建筑：", GUILayout.Width(50), GUILayout.Height(20));
            mCreateStoryInfo.location_type = (int)(TownBuildingEnum)EditorGUILayout.EnumPopup((TownBuildingEnum)mCreateStoryInfo.location_type, GUILayout.Width(150), GUILayout.Height(20));
            mCreateStoryInfo.id += mCreateStoryInfo.location_type * 100000;
        }
        inputId = long.Parse(EditorGUILayout.TextArea(inputId + "", GUILayout.Width(100), GUILayout.Height(20)));
        mCreateStoryInfo.id += inputId;
        GUILayout.Label("id：" + mCreateStoryInfo.id, GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.Label("备注：", GUILayout.Width(50), GUILayout.Height(20));
        mCreateStoryInfo.note = EditorGUILayout.TextArea(mCreateStoryInfo.note + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.EndHorizontal();
    }

    private void GUIFindStoryInfo()
    {
        //故事查询
        GUILayout.BeginHorizontal();
        GUILayout.Label("故事数据查询：", GUILayout.Width(100), GUILayout.Height(20));
        mFindStoryId = long.Parse(EditorGUILayout.TextArea(mFindStoryId + "", GUILayout.Width(100), GUILayout.Height(20)));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            QueryStoryInfoData(mFindStoryId);
        }
        if (GUILayout.Button("查询全部", GUILayout.Width(100), GUILayout.Height(20)))
        {
            QueryStoryInfoData(-1);
        }
        if (GUILayout.Button("查询客栈故事", GUILayout.Width(100), GUILayout.Height(20)))
        {
            QueryStoryInfoDataByScene(ScenesEnum.GameInnScene);
        }
        if (GUILayout.Button("查询小镇故事", GUILayout.Width(100), GUILayout.Height(20)))
        {
            QueryStoryInfoDataByScene(ScenesEnum.GameTownScene);
        }
        if (GUILayout.Button("查询竞技场故事", GUILayout.Width(100), GUILayout.Height(20)))
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
        if (GUILayout.Button("删除", GUILayout.Width(50), GUILayout.Height(20)))
        {
            storyInfoService.DeleteDataById(storyInfo.id);
            listStoryInfo.Remove(storyInfo);
        }
        if (GUILayout.Button("更新", GUILayout.Width(50), GUILayout.Height(20)))
        {
            storyInfoService.UpdateStoryData(storyInfo);
        }
        if (GUILayout.Button("显示详情", GUILayout.Width(100), GUILayout.Height(20)))
        {
            mFindStoryId = storyInfo.id;
            mObjContent.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
            QueryStoryInfoData(mFindStoryId);
            QueryStoryDetailsData(mFindStoryId);
        }
        GUILayout.Label("注释：", GUILayout.Width(50), GUILayout.Height(20));
        storyInfo.note = EditorGUILayout.TextArea(storyInfo.note + "", GUILayout.Width(200), GUILayout.Height(20));

        GUILayout.Label("id：" + storyInfo.id, GUILayout.Width(150), GUILayout.Height(20));
        storyInfo.story_scene = (int)(ScenesEnum)EditorGUILayout.EnumPopup("场景：", (ScenesEnum)storyInfo.story_scene, GUILayout.Width(300), GUILayout.Height(20));
        if (storyInfo.story_scene == (int)ScenesEnum.GameTownScene)
        {
            GUILayout.Label("故事发生地点：", GUILayout.Width(150), GUILayout.Height(20));
            storyInfo.location_type = (int)(TownBuildingEnum)EditorGUILayout.EnumPopup((TownBuildingEnum)storyInfo.location_type, GUILayout.Width(150), GUILayout.Height(20));

            GUILayout.Label("0外 1里：", GUILayout.Width(150), GUILayout.Height(20));
            storyInfo.out_in = int.Parse(EditorGUILayout.TextArea(storyInfo.out_in + "", GUILayout.Width(50), GUILayout.Height(20)));
        }
        GUILayout.Label("坐标：", GUILayout.Width(150), GUILayout.Height(20));
        if (GUILayout.Button("获取容器坐标", GUILayout.Width(150), GUILayout.Height(20)))
        {
            if (mObjContent == null)
            {
                LogUtil.LogError("容器没有定义");
            }
            else
            {
                storyInfo.position_x = mObjContent.transform.position.x;
                storyInfo.position_y = mObjContent.transform.position.y;
            }
        }
        storyInfo.position_x = float.Parse(EditorGUILayout.TextArea(storyInfo.position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
        storyInfo.position_y = float.Parse(EditorGUILayout.TextArea(storyInfo.position_y + "", GUILayout.Width(100), GUILayout.Height(20)));

        GUITriggerCondition(storyInfo);

        if (GUILayout.Button("更新", GUILayout.Width(50), GUILayout.Height(20)))
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
            if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                RefreshSceneData(listOrderStoryInfoDetails);
            }
            if (GUILayout.Button("保存", GUILayout.Width(100), GUILayout.Height(20)))
            {
                storyInfoService.UpdateStoryDetailsByIdAndOrder(mFindStoryId, mFindStroyOrder, listOrderStoryInfoDetails);
            }
            if (GUILayout.Button("<", GUILayout.Width(100), GUILayout.Height(20)))
            {
                mFindStroyOrder = (mFindStroyOrder - 1);
                listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(mFindStroyOrder);
                RefreshSceneData(listOrderStoryInfoDetails);
            }
            mFindStroyOrder = int.Parse(EditorGUILayout.TextArea(mFindStroyOrder + "", GUILayout.Width(100), GUILayout.Height(20)));
            if (GUILayout.Button(">", GUILayout.Width(100), GUILayout.Height(20)))
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
        StoryInfoDetailsBean removeTempData = null;
        foreach (StoryInfoDetailsBean itemData in listOrderStoryInfoDetails)
        {
            GUILayout.BeginHorizontal();
            if(itemData.type != (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                if (GUILayout.Button("删除", GUILayout.Width(200), GUILayout.Height(20)))
                {
                    storyInfoService.DeleteDetailsDataByIdOrderType(itemData.story_id, itemData.story_order,itemData.type);
                    listOrderStoryInfoDetails.Remove(itemData);
                }
            }
            if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            {
                if (GUILayout.Button("更新显示", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    RemoveSceneCharacterByName(itemData.npc_num + "");
                    RefreshSceneData(listOrderStoryInfoDetails);
                }
                if (GUILayout.Button("获取显示坐标", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    GameObject objItem = GetSceneObjByName(itemData.npc_num + "");
                    itemData.npc_position_x = objItem.transform.localPosition.x;
                    itemData.npc_position_y = objItem.transform.localPosition.y;
                }
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
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
                GUILayout.Label("NPCId:", GUILayout.Width(50), GUILayout.Height(20));
                itemData.npc_id = long.Parse(EditorGUILayout.TextArea(itemData.npc_id + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("姓名：" + npcInfo.title_name + "-" + npcInfo.name, GUILayout.Width(200), GUILayout.Height(20));
                GUILayout.Label("NPC序号:", GUILayout.Width(100), GUILayout.Height(20));
                itemData.npc_num = int.Parse(EditorGUILayout.TextArea(itemData.npc_num + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("NPC站位 ", GUILayout.Width(100), GUILayout.Height(20));
                GUILayout.Label("NPC位置X:", GUILayout.Width(120), GUILayout.Height(20));
                itemData.npc_position_x = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC位置Y:", GUILayout.Width(120), GUILayout.Height(20));
                itemData.npc_position_y = float.Parse(EditorGUILayout.TextArea(itemData.npc_position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("NPC朝向1左2右:", GUILayout.Width(120), GUILayout.Height(20));
                itemData.npc_face = int.Parse(EditorGUILayout.TextArea(itemData.npc_face + "", GUILayout.Width(50), GUILayout.Height(20)));


            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                GUILayout.BeginVertical();
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Label("对话 ", GUILayout.Width(120), GUILayout.Height(20));
                if (GUILayout.Button("添加子对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    if (listStoryTextInfo == null)
                        listStoryTextInfo = new List<TextInfoBean>();
                    TextInfoBean textInfo = new TextInfoBean();
                    textInfo.id = itemData.text_mark_id * 1000 + listStoryTextInfo.Count + 1;
                    textInfo.text_id = textInfo.id;
                    textInfo.type = 0;
                    textInfo.mark_id = itemData.text_mark_id;
                    textInfo.user_id = 0;
                    textInfo.text_order = listStoryTextInfo.Count + 1;
                    listStoryTextInfo.Add(textInfo);
                }
                if (GUILayout.Button("删除所有对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
                GUILayout.EndHorizontal();
                if (listStoryTextInfo != null)
                {
                    TextInfoBean removeTempText = null;
                    foreach (TextInfoBean textInfo in listStoryTextInfo)
                    {

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("删除子对话", GUILayout.Width(120), GUILayout.Height(20)))
                        {
                            removeTempText = textInfo;
                            textInfoService.DeleteDataById(TextEnum.Story, textInfo.id);
                        }
                        if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                        {
                            textInfoService.UpdateDataById(TextEnum.Story, textInfo.id, textInfo);
                        }
                        GUILayout.Label("ID", GUILayout.Width(50), GUILayout.Height(20));
                        textInfo.id = long.Parse(EditorGUILayout.TextArea(textInfo.id + "", GUILayout.Width(120), GUILayout.Height(20)));
                        GUILayout.Label("对话类型", GUILayout.Width(100), GUILayout.Height(20));
                        textInfo.type = (int)(TextInfoTypeEnum)EditorGUILayout.EnumPopup((TextInfoTypeEnum)textInfo.type, GUILayout.Width(100), GUILayout.Height(20));
                        GUILayout.Label("对话顺序", GUILayout.Width(100), GUILayout.Height(20));
                        textInfo.text_order = int.Parse(EditorGUILayout.TextArea(textInfo.text_order + "", GUILayout.Width(100), GUILayout.Height(20)));
                        GUILayout.Label("下一对话", GUILayout.Width(100), GUILayout.Height(20));
                        textInfo.next_order = int.Parse(EditorGUILayout.TextArea(textInfo.next_order + "", GUILayout.Width(100), GUILayout.Height(20)));
                        if (textInfo.type == 0)
                        {

                            GUILayout.Label("userID", GUILayout.Width(100), GUILayout.Height(20));
                            textInfo.user_id = long.Parse(EditorGUILayout.TextArea(textInfo.user_id + "", GUILayout.Width(100), GUILayout.Height(20)));
                            GUILayout.Label("姓名", GUILayout.Width(100), GUILayout.Height(20));
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
                            GUILayout.Label(npcInfo.title_name + "-" + npcInfo.name, GUILayout.Width(120), GUILayout.Height(20));
                            GUILayout.Label("指定的姓名", GUILayout.Width(120), GUILayout.Height(20));
                            textInfo.name = EditorGUILayout.TextArea(textInfo.name, GUILayout.Width(100), GUILayout.Height(20));

                        }
                        else if (textInfo.type == 1)
                        {
                            GUILayout.Label("select type", GUILayout.Width(120), GUILayout.Height(20));
                            textInfo.select_type = int.Parse(EditorGUILayout.TextArea(textInfo.select_type + "", GUILayout.Width(100), GUILayout.Height(20)));
                            GUILayout.Label("分支选择", GUILayout.Width(120), GUILayout.Height(20));
                            if (textInfo.select_type == 0)
                            {
                                GUILayout.Label("默认对话", GUILayout.Width(120), GUILayout.Height(20));
                            }
                            else
                            {
                                GUILayout.Label("分支选项 下一句对话ID", GUILayout.Width(200), GUILayout.Height(20));
                                textInfo.next_order = int.Parse(EditorGUILayout.TextArea(textInfo.next_order + "", GUILayout.Width(100), GUILayout.Height(20)));
                            }

                        }
                        else if (textInfo.type == 5)
                        {
                            GUILayout.Label("黑幕时间", GUILayout.Width(120), GUILayout.Height(20));
                            textInfo.wait_time = float.Parse(EditorGUILayout.TextArea(textInfo.wait_time + "", GUILayout.Width(100), GUILayout.Height(20)));
                        }

                        GUILayout.Label("对话内容", GUILayout.Width(120), GUILayout.Height(20));
                        textInfo.content = EditorGUILayout.TextArea(textInfo.content, GUILayout.Width(400), GUILayout.Height(20));

                        GUILayout.Label("增加的好感：", GUILayout.Width(120), GUILayout.Height(20));
                        textInfo.add_favorability = int.Parse(EditorGUILayout.TextArea(textInfo.add_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
                        if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                        {
                            textInfoService.UpdateDataById(TextEnum.Story, textInfo.id, textInfo);
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (removeTempText != null)
                        listStoryTextInfo.Remove(removeTempText);
                }

                GUILayout.EndVertical();

            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Expression)
            {
                GUILayout.Label("指定NPC展现表情 ", GUILayout.Width(150), GUILayout.Height(20));
                GUILayout.Label("NPC编号：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.npc_num = int.Parse(EditorGUILayout.TextArea(itemData.npc_num + "", GUILayout.Width(200), GUILayout.Height(20)));
                itemData.expression = (int)(CharacterExpressionEnum)EditorGUILayout.EnumPopup("表情编号：", (CharacterExpressionEnum)itemData.expression, GUILayout.Width(300), GUILayout.Height(20));
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt)
            {
                GUILayout.Label("场景互动 ", GUILayout.Width(120), GUILayout.Height(20));
                GUILayout.Label("互动物体名称：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.scene_intobj_name = EditorGUILayout.TextArea(itemData.scene_intobj_name, GUILayout.Width(200), GUILayout.Height(20));
                GUILayout.Label("互动类型名称：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.scene_intcomponent_name = EditorGUILayout.TextArea(itemData.scene_intcomponent_name, GUILayout.Width(200), GUILayout.Height(20));
                GUILayout.Label("互动方法：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.scene_intcomponent_method = EditorGUILayout.TextArea(itemData.scene_intcomponent_method, GUILayout.Width(200), GUILayout.Height(20));
                GUILayout.Label("互动方法参数：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.scene_intcomponent_parameters = EditorGUILayout.TextArea(itemData.scene_intcomponent_parameters, GUILayout.Width(200), GUILayout.Height(20));

            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory)
            {
                GUILayout.Label("删除角色(num,num)：", GUILayout.Width(120), GUILayout.Height(20));
                itemData.npc_destroy = EditorGUILayout.TextArea(itemData.npc_destroy, GUILayout.Width(200), GUILayout.Height(20));

            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext)
            {
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
                GUILayout.Label("延迟执行 ", GUILayout.Width(100), GUILayout.Height(20));
                GUILayout.Label("延迟时间s:", GUILayout.Width(120), GUILayout.Height(20));
                itemData.wait_time = float.Parse(EditorGUILayout.TextArea(itemData.wait_time + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition)
            {
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
                GUILayout.Label("摄像头位置 ", GUILayout.Width(150), GUILayout.Height(20));
                GUILayout.Label("x:", GUILayout.Width(50), GUILayout.Height(20));
                itemData.camera_position_x = float.Parse(EditorGUILayout.TextArea(itemData.camera_position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
                GUILayout.Label("y:", GUILayout.Width(50), GUILayout.Height(20));
                itemData.camera_position_y = float.Parse(EditorGUILayout.TextArea(itemData.camera_position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter)
            {
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
                GUILayout.Label("摄像头跟随角色序号 ", GUILayout.Width(200), GUILayout.Height(20));
                itemData.camera_follow_character = int.Parse(EditorGUILayout.TextArea(itemData.camera_follow_character + "", GUILayout.Width(100), GUILayout.Height(20)));
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound)
            {
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    removeTempData = itemData;
                }
                itemData.audio_sound = (int)(AudioSoundEnum)EditorGUILayout.EnumPopup("音效类型：", (AudioSoundEnum)itemData.audio_sound, GUILayout.Width(300), GUILayout.Height(20));
            }
            GUILayout.EndHorizontal();
        }
        if (removeTempData != null)
        {
            listAllStoryInfoDetails.Remove(removeTempData);
            listOrderStoryInfoDetails.Remove(removeTempData);
            RemoveSceneCharacterByName(removeTempData.npc_num + "");
        }

        GUILayout.Space(50);
        if (GUILayout.Button("添加站位", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition);
        }
        if (GUILayout.Button("添加人物表情", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Expression);
        }
        if (GUILayout.Button("添加对话", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk);
        }
        if (GUILayout.Button("删除人物", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory);
        }
        if (GUILayout.Button("添加场景互动", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt);
        }
        if (GUILayout.Button("添加延迟", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext);
        }
        if (GUILayout.Button("添加摄像头坐标", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition);
        }
        if (GUILayout.Button("添加摄像头跟随角色", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter);
        }
        if (GUILayout.Button("添加音效播放", GUILayout.Width(200), GUILayout.Height(20)))
        {
            CreateStoryInfoDetailsDataByType(mFindStoryId, StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound);
        }
        if (GUILayout.Button("保存该序号下的故事数据", GUILayout.Width(200), GUILayout.Height(20)))
        {
            storyInfoService.UpdateStoryDetailsByIdAndOrder(mFindStoryId, mFindStroyOrder, listOrderStoryInfoDetails);
        }
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

        if (characterData == null)
        {
            LogUtil.LogError("没有找到id为"+ createNpcId + "的NPC");
            return null;
        }

        objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        baseNpcAI.Awake();
       
        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(baseNpcAI.gameObject, "Body");
        characterDress.Awake();

        CharacterBodyCpt characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(baseNpcAI.gameObject, "Body");
        characterBody.Awake();

        baseNpcAI.transform.localPosition = position;
        baseNpcAI.SetCharacterData(gameItemsManager, characterData);
        baseNpcAI.name = "" + number;
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
            if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            {
                BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(mObjContent, itemData.npc_num + "");
                if (npcAI == null)
                {
                    NpcInfoBean npcInfoBean;
                    if (itemData.npc_id == 0)
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
                    GameObject objNpc = CreateNpc(npcInfoBean.npc_id, new Vector3(itemData.npc_position_x, itemData.npc_position_y), itemData.npc_num);
                    npcAI = objNpc.GetComponent<BaseNpcAI>();
                }
                else
                {
                    npcAI.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
                    npcAI.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
                }
                //设置朝向
                npcAI.SetCharacterFace(itemData.npc_face);
            }
            //如果是对话 查询对话数据
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk)
            {
                listStoryTextInfo = textInfoService.QueryDataByMarkId(TextEnum.Story, itemData.text_mark_id);
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory)
            {
                int[] numList = StringUtil.SplitBySubstringForArrayInt(itemData.npc_destroy, ',');
                foreach (int num in numList)
                {
                    BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(mObjContent, num + "");
                    DestroyImmediate(npcAI.gameObject);
                }
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition)
            {
                Vector3 cameraWorldPosition = mObjContent.transform.TransformPoint(new Vector3(itemData.camera_position_x, itemData.camera_position_y, -10));
                mCamera2D.Follow = null;
                mCamera2D.transform.position = cameraWorldPosition;
            }
            else if (itemData.type == (int)StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter)
            {
                BaseNpcAI npcAI = CptUtil.GetCptInChildrenByName<BaseNpcAI>(mObjContent, itemData.camera_follow_character + "");
                mCamera2D.Follow = npcAI.transform;
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
                itemDetailsInfo.npc_num = 1;
                break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Expression:
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


    /// <summary>
    /// 前置条件UI
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUITriggerCondition(StoryInfoBean storyInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("触发条件：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加条件", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storyInfo.trigger_condition += ("|" + EnumUtil.GetEnumName(EventTriggerEnum.Year) + ":" + "1|");
        }
        List<string> listTriggerData = StringUtil.SplitBySubstringForListStr(storyInfo.trigger_condition, '|');
        storyInfo.trigger_condition = "";
        for (int i = 0; i < listTriggerData.Count; i++)
        {
            string itemTriggerData = listTriggerData[i];
            if (CheckUtil.StringIsNull(itemTriggerData))
            {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listTriggerData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemTriggerData = StringUtil.SplitBySubstringForListStr(itemTriggerData, ':');
            listItemTriggerData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("触发条件", EnumUtil.GetEnum<EventTriggerEnum>(listItemTriggerData[0]), GUILayout.Width(300), GUILayout.Height(20)));
            listItemTriggerData[1] = EditorGUILayout.TextArea(listItemTriggerData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storyInfo.trigger_condition += (listItemTriggerData[0] + ":" + listItemTriggerData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }
}