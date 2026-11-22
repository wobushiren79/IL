using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using static CharacterExpressionCpt;

/// <summary>
/// 剧情创建辅助工具
/// 布局：顶部工具栏 / 左侧剧情列表 / 右侧剧情详情（基础信息 + 步骤页签 + 详情卡片）/ 底部辅助工具与状态栏
/// 注意：本工具内的数据修改仅作用于内存预览，正式数据请编辑 Excel 并重新导出
/// </summary>
public class StoryInfoCreateWindowsEditor : EditorWindow
{
    [MenuItem("游戏/剧情创建")]
    static void CreateWindows()
    {
        StoryInfoCreateWindowsEditor window = GetWindow<StoryInfoCreateWindowsEditor>();
        window.minSize = new Vector2(960, 540);
        window.Show();
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
        //窗口重开/域重载后重建样式与状态
        stylesInitialized = false;
        mSelectedStory = null;
        mStatusMessage = null;
        //默认加载全部剧情，打开即所见
        QueryStoryInfoData(-1);
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

    #region 数据字段
    private string mNpcCreateIdStr = "";
    private StoryInfoBean mCreateStoryInfo = new StoryInfoBean();
    private long mFindStoryId = 0;
    private int mFindStroyOrder = 1;

    List<StoryInfoBean> listStoryInfo = new List<StoryInfoBean>();
    List<StoryInfoDetailsBean> listAllStoryInfoDetails = new List<StoryInfoDetailsBean>();
    List<StoryInfoDetailsBean> listOrderStoryInfoDetails = new List<StoryInfoDetailsBean>();
    List<TextInfoBean> listStoryTextInfo = new List<TextInfoBean>();
    Dictionary<long, NpcInfoBean> mapNpcInfo = new Dictionary<long, NpcInfoBean>();

    private long inputId = 0;
    #endregion

    #region UI状态字段
    private string mSearchText = "";                    //搜索关键词（ID 或备注）
    private int mSceneFilter = -1;                      //-1 全部；-2 按ID自定义查询；其余为 ScenesEnum 值
    private StoryInfoBean mSelectedStory = null;        //当前选中剧情
    private long mQueryStoryIdInput = 0;                //工具栏按ID查询输入
    private int mOrderInput = 1;                        //步骤跳转输入
    private Vector2 mScrollLeft = Vector2.zero;
    private Vector2 mScrollRight = Vector2.zero;
    private bool mShowTools = false;                    //底部辅助工具折叠
    private string mStatusMessage = null;
    private MessageType mStatusType = MessageType.Info;
    #endregion

    #region 样式定义
    private bool stylesInitialized = false;
    private GUIStyle sectionHeaderStyle;     //分区标题
    private GUIStyle cardStyle;              //卡片容器
    private GUIStyle cardTitleStyle;         //卡片内小标题
    private GUIStyle tagStyle;               //彩色标签
    private GUIStyle listItemStyle;          //列表项
    private GUIStyle listItemSelectedStyle;  //列表项选中
    private GUIStyle listItemTitleStyle;     //列表项标题
    private GUIStyle searchFieldStyle;       //工具栏搜索框
    private GUIStyle orderTabStyle;          //步骤页签
    private GUIStyle orderTabSelectedStyle;  //步骤页签选中
    private GUIStyle placeholderStyle;       //输入框占位提示
    private GUIStyle speakerPlayerStyle;     //对话-玩家
    private GUIStyle speakerNpcStyle;        //对话-NPC
    private GUIStyle contentWrapStyle;       //对话内容换行
    private GUIStyle idResultStyle;          //ID生成器结果

    private const float LabelWidthBase = 110f;   //基础信息表单标签宽
    private const float LabelWidthCard = 140f;   //详情卡片表单标签宽

    private void InitStyles()
    {
        if (stylesInitialized) return;
        stylesInitialized = true;
        bool isPro = EditorGUIUtility.isProSkin;

        sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            fixedHeight = 24,
            normal = { textColor = isPro ? new Color(0.9f, 0.9f, 0.9f) : new Color(0.1f, 0.1f, 0.1f) }
        };
        cardStyle = new GUIStyle("HelpBox")
        {
            padding = new RectOffset(10, 10, 8, 8),
            margin = new RectOffset(4, 4, 4, 4)
        };
        cardTitleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 12 };
        tagStyle = new GUIStyle(EditorStyles.miniBoldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(4, 4, 1, 1),
            margin = new RectOffset(0, 4, 2, 2),
            normal = { background = MakeTex(2, 2, Color.white), textColor = Color.white }
        };
        Color itemBg = isPro ? new Color(1f, 1f, 1f, 0.06f) : new Color(0f, 0f, 0f, 0.05f);
        Color itemSelectedBg = isPro ? new Color(0.29f, 0.52f, 0.88f, 0.55f) : new Color(0.29f, 0.52f, 0.88f, 0.35f);
        listItemStyle = new GUIStyle()
        {
            padding = new RectOffset(6, 6, 5, 5),
            margin = new RectOffset(2, 2, 1, 1),
            normal = { background = MakeTex(2, 2, itemBg) }
        };
        listItemSelectedStyle = new GUIStyle(listItemStyle)
        {
            normal = { background = MakeTex(2, 2, itemSelectedBg) }
        };
        listItemTitleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 12 };
        searchFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField") ?? EditorStyles.toolbarTextField;
        orderTabStyle = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 22,
            margin = new RectOffset(1, 1, 2, 2),
            padding = new RectOffset(4, 4, 2, 2)
        };
        orderTabSelectedStyle = new GUIStyle(orderTabStyle)
        {
            fontStyle = FontStyle.Bold,
            normal = { background = MakeTex(2, 2, isPro ? new Color(0.29f, 0.52f, 0.88f) : new Color(0.35f, 0.55f, 0.90f)), textColor = Color.white }
        };
        placeholderStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = isPro ? new Color(1f, 1f, 1f, 0.35f) : new Color(0f, 0f, 0f, 0.35f) }
        };
        speakerPlayerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = isPro ? new Color(0.5f, 0.9f, 0.5f) : new Color(0.1f, 0.6f, 0.1f) }
        };
        speakerNpcStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = isPro ? new Color(0.55f, 0.8f, 1f) : new Color(0.1f, 0.45f, 0.8f) }
        };
        contentWrapStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
        idResultStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            normal = { textColor = isPro ? new Color(0.55f, 0.85f, 1f) : new Color(0.1f, 0.45f, 0.8f) }
        };
    }

    /// <summary>
    /// 自制纯色纹理（编辑器样式背景用）
    /// </summary>
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    #endregion

    #region 绘制辅助
    /// <summary>
    /// 1px 分隔线
    /// </summary>
    private void DrawSeparator()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.12f) : new Color(0f, 0f, 0f, 0.15f));
        GUILayout.Space(4);
    }

    /// <summary>
    /// 带真占位提示的输入框（空值时显示灰色提示，不吃焦点）
    /// </summary>
    private string DrawPlaceholderTextField(string text, string placeholder, params GUILayoutOption[] options)
    {
        Rect rect = EditorGUILayout.GetControlRect(options);
        string newText = EditorGUI.TextField(rect, text ?? "");
        if (string.IsNullOrEmpty(newText))
            GUI.Label(rect, " " + placeholder, placeholderStyle);
        return newText;
    }

    /// <summary>
    /// 彩色标签（背景色由 GUI.backgroundColor 染色）
    /// </summary>
    private void DrawColoredTag(string text, Color color, float width = 48)
    {
        Color oldBg = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUILayout.Label(text, tagStyle, GUILayout.Width(width), GUILayout.Height(16));
        GUI.backgroundColor = oldBg;
    }

    /// <summary>
    /// X/Y 坐标并排输入
    /// </summary>
    private void DrawPositionXY(string label, ref float x, ref float y)
    {
        EditorGUILayout.BeginHorizontal();
        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUILayout.PrefixLabel(label);
        EditorGUIUtility.labelWidth = 14;
        x = EditorGUILayout.FloatField("X", x, GUILayout.MinWidth(50));
        y = EditorGUILayout.FloatField("Y", y, GUILayout.MinWidth(50));
        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndHorizontal();
    }

    private string GetSceneShortName(int scene)
    {
        switch ((ScenesEnum)scene)
        {
            case ScenesEnum.MainScene: return "主菜单";
            case ScenesEnum.GameInnScene: return "客栈";
            case ScenesEnum.GameTownScene: return "小镇";
            case ScenesEnum.GameArenaScene: return "竞技场";
            case ScenesEnum.GameMountainScene: return "山地";
            case ScenesEnum.GameForestScene: return "森林";
            case ScenesEnum.GameSquareScene: return "广场";
            case ScenesEnum.GameInfiniteTowersScene: return "无限塔";
            case ScenesEnum.GameCourtyardScene: return "庭院";
            case ScenesEnum.LoadingScene: return "加载";
            default: return ((ScenesEnum)scene).ToString();
        }
    }

    private Color GetSceneColor(int scene)
    {
        bool isPro = EditorGUIUtility.isProSkin;
        switch ((ScenesEnum)scene)
        {
            case ScenesEnum.GameInnScene: return isPro ? new Color(0.40f, 0.60f, 0.90f) : new Color(0.25f, 0.45f, 0.80f);
            case ScenesEnum.GameTownScene: return isPro ? new Color(0.40f, 0.75f, 0.50f) : new Color(0.20f, 0.60f, 0.35f);
            case ScenesEnum.GameArenaScene: return isPro ? new Color(0.85f, 0.50f, 0.40f) : new Color(0.75f, 0.35f, 0.25f);
            default: return isPro ? new Color(0.60f, 0.60f, 0.60f) : new Color(0.45f, 0.45f, 0.45f);
        }
    }

    private string GetDetailTypeName(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum type)
    {
        switch (type)
        {
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition: return "NPC站位";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression: return "NPC表情";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip: return "NPC穿着";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory: return "删除NPC";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk: return "对话";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext: return "延迟跳转";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition: return "道具站位";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition: return "员工站位";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect: return "粒子特效";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime: return "设置时间";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition: return "镜头位置";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter: return "镜头跟随";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound: return "音效播放";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic: return "音乐播放";
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt: return "场景互动";
            default: return "未知类型";
        }
    }

    private Color GetDetailTypeColor(StoryInfoDetailsBean.StoryInfoDetailsTypeEnum type)
    {
        bool isPro = EditorGUIUtility.isProSkin;
        switch (type)
        {
            //NPC组 蓝
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                return isPro ? new Color(0.35f, 0.55f, 0.95f) : new Color(0.20f, 0.40f, 0.85f);
            //对话组 绿
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                return isPro ? new Color(0.35f, 0.75f, 0.45f) : new Color(0.15f, 0.60f, 0.30f);
            //摆放组 青
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition:
                return isPro ? new Color(0.30f, 0.70f, 0.70f) : new Color(0.15f, 0.55f, 0.55f);
            //氛围组 金
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime:
                return isPro ? new Color(0.85f, 0.70f, 0.30f) : new Color(0.70f, 0.55f, 0.15f);
            //镜头组 紫
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                return isPro ? new Color(0.65f, 0.45f, 0.85f) : new Color(0.50f, 0.30f, 0.75f);
            //音频组 橙
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic:
                return isPro ? new Color(0.90f, 0.55f, 0.30f) : new Color(0.80f, 0.40f, 0.15f);
            //场景互动 灰蓝
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
            default:
                return isPro ? new Color(0.55f, 0.60f, 0.65f) : new Color(0.40f, 0.45f, 0.50f);
        }
    }

    /// <summary>
    /// 获取NPC显示名（0玩家 -1妻子，其余查NPC配置表）
    /// </summary>
    private string GetNpcDisplayName(long npcId)
    {
        if (npcId == 0) return "玩家";
        if (npcId == -1) return "妻子";
        if (mapNpcInfo.TryGetValue(npcId, out NpcInfoBean npcInfo) && npcInfo != null)
            return npcInfo.title_name_language + "-" + npcInfo.name;
        return "未知NPC(" + npcId + ")";
    }
    #endregion

    #region 状态与流程辅助
    private void SetStatus(string msg, MessageType type)
    {
        mStatusMessage = msg;
        mStatusType = type;
        Repaint();
    }

    /// <summary>
    /// 搜索过滤（纯客户端过滤，按 ID 或备注）
    /// </summary>
    private bool MatchesSearch(StoryInfoBean story)
    {
        if (string.IsNullOrEmpty(mSearchText)) return true;
        if (story.id.ToString().Contains(mSearchText)) return true;
        if (!string.IsNullOrEmpty(story.note) && story.note.ToLowerInvariant().Contains(mSearchText.ToLowerInvariant())) return true;
        return false;
    }

    /// <summary>
    /// 当前剧情所有存在的步骤序号（去重排序）
    /// </summary>
    private List<int> GetExistingOrders()
    {
        List<int> orders = new List<int>();
        if (listAllStoryInfoDetails == null) return orders;
        foreach (StoryInfoDetailsBean itemData in listAllStoryInfoDetails)
            if (!orders.Contains(itemData.story_order))
                orders.Add(itemData.story_order);
        orders.Sort();
        return orders;
    }

    /// <summary>
    /// 选中剧情：定位场景容器并加载详情（不再窄化左侧列表）
    /// </summary>
    private void SelectStory(StoryInfoBean story)
    {
        mSelectedStory = story;
        mFindStoryId = story.id;
        StoryInfoHandler.Instance.builderForStory.transform.position = new Vector3(story.position_x, story.position_y);
        QueryStoryDetailsData(mFindStoryId);
        //当前步骤序号在新剧情中不存在时，回退到最小步骤
        List<int> orders = GetExistingOrders();
        if (orders.Count > 0 && !orders.Contains(mFindStroyOrder))
            SwitchOrder(orders[0]);
        SetStatus("已定位并加载剧情 " + story.id, MessageType.Info);
    }

    /// <summary>
    /// 切换剧情步骤（查询与场景刷新必须成对调用）
    /// </summary>
    private void SwitchOrder(int order)
    {
        mFindStroyOrder = order;
        mOrderInput = order;
        listOrderStoryInfoDetails = GetStoryInfoDetailsByOrder(order);
        RefreshSceneData(listOrderStoryInfoDetails);
    }

    /// <summary>
    /// 清空全部数据与选中状态
    /// </summary>
    private void ClearAll()
    {
        if (listStoryInfo != null) listStoryInfo.Clear();
        if (listAllStoryInfoDetails != null) listAllStoryInfoDetails.Clear();
        if (listOrderStoryInfoDetails != null) listOrderStoryInfoDetails.Clear();
        listStoryTextInfo = null;
        mSelectedStory = null;
        SetStatus("已清空", MessageType.Info);
    }

    /// <summary>
    /// 刷新：重跑当前查询并重载选中剧情详情
    /// </summary>
    private void RefreshCurrent()
    {
        if (mSceneFilter == -1) QueryStoryInfoData(-1);
        else if (mSceneFilter == -2) QueryStoryInfoData(mQueryStoryIdInput);
        else QueryStoryInfoDataByScene((ScenesEnum)mSceneFilter);
        if (mSelectedStory != null)
            QueryStoryDetailsData(mSelectedStory.id);
        SetStatus("已刷新剧情数据", MessageType.Info);
    }
    #endregion

    #region OnGUI 分派
    private void OnGUI()
    {
        InitStyles();
        DrawToolbar();
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
        DrawLeftPanel();
        DrawRightPanel();
        EditorGUILayout.EndHorizontal();
        DrawToolsFoldout();
        DrawStatusBar();
    }

    /// <summary>
    /// 顶部工具栏：搜索 / 场景筛选 / 按ID查询 / 刷新 / 清空
    /// </summary>
    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("搜索:", GUILayout.Width(36));
        mSearchText = GUILayout.TextField(mSearchText ?? "", searchFieldStyle, GUILayout.Width(160));
        GUILayout.Space(10);

        DrawSceneFilterButton("全部", -1);
        DrawSceneFilterButton("客栈", (int)ScenesEnum.GameInnScene);
        DrawSceneFilterButton("小镇", (int)ScenesEnum.GameTownScene);
        DrawSceneFilterButton("竞技场", (int)ScenesEnum.GameArenaScene);
        GUILayout.Space(10);

        GUILayout.Label("ID:", GUILayout.Width(22));
        mQueryStoryIdInput = EditorGUILayout.LongField(mQueryStoryIdInput, EditorStyles.toolbarTextField, GUILayout.Width(110));
        if (GUILayout.Button("查询", EditorStyles.toolbarButton, GUILayout.Width(44)))
        {
            QueryStoryInfoData(mQueryStoryIdInput);
            mSceneFilter = -2;
            if (listStoryInfo != null && listStoryInfo.Count == 1)
                SelectStory(listStoryInfo[0]);
            else
                SetStatus("未找到 ID 为 " + mQueryStoryIdInput + " 的剧情", MessageType.Warning);
        }

        GUILayout.FlexibleSpace();
        GUIContent refreshContent = new GUIContent(" 刷新", EditorGUIUtility.IconContent("d_Refresh").image);
        if (GUILayout.Button(refreshContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
            RefreshCurrent();
        if (GUILayout.Button("清空", EditorStyles.toolbarButton, GUILayout.Width(44)))
            ClearAll();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSceneFilterButton(string label, int filter)
    {
        bool isActive = mSceneFilter == filter;
        bool newActive = GUILayout.Toggle(isActive, label, EditorStyles.toolbarButton, GUILayout.Width(52));
        if (newActive && !isActive)
        {
            mSceneFilter = filter;
            if (filter == -1) QueryStoryInfoData(-1);
            else QueryStoryInfoDataByScene((ScenesEnum)filter);
        }
    }

    /// <summary>
    /// 左栏：剧情列表
    /// </summary>
    private void DrawLeftPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(270));
        int showCount = 0;
        if (listStoryInfo != null)
            foreach (StoryInfoBean story in listStoryInfo)
                if (MatchesSearch(story)) showCount++;
        GUILayout.Label("剧情列表 (" + showCount + ")", cardTitleStyle);

        mScrollLeft = EditorGUILayout.BeginScrollView(mScrollLeft);
        if (listStoryInfo == null || listStoryInfo.Count == 0)
        {
            EditorGUILayout.HelpBox("暂无剧情数据，点击上方场景筛选加载", MessageType.Info);
        }
        else if (showCount == 0)
        {
            EditorGUILayout.HelpBox("没有匹配「" + mSearchText + "」的剧情", MessageType.Info);
        }
        else
        {
            foreach (StoryInfoBean story in listStoryInfo)
                if (MatchesSearch(story))
                    DrawStoryListItem(story);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 左栏单个剧情项：场景色块标签 + 备注 + ID，点击选中并定位场景
    /// </summary>
    private void DrawStoryListItem(StoryInfoBean story)
    {
        bool isSelected = mSelectedStory == story;
        EditorGUILayout.BeginVertical(isSelected ? listItemSelectedStyle : listItemStyle);
        GUILayout.BeginHorizontal();
        DrawColoredTag(GetSceneShortName(story.story_scene), GetSceneColor(story.story_scene), 44);
        string note = string.IsNullOrEmpty(story.note) ? "(无备注)" : story.note;
        GUILayout.Label(note, listItemTitleStyle);
        GUILayout.EndHorizontal();
        GUILayout.Label("ID: " + story.id, EditorStyles.miniLabel);
        EditorGUILayout.EndVertical();

        //整行点击选中（GetLastRect 在 EndVertical 后取值才准确）
        Rect itemRect = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseDown && itemRect.Contains(Event.current.mousePosition))
        {
            SelectStory(story);
            Event.current.Use();
            Repaint();
        }
        GUILayout.Space(2);
    }

    /// <summary>
    /// 右栏：选中剧情详情（基础信息 + 步骤导航 + 详情卡片流）
    /// </summary>
    private void DrawRightPanel()
    {
        EditorGUILayout.BeginVertical();
        mScrollRight = EditorGUILayout.BeginScrollView(mScrollRight);
        if (mSelectedStory == null)
        {
            EditorGUILayout.HelpBox("请在左侧列表选择一条剧情", MessageType.Info);
        }
        else
        {
            DrawBaseInfoCard();
            DrawOrderNavigator();
            DrawDetailCardList();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    #endregion

    #region 右栏-基础信息卡片
    private void DrawBaseInfoCard()
    {
        EditorGUILayout.BeginVertical(cardStyle);
        GUILayout.BeginHorizontal();
        GUILayout.Label("基础信息", sectionHeaderStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label("剧情 ID: " + mSelectedStory.id + "（只读）", EditorStyles.miniLabel);
        GUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("此处修改仅作用于内存预览，正式数据请编辑 Excel 并重新导出", MessageType.Warning);

        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = LabelWidthBase;

        mSelectedStory.note = EditorGUILayout.TextField("备注", mSelectedStory.note ?? "");
        mSelectedStory.story_scene = (int)(ScenesEnum)EditorGUILayout.EnumPopup("场景", (ScenesEnum)mSelectedStory.story_scene);
        if (mSelectedStory.story_scene == (int)ScenesEnum.GameTownScene)
        {
            mSelectedStory.location_type = (int)(TownBuildingEnum)EditorGUILayout.EnumPopup("城镇建筑", (TownBuildingEnum)mSelectedStory.location_type);
            mSelectedStory.out_in = EditorGUILayout.IntField("内外 (0外 1里)", mSelectedStory.out_in);
        }

        //剧情坐标 + 获取容器坐标
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("剧情坐标");
        EditorGUIUtility.labelWidth = 14;
        mSelectedStory.position_x = EditorGUILayout.FloatField("X", mSelectedStory.position_x, GUILayout.MinWidth(50));
        mSelectedStory.position_y = EditorGUILayout.FloatField("Y", mSelectedStory.position_y, GUILayout.MinWidth(50));
        EditorGUIUtility.labelWidth = LabelWidthBase;
        if (GUILayout.Button("获取容器坐标", GUILayout.Width(100)))
        {
            mSelectedStory.position_x = StoryInfoHandler.Instance.builderForStory.transform.position.x;
            mSelectedStory.position_y = StoryInfoHandler.Instance.builderForStory.transform.position.y;
            SetStatus("已获取容器坐标 (" + mSelectedStory.position_x + ", " + mSelectedStory.position_y + ")", MessageType.Info);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = oldLabelWidth;
        DrawTriggerConditions(mSelectedStory);
        EditorGUILayout.EndVertical();
        GUILayout.Space(6);
    }

    /// <summary>
    /// 触发条件编辑（保留 |EnumName:value| 字符串往返重建语义，新增畸形数据守卫）
    /// </summary>
    private void DrawTriggerConditions(StoryInfoBean storyInfo)
    {
        DrawSeparator();
        GUILayout.BeginHorizontal();
        GUILayout.Label("触发条件", cardTitleStyle, GUILayout.Width(60));
        if (GUILayout.Button("+ 添加条件", GUILayout.Width(90)))
            storyInfo.trigger_condition += ("|" + EventTriggerEnum.Year.GetEnumName() + ":" + "1|");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        List<string> listTriggerData = storyInfo.trigger_condition.SplitForListStr('|');
        storyInfo.trigger_condition = "";
        for (int i = 0; i < listTriggerData.Count; i++)
        {
            string itemTriggerData = listTriggerData[i];
            if (itemTriggerData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("×", GUILayout.Width(24)))
            {
                listTriggerData.RemoveAt(i);
                i--;
                EditorGUILayout.EndHorizontal();
                continue;
            }
            List<string> listItemTriggerData = itemTriggerData.SplitForListStr(':');
            if (listItemTriggerData.Count < 2)
            {
                //畸形数据守卫：提示并丢弃，避免越界崩溃
                GUILayout.Label("! 畸形数据: " + itemTriggerData, EditorStyles.miniLabel);
                EditorGUILayout.EndHorizontal();
                continue;
            }
            if (System.Enum.TryParse(listItemTriggerData[0], out EventTriggerEnum triggerEnum))
            {
                triggerEnum = (EventTriggerEnum)EditorGUILayout.EnumPopup(triggerEnum, GUILayout.Width(160));
                listItemTriggerData[0] = triggerEnum.GetEnumName();
            }
            else
            {
                //无法解析的条件名保留原文本，避免丢数据
                GUILayout.Label("! " + listItemTriggerData[0], EditorStyles.miniLabel, GUILayout.Width(160));
            }
            listItemTriggerData[1] = EditorGUILayout.TextField(listItemTriggerData[1] + "", GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();
            storyInfo.trigger_condition += (listItemTriggerData[0] + ":" + listItemTriggerData[1]) + "|";
        }
    }
    #endregion

    #region 右栏-步骤导航
    private void DrawOrderNavigator()
    {
        EditorGUILayout.BeginVertical(cardStyle);
        GUILayout.Label("剧情步骤", sectionHeaderStyle);

        if (listAllStoryInfoDetails == null || listAllStoryInfoDetails.Count == 0)
        {
            EditorGUILayout.HelpBox("该剧情没有详情数据", MessageType.Info);
            EditorGUILayout.EndVertical();
            GUILayout.Space(6);
            return;
        }

        List<int> orders = GetExistingOrders();
        //第一行：步骤页签排，点击直接切换
        GUILayout.BeginHorizontal();
        GUILayout.Label("步骤:", GUILayout.Width(40));
        foreach (int order in orders)
        {
            bool isCurrent = order == mFindStroyOrder;
            if (GUILayout.Button(order + "", isCurrent ? orderTabSelectedStyle : orderTabStyle, GUILayout.Width(32)))
                if (!isCurrent) SwitchOrder(order);
        }
        GUILayout.FlexibleSpace();
        int currentIndex = orders.IndexOf(mFindStroyOrder);
        GUILayout.Label("第 " + (currentIndex >= 0 ? (currentIndex + 1) + "" : "-") + " 步 / 共 " + orders.Count + " 步", EditorStyles.miniLabel);
        GUILayout.EndHorizontal();

        //第二行：翻页 / 跳转 / 刷新场景
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("◀", EditorStyles.miniButton, GUILayout.Width(28)))
            SwitchOrder(mFindStroyOrder - 1);
        if (GUILayout.Button("▶", EditorStyles.miniButton, GUILayout.Width(28)))
            SwitchOrder(mFindStroyOrder + 1);
        GUILayout.Space(10);
        GUILayout.Label("跳转到:", GUILayout.Width(50));
        mOrderInput = EditorGUILayout.IntField(mOrderInput, GUILayout.Width(50));
        if (GUILayout.Button("跳转", GUILayout.Width(50)))
            SwitchOrder(mOrderInput);
        GUILayout.Space(10);
        if (GUILayout.Button("刷新场景", GUILayout.Width(80)))
            RefreshSceneData(listOrderStoryInfoDetails);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        GUILayout.Space(6);
    }
    #endregion

    #region 右栏-详情卡片列表
    private void DrawDetailCardList()
    {
        GUILayout.Label("步骤详情（第 " + mFindStroyOrder + " 步）", sectionHeaderStyle);
        if (listOrderStoryInfoDetails == null || listOrderStoryInfoDetails.Count == 0)
        {
            EditorGUILayout.HelpBox("该步骤没有详情数据", MessageType.Info);
            return;
        }
        foreach (StoryInfoDetailsBean itemData in listOrderStoryInfoDetails)
            DrawDetailCard(itemData);
        GUILayout.Space(4);
        GUILayout.Label("[ 详情数据只读预览，正式编辑请修改 Excel 文件 ]", EditorStyles.centeredGreyMiniLabel);
    }

    /// <summary>
    /// 单个详情卡片：彩色类型标签 + 字段表单
    /// </summary>
    private void DrawDetailCard(StoryInfoDetailsBean itemData)
    {
        StoryInfoDetailsBean.StoryInfoDetailsTypeEnum detailType = itemData.GetStoryInfoDetailsType();
        EditorGUILayout.BeginVertical(cardStyle);
        //卡片标题行
        GUILayout.BeginHorizontal();
        DrawColoredTag(GetDetailTypeName(detailType), GetDetailTypeColor(detailType), 76);
        GUILayout.Label("order " + itemData.story_order + " · type " + itemData.type, EditorStyles.miniLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = LabelWidthCard;
        switch (detailType)
        {
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                DrawCardNpcPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
                DrawCardNpcExpression(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip:
                DrawCardNpcEquip(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                DrawCardNpcDestory(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                DrawCardTalk(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                DrawCardAutoNext(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
                DrawCardPropPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition:
                DrawCardWorkerPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect:
                DrawCardEffect(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime:
                DrawCardSetTime(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                DrawCardCameraPosition(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                DrawCardCameraFollowCharacter(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                DrawCardAudioSound(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic:
                DrawCardAudioMusic(itemData); break;
            case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                DrawCardSceneInt(itemData); break;
            default:
                EditorGUILayout.HelpBox("未知的详情类型: " + itemData.type, MessageType.Warning);
                break;
        }
        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndVertical();
        GUILayout.Space(4);
    }
    #endregion

    #region 详情卡片-15种类型
    /// <summary>
    /// NPC站位
    /// </summary>
    private void DrawCardNpcPosition(StoryInfoDetailsBean itemData)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("更新显示", GUILayout.Width(80)))
        {
            RemoveSceneObjByName("character_" + itemData.num);
            RefreshSceneData(listOrderStoryInfoDetails);
        }
        if (GUILayout.Button("获取显示坐标", GUILayout.Width(100)))
        {
            GameObject objItem = GetSceneObjByName("character_" + itemData.num);
            if (objItem == null)
                SetStatus("场景中不存在 character_" + itemData.num + "，请先点击「更新显示」", MessageType.Warning);
            else
            {
                itemData.position_x = objItem.transform.localPosition.x;
                itemData.position_y = objItem.transform.localPosition.y;
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        itemData.npc_id = EditorGUILayout.LongField("NPC ID(0自己 -1妻子)", itemData.npc_id);
        EditorGUILayout.LabelField("姓名", GetNpcDisplayName(itemData.npc_id));
        itemData.num = EditorGUILayout.IntField("NPC 序号", itemData.num);
        DrawPositionXY("NPC 位置", ref itemData.position_x, ref itemData.position_y);
        itemData.face = EditorGUILayout.IntField("朝向 (1左 2右)", itemData.face);
    }

    /// <summary>
    /// NPC表情
    /// </summary>
    private void DrawCardNpcExpression(StoryInfoDetailsBean itemData)
    {
        itemData.num = EditorGUILayout.IntField("NPC 编号", itemData.num);
        itemData.expression = (int)(CharacterExpressionEnum)EditorGUILayout.EnumPopup("表情", (CharacterExpressionEnum)itemData.expression);
    }

    /// <summary>
    /// NPC穿着
    /// </summary>
    private void DrawCardNpcEquip(StoryInfoDetailsBean itemData)
    {
        itemData.num = EditorGUILayout.IntField("NPC 序号", itemData.num);
        EditorGUILayout.LabelField("装备ID规则: -1默认 0不穿，可用逗号分割（前男后女）", EditorStyles.miniLabel);
        itemData.npc_hat = EditorGUILayout.TextField("头 ID", itemData.npc_hat);
        itemData.npc_clothes = EditorGUILayout.TextField("衣 ID", itemData.npc_clothes);
        itemData.npc_shoes = EditorGUILayout.TextField("鞋 ID", itemData.npc_shoes);
    }

    /// <summary>
    /// 删除NPC
    /// </summary>
    private void DrawCardNpcDestory(StoryInfoDetailsBean itemData)
    {
        itemData.npc_destroy = EditorGUILayout.TextField("删除角色序号(逗号分割)", itemData.npc_destroy);
        itemData.wait_time = EditorGUILayout.FloatField("延迟删除时间(s)", itemData.wait_time);
    }

    /// <summary>
    /// 对话（内嵌只读对话列表）
    /// </summary>
    private void DrawCardTalk(StoryInfoDetailsBean itemData)
    {
        EditorGUILayout.LabelField("文本标记ID", itemData.text_mark_id + "");
        EditorGUILayout.LabelField("[ 对话数据只读，请编辑 Excel ]", EditorStyles.miniLabel);
        GUILayout.Space(4);
        DrawTalkList();
    }

    /// <summary>
    /// 对话列表：说话人双色（玩家绿/NPC蓝），内容自动换行
    /// </summary>
    private void DrawTalkList()
    {
        if (listStoryTextInfo == null || listStoryTextInfo.Count == 0)
        {
            EditorGUILayout.HelpBox("未找到对应对话数据（文本剧情表中无此 text_mark_id 记录）", MessageType.Warning);
            return;
        }
        foreach (TextInfoBean textInfo in listStoryTextInfo)
        {
            EditorGUILayout.BeginVertical(listItemStyle);
            //行1：ID + 类型 + 顺序
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID:" + textInfo.id, EditorStyles.miniLabel, GUILayout.Width(110));
            GUILayout.Label(((TextInfoTypeEnum)textInfo.type).ToString(), EditorStyles.miniBoldLabel, GUILayout.Width(70));
            GUILayout.FlexibleSpace();
            GUILayout.Label("顺序 " + textInfo.text_order + " → 下一 " + textInfo.next_order, EditorStyles.miniLabel);
            GUILayout.EndHorizontal();
            //行2：说话人 / 黑幕时间
            if (textInfo.type == (int)TextInfoTypeEnum.Normal)
            {
                string speaker = GetNpcDisplayName(textInfo.user_id);
                GUIStyle speakerStyle = textInfo.user_id == 0 ? speakerPlayerStyle : speakerNpcStyle;
                string appointName = string.IsNullOrEmpty(textInfo.name) ? "" : "（指定姓名: " + textInfo.name + "）";
                GUILayout.Label(speaker + appointName, speakerStyle);
            }
            else if (textInfo.type == (int)TextInfoTypeEnum.Behind)
            {
                GUILayout.Label("黑幕时间: " + textInfo.wait_time + "s", EditorStyles.miniLabel);
            }
            //行3：对话内容
            if (!string.IsNullOrEmpty(textInfo.content))
                GUILayout.Label(textInfo.content, contentWrapStyle);
            EditorGUILayout.EndVertical();
            GUILayout.Space(3);
        }
    }

    /// <summary>
    /// 延迟跳转
    /// </summary>
    private void DrawCardAutoNext(StoryInfoDetailsBean itemData)
    {
        itemData.wait_time = EditorGUILayout.FloatField("延迟时间(s)", itemData.wait_time);
    }

    /// <summary>
    /// 道具站位
    /// </summary>
    private void DrawCardPropPosition(StoryInfoDetailsBean itemData)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("更新显示", GUILayout.Width(80)))
        {
            RemoveSceneObjByName("prop_" + itemData.num);
            RefreshSceneData(listOrderStoryInfoDetails);
        }
        if (GUILayout.Button("获取显示坐标", GUILayout.Width(100)))
        {
            GameObject objItem = GetSceneObjByName("prop_" + itemData.num);
            if (objItem == null)
                SetStatus("场景中不存在 prop_" + itemData.num + "，请先点击「更新显示」", MessageType.Warning);
            else
            {
                itemData.position_x = objItem.transform.localPosition.x;
                itemData.position_y = objItem.transform.localPosition.y;
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        itemData.key_name = EditorGUILayout.TextField("道具名称", itemData.key_name);
        itemData.num = EditorGUILayout.IntField("道具序号", itemData.num);
        DrawPositionXY("道具位置", ref itemData.position_x, ref itemData.position_y);
        itemData.face = EditorGUILayout.IntField("朝向 (1左 2右)", itemData.face);
    }

    /// <summary>
    /// 员工站位
    /// </summary>
    private void DrawCardWorkerPosition(StoryInfoDetailsBean itemData)
    {
        DrawPositionXY("员工站位", ref itemData.position_x, ref itemData.position_y);
        itemData.face = EditorGUILayout.IntField("朝向 (1左 2右)", itemData.face);
        DrawPositionXY("员工间隔", ref itemData.offset_x, ref itemData.offset_y);
        itemData.horizontal = EditorGUILayout.IntField("横排数", itemData.horizontal);
        itemData.vertical = EditorGUILayout.IntField("竖排数", itemData.vertical);
    }

    /// <summary>
    /// 粒子特效
    /// </summary>
    private void DrawCardEffect(StoryInfoDetailsBean itemData)
    {
        itemData.key_name = EditorGUILayout.TextField("粒子名称", itemData.key_name);
        DrawPositionXY("粒子位置", ref itemData.position_x, ref itemData.position_y);
        itemData.wait_time = EditorGUILayout.FloatField("持续时间(-1永久)", itemData.wait_time);
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    private void DrawCardSetTime(StoryInfoDetailsBean itemData)
    {
        itemData.time_hour = EditorGUILayout.IntField("小时", itemData.time_hour);
        itemData.time_minute = EditorGUILayout.IntField("分钟", itemData.time_minute);
    }

    /// <summary>
    /// 镜头位置
    /// </summary>
    private void DrawCardCameraPosition(StoryInfoDetailsBean itemData)
    {
        DrawPositionXY("镜头位置", ref itemData.position_x, ref itemData.position_y);
    }

    /// <summary>
    /// 镜头跟随角色
    /// </summary>
    private void DrawCardCameraFollowCharacter(StoryInfoDetailsBean itemData)
    {
        itemData.num = EditorGUILayout.IntField("跟随角色序号", itemData.num);
    }

    /// <summary>
    /// 音效播放
    /// </summary>
    private void DrawCardAudioSound(StoryInfoDetailsBean itemData)
    {
        itemData.audio_sound = (int)(AudioSoundEnum)EditorGUILayout.EnumPopup("音效类型", (AudioSoundEnum)itemData.audio_sound);
    }

    /// <summary>
    /// 音乐播放
    /// </summary>
    private void DrawCardAudioMusic(StoryInfoDetailsBean itemData)
    {
        itemData.audio_music = (int)(AudioMusicEnum)EditorGUILayout.EnumPopup("音乐类型", (AudioMusicEnum)itemData.audio_music);
    }

    /// <summary>
    /// 场景互动
    /// </summary>
    private void DrawCardSceneInt(StoryInfoDetailsBean itemData)
    {
        itemData.scene_intobj_name = EditorGUILayout.TextField("互动物体名称", itemData.scene_intobj_name);
        itemData.scene_intcomponent_name = EditorGUILayout.TextField("互动类型名称", itemData.scene_intcomponent_name);
        itemData.scene_intcomponent_method = EditorGUILayout.TextField("互动方法", itemData.scene_intcomponent_method);
        itemData.scene_intcomponent_parameters = EditorGUILayout.TextField("互动方法参数", itemData.scene_intcomponent_parameters);
    }
    #endregion

    #region 底部-辅助工具与状态栏
    /// <summary>
    /// 底部折叠区：人物创建 / 新剧情ID生成器
    /// </summary>
    private void DrawToolsFoldout()
    {
        mShowTools = EditorGUILayout.Foldout(mShowTools, "辅助工具（人物创建 / 新剧情ID生成）", true);
        if (!mShowTools) return;
        EditorGUILayout.BeginVertical(cardStyle);
        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = LabelWidthBase;

        //人物创建
        GUILayout.BeginHorizontal();
        GUILayout.Label("人物创建:", GUILayout.Width(70));
        mNpcCreateIdStr = DrawPlaceholderTextField(mNpcCreateIdStr, "输入人物ID（0=玩家，-1=妻子）", GUILayout.Width(200));
        if (GUILayout.Button("创建", GUILayout.Width(60)))
        {
            if (long.TryParse(mNpcCreateIdStr, out long createNpcId))
            {
                GameObject objNpc = CreateNpc(createNpcId, Vector3.zero, 0);
                if (objNpc != null)
                    SetStatus("已创建人物 " + createNpcId + "（" + GetNpcDisplayName(createNpcId) + "）", MessageType.Info);
                else
                    SetStatus("创建失败，未找到 ID 为 " + createNpcId + " 的 NPC", MessageType.Error);
            }
            else
            {
                SetStatus("人物 ID 不规范: " + mNpcCreateIdStr, MessageType.Error);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        DrawSeparator();

        //新剧情ID生成器（公式与原逻辑一致：场景*10000000 + 建筑*100000 + 序号）
        GUILayout.Label("新剧情 ID 生成（只读，请编辑 Excel 文件）", cardTitleStyle);
        mCreateStoryInfo.story_scene = (int)(ScenesEnum)EditorGUILayout.EnumPopup("场景", (ScenesEnum)mCreateStoryInfo.story_scene);
        long newStoryId = (long)mCreateStoryInfo.story_scene * 10000000;
        if (mCreateStoryInfo.story_scene == (int)ScenesEnum.GameTownScene)
        {
            mCreateStoryInfo.location_type = (int)(TownBuildingEnum)EditorGUILayout.EnumPopup("城镇建筑", (TownBuildingEnum)mCreateStoryInfo.location_type);
            newStoryId += (long)mCreateStoryInfo.location_type * 100000;
        }
        inputId = EditorGUILayout.LongField("序号", inputId);
        newStoryId += inputId;
        mCreateStoryInfo.id = newStoryId;
        EditorGUILayout.LabelField("生成 ID", newStoryId + "", idResultStyle);
        mCreateStoryInfo.note = EditorGUILayout.TextField("备注", mCreateStoryInfo.note ?? "");

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 底部状态栏
    /// </summary>
    private void DrawStatusBar()
    {
        if (string.IsNullOrEmpty(mStatusMessage)) return;
        GUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox(mStatusMessage, mStatusType);
        if (GUILayout.Button("×", GUILayout.Width(22), GUILayout.Height(22)))
            mStatusMessage = null;
        GUILayout.EndHorizontal();
    }
    #endregion

    #region 数据查询与场景预览（原逻辑保留，勿改）
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

    protected void RemoveStoryInfoDetailsItem(StoryInfoDetailsBean itemData)
    {
        listAllStoryInfoDetails.Remove(itemData);
        listOrderStoryInfoDetails.Remove(itemData);
        if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition)
            RemoveSceneObjByName("character_" + itemData.num);
        else if (itemData.GetStoryInfoDetailsType() == StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition)
            RemoveSceneObjByName("prop_" + itemData.num);
    }
    #endregion
}
