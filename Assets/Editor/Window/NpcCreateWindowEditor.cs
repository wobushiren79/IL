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
        //Npc 创建UI
        EditorUI.GUINpcInfoCreate(npcInfoService, gameItemsManager, mObjNpcContainer, mObjNpcModel, npcInfoForCreate);
        //NPC 查询UI
        EditorUI.GUINpcInfoFind(
            npcInfoService, gameItemsManager,
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
            CharacterBean characterData= npcInfoManager.GetCharacterDataById(copyNpcId);
            characterData.npcInfoData.id = copyNpcNewId;
            characterData.npcInfoData.npc_id = copyNpcNewId;
            npcInfoService.InsertData(characterData.npcInfoData);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //Npc 对话逻辑添加UI
        long[] findIDs = StringUtil.SplitBySubstringForArrayLong(findNpcIdsStr, ',');
        if (findIDs != null && findIDs.Length > 0)
        {
            EditorUI.GUINpcTalkCreateByMarkId(findIDs[0], (int)npcTalkInfoTypeForCreate, mapNpcTalkInfoForFind);
        }
        //NPC 对话查询UI
        EditorUI.GUINpcTalkFind(
            textInfoService, 
            long.Parse(findNpcIdsStr), npcTalkInfoTypeForCreate, mapNpcTalkInfoForFind,
            out npcTalkInfoTypeForCreate);
        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //团队 创建UI
        EditorUI.GUINpcTeamCreate(npcTeamService, npcTeamDataForCreate);
        //团队 查询UI
        EditorUI.GUINpcTeamFind(
            textInfoService, npcTeamService,
            npcTeamFindId, listNpcTeamDataForFind, mapNpcTeamTalkInfoForFind,
            out npcTeamFindId, out listNpcTeamDataForFind, out mapNpcTeamTalkInfoForFind);

        GUILayout.Label("-----------------------------------------------------------------------------------------------------------");
        //团队 对话查询
        EditorUI.GUINpcTeamTalkFind(textInfoService, mapNpcTeamTalkInfoForFind);
        GUILayout.EndScrollView();
    }
}