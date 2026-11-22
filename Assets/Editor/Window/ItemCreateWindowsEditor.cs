using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

/// <summary>
/// 物品/商店/成就/建造/菜谱/技能 数据查看与创建辅助工具。
/// 配置数据均为只读展示（实际修改请编辑 Excel 配置表），物品页签提供新建 ID 计算辅助。
/// </summary>
public class ItemCreateWindowsEditor : EditorWindow
{
    //======================== 管理器 ========================
    GameItemsManager gameItemsManager;
    StoreInfoManager storeInfoManager;
    InnBuildManager innBuildManager;

    //======================== 页签 ========================
    static readonly string[] TabNames = { "物品", "商店", "成就", "建造", "菜谱", "技能" };
    const int TabItems = 0, TabStore = 1, TabAch = 2, TabBuild = 3, TabMenu = 4, TabSkill = 5;
    int currentTab = TabItems;
    readonly Vector2[] tabScrolls = new Vector2[TabNames.Length];
    readonly string[] nameFilters = new string[TabNames.Length];

    //======================== 创建区数据 ========================
    ItemsInfoBean createItemsInfo = new ItemsInfoBean();
    Sprite spriteCreateIcon;
    GeneralEnum createItemType;
    long inputId = 0;

    StoreInfoBean createStoreInfo = new StoreInfoBean();
    AchievementInfoBean createAchInfo = new AchievementInfoBean();
    MenuInfoBean createMenuInfo = new MenuInfoBean();
    SkillInfoBean createSkillInfo = new SkillInfoBean();

    //======================== 查询区数据 ========================
    string findIds = "";
    string findStoreIds = "";
    string findAchIds = "";
    string findMenuIds = "";
    string findSkillIds = "";

    List<ItemsInfoBean> listFindItem = new List<ItemsInfoBean>();
    List<StoreInfoBean> listFindStoreItem = new List<StoreInfoBean>();
    List<AchievementInfoBean> listFindAchItem = new List<AchievementInfoBean>();
    List<BuildItemBean> listFindBuildItem = new List<BuildItemBean>();
    List<MenuInfoBean> listFindMenuItem = new List<MenuInfoBean>();
    List<SkillInfoBean> listFindSkillItem = new List<SkillInfoBean>();

    //======================== 状态缓存 ========================
    readonly Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
    readonly Dictionary<string, Texture2D> iconCache = new Dictionary<string, Texture2D>();
    float flowX;

    //======================== 样式 ========================
    GUIStyle styleToolbarTitle;
    GUIStyle styleSectionFoldout;
    GUIStyle styleRowFoldout;
    GUIStyle styleBigId;
    GUIStyle styleWarn;

    [MenuItem("游戏/物品创建")]
    static void CreateWindows()
    {
        var window = GetWindow<ItemCreateWindowsEditor>("物品创建工具");
        window.minSize = new Vector2(760, 480);
        window.Show();
    }

    private void OnEnable()
    {
        gameItemsManager = new GameItemsManager();
        gameItemsManager.Awake();
        storeInfoManager = new StoreInfoManager();
        storeInfoManager.Awake();
        innBuildManager = new InnBuildManager();
        innBuildManager.Awake();
    }

    /// <summary>
    /// 清空查询结果与图标缓存
    /// </summary>
    public void RefreshData()
    {
        listFindItem.Clear();
        listFindStoreItem.Clear();
        listFindAchItem.Clear();
        listFindBuildItem.Clear();
        listFindMenuItem.Clear();
        listFindSkillItem.Clear();
        iconCache.Clear();
    }

    private void InitStyles()
    {
        if (styleWarn != null) return;
        styleToolbarTitle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 13 };
        styleSectionFoldout = new GUIStyle(EditorStyles.foldout) { fontSize = 13, fontStyle = FontStyle.Bold };
        styleRowFoldout = new GUIStyle(EditorStyles.foldout) { fontSize = 12, fontStyle = FontStyle.Bold };
        styleBigId = new GUIStyle(EditorStyles.boldLabel) { fontSize = 18 };
        styleWarn = new GUIStyle(EditorStyles.boldLabel);
    }

    private void OnGUI()
    {
        InitStyles();

        //顶部工具栏：标题 + 页签 + 刷新
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("物品创建工具", styleToolbarTitle, GUILayout.Width(90));
        int newTab = GUILayout.Toolbar(currentTab, TabNames, EditorStyles.toolbarButton, GUILayout.Width(430), GUILayout.Height(20));
        if (newTab != currentTab) currentTab = newTab;
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("刷新", EditorStyles.toolbarButton, GUILayout.Width(60)))
            RefreshData();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4);

        tabScrolls[currentTab] = EditorGUILayout.BeginScrollView(tabScrolls[currentTab]);
        switch (currentTab)
        {
            case TabItems: GUITabItems(); break;
            case TabStore: GUITabStore(); break;
            case TabAch: GUITabAch(); break;
            case TabBuild: GUITabBuild(); break;
            case TabMenu: GUITabMenu(); break;
            case TabSkill: GUITabSkill(); break;
        }
        EditorGUILayout.EndScrollView();
    }

    //############################################################################
    //                              通用绘制辅助
    //############################################################################

    bool GetFoldout(string key, bool def = false)
    {
        return foldoutStates.TryGetValue(key, out bool value) ? value : def;
    }

    /// <summary>区块折叠标题</summary>
    bool SectionFoldout(string key, string title, bool def = true)
    {
        bool open = GetFoldout(key, def);
        bool newOpen = EditorGUILayout.Foldout(open, title, true, styleSectionFoldout);
        if (newOpen != open) foldoutStates[key] = newOpen;
        return newOpen;
    }

    /// <summary>数据行折叠标题：标题 + valid 警告 + 右侧小图标</summary>
    bool RowFoldout(string key, string title, string iconPath, int valid)
    {
        bool open = GetFoldout(key);
        EditorGUILayout.BeginHorizontal();
        bool newOpen = EditorGUILayout.Foldout(open, title, true, styleRowFoldout);
        if (newOpen != open) foldoutStates[key] = newOpen;
        if (valid == 0)
        {
            Color oldColor = GUI.color;
            GUI.color = new Color(1f, 0.4f, 0.4f);
            GUILayout.Label("valid=0", styleWarn, GUILayout.Width(60));
            GUI.color = oldColor;
        }
        GUILayout.FlexibleSpace();
        DrawIcon(iconPath, 22, false);
        EditorGUILayout.EndHorizontal();
        return newOpen;
    }

    /// <summary>valid 字段，为 0 时给出红色警告（运行时会过滤该行）</summary>
    int DrawValidField(int valid)
    {
        EditorGUILayout.BeginHorizontal();
        valid = EditorGUILayout.IntField(new GUIContent("有效 valid", "配置表 valid 列，运行时只加载 valid != 0 的行"), valid, GUILayout.Width(150));
        if (valid == 0)
        {
            Color oldColor = GUI.color;
            GUI.color = new Color(1f, 0.4f, 0.4f);
            GUILayout.Label("⚠ valid=0，该行运行时会被过滤", styleWarn);
            GUI.color = oldColor;
        }
        EditorGUILayout.EndHorizontal();
        return valid;
    }

    //---------------- 字段控件 ----------------

    long FLong(string label, long value, float width = 220)
    {
        return EditorGUILayout.LongField(label, value, GUILayout.Width(width), GUILayout.Height(20));
    }

    int FInt(string label, int value, float width = 150)
    {
        return EditorGUILayout.IntField(label, value, GUILayout.Width(width), GUILayout.Height(20));
    }

    int FInt(GUIContent label, int value, float width = 150)
    {
        return EditorGUILayout.IntField(label, value, GUILayout.Width(width), GUILayout.Height(20));
    }

    float FFloat(string label, float value, float width = 150)
    {
        return EditorGUILayout.FloatField(label, value, GUILayout.Width(width), GUILayout.Height(20));
    }

    string FText(string label, string value, float width = 220)
    {
        return EditorGUILayout.TextField(label, value ?? "", GUILayout.Width(width), GUILayout.Height(20));
    }

    string FTextExpand(string label, string value)
    {
        return EditorGUILayout.TextField(label, value ?? "", GUILayout.Height(20));
    }

    T FEnum<T>(string label, T value, float width = 240) where T : Enum
    {
        return (T)EditorGUILayout.EnumPopup(label, value, GUILayout.Width(width), GUILayout.Height(20));
    }

    //---------------- 图标 ----------------

    Texture2D GetIconTex(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (iconCache.TryGetValue(path, out Texture2D tex)) return tex;
        tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        iconCache[path] = tex;
        return tex;
    }

    void DrawIcon(string path, int size = 64, bool placeholder = true)
    {
        Texture2D tex = GetIconTex(path);
        if (tex != null)
            GUILayout.Label(tex, GUILayout.Width(size), GUILayout.Height(size));
        else if (placeholder)
            GUILayout.Box("", GUILayout.Width(size), GUILayout.Height(size));
    }

    //---------------- 流式按钮（超出宽度自动换行） ----------------

    void FlowBegin()
    {
        flowX = 4;
        EditorGUILayout.BeginHorizontal();
    }

    bool FlowButton(string name, float width = 100)
    {
        CheckFlowWrap(width);
        return GUILayout.Button(name, GUILayout.Width(width), GUILayout.Height(22));
    }

    string FlowText(string value, float width = 140)
    {
        CheckFlowWrap(width);
        return EditorGUILayout.TextField(value ?? "", GUILayout.Width(width), GUILayout.Height(22));
    }

    void FlowLabel(string name, float width = 30)
    {
        CheckFlowWrap(width);
        GUILayout.Label(name, GUILayout.Width(width), GUILayout.Height(22));
    }

    void CheckFlowWrap(float width)
    {
        float rowLimit = Mathf.Max(300, position.width - 40);
        if (flowX + width > rowLimit)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            flowX = 4;
        }
        flowX += width + 6;
    }

    void FlowEnd()
    {
        EditorGUILayout.EndHorizontal();
    }

    //---------------- 结果筛选 ----------------

    /// <summary>结果统计 + 名字筛选，返回筛选后的列表</summary>
    List<T> DrawResultFilter<T>(int tab, List<T> list, Func<T, string> nameGetter)
    {
        if (list == null) list = new List<T>();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"查询结果：{list.Count} 条", EditorStyles.boldLabel, GUILayout.Width(130));
        GUILayout.Label("名字筛选：", GUILayout.Width(60));
        nameFilters[tab] = EditorGUILayout.TextField(nameFilters[tab] ?? "", GUILayout.Width(160));
        EditorGUILayout.EndHorizontal();
        string filter = nameFilters[tab];
        if (filter.IsNull()) return list;
        return list.FindAll(item =>
        {
            string name = nameGetter(item);
            return name != null && name.Contains(filter);
        });
    }

    //---------------- 键值枚举列表（前置/奖励/效果），安全解析 ----------------

    /// <summary>
    /// 编辑 "枚举名:值|枚举名:值|" 格式的数据。
    /// 旧实现直接按下标取值，遇到格式异常数据会抛异常导致整个窗口崩溃；
    /// 这里对无法解析的行保留原文并标黄提示。
    /// </summary>
    string GUIDataList<E>(string title, string data, E addDefault = default) where E : Enum
    {
        E addValue = ReferenceEquals(addDefault, null) ? EnumExtension.GetEnumValueByPosition<E>(0) : addDefault;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(title, EditorStyles.boldLabel, GUILayout.Width(90));
        if (GUILayout.Button("+ 添加", GUILayout.Width(60), GUILayout.Height(18)))
            data += addValue.GetEnumName() + ":1|";
        EditorGUILayout.EndHorizontal();

        List<string> listData = data.SplitForListStr('|');
        data = "";
        for (int i = 0; i < listData.Count; i++)
        {
            string itemData = listData[i];
            if (itemData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            bool deleted = GUILayout.Button("删除", GUILayout.Width(40), GUILayout.Height(18));
            if (!deleted)
            {
                string[] parts = itemData.Split(':');
                object parsed;
                if (parts.Length >= 2 && Enum.TryParse(typeof(E), parts[0], out parsed))
                {
                    E enumValue = (E)EditorGUILayout.EnumPopup((E)parsed, GUILayout.Width(180), GUILayout.Height(18));
                    string valueText = EditorGUILayout.TextField(parts[1], GUILayout.Width(90), GUILayout.Height(18));
                    data += enumValue.GetEnumName() + ":" + valueText + "|";
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = new Color(1f, 0.8f, 0.2f);
                    GUILayout.Label("数据异常", GUILayout.Width(60), GUILayout.Height(18));
                    GUI.color = oldColor;
                    string raw = EditorGUILayout.TextField(itemData, GUILayout.Width(280), GUILayout.Height(18));
                    data += raw + "|";
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return data;
    }

    //############################################################################
    //                              物品页签
    //############################################################################

    void GUITabItems()
    {
        GUICreateItem();
        EditorGUILayout.Space(8);
        GUIFindItem();
    }

    void GUICreateItem()
    {
        if (!SectionFoldout("sec_create_item", "创建物品（ID 计算辅助）")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.HelpBox("ID 规则：类型×100000 + 图标名段位（normal=0 / special=1 / work=3 / team=4 / anim=5）×10000 + 输入ID；"
            + "职业装图标名带 special 时 +10000；菜谱类型 = 类型×100000 + 菜谱ID。", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        createItemType = FEnum("物品类型", createItemType);
        createItemsInfo.items_type = (int)createItemType;
        EditorGUILayout.EndHorizontal();

        //名字（服装类型自动维护 -头/-衣/-鞋 后缀，切换类型时先剥掉旧后缀）
        string editedName = FTextExpand("名字", createItemsInfo.name_language);
        createItemsInfo.name_language = ApplyDressSuffix(editedName, createItemType);

        //图标选择 + 预览
        EditorGUILayout.BeginHorizontal();
        spriteCreateIcon = EditorGUILayout.ObjectField(new GUIContent("选择图片", "选择后 icon_key 自动取图片名"),
            spriteCreateIcon, typeof(Sprite), false, GUILayout.Width(320), GUILayout.Height(20)) as Sprite;
        if (spriteCreateIcon != null)
        {
            createItemsInfo.icon_key = spriteCreateIcon.name;
            GUILayout.Label(spriteCreateIcon.texture, GUILayout.Width(64), GUILayout.Height(64));
            EditorGUILayout.SelectableLabel(spriteCreateIcon.name, GUILayout.Width(200), GUILayout.Height(20));
        }
        else
        {
            createItemsInfo.icon_key = FText("icon_key", createItemsInfo.icon_key);
            DrawIcon(GetItemIconPath(createItemsInfo), 64);
        }
        EditorGUILayout.EndHorizontal();

        //ID 计算
        long autoId = createItemsInfo.items_type * 100000L;
        string iconName = spriteCreateIcon != null ? spriteCreateIcon.name : "";
        if (createItemType == GeneralEnum.Hat || createItemType == GeneralEnum.Clothes || createItemType == GeneralEnum.Shoes)
        {
            if (iconName.Contains("special")) autoId += 1 * 10000;
            else if (iconName.Contains("work")) autoId += 3 * 10000;
            else if (iconName.Contains("team")) autoId += 4 * 10000;
            else if (iconName.Contains("anim")) autoId += 5 * 10000;
            //normal 段位为 0
        }
        else if (createItemType == GeneralEnum.Chef || createItemType == GeneralEnum.Waiter
            || createItemType == GeneralEnum.Accoutant || createItemType == GeneralEnum.Accost
            || createItemType == GeneralEnum.Beater)
        {
            if (iconName.Contains("special")) autoId += 1 * 10000;
        }

        EditorGUILayout.BeginHorizontal();
        if (createItemType == GeneralEnum.Menu)
        {
            createItemsInfo.add_id = FLong("增加的菜谱ID", createItemsInfo.add_id);
            autoId += createItemsInfo.add_id;
            inputId = 0;
        }
        else
        {
            inputId = FLong("输入ID", inputId);
        }
        EditorGUILayout.EndHorizontal();

        createItemsInfo.id = inputId + autoId;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("物品ID：", EditorStyles.boldLabel, GUILayout.Width(60));
        EditorGUILayout.SelectableLabel(createItemsInfo.id + "", styleBigId, GUILayout.Width(160), GUILayout.Height(24));
        if (GUILayout.Button("复制ID", GUILayout.Width(60), GUILayout.Height(22)))
            GUIUtility.systemCopyBuffer = createItemsInfo.id + "";
        if (GUILayout.Button("重置", GUILayout.Width(60), GUILayout.Height(22)))
        {
            createItemsInfo = new ItemsInfoBean();
            spriteCreateIcon = null;
            inputId = 0;
        }
        EditorGUILayout.EndHorizontal();

        createItemsInfo.content_language = FTextExpand("描述", createItemsInfo.content_language);

        GUILayout.Label("属性加成", EditorStyles.boldLabel);
        GUIItemAttributes(createItemsInfo);

        EditorGUILayout.EndVertical();
    }

    /// <summary>服装类物品名字自动带 -头/-衣/-鞋 后缀；切换类型时先移除旧后缀</summary>
    static string ApplyDressSuffix(string itemName, GeneralEnum itemType)
    {
        if (string.IsNullOrEmpty(itemName)) return itemName;
        string[] suffixes = { "-头", "-衣", "-鞋" };
        foreach (string suffix in suffixes)
        {
            if (itemName.EndsWith(suffix))
            {
                itemName = itemName.Substring(0, itemName.Length - suffix.Length);
                break;
            }
        }
        switch (itemType)
        {
            case GeneralEnum.Hat: itemName += "-头"; break;
            case GeneralEnum.Clothes: itemName += "-衣"; break;
            case GeneralEnum.Shoes: itemName += "-鞋"; break;
        }
        return itemName;
    }

    /// <summary>属性加成两行网格（命厨速算 / 魅武运忠），tooltip 标明含义</summary>
    static void GUIItemAttributes(ItemsInfoBean item)
    {
        EditorGUILayout.BeginHorizontal();
        item.add_life = EditorGUILayout.IntField(new GUIContent("命", "增加生命值"), item.add_life, GUILayout.Width(130));
        item.add_cook = EditorGUILayout.IntField(new GUIContent("厨", "增加做菜"), item.add_cook, GUILayout.Width(130));
        item.add_speed = EditorGUILayout.IntField(new GUIContent("速", "增加跑堂"), item.add_speed, GUILayout.Width(130));
        item.add_account = EditorGUILayout.IntField(new GUIContent("算", "增加算账"), item.add_account, GUILayout.Width(130));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        item.add_charm = EditorGUILayout.IntField(new GUIContent("魅", "增加吆喝"), item.add_charm, GUILayout.Width(130));
        item.add_force = EditorGUILayout.IntField(new GUIContent("武", "增加武力"), item.add_force, GUILayout.Width(130));
        item.add_lucky = EditorGUILayout.IntField(new GUIContent("运", "增加切菜"), item.add_lucky, GUILayout.Width(130));
        item.add_loyal = EditorGUILayout.IntField(new GUIContent("忠", "增加信任"), item.add_loyal, GUILayout.Width(130));
        EditorGUILayout.EndHorizontal();
    }

    void GUIFindItem()
    {
        if (!SectionFoldout("sec_find_item", "查询物品")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        FlowLabel("ID：");
        findIds = FlowText(findIds, 150);
        if (FlowButton("查询", 60))
            listFindItem = gameItemsManager.GetItemsByIds(findIds.SplitForArrayLong(','));
        if (FlowButton("查询所有", 70))
            listFindItem = gameItemsManager.GetAllItems();
        if (FlowButton("礼物")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Gift);
        if (FlowButton("读物")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Read);
        if (FlowButton("书籍")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Book);
        if (FlowButton("菜谱")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Menu);
        if (FlowButton("技能书")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.SkillBook);
        if (FlowButton("所有药")) listFindItem = gameItemsManager.GetMedicineList();
        if (FlowButton("厨师道具")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Chef);
        if (FlowButton("伙计道具")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Waiter);
        if (FlowButton("账房道具")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accoutant);
        if (FlowButton("接待道具")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accost);
        if (FlowButton("打手道具")) listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Beater);
        if (FlowButton("所有服装")) listFindItem = gameItemsManager.GetClothesList();
        if (FlowButton("其他")) listFindItem = gameItemsManager.GetOtherList();
        FlowEnd();

        List<ItemsInfoBean> showList = DrawResultFilter(TabItems, listFindItem, item => item.name_language);
        foreach (ItemsInfoBean item in showList)
            GUIItemsInfoRow(item);

        EditorGUILayout.EndVertical();
    }

    void GUIItemsInfoRow(ItemsInfoBean item)
    {
        string key = "item_" + item.id;
        string title = $"ID:{item.id}  {item.name_language}  [{(GeneralEnum)item.items_type}]";
        string iconPath = GetItemIconPath(item);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (RowFoldout(key, title, iconPath, item.valid))
        {
            EditorGUILayout.BeginHorizontal();
            DrawIcon(iconPath, 64);
            EditorGUILayout.BeginVertical();
            item.id = FLong("ID", item.id);
            item.rarity = FInt("稀有度", item.rarity);
            item.items_type = (int)FEnum("物品类型", (GeneralEnum)item.items_type);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            item.name_language = FText("名字", item.name_language, 280);
            item.content_language = FText("描述", item.content_language, 280);
            item.icon_key = FText("icon_key", item.icon_key, 280);
            item.anim_key = FText("动画key", item.anim_key, 280);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            item.valid = DrawValidField(item.valid);

            GeneralEnum itemType = (GeneralEnum)item.items_type;
            if (itemType != GeneralEnum.Menu && itemType != GeneralEnum.Medicine
                && itemType != GeneralEnum.SkillBook && itemType != GeneralEnum.Read)
            {
                GUILayout.Label("属性加成", EditorStyles.boldLabel);
                GUIItemAttributes(item);
                item.rotation_angle = FInt(new GUIContent("旋转角度", "选择角度"), item.rotation_angle);
            }
            if (itemType == GeneralEnum.Medicine)
            {
                item.effect = GUIDataList<EffectTypeEnum>("效果", item.effect);
                item.effect_details = GUIDataList<EffectDetailsEnum>("效果详情", item.effect_details);
            }
            else if (itemType == GeneralEnum.Menu)
            {
                item.add_id = FLong("绑定菜谱ID", item.add_id);
            }
            else if (itemType == GeneralEnum.SkillBook)
            {
                item.add_id = FLong("绑定技能ID", item.add_id);
            }
            else if (itemType == GeneralEnum.Read)
            {
                item.add_id = FLong("绑定textlook markID", item.add_id, 260);
            }
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>按物品类型拼接图标路径</summary>
    static string GetItemIconPath(ItemsInfoBean item)
    {
        if (item.icon_key.IsNull()) return null;
        string dir = "Assets/Texture/";
        switch ((GeneralEnum)item.items_type)
        {
            case GeneralEnum.Hat: dir += "Character/Dress/Hat/"; break;
            case GeneralEnum.Clothes: dir += "Character/Dress/Clothes/"; break;
            case GeneralEnum.Shoes: dir += "Character/Dress/Shoes/"; break;
            case GeneralEnum.Mask: dir += "Character/Dress/Mask/"; break;
            case GeneralEnum.Medicine: dir += "Items/Medicine/"; break;
            case GeneralEnum.Chef: dir += "Items/Chef/"; break;
            case GeneralEnum.Waiter: dir += "Items/Waiter/"; break;
            case GeneralEnum.Accoutant: dir += "Items/Accountant/"; break;
            case GeneralEnum.Accost: dir += "Items/Accost/"; break;
            case GeneralEnum.Beater: dir += "Items/Beater/"; break;
            case GeneralEnum.Book:
            case GeneralEnum.SkillBook:
            case GeneralEnum.Menu:
            case GeneralEnum.Read:
            case GeneralEnum.Gift: dir += "Common/UI/"; break;
            default: dir += "Items/"; break;
        }
        return dir + item.icon_key + ".png";
    }

    //############################################################################
    //                              商店页签
    //############################################################################

    void GUITabStore()
    {
        if (SectionFoldout("sec_create_store", "创建商品"))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIStoreItem(createStoreInfo, true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(8);
        GUIFindStoreItem();
    }

    void GUIFindStoreItem()
    {
        if (!SectionFoldout("sec_find_store", "查询商品")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        FlowLabel("ID：");
        findStoreIds = FlowText(findStoreIds, 150);
        if (FlowButton("查询", 60))
        {
            listFindStoreItem = new List<StoreInfoBean>();
            foreach (long id in findStoreIds.SplitForArrayLong(','))
            {
                StoreInfoBean data = StoreInfoCfg.GetItemData(id);
                if (data != null) listFindStoreItem.Add(data);
            }
        }
        if (FlowButton("查询所有", 70))
        {
            listFindStoreItem = new List<StoreInfoBean>();
            var dic = StoreInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindStoreItem.Add(item.Value);
        }
        if (FlowButton("百宝斋")) storeInfoManager.GetStoreInfoForGrocery(list => listFindStoreItem = list);
        if (FlowButton("绸缎庄")) storeInfoManager.GetStoreInfoForDress(list => listFindStoreItem = list);
        if (FlowButton("建造商品")) storeInfoManager.GetStoreInfoForCarpenter(list => listFindStoreItem = list);
        if (FlowButton("药店")) storeInfoManager.GetStoreInfoForPharmacy(list => listFindStoreItem = list);
        if (FlowButton("公会商品")) storeInfoManager.GetStoreInfoForGuildGoods(list => listFindStoreItem = list);
        if (FlowButton("职业升级")) storeInfoManager.GetStoreInfoForGuildImprove(list => listFindStoreItem = list);
        if (FlowButton("客栈升级")) storeInfoManager.GetStoreInfoForGuildInnLevel(list => listFindStoreItem = list);
        if (FlowButton("斗技场")) storeInfoManager.GetStoreInfoForArenaInfo(list => listFindStoreItem = list);
        if (FlowButton("斗技场商品")) storeInfoManager.GetStoreInfoForArenaGoods(list => listFindStoreItem = list);
        if (FlowButton("床商品")) storeInfoManager.GetStoreInfoForCarpenterBed(list => listFindStoreItem = list);
        FlowEnd();

        List<StoreInfoBean> showList = DrawResultFilter(TabStore, listFindStoreItem, GetStoreMarkName);
        foreach (StoreInfoBean storeInfo in showList)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIStoreItem(storeInfo, false);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    /// <summary>商品关联的物品/建筑名字（用于标题与筛选）</summary>
    string GetStoreMarkName(StoreInfoBean storeInfo)
    {
        if (storeInfo == null) return "";
        if (storeInfo.mark_type == 1)
        {
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(storeInfo.mark_id);
            if (itemsInfo != null) return itemsInfo.name_language;
        }
        else if (storeInfo.mark_type == 2)
        {
            BuildItemBean buildInfo = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildInfo != null) return buildInfo.name_language;
        }
        return storeInfo.name_language ?? "";
    }

    void GUIStoreItem(StoreInfoBean storeInfo, bool isCreate)
    {
        bool showBody = true;
        if (!isCreate)
        {
            string markName = GetStoreMarkName(storeInfo);
            string title = $"ID:{storeInfo.id}  [{(StoreTypeEnum)storeInfo.type}]  {markName}";
            showBody = RowFoldout("store_" + storeInfo.id, title, GetStoreIconPath(storeInfo), storeInfo.valid);
        }
        if (!showBody) return;

        EditorGUILayout.BeginHorizontal();
        storeInfo.id = FLong("商品ID", storeInfo.id);
        storeInfo.type = (int)FEnum("商品类型", (StoreTypeEnum)storeInfo.type);
        EditorGUILayout.EndHorizontal();
        if (!isCreate)
            storeInfo.valid = DrawValidField(storeInfo.valid);

        switch ((StoreTypeEnum)storeInfo.type)
        {
            case StoreTypeEnum.Improve:
                GUIStoreItemForImprove(storeInfo);
                break;
            case StoreTypeEnum.InnLevel:
                GUIStoreItemForInnLevel(storeInfo);
                break;
            case StoreTypeEnum.ArenaInfo:
                GUIStoreItemForArenaInfo(storeInfo);
                break;
            case StoreTypeEnum.ArenaGoods:
            case StoreTypeEnum.Guild:
            case StoreTypeEnum.Carpenter:
            case StoreTypeEnum.Grocery:
            case StoreTypeEnum.Dress:
            case StoreTypeEnum.Pharmacy:
            case StoreTypeEnum.CarpenterBed:
                GUIStoreItemForGoods(storeInfo);
                break;
        }
    }

    /// <summary>商品图标：取关联物品/建筑的图标</summary>
    string GetStoreIconPath(StoreInfoBean storeInfo)
    {
        if (storeInfo.mark_type == 1)
        {
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(storeInfo.mark_id);
            if (itemsInfo != null) return GetItemIconPath(itemsInfo);
        }
        else if (storeInfo.mark_type == 2)
        {
            BuildItemBean buildInfo = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildInfo != null) return GetBuildIconPath(buildInfo);
        }
        return null;
    }

    void GUIStoreItemForGoods(StoreInfoBean storeInfo)
    {
        //对应类型（1=物品 2=建筑材料），用下拉替代原来的手填数字
        int markTypeIndex = Mathf.Clamp(storeInfo.mark_type - 1, 0, 1);
        markTypeIndex = EditorGUILayout.Popup("商品对应类型", markTypeIndex,
            new[] { "1 - 物品", "2 - 建筑材料" }, GUILayout.Width(240), GUILayout.Height(20));
        storeInfo.mark_type = markTypeIndex + 1;

        EditorGUILayout.BeginHorizontal();
        storeInfo.mark_id = FLong("对应物品ID", storeInfo.mark_id);
        //实时预览关联目标
        if (storeInfo.mark_type == 1)
        {
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(storeInfo.mark_id);
            if (itemsInfo != null)
            {
                storeInfo.mark = itemsInfo.items_type + "";
                DrawIcon(GetItemIconPath(itemsInfo), 32, false);
                GUILayout.Label(itemsInfo.name_language, EditorStyles.boldLabel, GUILayout.Height(32));
            }
            else if (storeInfo.mark_id != 0)
            {
                GUILayout.Label("未找到物品", EditorStyles.miniLabel, GUILayout.Height(20));
            }
        }
        else if (storeInfo.mark_type == 2)
        {
            BuildItemBean buildInfo = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildInfo != null)
            {
                storeInfo.mark = buildInfo.build_type + "";
                DrawIcon(GetBuildIconPath(buildInfo), 32, false);
                GUILayout.Label(buildInfo.name_language, EditorStyles.boldLabel, GUILayout.Height(32));
            }
            else if (storeInfo.mark_id != 0)
            {
                GUILayout.Label("未找到建筑", EditorStyles.miniLabel, GUILayout.Height(20));
            }
        }
        EditorGUILayout.EndHorizontal();

        storeInfo.get_number = FInt("获得数量", storeInfo.get_number);
        GUIPriceLMS(storeInfo);

        switch ((StoreTypeEnum)storeInfo.type)
        {
            case StoreTypeEnum.ArenaGoods:
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForArenaGoodsTypeEnum)storeInfo.store_goods_type);
                EditorGUILayout.BeginHorizontal();
                storeInfo.trophy_elementary = FLong("奖杯1", storeInfo.trophy_elementary, 140);
                storeInfo.trophy_intermediate = FLong("奖杯2", storeInfo.trophy_intermediate, 140);
                storeInfo.trophy_advanced = FLong("奖杯3", storeInfo.trophy_advanced, 140);
                storeInfo.trophy_legendary = FLong("奖杯4", storeInfo.trophy_legendary, 140);
                EditorGUILayout.EndHorizontal();
                break;
            case StoreTypeEnum.Guild:
                storeInfo.guild_coin = FLong("公会勋章", storeInfo.guild_coin);
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForGuildGoodsTypeEnum)storeInfo.store_goods_type);
                break;
            case StoreTypeEnum.Carpenter:
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForCarpenterTypeEnum)storeInfo.store_goods_type);
                if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
                {
                    EditorGUILayout.BeginHorizontal();
                    storeInfo.mark = FText("扩建等级", storeInfo.mark, 160);
                    storeInfo.mark_x = FInt("宽 w", storeInfo.mark_x, 120);
                    storeInfo.mark_y = FInt("高 h", storeInfo.mark_y, 120);
                    EditorGUILayout.EndHorizontal();
                }
                break;
            case StoreTypeEnum.Grocery:
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForGroceryTypeEnum)storeInfo.store_goods_type);
                break;
            case StoreTypeEnum.Dress:
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForDressTypeEnum)storeInfo.store_goods_type);
                break;
            case StoreTypeEnum.Pharmacy:
                storeInfo.store_goods_type = (int)FEnum("商品类型", (StoreForPharmacyTypeEnum)storeInfo.store_goods_type);
                break;
        }

        storeInfo.name_language = FTextExpand("备用名字", storeInfo.name_language);
        storeInfo.content_language = FTextExpand("备用描述", storeInfo.content_language);
    }

    void GUIStoreItemForImprove(StoreInfoBean storeInfo)
    {
        storeInfo.mark_type = FInt("考试等级", storeInfo.mark_type);
        GUIPriceLMS(storeInfo);
        storeInfo.mark = FText("消耗时间(小时)", storeInfo.mark);
        storeInfo.name_language = FTextExpand("名字", storeInfo.name_language);
        storeInfo.content_language = FTextExpand("描述", storeInfo.content_language);
        storeInfo.pre_data_minigame = GUIDataList<PreTypeForMiniGameEnum>("小游戏数据", storeInfo.pre_data_minigame, PreTypeForMiniGameEnum.WinSurvivalTime);
    }

    void GUIStoreItemForInnLevel(StoreInfoBean storeInfo)
    {
        storeInfo.mark_type = FInt("客栈等级", storeInfo.mark_type);
        storeInfo.name_language = FTextExpand("名字", storeInfo.name_language);
        storeInfo.content_language = FTextExpand("描述", storeInfo.content_language);
        storeInfo.pre_data = GUIDataList<PreTypeEnum>("前置条件", storeInfo.pre_data, PreTypeEnum.PayMoneyL);
        storeInfo.reward_data = GUIDataList<RewardTypeEnum>("奖励", storeInfo.reward_data, RewardTypeEnum.AddItems);
    }

    void GUIStoreItemForArenaInfo(StoreInfoBean storeInfo)
    {
        storeInfo.mark_type = (int)FEnum("竞赛等级", (TrophyTypeEnum)storeInfo.mark_type);

        //职业存的是枚举名字符串，非法值回退到第一个枚举
        WorkerEnum worker;
        object parsedWorker;
        if (storeInfo.pre_data.IsNull() || !Enum.TryParse(typeof(WorkerEnum), storeInfo.pre_data, out parsedWorker))
            worker = (WorkerEnum)Enum.GetValues(typeof(WorkerEnum)).GetValue(0);
        else
            worker = (WorkerEnum)parsedWorker;
        storeInfo.pre_data = FEnum("职业", worker).GetEnumName();

        GUILayout.Label("报名费", EditorStyles.boldLabel);
        GUIPriceLMS(storeInfo);
        storeInfo.mark = FText("消耗时间(小时)", storeInfo.mark);
        storeInfo.pre_data_minigame = GUIDataList<PreTypeForMiniGameEnum>("小游戏前置", storeInfo.pre_data_minigame, PreTypeForMiniGameEnum.WinSurvivalTime);
        storeInfo.reward_data = GUIDataList<RewardTypeEnum>("奖励", storeInfo.reward_data, RewardTypeEnum.AddItems);
    }

    /// <summary>价格 L/M/S 一行</summary>
    static void GUIPriceLMS(StoreInfoBean storeInfo)
    {
        EditorGUILayout.BeginHorizontal();
        storeInfo.price_l = EditorGUILayout.LongField(new GUIContent("价格L", "金币"), storeInfo.price_l, GUILayout.Width(140));
        storeInfo.price_m = EditorGUILayout.LongField(new GUIContent("M", "银币"), storeInfo.price_m, GUILayout.Width(110));
        storeInfo.price_s = EditorGUILayout.LongField(new GUIContent("S", "铜币"), storeInfo.price_s, GUILayout.Width(110));
        EditorGUILayout.EndHorizontal();
    }

    //############################################################################
    //                              成就页签
    //############################################################################

    void GUITabAch()
    {
        if (SectionFoldout("sec_create_ach", "创建成就"))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIAchItem(createAchInfo, true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(8);
        GUIFindAchItem();
    }

    void GUIFindAchItem()
    {
        if (!SectionFoldout("sec_find_ach", "查询成就")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        FlowLabel("ID：");
        findAchIds = FlowText(findAchIds, 150);
        if (FlowButton("查询", 60))
        {
            listFindAchItem = new List<AchievementInfoBean>();
            foreach (long id in findAchIds.SplitForArrayLong(','))
            {
                AchievementInfoBean data = AchievementInfoCfg.GetItemData(id);
                if (data != null) listFindAchItem.Add(data);
            }
        }
        if (FlowButton("查询所有", 70))
        {
            listFindAchItem = new List<AchievementInfoBean>();
            var dic = AchievementInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindAchItem.Add(item.Value);
        }
        if (FlowButton("通用成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Normal);
        if (FlowButton("厨师成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Chef);
        if (FlowButton("伙计成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Waiter);
        if (FlowButton("账房成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Account);
        if (FlowButton("接待成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Accost);
        if (FlowButton("打手成就")) listFindAchItem = GetAchByType(AchievementTypeEnum.Beater);
        FlowEnd();

        List<AchievementInfoBean> showList = DrawResultFilter(TabAch, listFindAchItem, item => item.name_language);
        foreach (AchievementInfoBean itemAchInfo in showList)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIAchItem(itemAchInfo, false);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    static List<AchievementInfoBean> GetAchByType(AchievementTypeEnum type)
    {
        List<AchievementInfoBean> result = new List<AchievementInfoBean>();
        var dic = AchievementInfoCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.type == (int)type)
                result.Add(item.Value);
        return result;
    }

    void GUIAchItem(AchievementInfoBean achievementInfo, bool isCreate)
    {
        bool showBody = true;
        if (!isCreate)
        {
            string title = $"ID:{achievementInfo.id}  {achievementInfo.name_language}  [{(AchievementTypeEnum)achievementInfo.type}]";
            showBody = RowFoldout("ach_" + achievementInfo.id, title, GetAchIconPath(achievementInfo), achievementInfo.valid);
        }
        if (!showBody) return;

        EditorGUILayout.BeginHorizontal();
        DrawIcon(GetAchIconPath(achievementInfo), 64);
        EditorGUILayout.BeginVertical();
        achievementInfo.id = FLong("成就ID", achievementInfo.id);
        achievementInfo.type = (int)FEnum("成就类型", (AchievementTypeEnum)achievementInfo.type);
        achievementInfo.icon_key = FText("icon_key", achievementInfo.icon_key);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (!isCreate)
            achievementInfo.valid = DrawValidField(achievementInfo.valid);

        achievementInfo.name_language = FTextExpand("名称", achievementInfo.name_language);
        achievementInfo.content_language = FTextExpand("内容", achievementInfo.content_language);
        achievementInfo.pre_ach_ids = FText("前置成就IDs", achievementInfo.pre_ach_ids, 280);
        achievementInfo.pre_data = GUIDataList<PreTypeEnum>("前置条件", achievementInfo.pre_data, PreTypeEnum.PayMoneyL);
        achievementInfo.reward_data = GUIDataList<RewardTypeEnum>("奖励", achievementInfo.reward_data, RewardTypeEnum.AddWorkerNumber);
    }

    static string GetAchIconPath(AchievementInfoBean achievementInfo)
    {
        if (achievementInfo.icon_key.IsNull()) return null;
        return "Assets/Texture/Common/UI/" + achievementInfo.icon_key + ".png";
    }

    //############################################################################
    //                              建造页签
    //############################################################################

    void GUITabBuild()
    {
        EditorGUILayout.HelpBox("建造数据为只读展示，新增/修改请编辑 Excel 配置表。", MessageType.None);
        GUIFindBuildItem();
    }

    void GUIFindBuildItem()
    {
        if (!SectionFoldout("sec_find_build", "查询建造物品")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        if (FlowButton("查询所有", 70))
        {
            listFindBuildItem = new List<BuildItemBean>();
            var dic = BuildItemCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindBuildItem.Add(item.Value);
        }
        if (FlowButton("地板")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Floor);
        if (FlowButton("墙壁")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Wall);
        if (FlowButton("桌椅")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Table);
        if (FlowButton("灶台")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Stove);
        if (FlowButton("柜台")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Counter);
        if (FlowButton("装饰")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Decoration);
        if (FlowButton("正门")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Door);
        if (FlowButton("楼梯")) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.Stairs);
        if (FlowButton("床-基础", 80)) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedBase);
        if (FlowButton("床-床栏", 80)) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedBar);
        if (FlowButton("床-床单", 80)) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedSheets);
        if (FlowButton("床-枕头", 80)) listFindBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedPillow);
        FlowEnd();

        List<BuildItemBean> showList = DrawResultFilter(TabBuild, listFindBuildItem, item => item.name_language);
        foreach (BuildItemBean itemData in showList)
            GUIBuildItemRow(itemData);

        EditorGUILayout.EndVertical();
    }

    static List<BuildItemBean> GetBuildItemByType(BuildItemTypeEnum type)
    {
        List<BuildItemBean> result = new List<BuildItemBean>();
        var dic = BuildItemCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.build_type == (int)type)
                result.Add(item.Value);
        return result;
    }

    void GUIBuildItemRow(BuildItemBean buildItem)
    {
        string iconPath = GetBuildIconPath(buildItem);
        string title = $"ID:{buildItem.id}  {buildItem.name_language}  [{(BuildItemTypeEnum)buildItem.build_type}]";

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (RowFoldout("build_" + buildItem.id, title, iconPath, buildItem.valid))
        {
            EditorGUILayout.BeginHorizontal();
            DrawIcon(iconPath, 64);
            EditorGUILayout.BeginVertical();
            buildItem.id = FLong("ID", buildItem.id);
            buildItem.build_type = (int)FEnum("类型", (BuildItemTypeEnum)buildItem.build_type);
            buildItem.aesthetics = FFloat("美观", buildItem.aesthetics);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            buildItem.model_name = FText("模型ID", buildItem.model_name, 260);
            buildItem.icon_key = FText("icon_key", buildItem.icon_key, 260);
            BuildItemTypeEnum buildType = (BuildItemTypeEnum)buildItem.build_type;
            switch (buildType)
            {
                case BuildItemTypeEnum.Table:
                case BuildItemTypeEnum.Counter:
                case BuildItemTypeEnum.Stove:
                case BuildItemTypeEnum.Door:
                case BuildItemTypeEnum.Decoration:
                    buildItem.icon_list = FText("icon_list", buildItem.icon_list, 260);
                    break;
                case BuildItemTypeEnum.Floor:
                case BuildItemTypeEnum.Wall:
                    buildItem.tile_name = FText("tile名字", buildItem.tile_name, 260);
                    break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            buildItem.valid = DrawValidField(buildItem.valid);
            //框架更新后 name/content 为文本ID，展示需走 language 字段
            buildItem.name_language = FTextExpand("名称", buildItem.name_language);
            buildItem.content_language = FTextExpand("形容", buildItem.content_language);
        }
        EditorGUILayout.EndVertical();
    }

    static string GetBuildIconPath(BuildItemBean buildItem)
    {
        if (buildItem.icon_key.IsNull()) return null;
        string dir;
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor: dir = "Assets/Texture/Tile/Floor"; break;
            case BuildItemTypeEnum.Wall: dir = "Assets/Texture/Tile/Wall"; break;
            case BuildItemTypeEnum.Table: dir = "Assets/Texture/InnBuild/TableAndChair"; break;
            case BuildItemTypeEnum.Stove: dir = "Assets/Texture/InnBuild/Stove"; break;
            case BuildItemTypeEnum.Counter: dir = "Assets/Texture/InnBuild/Counter"; break;
            case BuildItemTypeEnum.Decoration: dir = "Assets/Texture/InnBuild/Decoration"; break;
            case BuildItemTypeEnum.Door: dir = "Assets/Texture/InnBuild/Door"; break;
            case BuildItemTypeEnum.BedBar:
            case BuildItemTypeEnum.BedBase:
            case BuildItemTypeEnum.BedPillow:
            case BuildItemTypeEnum.BedSheets: dir = "Assets/Texture/InnBuild/Bed"; break;
            default: return null;
        }
        return dir + "/" + buildItem.icon_key + ".png";
    }

    //############################################################################
    //                              菜谱页签
    //############################################################################

    void GUITabMenu()
    {
        EditorGUILayout.HelpBox("菜谱数据为只读展示，新增/修改请编辑 Excel 配置表。下方预览可辅助计算利润与售价。", MessageType.None);
        if (SectionFoldout("sec_create_menu", "新建菜谱预览（利润计算辅助）", false))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIMenuItem(createMenuInfo, true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(8);
        GUIFindMenu();
    }

    void GUIFindMenu()
    {
        if (!SectionFoldout("sec_find_menu", "查询菜谱")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        FlowLabel("ID：");
        findMenuIds = FlowText(findMenuIds, 150);
        if (FlowButton("查询", 60))
        {
            listFindMenuItem = new List<MenuInfoBean>();
            foreach (long id in findMenuIds.SplitForArrayLong(','))
            {
                MenuInfoBean data = MenuInfoCfg.GetItemData(id);
                if (data != null) listFindMenuItem.Add(data);
            }
        }
        if (FlowButton("查询所有", 70))
        {
            listFindMenuItem = new List<MenuInfoBean>();
            var dic = MenuInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindMenuItem.Add(item.Value);
        }
        FlowEnd();

        List<MenuInfoBean> showList = DrawResultFilter(TabMenu, listFindMenuItem, item => item.name_language);
        foreach (MenuInfoBean itemData in showList)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIMenuItem(itemData, false);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void GUIMenuItem(MenuInfoBean menuInfo, bool isCreate)
    {
        bool showBody = true;
        if (!isCreate)
        {
            string title = $"ID:{menuInfo.id}  {menuInfo.name_language}";
            showBody = RowFoldout("menu_" + menuInfo.id, title, GetMenuIconPath(menuInfo), menuInfo.valid);
        }
        if (!showBody) return;

        EditorGUILayout.BeginHorizontal();
        DrawIcon(GetMenuIconPath(menuInfo), 64);
        EditorGUILayout.BeginVertical();
        menuInfo.id = FLong("ID", menuInfo.id);
        menuInfo.rarity = FInt("稀有度", menuInfo.rarity);
        menuInfo.cook_time = FFloat("烹饪时间(秒)", menuInfo.cook_time, 180);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        //框架更新后 name/content 为文本ID，展示需走 language 字段
        menuInfo.name_language = FText("名称", menuInfo.name_language, 280);
        menuInfo.content_language = FText("内容", menuInfo.content_language, 280);
        menuInfo.icon_key = FText("图片名称", menuInfo.icon_key, 280);
        menuInfo.anim_key = FText("动画名称", menuInfo.anim_key, 280);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (!isCreate)
            menuInfo.valid = DrawValidField(menuInfo.valid);

        //利润计算：成本按材料固定单价折算，修改期望利润自动反推售价S
        long cost = menuInfo.ing_oilsalt * 5 + menuInfo.ing_meat * 10
            + menuInfo.ing_riverfresh * 10 + menuInfo.ing_seafood * 50
            + menuInfo.ing_vegetables * 5 + menuInfo.ing_melonfruit * 5
            + menuInfo.ing_waterwine * 10 + menuInfo.ing_flour * 5;
        long profit = menuInfo.price_s - cost;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"材料成本：{cost}", GUILayout.Width(110));
        GUILayout.Label($"当前利润：{profit}", EditorStyles.boldLabel, GUILayout.Width(110));
        int setProfit = EditorGUILayout.IntField(new GUIContent("期望利润", "修改后自动反推售价S"),
            (int)profit, GUILayout.Width(200));
        if (setProfit != profit)
            menuInfo.price_s = (int)(setProfit + cost);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        menuInfo.price_l = FInt("价格L", menuInfo.price_l, 130);
        menuInfo.price_m = FInt("M", menuInfo.price_m, 100);
        menuInfo.price_s = FInt("S", menuInfo.price_s, 100);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("材料", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        menuInfo.ing_oilsalt = FInt("油盐", menuInfo.ing_oilsalt, 110);
        menuInfo.ing_meat = FInt("鲜肉", menuInfo.ing_meat, 110);
        menuInfo.ing_riverfresh = FInt("河鲜", menuInfo.ing_riverfresh, 110);
        menuInfo.ing_seafood = FInt("海鲜", menuInfo.ing_seafood, 110);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        menuInfo.ing_vegetables = FInt("蔬菜", menuInfo.ing_vegetables, 110);
        menuInfo.ing_melonfruit = FInt("瓜果", menuInfo.ing_melonfruit, 110);
        menuInfo.ing_waterwine = FInt("酒水", menuInfo.ing_waterwine, 110);
        menuInfo.ing_flour = FInt("面粉", menuInfo.ing_flour, 110);
        EditorGUILayout.EndHorizontal();
    }

    static string GetMenuIconPath(MenuInfoBean menuInfo)
    {
        if (menuInfo.icon_key.IsNull()) return null;
        return "Assets/Texture/Food/" + menuInfo.icon_key + ".png";
    }

    //############################################################################
    //                              技能页签
    //############################################################################

    void GUITabSkill()
    {
        EditorGUILayout.HelpBox("技能数据为只读展示，新增/修改请编辑 Excel 配置表。", MessageType.None);
        if (SectionFoldout("sec_create_skill", "新建技能预览", false))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUISkillItem(createSkillInfo, true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(8);
        GUIFindSkill();
    }

    void GUIFindSkill()
    {
        if (!SectionFoldout("sec_find_skill", "查询技能")) return;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        FlowBegin();
        FlowLabel("ID：");
        findSkillIds = FlowText(findSkillIds, 150);
        if (FlowButton("查询", 60))
        {
            listFindSkillItem = new List<SkillInfoBean>();
            foreach (long id in findSkillIds.SplitForArrayLong(','))
            {
                SkillInfoBean data = SkillInfoCfg.GetItemData(id);
                if (data != null) listFindSkillItem.Add(data);
            }
        }
        if (FlowButton("查询所有", 70))
        {
            listFindSkillItem = new List<SkillInfoBean>();
            var dic = SkillInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindSkillItem.Add(item.Value);
        }
        FlowEnd();

        List<SkillInfoBean> showList = DrawResultFilter(TabSkill, listFindSkillItem, item => item.name_language);
        foreach (SkillInfoBean itemData in showList)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUISkillItem(itemData, false);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void GUISkillItem(SkillInfoBean skillInfo, bool isCreate)
    {
        bool showBody = true;
        if (!isCreate)
        {
            string title = $"ID:{skillInfo.id}  {skillInfo.name_language}";
            showBody = RowFoldout("skill_" + skillInfo.id, title, GetSkillIconPath(skillInfo), skillInfo.valid);
        }
        if (!showBody) return;

        EditorGUILayout.BeginHorizontal();
        DrawIcon(GetSkillIconPath(skillInfo), 64);
        EditorGUILayout.BeginVertical();
        skillInfo.id = FLong("ID", skillInfo.id);
        skillInfo.use_number = FInt("使用数量", skillInfo.use_number);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        //框架更新后 name/content 为文本ID，展示需走 language 字段
        skillInfo.name_language = FText("名称", skillInfo.name_language, 280);
        skillInfo.content_language = FText("介绍", skillInfo.content_language, 280);
        skillInfo.icon_key = FText("图片名称", skillInfo.icon_key, 280);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (!isCreate)
            skillInfo.valid = DrawValidField(skillInfo.valid);

        skillInfo.effect = GUIDataList<EffectTypeEnum>("效果", skillInfo.effect);
        skillInfo.effect_details = GUIDataList<EffectDetailsEnum>("效果详情", skillInfo.effect_details);
        skillInfo.pre_data = GUIDataList<PreTypeEnum>("解锁条件", skillInfo.pre_data);
    }

    static string GetSkillIconPath(SkillInfoBean skillInfo)
    {
        if (skillInfo.icon_key.IsNull()) return null;
        return "Assets/Texture/Common/UI/" + skillInfo.icon_key + ".png";
    }
}
