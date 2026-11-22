using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

/// <summary>
/// NPC创建工具
/// </summary>
public class NpcCreateWindowEditor : EditorWindow
{
    //Tab分页
    private enum TabEnum { NpcFind = 0, NpcTalk = 1, NpcTeam = 2, CreatePreview = 3 }
    private static readonly string[] TAB_NAMES = { "NPC查询", "NPC对话", "NPC团队", "创建预览" };
    private int curTab = (int)TabEnum.NpcFind;

    //场景预览对象
    private GameObject mObjNpcContainer;
    private GameObject mObjNpcModel;

    private Vector2 scrollPosition = Vector2.zero;

    //NPC查询
    private string findNpcIdsStr = "";
    private NpcTypeEnum findNpcType = NpcTypeEnum.Town;
    private List<NpcInfoBean> listNpcDataForFind = new List<NpcInfoBean>();
    private Dictionary<long, bool> mapNpcFoldout = new Dictionary<long, bool>();
    private string npcQueryWarn = "";

    //NPC对话
    private long npcTalkUserId = 0;
    private TextTalkTypeEnum npcTalkType = TextTalkTypeEnum.Normal;
    private Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind = new Dictionary<long, List<TextInfoBean>>();
    private Dictionary<long, bool> mapTalkFoldout = new Dictionary<long, bool>();

    //NPC团队
    private string findTeamIdsStr = "";
    private NpcTeamTypeEnum findTeamType = NpcTeamTypeEnum.Customer;
    private List<NpcTeamBean> listNpcTeamDataForFind = new List<NpcTeamBean>();
    private Dictionary<long, bool> mapTeamFoldout = new Dictionary<long, bool>();
    private Dictionary<long, List<TextInfoBean>> mapNpcTeamTalkInfoForFind = new Dictionary<long, List<TextInfoBean>>();
    private long curTeamTalkTeamId = -1;

    //创建预览（仅用于场景显示，不会写入配置）
    private NpcInfoBean npcInfoForCreate = new NpcInfoBean();
    private NpcTeamBean npcTeamDataForCreate = new NpcTeamBean();

    //朝向选项
    private static readonly string[] FACE_NAMES = { "未知", "左", "右" };
    private static readonly int[] FACE_VALUES = { 0, 1, 2 };

    //样式
    private static GUIStyle styleSectionTitle;
    private static GUIStyle styleTalkContent;

    [MenuItem("游戏/NPC创建")]
    static void CreateWindows()
    {
        NpcCreateWindowEditor window = GetWindow<NpcCreateWindowEditor>();
        window.minSize = new Vector2(760, 420);
        window.Show();
    }

    public NpcCreateWindowEditor()
    {
        titleContent = new GUIContent("NPC创建工具");
    }

    private void OnDestroy()
    {
        CptUtil.RemoveChildsByActiveInEditor(mObjNpcContainer);
    }

    private void OnEnable()
    {
        NpcInfoHandler.Instance.manager.Awake();
        GameItemsHandler.Instance.manager.Awake();
    }

    public void OnDisable()
    {
        GameItemsHandler.Instance.DestorySelf(1);
        GameDataHandler.Instance.DestorySelf(1);
        NpcInfoHandler.Instance.DestorySelf(1);
    }

    private void RefreshData()
    {
        listNpcDataForFind.Clear();
        listNpcTeamDataForFind.Clear();
        mapNpcTalkInfoForFind.Clear();
        mapNpcTeamTalkInfoForFind.Clear();
        mapNpcFoldout.Clear();
        mapTalkFoldout.Clear();
        mapTeamFoldout.Clear();
        curTeamTalkTeamId = -1;

        NpcInfoHandler.Instance.manager.Awake();
        GameItemsHandler.Instance.manager.Awake();
    }

    private void InitStyles()
    {
        if (styleSectionTitle == null)
        {
            styleSectionTitle = new GUIStyle(EditorStyles.boldLabel);
            styleSectionTitle.fontSize = 12;
            styleSectionTitle.normal.textColor = EditorGUIUtility.isProSkin
                ? new Color(0.55f, 0.75f, 1f)
                : new Color(0.1f, 0.3f, 0.6f);
        }
        if (styleTalkContent == null)
        {
            styleTalkContent = new GUIStyle(EditorStyles.label);
            styleTalkContent.wordWrap = true;
            styleTalkContent.padding.left = 24;
        }
    }

    private void OnGUI()
    {
        InitStyles();

        DrawSceneSetting();

        GUILayout.Space(4);
        curTab = GUILayout.Toolbar(curTab, TAB_NAMES, GUILayout.Height(24));
        GUILayout.Space(4);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        switch ((TabEnum)curTab)
        {
            case TabEnum.NpcFind:
                DrawNpcFindTab();
                break;
            case TabEnum.NpcTalk:
                DrawNpcTalkTab();
                break;
            case TabEnum.NpcTeam:
                DrawNpcTeamTab();
                break;
            case TabEnum.CreatePreview:
                DrawCreatePreviewTab();
                break;
        }
        GUILayout.EndScrollView();
    }

    #region 顶部场景设置
    private void DrawSceneSetting()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("NPC容器", 60);
        mObjNpcContainer = EditorGUILayout.ObjectField(mObjNpcContainer, typeof(GameObject), true, GUILayout.Width(200)) as GameObject;
        GUILayout.Space(20);
        EditorUI.GUIText("NPC模型", 60);
        mObjNpcModel = EditorGUILayout.ObjectField(mObjNpcModel, typeof(GameObject), true, GUILayout.Width(200)) as GameObject;
        GUILayout.Space(20);
        if (EditorUI.GUIButton("刷新数据", 100))
            RefreshData();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    #endregion

    #region NPC查询
    private void DrawNpcFindTab()
    {
        //查询条件
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("NPC ID（逗号分隔）", 125);
        findNpcIdsStr = EditorGUILayout.TextField(findNpcIdsStr, GUILayout.Width(220));
        if (EditorUI.GUIButton("查询", 80))
            QueryNpcByIds();
        if (EditorUI.GUIButton("查询全部", 80))
        {
            listNpcDataForFind = GetAllNpc();
            npcQueryWarn = "";
            mapNpcFoldout.Clear();
        }
        if (EditorUI.GUIButton("清空结果", 80))
        {
            listNpcDataForFind.Clear();
            mapNpcFoldout.Clear();
            npcQueryWarn = "";
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("按类型查询", 125);
        findNpcType = (NpcTypeEnum)EditorGUILayout.EnumPopup(findNpcType, GUILayout.Width(160));
        if (EditorUI.GUIButton("查询", 80))
        {
            listNpcDataForFind = GetNpcByType(findNpcType);
            npcQueryWarn = "";
            mapNpcFoldout.Clear();
        }
        GUILayout.EndHorizontal();
        if (!npcQueryWarn.IsNull())
            EditorGUILayout.HelpBox(npcQueryWarn, MessageType.Warning);
        GUILayout.EndVertical();

        //查询结果
        EditorUI.GUIText("查询结果：共 " + listNpcDataForFind.Count + " 条", 200);
        for (int i = 0; i < listNpcDataForFind.Count; i++)
            DrawNpcFindItem(listNpcDataForFind[i]);
    }

    private void QueryNpcByIds()
    {
        long[] ids = ParseIds(findNpcIdsStr, out int invalidCount);
        listNpcDataForFind = GetNpcByIds(ids);
        npcQueryWarn = invalidCount > 0 ? "有 " + invalidCount + " 个无效输入已被忽略" : "";
        mapNpcFoldout.Clear();
    }

    private void DrawNpcFindItem(NpcInfoBean itemData)
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("显示", 50))
            ShowNpcSafe(itemData);
        if (EditorUI.GUIButton("对话", 50))
        {
            npcTalkUserId = itemData.id;
            curTab = (int)TabEnum.NpcTalk;
            QueryNpcTalk();
        }
        mapNpcFoldout.TryGetValue(itemData.id, out bool foldout);
        string summary = "ID:" + itemData.id + "  " + itemData.name_language
            + "  [" + (NpcTypeEnum)itemData.npc_type + "]  " + itemData.title_name_language;
        bool newFoldout = EditorGUILayout.Foldout(foldout, summary, true);
        mapNpcFoldout[itemData.id] = newFoldout;
        GUILayout.EndHorizontal();

        if (newFoldout)
            DrawNpcInfoDetail(itemData, true);
        GUILayout.EndVertical();
    }
    #endregion

    #region NPC对话
    private void DrawNpcTalkTab()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("NPC ID", 50);
        npcTalkUserId = EditorGUILayout.LongField(npcTalkUserId, GUILayout.Width(100));
        EditorUI.GUIText("对话类型", 60);
        npcTalkType = (TextTalkTypeEnum)EditorGUILayout.EnumPopup(npcTalkType, GUILayout.Width(150));
        if (EditorUI.GUIButton("查询对话", 100))
            QueryNpcTalk();
        GUILayout.EndHorizontal();

        //添加对话逻辑
        long nextMarkId = GetNextTalkMarkId();
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("下一个markId: " + nextMarkId, 200);
        if (EditorUI.GUIButton("添加对话逻辑", 120))
        {
            if (EditorUI.GUIDialog("添加对话逻辑",
                "确定为 NPC " + npcTalkUserId + " 添加 markId " + nextMarkId + " 的对话组吗？\n（建议先查询确认已有对话，避免与Excel中的数据冲突）"))
            {
                if (mapNpcTalkInfoForFind.ContainsKey(nextMarkId))
                    EditorUI.GUIDialog("错误", "markId " + nextMarkId + " 已存在！");
                else
                {
                    mapNpcTalkInfoForFind.Add(nextMarkId, new List<TextInfoBean>());
                    mapTalkFoldout[nextMarkId] = true;
                }
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("对话数据为只读，正式编辑请修改Excel配置表", MessageType.Info);
        GUILayout.EndVertical();

        foreach (var mapItem in mapNpcTalkInfoForFind)
            DrawTalkGroup(mapItem.Key, mapItem.Value);
    }

    private void QueryNpcTalk()
    {
        List<TextInfoBean> listNpcTalkInfo = QueryTalkByUserIdAndType(npcTalkUserId, npcTalkType);
        HandleTalkInfoDataByMarkId(listNpcTalkInfo, mapNpcTalkInfoForFind);
        mapTalkFoldout.Clear();
    }

    /// <summary>
    /// 获取下一个可用的对话markId（取当前最大序号+1，避免序号不连续时冲突）
    /// </summary>
    private long GetNextTalkMarkId()
    {
        long baseMarkId = npcTalkUserId * 100000 + (long)npcTalkType * 10000;
        long maxSeq = 0;
        foreach (long markId in mapNpcTalkInfoForFind.Keys)
        {
            if (markId >= baseMarkId && markId < baseMarkId + 10000)
            {
                long seq = markId - baseMarkId;
                if (seq > maxSeq)
                    maxSeq = seq;
            }
        }
        return baseMarkId + maxSeq + 1;
    }
    #endregion

    #region NPC团队
    private void DrawNpcTeamTab()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("团队ID（逗号分隔）", 125);
        findTeamIdsStr = EditorGUILayout.TextField(findTeamIdsStr, GUILayout.Width(220));
        if (EditorUI.GUIButton("查询", 80))
        {
            listNpcTeamDataForFind = GetTeamByIds(ParseIds(findTeamIdsStr, out _));
            mapTeamFoldout.Clear();
            curTeamTalkTeamId = -1;
        }
        if (EditorUI.GUIButton("查询全部", 80))
        {
            listNpcTeamDataForFind = GetAllTeams();
            mapTeamFoldout.Clear();
            curTeamTalkTeamId = -1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("按类型查询", 125);
        findTeamType = (NpcTeamTypeEnum)EditorGUILayout.EnumPopup(findTeamType, GUILayout.Width(160));
        if (EditorUI.GUIButton("查询", 80))
        {
            listNpcTeamDataForFind = GetTeamByType(findTeamType);
            mapTeamFoldout.Clear();
            curTeamTalkTeamId = -1;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        EditorUI.GUIText("查询结果：共 " + listNpcTeamDataForFind.Count + " 条", 200);
        for (int i = 0; i < listNpcTeamDataForFind.Count; i++)
            DrawTeamFindItem(listNpcTeamDataForFind[i]);
    }

    private void DrawTeamFindItem(NpcTeamBean itemData)
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        mapTeamFoldout.TryGetValue(itemData.id, out bool foldout);
        string summary = "ID:" + itemData.id + "  " + itemData.name_language + "  [" + (NpcTeamTypeEnum)itemData.team_type + "]";
        bool newFoldout = EditorGUILayout.Foldout(foldout, summary, true);
        mapTeamFoldout[itemData.id] = newFoldout;
        GUILayout.EndHorizontal();

        if (newFoldout)
        {
            DrawNpcTeamDetail(itemData);
            GUILayout.BeginHorizontal();
            if (EditorUI.GUIButton("查询团队对话", 120))
            {
                List<TextInfoBean> listTalk = QueryTalkByMarkIds(itemData.GetTalkIds());
                HandleTalkInfoDataByMarkId(listTalk, mapNpcTeamTalkInfoForFind);
                curTeamTalkTeamId = itemData.id;
            }
            GUILayout.EndHorizontal();
            if (curTeamTalkTeamId == itemData.id)
            {
                DrawSectionTitle("团队对话");
                if (mapNpcTeamTalkInfoForFind.Count == 0)
                    EditorGUILayout.HelpBox("该团队没有配置对话", MessageType.None);
                foreach (var mapItem in mapNpcTeamTalkInfoForFind)
                    DrawTalkGroup(mapItem.Key, mapItem.Value);
            }
        }
        GUILayout.EndVertical();
    }
    #endregion

    #region 创建预览
    private void DrawCreatePreviewTab()
    {
        EditorGUILayout.HelpBox("此页面的数据仅用于场景预览，不会被保存。正式数据请编辑Excel配置表后点击【刷新数据】。", MessageType.Info);

        GUILayout.BeginVertical("box");
        if (EditorUI.GUIButton("在场景中显示此NPC", 160))
            ShowNpcSafe(npcInfoForCreate);
        DrawNpcInfoDetail(npcInfoForCreate, false);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        DrawSectionTitle("NPC团队模板");
        GUILayout.BeginVertical("box");
        DrawNpcTeamDetail(npcTeamDataForCreate);
        GUILayout.EndVertical();
    }
    #endregion

    #region NPC详情绘制
    /// <summary>
    /// 绘制NPC完整数据（分组显示）
    /// </summary>
    private void DrawNpcInfoDetail(NpcInfoBean npcInfo, bool showScenePosButton)
    {
        NpcTypeEnum npcType = (NpcTypeEnum)npcInfo.npc_type;

        //基础信息
        DrawSectionTitle("基础信息");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("ID", 24);
        npcInfo.id = EditorGUILayout.LongField(npcInfo.id, GUILayout.Width(80));
        EditorUI.GUIText("有效", 30);
        npcInfo.valid = (int)(ValidEnum)EditorGUILayout.EnumPopup((ValidEnum)npcInfo.valid, GUILayout.Width(70));
        EditorUI.GUIText("类型", 30);
        npcInfo.npc_type = (int)(NpcTypeEnum)EditorGUILayout.EnumPopup((NpcTypeEnum)npcInfo.npc_type, GUILayout.Width(110));
        EditorUI.GUIText("对话选项", 60);
        npcInfo.talk_types = EditorGUILayout.TextField(npcInfo.talk_types + "", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        if (npcType != NpcTypeEnum.Passerby)
        {
            GUILayout.BeginHorizontal();
            EditorUI.GUIText("姓名", 30);
            npcInfo.name_language = EditorGUILayout.TextField(npcInfo.name_language + "", GUILayout.Width(100));
            EditorUI.GUIText("性别", 30);
            npcInfo.sex = (int)(SexEnum)EditorGUILayout.EnumPopup((SexEnum)npcInfo.sex, GUILayout.Width(80));
            EditorUI.GUIText("称号", 30);
            npcInfo.title_name_language = EditorGUILayout.TextField(npcInfo.title_name_language + "", GUILayout.Width(100));
            EditorUI.GUIText("朝向", 30);
            npcInfo.face = EditorGUILayout.IntPopup(npcInfo.face, FACE_NAMES, FACE_VALUES, GUILayout.Width(60));
            EditorUI.GUIText("可结婚", 50);
            npcInfo.marry_status = EditorGUILayout.IntField(npcInfo.marry_status, GUILayout.Width(40));
            GUILayout.EndHorizontal();
        }

        //位置
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("位置X", 40);
        npcInfo.position_x = EditorGUILayout.FloatField(npcInfo.position_x, GUILayout.Width(80));
        EditorUI.GUIText("位置Y", 40);
        npcInfo.position_y = EditorGUILayout.FloatField(npcInfo.position_y, GUILayout.Width(80));
        if (showScenePosButton && EditorUI.GUIButton("获取场景位置", 100))
            GetScenePosition(npcInfo);
        GUILayout.EndHorizontal();

        if (npcType != NpcTypeEnum.Passerby)
        {
            //外貌
            DrawSectionTitle("外貌");
            string eyePath = "Assets/Texture/Character/Eye";
            string mouthPath = "Assets/Texture/Character/Mouth";
            string hairPath = "Assets/Texture/Character/Hair";
            GUILayout.BeginHorizontal();
            npcInfo.eye_id = DrawFacePart("眼睛", npcInfo.eye_id, eyePath);
            npcInfo.eye_color = DrawColorEditor(npcInfo.eye_color);
            GUILayout.Space(15);
            npcInfo.mouth_id = DrawFacePart("嘴巴", npcInfo.mouth_id, mouthPath);
            npcInfo.mouth_color = DrawColorEditor(npcInfo.mouth_color);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            npcInfo.hair_id = DrawFacePart("头发", npcInfo.hair_id, hairPath);
            npcInfo.hair_color = DrawColorEditor(npcInfo.hair_color);
            GUILayout.Space(15);
            EditorUI.GUIText("皮肤颜色", 60);
            npcInfo.skin_color = DrawColorEditor(npcInfo.skin_color);
            GUILayout.EndHorizontal();

            //属性
            DrawSectionTitle("属性");
            GUILayout.BeginHorizontal();
            npcInfo.attributes_life = DrawIntAttr("命", npcInfo.attributes_life, 18);
            npcInfo.attributes_cook = DrawIntAttr("厨", npcInfo.attributes_cook, 18);
            npcInfo.attributes_speed = DrawIntAttr("速", npcInfo.attributes_speed, 18);
            npcInfo.attributes_account = DrawIntAttr("算", npcInfo.attributes_account, 18);
            npcInfo.attributes_charm = DrawIntAttr("魅", npcInfo.attributes_charm, 18);
            npcInfo.attributes_force = DrawIntAttr("武", npcInfo.attributes_force, 18);
            npcInfo.attributes_lucky = DrawIntAttr("运", npcInfo.attributes_lucky, 18);
            GUILayout.EndHorizontal();
            if (npcType == NpcTypeEnum.RecruitTown)
            {
                GUILayout.BeginHorizontal();
                npcInfo.attributes_loyal = DrawIntAttr("忠诚", npcInfo.attributes_loyal, 34);
                npcInfo.wage_s = DrawIntAttr("工资S", npcInfo.wage_s, 44);
                npcInfo.wage_m = DrawIntAttr("工资M", npcInfo.wage_m, 44);
                npcInfo.wage_l = DrawIntAttr("工资L", npcInfo.wage_l, 44);
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            EditorUI.GUIText("喜欢道具ID(,)", 90);
            npcInfo.love_items = EditorGUILayout.TextField(npcInfo.love_items + "", GUILayout.Width(180));
            EditorUI.GUIText("喜欢菜品(,)", 80);
            npcInfo.love_menus = EditorGUILayout.TextField(npcInfo.love_menus + "", GUILayout.Width(120));
            EditorUI.GUIText("技能(,)", 50);
            npcInfo.skill_ids = EditorGUILayout.TextField(npcInfo.skill_ids + "", GUILayout.Width(120));
            GUILayout.EndHorizontal();
        }

        //服装
        DrawSectionTitle("服装");
        npcInfo.mask_id = DrawDressRow("面具", npcInfo.mask_id, "Assets/Texture/Character/Dress/Mask");
        npcInfo.hat_id = DrawDressRow("帽子", npcInfo.hat_id, "Assets/Texture/Character/Dress/Hat");
        npcInfo.clothes_id = DrawDressRow("衣服", npcInfo.clothes_id, "Assets/Texture/Character/Dress/Clothes");
        npcInfo.shoes_id = DrawDressRow("鞋子", npcInfo.shoes_id, "Assets/Texture/Character/Dress/Shoes");
        npcInfo.hand_id = DrawDressRow("武器", npcInfo.hand_id, null);

        //出现条件
        DrawSectionTitle("出现条件");
        npcInfo.condition = EditorUI.GUIListData<ShowConditionEnum>("NPC出现条件", npcInfo.condition);
    }

    /// <summary>
    /// 绘制脸部部件（眼睛/嘴巴/头发）：标签+ID输入+预览图
    /// </summary>
    private static string DrawFacePart(string label, string partId, string texFolder)
    {
        EditorUI.GUIText(label, 30);
        partId = EditorGUILayout.TextField(partId + "", GUILayout.Width(110));
        EditorUI.GUIPic(texFolder + "/" + partId, 28, 28);
        EditorUI.GUIText("颜色", 30);
        return partId;
    }

    /// <summary>
    /// 绘制服装行：标签+道具ID+图标+道具名
    /// </summary>
    private static long DrawDressRow(string label, long itemId, string iconFolder)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText(label, 40);
        itemId = EditorGUILayout.LongField(itemId, GUILayout.Width(90));
        if (itemId != 0)
        {
            ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemId);
            if (itemsInfo != null)
            {
                if (!iconFolder.IsNull())
                    EditorUI.GUIPic(iconFolder + "/" + itemsInfo.icon_key, 28, 28);
                EditorUI.GUIText(itemsInfo.name_language, 150);
            }
            else
            {
                EditorUI.GUIText("（未找到该道具）", 110);
            }
        }
        GUILayout.EndHorizontal();
        return itemId;
    }

    /// <summary>
    /// 绘制属性小字段
    /// </summary>
    private static int DrawIntAttr(string label, int value, int labelWidth)
    {
        EditorUI.GUIText(label, labelWidth);
        return EditorGUILayout.IntField(value, GUILayout.Width(50));
    }

    /// <summary>
    /// 绘制颜色编辑（解析失败时兜底为白色）
    /// </summary>
    private static string DrawColorEditor(string colorStr)
    {
        Color color;
        try
        {
            color = colorStr.IsNull() ? Color.white : new ColorBean(colorStr).GetColor();
        }
        catch
        {
            color = Color.white;
        }
        Color newColor = EditorGUILayout.ColorField(GUIContent.none, color, GUILayout.Width(60), GUILayout.Height(20));
        return newColor.r + "," + newColor.g + "," + newColor.b + "," + newColor.a;
    }
    #endregion

    #region 团队详情绘制
    private void DrawNpcTeamDetail(NpcTeamBean teamData)
    {
        NpcTeamTypeEnum teamType = (NpcTeamTypeEnum)teamData.team_type;

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("有效", 30);
        teamData.valid = (int)(ValidEnum)EditorGUILayout.EnumPopup((ValidEnum)teamData.valid, GUILayout.Width(70));
        EditorUI.GUIText("类型", 30);
        teamData.team_type = (int)(NpcTeamTypeEnum)EditorGUILayout.EnumPopup(teamType, GUILayout.Width(110));
        EditorUI.GUIText("ID", 24);
        teamData.id = EditorGUILayout.LongField(teamData.id, GUILayout.Width(80));
        EditorUI.GUIText("名称", 30);
        teamData.name_language = EditorGUILayout.TextField(teamData.name_language + "", GUILayout.Width(120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("领袖IDs(,)", 70);
        teamData.team_leader = EditorGUILayout.TextField(teamData.team_leader + "", GUILayout.Width(180));
        EditorUI.GUIText("成员IDs(,)", 70);
        teamData.team_members = EditorGUILayout.TextField(teamData.team_members + "", GUILayout.Width(180));
        if (teamType == NpcTeamTypeEnum.Customer)
        {
            EditorUI.GUIText("成员数量上限", 80);
            teamData.team_number = EditorGUILayout.IntField(teamData.team_number, GUILayout.Width(50));
        }
        GUILayout.EndHorizontal();

        switch (teamType)
        {
            case NpcTeamTypeEnum.Rascal:
            case NpcTeamTypeEnum.Sundry:
            case NpcTeamTypeEnum.Entertain:
            case NpcTeamTypeEnum.Disappointed:
                GUILayout.BeginHorizontal();
                EditorUI.GUIText("持续时间", 60);
                teamData.effect_time = EditorGUILayout.TextField(teamData.effect_time + "", GUILayout.Width(70));
                EditorUI.GUIText("对话markId(,)", 90);
                teamData.talk_ids = EditorGUILayout.TextField(teamData.talk_ids + "", GUILayout.Width(160));
                EditorUI.GUIText("喊话markId(,)", 90);
                teamData.shout_ids = EditorGUILayout.TextField(teamData.shout_ids + "", GUILayout.Width(160));
                GUILayout.EndHorizontal();
                break;
        }

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("喜欢的菜品", 70);
        teamData.love_menus = EditorGUILayout.TextField(teamData.love_menus + "", GUILayout.Width(250));
        GUILayout.EndHorizontal();

        teamData.condition = EditorUI.GUIListData<ShowConditionEnum>("团队出现条件", teamData.condition);
    }
    #endregion

    #region 对话绘制
    /// <summary>
    /// 绘制一组对话（按markId分组，可折叠）
    /// </summary>
    private void DrawTalkGroup(long markId, List<TextInfoBean> listTextData)
    {
        GUILayout.BeginVertical("box");
        mapTalkFoldout.TryGetValue(markId, out bool foldout);
        bool newFoldout = EditorGUILayout.Foldout(foldout, "markId: " + markId + "  (" + listTextData.Count + "条)", true);
        mapTalkFoldout[markId] = newFoldout;

        if (newFoldout)
        {
            if (listTextData.Count == 0)
            {
                EditorGUILayout.HelpBox("（空对话组，请前往Excel添加对话内容）", MessageType.None);
            }
            else
            {
                GUILayout.BeginHorizontal();
                EditorUI.GUIText("对话类型", 60);
                listTextData[0].talk_type = (int)(TextTalkTypeEnum)EditorGUILayout.EnumPopup((TextTalkTypeEnum)listTextData[0].talk_type, GUILayout.Width(120));
                EditorUI.GUIText("好感条件Min", 80);
                listTextData[0].condition_min_favorability = EditorGUILayout.IntField(listTextData[0].condition_min_favorability, GUILayout.Width(50));
                EditorUI.GUIText("Max", 30);
                listTextData[0].condition_max_favorability = EditorGUILayout.IntField(listTextData[0].condition_max_favorability, GUILayout.Width(50));
                GUILayout.EndHorizontal();

                for (int i = 0; i < listTextData.Count; i++)
                    DrawTalkItem(listTextData[i]);
            }
        }
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 绘制单条对话（内容自动换行显示完整）
    /// </summary>
    private void DrawTalkItem(TextInfoBean itemTalk)
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("talkId:" + itemTalk.id, 130);
        EditorUI.GUIText("顺序:" + itemTalk.text_order, 70);
        EditorUI.GUIText("说话者:" + itemTalk.user_id, 110);
        EditorUI.GUIText(((TextInfoTypeEnum)itemTalk.type).ToString(), 90);
        if (!itemTalk.name.IsNull())
            EditorUI.GUIText("[" + itemTalk.name + "]", 80);
        GUILayout.EndHorizontal();
        GUILayout.Label(itemTalk.content, styleTalkContent);
        GUILayout.EndVertical();
    }
    #endregion

    #region 场景操作
    private void ShowNpcSafe(NpcInfoBean npcInfo)
    {
        if (mObjNpcContainer == null || mObjNpcModel == null)
        {
            EditorUI.GUIDialog("提示", "请先在顶部设置【NPC容器】和【NPC模型】");
            return;
        }
        ShowNpc(mObjNpcContainer, mObjNpcModel, new CharacterBean(npcInfo));
    }

    private void GetScenePosition(NpcInfoBean npcInfo)
    {
        if (mObjNpcContainer == null)
        {
            EditorUI.GUIDialog("提示", "请先在顶部设置【NPC容器】");
            return;
        }
        BaseNpcAI npcAI = mObjNpcContainer.GetComponentInChildren<BaseNpcAI>();
        if (npcAI == null)
        {
            EditorUI.GUIDialog("提示", "NPC容器下没有找到NPC，请先点击【显示】生成NPC");
            return;
        }
        npcInfo.position_x = npcAI.transform.position.x;
        npcInfo.position_y = npcAI.transform.position.y;
    }

    public static GameObject ShowNpc(GameObject objNpcContainer, GameObject objNpcModel, CharacterBean characterData)
    {
        CptUtil.RemoveChildsByActiveInEditor(objNpcContainer);
        GameObject objNpc = GameObject.Instantiate(objNpcModel, objNpcContainer.transform);
        objNpc.SetActive(true);
        objNpc.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);

        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        if (baseNpcAI == null)
        {
            LogUtil.LogError("NPC模型缺少 BaseNpcAI 组件");
            return objNpc;
        }
        baseNpcAI.Awake();

        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(baseNpcAI.gameObject, "Body");
        if (characterDress == null)
        {
            LogUtil.LogError("NPC模型缺少 CharacterDressCpt (Body) 组件");
            return objNpc;
        }
        characterDress.Awake();

        baseNpcAI.SetCharacterData(characterData);
        return objNpc;
    }
    #endregion

    #region 数据查询
    /// <summary>
    /// 安全解析逗号分隔的ID字符串（无效输入会被忽略并计数）
    /// </summary>
    private static long[] ParseIds(string idsStr, out int invalidCount)
    {
        List<long> listIds = new List<long>();
        invalidCount = 0;
        if (!idsStr.IsNull())
        {
            string[] parts = idsStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                if (long.TryParse(part.Trim(), out long id))
                    listIds.Add(id);
                else
                    invalidCount++;
            }
        }
        return listIds.ToArray();
    }

    private static List<NpcInfoBean> GetAllNpc()
    {
        List<NpcInfoBean> result = new List<NpcInfoBean>();
        var dic = NpcInfoCfg.GetAllData();
        if (dic != null) foreach (var item in dic) result.Add(item.Value);
        return result;
    }

    private static List<NpcInfoBean> GetNpcByIds(long[] ids)
    {
        List<NpcInfoBean> result = new List<NpcInfoBean>();
        if (ids == null) return result;
        foreach (long id in ids)
        {
            NpcInfoBean item = NpcInfoCfg.GetItemData(id);
            if (item != null) result.Add(item);
        }
        return result;
    }

    private static List<NpcInfoBean> GetNpcByType(NpcTypeEnum type)
    {
        List<NpcInfoBean> result = new List<NpcInfoBean>();
        var dic = NpcInfoCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.npc_type == (int)type)
                result.Add(item.Value);
        return result;
    }

    private static List<NpcTeamBean> GetAllTeams()
    {
        List<NpcTeamBean> result = new List<NpcTeamBean>();
        var dic = NpcTeamCfg.GetAllData();
        if (dic != null) foreach (var item in dic) result.Add(item.Value);
        return result;
    }

    private static List<NpcTeamBean> GetTeamByIds(long[] ids)
    {
        List<NpcTeamBean> result = new List<NpcTeamBean>();
        if (ids == null) return result;
        var dic = NpcTeamCfg.GetAllData();
        if (dic == null) return result;
        foreach (long id in ids)
            if (dic.TryGetValue(id, out NpcTeamBean item))
                result.Add(item);
        return result;
    }

    private static List<NpcTeamBean> GetTeamByType(NpcTeamTypeEnum type)
    {
        List<NpcTeamBean> result = new List<NpcTeamBean>();
        var dic = NpcTeamCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.team_type == (int)type)
                result.Add(item.Value);
        return result;
    }

    private static List<TextInfoBean> QueryTalkByMarkIds(long[] markIds)
    {
        List<TextInfoBean> result = new List<TextInfoBean>();
        if (markIds == null || markIds.Length == 0) return result;
        TextTalkBean[] array = TextTalkCfg.GetAllArrayData();
        if (array == null) return result;
        //用HashSet加速匹配，避免 O(n*m) 嵌套循环
        HashSet<long> setMarkIds = new HashSet<long>(markIds);
        foreach (TextTalkBean item in array)
            if (setMarkIds.Contains(item.mark_id))
                result.Add(ConvertTalkToTextInfo(item));
        return result;
    }

    private static List<TextInfoBean> QueryTalkByUserIdAndType(long userId, TextTalkTypeEnum talkType)
    {
        List<TextInfoBean> result = new List<TextInfoBean>();
        TextTalkBean[] array = TextTalkCfg.GetAllArrayData();
        if (array == null) return result;
        foreach (TextTalkBean item in array)
            if (item.user_id == userId && item.talk_type == (int)talkType)
                result.Add(ConvertTalkToTextInfo(item));
        return result;
    }

    private static TextInfoBean ConvertTalkToTextInfo(TextTalkBean src)
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

    /// <summary>
    /// 对话数据按markId分组
    /// </summary>
    private static void HandleTalkInfoDataByMarkId(List<TextInfoBean> listTalkInfo, Dictionary<long, List<TextInfoBean>> mapTalkInfo)
    {
        mapTalkInfo.Clear();
        foreach (TextInfoBean itemTalkInfo in listTalkInfo)
        {
            long markId = itemTalkInfo.mark_id;
            if (mapTalkInfo.TryGetValue(markId, out List<TextInfoBean> value))
                value.Add(itemTalkInfo);
            else
            {
                List<TextInfoBean> listTemp = new List<TextInfoBean>();
                listTemp.Add(itemTalkInfo);
                mapTalkInfo.Add(markId, listTemp);
            }
        }
    }
    #endregion

    #region 通用绘制
    /// <summary>
    /// 绘制分组标题+分隔线
    /// </summary>
    private static void DrawSectionTitle(string title)
    {
        GUILayout.Space(6);
        GUILayout.Label(title, styleSectionTitle);
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.4f));
        GUILayout.Space(2);
    }
    #endregion
}
