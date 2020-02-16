using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using Cinemachine;
using System.Collections;

public class NpcCreateWindowEditor : EditorWindow
{
    GameItemsManager gameItemsManager;
    NpcInfoManager npcInfoManager;
    NpcInfoService npcInfoService;
    TextInfoService textInfoService;
    NpcTeamService npcTeamService;

    private CinemachineVirtualCamera mCamera2D;
    private GameObject mObjNpcContainer;
    private GameObject mObjNpcModel;

    //NPC创建数据
    public NpcInfoBean npcInfoForCreate = new NpcInfoBean();
    //NPC查询IDS
    public string findIdsStr = "0";
    //NPC查询数据
    public List<CharacterBean> listNpcDataForFind = new List<CharacterBean>();
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
        mCamera2D = GameObject.Find("Camera2D").GetComponent<CinemachineVirtualCamera>();
        gameItemsManager = new GameItemsManager();
        npcInfoManager = new NpcInfoManager();
        npcInfoService = new NpcInfoService();
        textInfoService = new TextInfoService();
        npcTeamService = new NpcTeamService();

        gameItemsManager.Awake();
        npcInfoManager.Awake();

        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        npcInfoManager.npcInfoController.GetAllNpcInfo();
    }

    private void RefreshData()
    {
        listNpcDataForFind.Clear();
        listNpcTeamDataForFind.Clear();
        mapNpcTalkInfoForFind.Clear();
        mapNpcTeamTalkInfoForFind.Clear();

        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        npcInfoManager.npcInfoController.GetAllNpcInfo();
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
        EditorUI.GUINpcInfoCreate(npcInfoService,gameItemsManager, mObjNpcContainer, mObjNpcModel, npcInfoForCreate);
        EditorUI.GUINpcInfoFind(
            npcInfoService,npcInfoManager,gameItemsManager,
            mObjNpcContainer,mObjNpcModel,
            findIdsStr, listNpcDataForFind,
            out findIdsStr,out listNpcDataForFind
            );
        EditorUI.GUINpcTalkFind(textInfoService, long.Parse(findIdsStr), mapNpcTalkInfoForFind);
        GUINpcTalk();
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        EditorUI.GUINpcTeamCreate(npcTeamService, npcTeamDataForCreate);
        EditorUI.GUINpcTeamFind(npcTeamService, npcTeamFindId, listNpcTeamDataForFind, out npcTeamFindId, out listNpcTeamDataForFind);
        GUILayout.EndScrollView();
    }

    private void GUINpcTalk()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("添加对话逻辑(警告：一定要先查询对应对话再添加)", GUILayout.Width(300), GUILayout.Height(20)))
        {
            long markId = long.Parse(findIdsStr) * 100000;
            markId += (mapNpcTalkInfoForFind.Count + 1);
            markId += (int)mFindTalkType * 10000;
            List<TextInfoBean> listTemp = new List<TextInfoBean>();
            mapNpcTalkInfoForFind.Add(markId, listTemp);
        }
        GUILayout.EndHorizontal();
        if (mapNpcTalkInfoForFind == null || mapNpcTalkInfoForFind.Count == 0)
            return;


        long removeMarkId = 0;
        long removeTalkId = 0;
        foreach (var mapItemTalkInfo in mapNpcTalkInfoForFind)
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("markId：", GUILayout.Width(120), GUILayout.Height(20));
            long.Parse(EditorGUILayout.TextArea(mapItemTalkInfo.Key + "", GUILayout.Width(100), GUILayout.Height(20)));
            if (mapItemTalkInfo.Value.Count > 0)
            {
                GUILayout.Label("对话类型：", GUILayout.Width(120), GUILayout.Height(20));
                mapItemTalkInfo.Value[0].talk_type = (int)(TextTalkTypeEnum)EditorGUILayout.EnumPopup((TextTalkTypeEnum)mapItemTalkInfo.Value[0].talk_type, GUILayout.Width(100), GUILayout.Height(20));
                GUILayout.Label("条件-好感对话：", GUILayout.Width(120), GUILayout.Height(20));
                mapItemTalkInfo.Value[0].condition_min_favorability = int.Parse(EditorGUILayout.TextArea(mapItemTalkInfo.Value[0].condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));

            }
            if (mapItemTalkInfo.Value != null)
                foreach (TextInfoBean itemTalkInfo in mapItemTalkInfo.Value)
                {
                    itemTalkInfo.talk_type = mapItemTalkInfo.Value[0].talk_type;
                    itemTalkInfo.condition_min_favorability = mapItemTalkInfo.Value[0].condition_min_favorability;
                }
            if (GUILayout.Button("添加对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                TextInfoBean addText = new TextInfoBean();
                addText.mark_id = mapItemTalkInfo.Key;
                addText.id = addText.mark_id * 1000 + (mapItemTalkInfo.Value.Count + 1);
                addText.text_id = addText.id;
                addText.user_id = long.Parse(findIdsStr);
                addText.valid = 1;
                addText.text_order = 1;
                addText.talk_type = (int)mFindTalkType;
                mapItemTalkInfo.Value.Add(addText);
            }
            if (GUILayout.Button("删除markId下所有对话", GUILayout.Width(150), GUILayout.Height(20)))
            {
                removeMarkId = mapItemTalkInfo.Key;
                textInfoService.DeleteDataByMarkId(TextEnum.Talk, removeMarkId);
            }
            GUILayout.EndHorizontal();

            foreach (TextInfoBean itemTalkInfo in mapItemTalkInfo.Value)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
                }
                if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    removeTalkId = itemTalkInfo.id;
                    textInfoService.DeleteDataById(TextEnum.Talk, removeTalkId);
                }
                GUILayout.Label("talkId：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.id = long.Parse(EditorGUILayout.TextArea(itemTalkInfo.id + "", GUILayout.Width(150), GUILayout.Height(20)));
                itemTalkInfo.type = (int)(TextInfoTypeEnum)EditorGUILayout.EnumPopup((TextInfoTypeEnum)itemTalkInfo.type, GUILayout.Width(100), GUILayout.Height(20));
                if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select)
                {
                    GUILayout.Label("选择类型：", GUILayout.Width(100), GUILayout.Height(20));
                    itemTalkInfo.select_type = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.select_type + "", GUILayout.Width(50), GUILayout.Height(20)));
                }
                GUILayout.Label("增加的好感：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.add_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.add_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("对话顺序：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.text_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.text_order + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("指定下一句对话：", GUILayout.Width(120), GUILayout.Height(20));
                itemTalkInfo.next_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.next_order + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("触发条件-最低好感：", GUILayout.Width(120), GUILayout.Height(20));
                itemTalkInfo.condition_min_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("预设名字：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.name = EditorGUILayout.TextArea(itemTalkInfo.name + "", GUILayout.Width(50), GUILayout.Height(20));
                GUILayout.Label("对话内容：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.content = EditorGUILayout.TextArea(itemTalkInfo.content + "", GUILayout.Width(500), GUILayout.Height(20));
                if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
                }
                if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    removeTalkId = itemTalkInfo.id;
                    textInfoService.DeleteDataById(TextEnum.Talk, removeTalkId);
                }
                GUILayout.EndHorizontal();
            }
        }
        if (removeMarkId != 0)
            mapNpcTalkInfoForFind.Remove(removeMarkId);
        //if (removeTalkId != 0)
        //{
        //    GetNpcTalkInfoList(mFindTalkType);
        //}
    }

    private TextTalkTypeEnum mFindTalkType;





    /// <summary>
    /// NPC出现条件
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUINPCShowCondition(NpcInfoBean npcInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("出现条件：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加条件", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcInfo.condition += ("|" + EnumUtil.GetEnumName(ShowConditionEnum.InnLevel) + ":" + "1|");
        }
        List<string> listConditionData = StringUtil.SplitBySubstringForListStr(npcInfo.condition, '|');
        npcInfo.condition = "";
        for (int i = 0; i < listConditionData.Count; i++)
        {
            string itemConditionData = listConditionData[i];
            if (CheckUtil.StringIsNull(itemConditionData))
            {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listConditionData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemConditionData = StringUtil.SplitBySubstringForListStr(itemConditionData, ':');
            listItemConditionData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("出现条件", EnumUtil.GetEnum<ShowConditionEnum>(listItemConditionData[0]), GUILayout.Width(300), GUILayout.Height(20)));
            listItemConditionData[1] = EditorGUILayout.TextArea(listItemConditionData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            npcInfo.condition += (listItemConditionData[0] + ":" + listItemConditionData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }
}