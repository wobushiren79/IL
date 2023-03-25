using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using Cinemachine;
using System.Collections;
using UnityEngine.U2D;

public class NpcCreateWindowEditor : EditorWindow
{
    NpcInfoService npcInfoService;
    TextInfoService textInfoService;
    NpcTeamService npcTeamService;

    private GameObject mObjNpcContainer;
    private GameObject mObjNpcModel;

    //NPC创建数据
    public NpcInfoBean npcInfoForCreate = new NpcInfoBean();
    //NPC查询IDS
    public string findNpcIdsStr = "0";
    //NPC查询数据
    public List<NpcInfoBean> listNpcDataForFind = new List<NpcInfoBean>();
    public long copyNpcId = 0;
    public long copyNpcNewId = 0;
    //NPC 谈话创建数据
    public TextTalkTypeEnum npcTalkInfoTypeForCreate;
    //NPC 谈话信息
    public Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind = new Dictionary<long, List<TextInfoBean>>();

    //NPC团队创建数据
    public NpcTeamBean npcTeamDataForCreate = new NpcTeamBean();
    //NPC团队查询IDS
    public string npcTeamFindId = "";
    //查询的NPC团队数据
    public List<NpcTeamBean> listNpcTeamDataForFind = new List<NpcTeamBean>();
    //NPC团队 谈话信息
    public Dictionary<long, List<TextInfoBean>> mapNpcTeamTalkInfoForFind = new Dictionary<long, List<TextInfoBean>>();


    [MenuItem("Tools/Window/NpcCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(NpcCreateWindowEditor));
    }

    public NpcCreateWindowEditor()
    {
        this.titleContent = new GUIContent("Npc创建工具");
    }

    private void OnDestroy()
    {
        CptUtil.RemoveChildsByActiveInEditor(mObjNpcContainer);
    }

    private void OnEnable()
    {
        npcInfoService = new NpcInfoService();
        textInfoService = new TextInfoService();
        npcTeamService = new NpcTeamService();

        NpcInfoHandler.Instance.manager.Awake();
        GameItemsHandler.Instance.manager.Awake();
    }

    private void RefreshData()
    {
        listNpcDataForFind.Clear();
        listNpcTeamDataForFind.Clear();
        mapNpcTalkInfoForFind.Clear();
        mapNpcTeamTalkInfoForFind.Clear();

        NpcInfoHandler.Instance.manager.Awake();
        GameItemsHandler.Instance.manager.Awake();
    }


    public Vector2 scrollPosition = Vector2.zero;
    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
        {
            RefreshData();
        }

        mObjNpcContainer = EditorGUILayout.ObjectField(new GUIContent("Npc容器", ""), mObjNpcContainer, typeof(GameObject), true) as GameObject;
        mObjNpcModel = EditorGUILayout.ObjectField(new GUIContent("NPC模型", ""), mObjNpcModel, typeof(GameObject), true) as GameObject;
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //Npc 创建UI
        GUINpcInfoCreate(npcInfoService, mObjNpcContainer, mObjNpcModel, npcInfoForCreate);
        //NPC 查询UI
        GUINpcInfoFind(
            npcInfoService,
            mObjNpcContainer, mObjNpcModel,
            findNpcIdsStr, listNpcDataForFind,
            out findNpcIdsStr, out listNpcDataForFind
            );
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("复制Npc");
        EditorUI.GUIText(" NPC新ID");
        copyNpcNewId = EditorUI.GUIEditorText(copyNpcNewId, 100);
        EditorUI.GUIText(" NPC复制ID");
        copyNpcId = EditorUI.GUIEditorText(copyNpcId, 100);
        if (EditorUI.GUIButton("复制"))
        {
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(copyNpcId);
            characterData.npcInfoData.id = (int)copyNpcNewId;
            characterData.npcInfoData.npc_id = copyNpcNewId;
            npcInfoService.InsertData(characterData.npcInfoData);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //Npc 对话逻辑添加UI
        long[] findIDs = findNpcIdsStr.SplitForArrayLong(',');
        if (findIDs != null && findIDs.Length > 0)
        {
            GUINpcTalkCreateByMarkId(findIDs[0], (int)npcTalkInfoTypeForCreate, mapNpcTalkInfoForFind);
        }
        //NPC 对话查询UI
        GUINpcTalkFind(
            textInfoService,
            long.Parse(findNpcIdsStr), npcTalkInfoTypeForCreate, mapNpcTalkInfoForFind,
            out npcTalkInfoTypeForCreate);
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //团队 创建UI
        GUINpcTeamCreate(npcTeamService, npcTeamDataForCreate);
        //团队 查询UI
        GUINpcTeamFind(
            textInfoService, npcTeamService,
            npcTeamFindId, listNpcTeamDataForFind, mapNpcTeamTalkInfoForFind,
            out npcTeamFindId, out listNpcTeamDataForFind, out mapNpcTeamTalkInfoForFind);

        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //团队 对话查询
        GUINpcTeamTalkFind(textInfoService, mapNpcTeamTalkInfoForFind);
        GUILayout.EndScrollView();
    }


    /// <summary>
    /// NPC创建
    /// </summary>
    public static void GUINpcInfoCreate(
        NpcInfoService npcInfoService,
        GameObject objNpcContainer, GameObject objNpcModel,
        NpcInfoBean npcInfo)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
        {
            CharacterBean characterData = new CharacterBean(npcInfo);
            ShowNpc(objNpcContainer, objNpcModel, characterData);
        }
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcInfo.valid = 1;
            npcInfo.face = 1;
            npcInfoService.InsertData(npcInfo);
        }
        GUILayout.EndHorizontal();
        GUINpcInfoItem(objNpcContainer, npcInfo);
    }

    /// <summary>
    /// NPC查询
    /// </summary>
    public static void GUINpcInfoFind(
        NpcInfoService npcInfoService,
        GameObject objNpcContainer, GameObject objNpcModel,
        string findIdsStr, List<NpcInfoBean> listNpcDataForFind,
        out string outFindIdStr, out List<NpcInfoBean> outListNpcDataForFind)
    {
        GUILayout.Label("查询NPC", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPCId", GUILayout.Width(100), GUILayout.Height(20));
        findIdsStr = EditorGUILayout.TextArea(findIdsStr + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] ids = findIdsStr.SplitForArrayLong(',');
            listNpcDataForFind = npcInfoService.QueryDataByIds(ids);
        }
        if (GUILayout.Button("查询全部", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryAllData();
        }
        if (GUILayout.Button("查询路人NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Passerby);
        }
        if (GUILayout.Button("查询小镇NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Town);
        }
        if (GUILayout.Button("查询特殊NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Special);
        }
        if (GUILayout.Button("查询小镇可招募NPC", GUILayout.Width(120), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.RecruitTown);
        }
        if (GUILayout.Button("查询团队顾客", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.GuestTeam);
        }
        if (GUILayout.Button("查询其他NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Other);
        }

        GUILayout.EndHorizontal();
        foreach (NpcInfoBean itemData in listNpcDataForFind)
        {
            GUILayout.Label("------------------------------------");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
            {
                CharacterBean characterData = new CharacterBean(itemData);
                ShowNpc(objNpcContainer, objNpcModel, characterData);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.Update(itemData);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.DeleteData(itemData);
                listNpcDataForFind.Remove(itemData);
            }
            GUILayout.EndHorizontal();
            GUINpcInfoItem(objNpcContainer, itemData);
            GUILayout.Label("------------------------------------");
        }
        outFindIdStr = findIdsStr;
        outListNpcDataForFind = listNpcDataForFind;
    }

    /// <summary>
    /// Npc Item
    /// </summary>
    public static void GUINpcInfoItem(GameObject objNpcContainer, NpcInfoBean npcInfo)
    {
        if (GUILayout.Button("获取场景位置数据", GUILayout.Width(120), GUILayout.Height(20)))
        {
            BaseNpcAI npcAI = objNpcContainer.GetComponentInChildren<BaseNpcAI>();
            npcInfo.position_x = npcAI.transform.position.x;
            npcInfo.position_y = npcAI.transform.position.y;
        }
        GUILayout.Label("NPCID：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.id = int.Parse(EditorGUILayout.TextArea(npcInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcInfo.npc_id = npcInfo.id;
        GUILayout.BeginHorizontal();
        npcInfo.npc_type = (int)EditorUI.GUIEnum<NpcTypeEnum>("Npc类型：", npcInfo.npc_type);
        NpcTypeEnum npcType = (NpcTypeEnum)npcInfo.npc_type;
        GUILayout.Label("对话选项：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.talk_types = EditorUI.GUIEditorText(npcInfo.talk_types, 50);
        GUILayout.EndHorizontal();
        if (npcType != NpcTypeEnum.Passerby)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("姓名：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.name = EditorGUILayout.TextArea(npcInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("性别：1男 2女", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.sex = int.Parse(EditorGUILayout.TextArea(npcInfo.sex + "", GUILayout.Width(30), GUILayout.Height(20)));
            GUILayout.Label("称号：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.title_name = EditorGUILayout.TextArea(npcInfo.title_name + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("朝向 1左 2右", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.face = int.Parse(EditorGUILayout.TextArea(npcInfo.face + "", GUILayout.Width(30), GUILayout.Height(20)));
            GUILayout.Label("位置XY：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.position_x = float.Parse(EditorGUILayout.TextArea(npcInfo.position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
            npcInfo.position_y = float.Parse(EditorGUILayout.TextArea(npcInfo.position_y + "", GUILayout.Width(100), GUILayout.Height(20)));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("眼睛：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.eye_id = EditorGUILayout.TextArea(npcInfo.eye_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string eyePath = "Assets/Texture/Character/Eye/";
            EditorUI.GUIPic(eyePath + "/" + npcInfo.eye_id);
            GUILayout.Label("眼睛颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean eyeColorData = new ColorBean(npcInfo.eye_color);
            Color eyeColor = eyeColorData.GetColor(); ;
            eyeColor = EditorGUILayout.ColorField(eyeColor, GUILayout.Width(50), GUILayout.Height(20));
            npcInfo.eye_color = eyeColor.r + "," + eyeColor.g + "," + eyeColor.b + "," + eyeColor.a;

            GUILayout.Label("嘴巴：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.mouth_id = EditorGUILayout.TextArea(npcInfo.mouth_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string mouthPath = "Assets/Texture/Character/Mouth/";
            EditorUI.GUIPic(mouthPath + "/" + npcInfo.mouth_id);
            GUILayout.Label("嘴巴颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean mouthColorData = new ColorBean(npcInfo.mouth_color);
            Color mouthColor = mouthColorData.GetColor(); ;
            mouthColor = EditorGUILayout.ColorField(mouthColor, GUILayout.Width(50), GUILayout.Height(20));
            npcInfo.mouth_color = mouthColor.r + "," + mouthColor.g + "," + mouthColor.b + "," + mouthColor.a;

            GUILayout.Label("头发：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.hair_id = EditorGUILayout.TextArea(npcInfo.hair_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string hairPath = "Assets/Texture/Character/Hair/";
            EditorUI.GUIPic(hairPath + "/" + npcInfo.hair_id);
            GUILayout.Label("头发颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean hairColorData = new ColorBean(npcInfo.hair_color);
            Color hairColor = hairColorData.GetColor(); ;
            hairColor = EditorGUILayout.ColorField(hairColor, GUILayout.Width(50), GUILayout.Height(20));
            npcInfo.hair_color = hairColor.r + "," + hairColor.g + "," + hairColor.b + "," + hairColor.a;

            GUILayout.Label("皮肤颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean skinColorData = new ColorBean(npcInfo.skin_color);
            Color skinColor = skinColorData.GetColor(); ;
            skinColor = EditorGUILayout.ColorField(skinColor, GUILayout.Width(50), GUILayout.Height(20));
            npcInfo.skin_color = skinColor.r + "," + skinColor.g + "," + skinColor.b + "," + skinColor.a;

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            GUILayout.Label("命：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_life = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_life + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("厨：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_cook = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_cook + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("速：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_speed = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_speed + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("算：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_account = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_account + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("魅：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_charm = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_charm + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("武：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_force = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_force + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("运：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_lucky = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_lucky + "", GUILayout.Width(50), GUILayout.Height(20)));
            if (npcType == NpcTypeEnum.RecruitTown)
            {
                GUILayout.Label("忠诚：", GUILayout.Width(30), GUILayout.Height(20));
                npcInfo.attributes_loyal = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_loyal + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("工资 S：", GUILayout.Width(30), GUILayout.Height(20));
                npcInfo.wage_s = int.Parse(EditorGUILayout.TextArea(npcInfo.wage_s + "", GUILayout.Width(50), GUILayout.Height(20)));
            }
            GUILayout.Label("喜欢的东西ID（用,分隔）：", GUILayout.Width(150), GUILayout.Height(20));
            npcInfo.love_items = EditorGUILayout.TextArea(npcInfo.love_items + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("喜欢的菜品：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.love_menus = EditorGUILayout.TextArea(npcInfo.love_menus + "", GUILayout.Width(50), GUILayout.Height(20));
            GUILayout.Label("技能：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.skill_ids = EditorUI.GUIEditorText(npcInfo.skill_ids, 100);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("|", 10);
        EditorUI.GUIText("面具：", 50);
        npcInfo.mask_id = long.Parse(EditorGUILayout.TextArea(npcInfo.mask_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string maskPath = "Assets/Texture/Character/Dress/Mask/";
        ItemsInfoBean maskInfo = GameItemsHandler.Instance.manager.GetItemsById(npcInfo.mask_id);
        if (maskInfo != null)
            EditorUI.GUIPic(maskPath + "/" + maskInfo.icon_key);
        EditorUI.GUIText("|", 10);
        EditorUI.GUIText("帽子：", 50);
        npcInfo.hat_id = long.Parse(EditorGUILayout.TextArea(npcInfo.hat_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string hatPath = "Assets/Texture/Character/Dress/Hat/";
        ItemsInfoBean hatInfo = GameItemsHandler.Instance.manager.GetItemsById(npcInfo.hat_id);
        if (hatInfo != null)
            EditorUI.GUIPic(hatPath + "/" + hatInfo.icon_key);
        EditorUI.GUIText("|", 10);
        EditorUI.GUIText("衣服：", 50);
        npcInfo.clothes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.clothes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string clothesPath = "Assets/Texture/Character/Dress/Clothes/";
        ItemsInfoBean clothesInfo = GameItemsHandler.Instance.manager.GetItemsById(npcInfo.clothes_id);
        if (clothesInfo != null)
            EditorUI.GUIPic(clothesPath + "/" + clothesInfo.icon_key);
        EditorUI.GUIText("|", 10);
        EditorUI.GUIText("鞋子：", 50);
        npcInfo.shoes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.shoes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string shoesPath = "Assets/Texture/Character/Dress/Shoes/";
        ItemsInfoBean shoesInfo = GameItemsHandler.Instance.manager.GetItemsById(npcInfo.shoes_id);
        if (shoesInfo != null)
            EditorUI.GUIPic(shoesPath + "/" + shoesInfo.icon_key);
        EditorUI.GUIText("|", 10);
        EditorUI.GUIText("武器：", 50);
        npcInfo.hand_id = long.Parse(EditorGUILayout.TextArea(npcInfo.hand_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        ItemsInfoBean handInfo = GameItemsHandler.Instance.manager.GetItemsById(npcInfo.hand_id);
        if (handInfo != null)
            EditorUI.GUIText(handInfo.name, 50);

        EditorUI.GUIText("|", 10);
        GUILayout.EndHorizontal();
        npcInfo.condition = EditorUI.GUIListData<ShowConditionEnum>("Npc出现条件", npcInfo.condition);
    }

    /// <summary>
    /// 团队创建 UI
    /// </summary>
    public static void GUINpcTeamCreate(NpcTeamService npcTeamService, NpcTeamBean npcTeamData)
    {
        GUILayout.Label("Npc团队创建", GUILayout.Width(100), GUILayout.Height(20));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcTeamData.valid = 1;
            npcTeamService.InsertData(npcTeamData);
        }
        GUILayout.EndHorizontal();

        GUINpcTeamItem(npcTeamData);
    }

    /// <summary>
    /// 团队查询 UI
    /// </summary>
    public static void GUINpcTeamFind(
        TextInfoService textInfoService, NpcTeamService npcTeamService,
        string findIdsStr, List<NpcTeamBean> listFindData, Dictionary<long, List<TextInfoBean>> mapTeamTalkInfo,
        out string outFindIdsStr, out List<NpcTeamBean> outlistFindData, out Dictionary<long, List<TextInfoBean>> outMapTeamTalkInfo)
    {
        GUILayout.Label("Npc团队查询", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        outFindIdsStr = EditorGUILayout.TextArea(findIdsStr + "", GUILayout.Width(200), GUILayout.Height(20));

        if (GUILayout.Button("查询团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] findIds = findIdsStr.SplitForArrayLong(',');
            listFindData = npcTeamService.QueryDataById(findIds);
        }

        if (GUILayout.Button("查询顾客团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Customer);
        }
        if (GUILayout.Button("查询好友团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Friend);
        }
        if (GUILayout.Button("查询捣乱团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Rascal);
        }
        if (GUILayout.Button("查询杂项团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Sundry);
        }
        if (GUILayout.Button("查询助兴团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Entertain);
        }
        if (GUILayout.Button("查询扫兴团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Disappointed);
        }
        GUILayout.EndHorizontal();
        if (listFindData != null)
        {
            NpcTeamBean itemRemoveData = null;
            foreach (NpcTeamBean itemData in listFindData)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    npcTeamService.Update(itemData);
                }
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    npcTeamService.DeleteDataById(itemData.id);
                    itemRemoveData = itemData;
                }
                if (GUILayout.Button("查询团队对话", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    List<TextInfoBean> listNpcTeamTalkInfo = textInfoService.QueryDataByMarkId(TextEnum.Talk, itemData.GetTalkIds());
                    HandleTalkInfoDataByMarkId(listNpcTeamTalkInfo, mapTeamTalkInfo);
                }
                GUILayout.EndHorizontal();
                GUINpcTeamItem(itemData);
                GUILayout.Space(20);
            }
            if (itemRemoveData != null)
            {
                listFindData.Remove(itemRemoveData);
                itemRemoveData = null;
            }
        }
        outlistFindData = listFindData;
        outMapTeamTalkInfo = mapTeamTalkInfo;
    }

    /// <summary>
    /// 团队Iteam UI
    /// </summary>
    public static void GUINpcTeamItem(NpcTeamBean npcTeamData)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("是否启用", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.valid = (int)(ValidEnum)EditorGUILayout.EnumPopup((ValidEnum)npcTeamData.valid, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("团队类型 " + npcTeamData.team_type, GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_type = (int)(NpcTeamTypeEnum)EditorGUILayout.EnumPopup((NpcTeamTypeEnum)npcTeamData.team_type, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("团队名称", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.name = EditorGUILayout.TextArea(npcTeamData.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("团队ID", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.id = int.Parse(EditorGUILayout.TextArea(npcTeamData.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcTeamData.team_id = npcTeamData.id;
        GUILayout.Label("团队领袖IDs(,)", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_leader = EditorGUILayout.TextArea(npcTeamData.team_leader + "", GUILayout.Width(200), GUILayout.Height(20));
        GUILayout.Label("团队成员IDs(,)", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_members = EditorGUILayout.TextArea(npcTeamData.team_members + "", GUILayout.Width(200), GUILayout.Height(20));
        switch ((NpcTeamTypeEnum)npcTeamData.team_type)
        {
            case NpcTeamTypeEnum.Customer:
                GUILayout.Label("成员数量最大值", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.team_number = int.Parse(EditorGUILayout.TextArea(npcTeamData.team_number + "", GUILayout.Width(50), GUILayout.Height(20)));
                break;
            case NpcTeamTypeEnum.Friend:
                break;
            case NpcTeamTypeEnum.Rascal:
            case NpcTeamTypeEnum.Sundry:
            case NpcTeamTypeEnum.Entertain:
            case NpcTeamTypeEnum.Disappointed:
                GUILayout.Label("持续时间", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.effect_time = EditorUI.GUIEditorText(npcTeamData.effect_time, 100);
                GUILayout.Label("对话markId(,)", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.talk_ids = EditorUI.GUIEditorText(npcTeamData.talk_ids, 200);
                GUILayout.Label("喊话markId(,)", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.shout_ids = EditorUI.GUIEditorText(npcTeamData.shout_ids, 200);
                break;
        }
        EditorUI.GUIText("喜欢的菜品");
        npcTeamData.love_menus = EditorGUILayout.TextArea(npcTeamData.love_menus, GUILayout.Width(250), GUILayout.Height(20));
        npcTeamData.condition = EditorUI.GUIListData<ShowConditionEnum>("团队出现条件", npcTeamData.condition);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Npc对话逻辑添加
    /// </summary>
    /// <param name="npcId"></param>
    /// <param name="talkType"></param>
    /// <param name="mapNpcTalkInfoForFind"></param>
    public static void GUINpcTalkCreateByMarkId(long npcId, int talkType, Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NpcId:" + npcId, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("talkType:" + talkType, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加对话逻辑(警告：一定要先查询对应对话再添加)", GUILayout.Width(300), GUILayout.Height(20)))
        {
            long markId = npcId * 100000;
            markId += talkType * 10000;
            markId += (mapNpcTalkInfoForFind.Count + 1);
            List<TextInfoBean> listTemp = new List<TextInfoBean>();
            mapNpcTalkInfoForFind.Add(markId, listTemp);
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Npc对话查询 UI
    /// </summary>
    public static void GUINpcTalkFind(TextInfoService textInfoService,
        long npcId, TextTalkTypeEnum talkType, Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind,
        out TextTalkTypeEnum outTalkType)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NpcID:" + npcId, GUILayout.Width(120));
        bool isFind = false;
        if (GUILayout.Button("查询普通对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Normal;
        }
        if (GUILayout.Button("查询第一次对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.First;
        }
        if (GUILayout.Button("查询招募对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Recruit;
        }
        if (GUILayout.Button("查询礼物对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Gift;
        }
        if (GUILayout.Button("查询后续事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Subsequent;
        }
        if (GUILayout.Button("查询捣乱事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Rascal;
        }
        if (GUILayout.Button("查询杂项事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Sundry;
        }
        if (GUILayout.Button("查询喊话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Shout;
        }
        if (GUILayout.Button("查询交换对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Exchange;
        }
        if (isFind)
        {
            List<TextInfoBean> listNpcTalkInfo = textInfoService.QueryDataByTalkType(TextEnum.Talk, talkType, npcId);
            HandleTalkInfoDataByMarkId(listNpcTalkInfo, mapNpcTalkInfoForFind);
        }
        GUILayout.EndHorizontal();
        outTalkType = talkType;
        if (mapNpcTalkInfoForFind == null)
            return;
        long deleteMarkId = 0;
        foreach (var mapItemTalkInfo in mapNpcTalkInfoForFind)
        {
            long markId = mapItemTalkInfo.Key;
            List<TextInfoBean> listTextData = mapItemTalkInfo.Value;
            if (GUILayout.Button("删除markId下所有对话", GUILayout.Width(150), GUILayout.Height(20)))
            {
                textInfoService.DeleteDataByMarkId(TextEnum.Talk, markId);
                deleteMarkId = markId;
            }
            GUINpcTextInfoItemForMarkId(textInfoService, npcId, talkType, markId, listTextData, out listTextData);
        }
        if (deleteMarkId != 0)
        {
            mapNpcTalkInfoForFind.Remove(deleteMarkId);
            deleteMarkId = 0;
        }
    }

    /// <summary>
    ///  团队对话查询
    /// </summary>
    public static void GUINpcTeamTalkFind(TextInfoService textInfoService,
        Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind)
    {
        if (mapNpcTalkInfoForFind == null)
            return;
        foreach (var mapItemTalkInfo in mapNpcTalkInfoForFind)
        {
            long markId = mapItemTalkInfo.Key;
            List<TextInfoBean> listTextData = mapItemTalkInfo.Value;
            GUINpcTextInfoItemForMarkId(textInfoService, 0, TextTalkTypeEnum.Normal, markId, listTextData, out listTextData);
        }
    }

    /// <summary>
    /// NPC 对话ITEM
    /// </summary>
    /// <param name="textInfoService"></param>
    /// <param name="markId"></param>
    /// <param name="listTextData"></param>
    public static void GUINpcTextInfoItemForMarkId(TextInfoService textInfoService,
        long userId, TextTalkTypeEnum talkType, long markId, List<TextInfoBean> listTextData,
        out List<TextInfoBean> outListTextData)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("markId：" + markId, GUILayout.Width(150), GUILayout.Height(20));
        if (listTextData.Count > 0)
        {
            GUILayout.Label("对话类型：", GUILayout.Width(120), GUILayout.Height(20));
            listTextData[0].talk_type = (int)(TextTalkTypeEnum)EditorGUILayout.EnumPopup((TextTalkTypeEnum)listTextData[0].talk_type, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("条件-好感对话：", GUILayout.Width(120), GUILayout.Height(20));
            listTextData[0].condition_min_favorability = int.Parse(EditorGUILayout.TextArea(listTextData[0].condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));

        }
        if (listTextData != null)
            foreach (TextInfoBean itemTalkInfo in listTextData)
            {
                itemTalkInfo.talk_type = listTextData[0].talk_type;
                itemTalkInfo.condition_min_favorability = listTextData[0].condition_min_favorability;
            }
        if (GUILayout.Button("添加对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            TextInfoBean addText = new TextInfoBean();
            addText.mark_id = markId;
            addText.id = (int)(addText.mark_id * 1000 + (listTextData.Count + 1));
            addText.text_id = addText.id;
            addText.user_id = listTextData.Count > 0 ? listTextData[0].user_id : userId;
            addText.valid = 1;
            addText.text_order = 1;
            addText.talk_type = listTextData.Count > 0 ? listTextData[0].talk_type : (int)talkType;
            listTextData.Add(addText);
        }
        if (GUILayout.Button("保存当前所有对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            foreach (TextInfoBean itemTalkInfo in listTextData)
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
        }
        GUILayout.EndHorizontal();
        TextInfoBean removeTalkInfo = null;
        foreach (TextInfoBean itemTalkInfo in listTextData)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
            if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                removeTalkInfo = itemTalkInfo;
                textInfoService.DeleteDataById(TextEnum.Talk, itemTalkInfo.id);
            }
            GUILayout.Label("talkId：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.id = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.id + "", GUILayout.Width(150), GUILayout.Height(20)));
            itemTalkInfo.text_id = itemTalkInfo.id;
            GUILayout.Label("对话顺序：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.text_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.text_order + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("说话者ID：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.user_id = EditorUI.GUIEditorText(itemTalkInfo.user_id, 150);
            itemTalkInfo.type = (int)(TextInfoTypeEnum)EditorGUILayout.EnumPopup((TextInfoTypeEnum)itemTalkInfo.type, GUILayout.Width(100), GUILayout.Height(20));
            if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select)
            {
                GUILayout.Label("选择类型：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.select_type = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.select_type + "", GUILayout.Width(50), GUILayout.Height(20)));
            }
            else
            {
                if (itemTalkInfo.type == (int)TextInfoTypeEnum.Behind)
                {
                    GUILayout.Label("黑屏时间：", GUILayout.Width(100), GUILayout.Height(20));
                    itemTalkInfo.wait_time = EditorUI.GUIEditorText(itemTalkInfo.wait_time, 50);
                }
                GUILayout.Label("增加的好感：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.add_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.add_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
            }
            GUILayout.Label("指定下一句对话：", GUILayout.Width(120), GUILayout.Height(20));
            itemTalkInfo.next_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.next_order + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("触发条件-最低好感：", GUILayout.Width(120), GUILayout.Height(20));
            itemTalkInfo.condition_min_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("预设名字：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.name = EditorGUILayout.TextArea(itemTalkInfo.name + "", GUILayout.Width(50), GUILayout.Height(20));
            GUILayout.Label("对话内容：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.content = EditorGUILayout.TextArea(itemTalkInfo.content + "", GUILayout.Width(500), GUILayout.Height(20));
            itemTalkInfo.reward_data = EditorUI.GUIListData<RewardTypeEnum>("奖励", itemTalkInfo.reward_data);
            if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select && itemTalkInfo.select_type == 1)
            {
                itemTalkInfo.pre_data = EditorUI.GUIListData<PreTypeEnum>("付出", itemTalkInfo.pre_data);
                itemTalkInfo.pre_data_minigame = EditorUI.GUIListData<PreTypeForMiniGameEnum>("小游戏数据", itemTalkInfo.pre_data_minigame);
            }

            if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
            if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                removeTalkInfo = itemTalkInfo;
                textInfoService.DeleteDataById(TextEnum.Talk, itemTalkInfo.id);
            }
            GUILayout.EndHorizontal();
        }
        if (removeTalkInfo != null)
        {
            listTextData.Remove(removeTalkInfo);
            removeTalkInfo = null;
        }
        outListTextData = listTextData;
    }

    /// <summary>
    /// 通过markID处理聊天信息
    /// </summary>
    /// <param name="listNpcTalkInfo"></param>
    /// <param name="mapNpcTeamTalkInfoForFind"></param>
    public static void HandleTalkInfoDataByMarkId(List<TextInfoBean> listNpcTalkInfo, Dictionary<long, List<TextInfoBean>> mapNpcTeamTalkInfoForFind)
    {
        mapNpcTeamTalkInfoForFind.Clear();
        foreach (TextInfoBean itemTalkInfo in listNpcTalkInfo)
        {
            long markId = itemTalkInfo.mark_id;
            if (mapNpcTeamTalkInfoForFind.TryGetValue(markId, out List<TextInfoBean> value))
            {
                value.Add(itemTalkInfo);
            }
            else
            {
                List<TextInfoBean> listTemp = new List<TextInfoBean>();
                listTemp.Add(itemTalkInfo);
                mapNpcTeamTalkInfoForFind.Add(markId, listTemp);
            }
        }
    }

    /// <summary>
    ///  展示NPC
    /// </summary>
    public static GameObject ShowNpc(GameObject objNpcContainer, GameObject objNpcModel, CharacterBean characterData)
    {
        CptUtil.RemoveChildsByActiveInEditor(objNpcContainer);
        GameObject objNpc = GameObject.Instantiate(objNpcModel, objNpcContainer.transform);
        objNpc.SetActive(true);
        objNpc.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);

        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        baseNpcAI.Awake();

        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(baseNpcAI.gameObject, "Body");
        characterDress.Awake();

        baseNpcAI.SetCharacterData(characterData);
        return objNpc;
    }
}