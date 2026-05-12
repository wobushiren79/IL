using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemCreateWindowsEditor : EditorWindow
{
    GameItemsManager gameItemsManager;
    StoreInfoManager storeInfoManager;
    InnBuildManager innBuildManager;

    [MenuItem("Tools/Window/ItemCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(ItemCreateWindowsEditor));
    }

    public ItemCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("物品创建工具");
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

    public void RefreshData()
    {
        listFindItem.Clear();
        listFindStoreItem.Clear();
        listFindAchItem.Clear();
        listFindBuildItem.Clear();
        listFindMenuItem.Clear();
        listFindSkillItem.Clear();
    }

    private Vector2 scrollPosition = Vector2.zero;

    public ItemsInfoBean createItemsInfo = new ItemsInfoBean();
    public Sprite spriteCreateIcon;
    public GeneralEnum createItemType;

    public StoreInfoBean createStoreInfo = new StoreInfoBean();
    public StoreTypeEnum createStoreItemType;
    public BuildItemBean createBuildItem = new BuildItemBean();

    public AchievementInfoBean createAchInfo = new AchievementInfoBean();
    long inputId = 0;

    public MenuInfoBean createMenuInfo = new MenuInfoBean();
    public string menuFindIds = "";

    public SkillInfoBean createSkillInfo = new SkillInfoBean();
    public string skillFindIds = "";

    public List<ItemsInfoBean> listFindItem = new List<ItemsInfoBean>();
    public List<StoreInfoBean> listFindStoreItem = new List<StoreInfoBean>();
    public List<AchievementInfoBean> listFindAchItem = new List<AchievementInfoBean>();
    public List<BuildItemBean> listFindBuildItem = new List<BuildItemBean>();
    public List<MenuInfoBean> listFindMenuItem = new List<MenuInfoBean>();
    public List<SkillInfoBean> listFindSkillItem = new List<SkillInfoBean>();

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
        {
            RefreshData();
        }

        GUICreateItem();
        GUIFindItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUIBuildItemCreate(createBuildItem);
        GUIBuildItemFind(listFindBuildItem, out listFindBuildItem);
        GUILayout.Label("------------------------------------------------------------------------------------------------");

        GUICreateStoreItem();
        GUIFindStoreItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUICreateAchItem();
        GUIFindAchItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUIMenuCreate(createMenuInfo);
        GUIMenuFind(menuFindIds, listFindMenuItem, out menuFindIds, out listFindMenuItem);
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUISkillCreate(createSkillInfo);
        GUISkillFind(skillFindIds, listFindSkillItem, out skillFindIds, out listFindSkillItem);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void GUICreateAchItem()
    {
        GUILayout.Label("创建成就");
        GUIAchItem(createAchInfo, true);
    }

    private string findAchIds = "";

    private void GUIFindAchItem()
    {
        GUILayout.Label("查询成就");
        GUILayout.BeginHorizontal();
        GUILayout.Label("成就ID：", GUILayout.Width(50), GUILayout.Height(20));
        findAchIds = EditorGUILayout.TextArea(findAchIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
        }
        if (GUILayout.Button("查询所有成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = new List<AchievementInfoBean>();
            var dic = AchievementInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listFindAchItem.Add(item.Value);
        }
        if (GUILayout.Button("查询通用成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Normal);
        }
        if (GUILayout.Button("查询厨师成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Chef);
        }
        if (GUILayout.Button("查询伙计成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Waiter);
        }
        if (GUILayout.Button("查询账房成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Account);
        }
        if (GUILayout.Button("查询接待成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Accost);
        }
        if (GUILayout.Button("查询打手成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = GetAchByType(AchievementTypeEnum.Beater);
        }
        GUILayout.EndHorizontal();
        if (listFindAchItem == null)
            return;
        foreach (AchievementInfoBean itemAchInfo in listFindAchItem)
        {
            GUIAchItem(itemAchInfo, false);
        }
    }

    private List<AchievementInfoBean> GetAchByType(AchievementTypeEnum type)
    {
        List<AchievementInfoBean> result = new List<AchievementInfoBean>();
        var dic = AchievementInfoCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.type == (int)type)
                result.Add(item.Value);
        return result;
    }

    private void GUIAchItem(AchievementInfoBean achievementInfo, bool isCreate)
    {
        EditorGUILayout.BeginHorizontal();
        if (!isCreate)
        {
            GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));
        }

        GUILayout.Label("成就ID：", GUILayout.Width(50), GUILayout.Height(20));
        achievementInfo.id = int.Parse(EditorGUILayout.TextArea(achievementInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        achievementInfo.type = (int)(AchievementTypeEnum)EditorGUILayout.EnumPopup("成就类型", (AchievementTypeEnum)achievementInfo.type, GUILayout.Width(300), GUILayout.Height(20));
        string path = "Assets/Texture/Common/UI/";
        path += (achievementInfo.icon_key + ".png");
        Texture2D iconTex = EditorGUIUtility.FindTexture(path);
        if (iconTex)
        {
            GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
        }
        GUILayout.Label("icon_key：", GUILayout.Width(150), GUILayout.Height(20));
        achievementInfo.icon_key = EditorGUILayout.TextArea(achievementInfo.icon_key + "", GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.Label("名称：", GUILayout.Width(100), GUILayout.Height(20));
        achievementInfo.name_language = EditorGUILayout.TextArea(achievementInfo.name_language + "", GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.Label("内容：", GUILayout.Width(100), GUILayout.Height(20));
        achievementInfo.content_language = EditorGUILayout.TextArea(achievementInfo.content_language + "", GUILayout.Width(150), GUILayout.Height(20));
        EditorUI.GUIText("前置成就:");
        achievementInfo.pre_ach_ids = EditorUI.GUIEditorText(achievementInfo.pre_ach_ids);
        EditorGUILayout.BeginVertical();
        GUILayout.Label("前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
        {
            achievementInfo.pre_data += ("|" + PreTypeEnum.PayMoneyL.GetEnumName() + ":" + "1|");
        }
        List<string> listPreData = achievementInfo.pre_data.SplitForListStr('|');
        achievementInfo.pre_data = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (itemPreData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listPreData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemPreData = itemPreData.SplitForListStr(':');
            listItemPreData[0] = EditorGUILayout.EnumPopup("前置类型", listItemPreData[0].GetEnumName().GetEnum<PreTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20)).GetEnumName();
            listItemPreData[1] = EditorGUILayout.TextArea(listItemPreData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            achievementInfo.pre_data += (listItemPreData[0] + ":" + listItemPreData[1]) + "|";
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        GUILayout.Label("奖励：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加奖励", GUILayout.Width(100), GUILayout.Height(20)))
        {
            achievementInfo.reward_data += (RewardTypeEnum.AddWorkerNumber.GetEnumName() + ":" + "1|");
        }
        List<string> listRewardData = achievementInfo.reward_data.SplitForListStr('|');
        achievementInfo.reward_data = "";
        for (int i = 0; i < listRewardData.Count; i++)
        {
            string itemRewardData = listRewardData[i];
            if (itemRewardData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listRewardData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemRewardData = itemRewardData.SplitForListStr(':');
            listItemRewardData[0] = EditorGUILayout.EnumPopup("奖励类型", listItemRewardData[0].GetEnumName().GetEnum<RewardTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20)).GetEnumName();
            listItemRewardData[1] = EditorGUILayout.TextArea(listItemRewardData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            achievementInfo.reward_data += (listItemRewardData[0] + ":" + listItemRewardData[1]) + "|";
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
    }

    private void GUICreateStoreItem()
    {
        GUILayout.Label("创建商品");
        GUIStoreItem(createStoreInfo, true);
    }

    private string findStoreIds = "";

    private void GUIFindStoreItem()
    {
        GUILayout.Label("查询物品");
        GUILayout.BeginHorizontal();
        GUILayout.Label("商品ID：", GUILayout.Width(50), GUILayout.Height(20));
        findStoreIds = EditorGUILayout.TextArea(findStoreIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20))) { }
        if (GUILayout.Button("查询百宝斋商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForGrocery((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询绸缎庄商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForDress((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询建造商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForCarpenter((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询药店商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForPharmacy((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询公会商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForGuildGoods((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询职业升级", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForGuildImprove((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询客栈升级", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForGuildInnLevel((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询斗技场", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForArenaInfo((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询斗技场商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForArenaGoods((listData) => { listFindStoreItem = listData; });
        if (GUILayout.Button("查询床商品", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfoManager.GetStoreInfoForCarpenterBed((listData) => { listFindStoreItem = listData; });
        GUILayout.EndHorizontal();
        if (listFindStoreItem == null)
            return;
        foreach (StoreInfoBean itemStoreInfo in listFindStoreItem)
            GUIStoreItem(itemStoreInfo, false);
    }

    private void GUIStoreItem(StoreInfoBean storeInfo, bool isCreate)
    {
        EditorGUILayout.BeginHorizontal();
        if (!isCreate)
            GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));

        GUILayout.Label("商品ID：", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.id = int.Parse(EditorGUILayout.TextArea(storeInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.type = (int)(StoreTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreTypeEnum)storeInfo.type, GUILayout.Width(300), GUILayout.Height(20));
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

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
    }

    private void GUIStoreItemForGoods(StoreInfoBean storeInfo)
    {
        GUILayout.Label("商品对应类型 1.item 2.建筑材料：", GUILayout.Width(200), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(20), GUILayout.Height(20)));

        GUILayout.Label("商品对应物品ID：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_id = long.Parse(EditorGUILayout.TextArea(storeInfo.mark_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        if (storeInfo.mark_type == 1)
        {
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(storeInfo.mark_id);
            if (itemsInfo != null)
            {
                storeInfo.mark = itemsInfo.items_type + "";
                GUILayout.Label("名字：" + itemsInfo.name, GUILayout.Width(250), GUILayout.Height(20));
            }
        }
        else if (storeInfo.mark_type == 2)
        {
            BuildItemBean buildInfo = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildInfo != null)
            {
                storeInfo.mark = buildInfo.build_type + "";
                GUILayout.Label("名字：" + buildInfo.name, GUILayout.Width(250), GUILayout.Height(20));
            }
        }
        EditorUI.GUIText("获得数量");
        storeInfo.get_number = EditorUI.GUIEditorText(storeInfo.get_number, 50);
        GUILayout.Label("价格--", GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.Label("LMS：", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.price_l = long.Parse(EditorGUILayout.TextArea(storeInfo.price_l + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_m = long.Parse(EditorGUILayout.TextArea(storeInfo.price_m + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_s = long.Parse(EditorGUILayout.TextArea(storeInfo.price_s + "", GUILayout.Width(100), GUILayout.Height(20)));
        switch ((StoreTypeEnum)storeInfo.type)
        {
            case StoreTypeEnum.ArenaGoods:
                storeInfo.store_goods_type = (int)(StoreForArenaGoodsTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForArenaGoodsTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                GUILayout.Label("奖杯1234：", GUILayout.Width(50), GUILayout.Height(20));
                storeInfo.trophy_elementary = long.Parse(EditorGUILayout.TextArea(storeInfo.trophy_elementary + "", GUILayout.Width(100), GUILayout.Height(20)));
                storeInfo.trophy_intermediate = long.Parse(EditorGUILayout.TextArea(storeInfo.trophy_intermediate + "", GUILayout.Width(100), GUILayout.Height(20)));
                storeInfo.trophy_advanced = long.Parse(EditorGUILayout.TextArea(storeInfo.trophy_advanced + "", GUILayout.Width(100), GUILayout.Height(20)));
                storeInfo.trophy_legendary = long.Parse(EditorGUILayout.TextArea(storeInfo.trophy_legendary + "", GUILayout.Width(100), GUILayout.Height(20)));
                break;
            case StoreTypeEnum.Guild:
                GUILayout.Label("公会勋章：", GUILayout.Width(50), GUILayout.Height(20));
                storeInfo.guild_coin = long.Parse(EditorGUILayout.TextArea(storeInfo.guild_coin + "", GUILayout.Width(100), GUILayout.Height(20)));
                storeInfo.store_goods_type = (int)(StoreForGuildGoodsTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForGuildGoodsTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                break;
            case StoreTypeEnum.Carpenter:
                storeInfo.store_goods_type = (int)(StoreForCarpenterTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForCarpenterTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
                {
                    GUILayout.Label("扩建等级", GUILayout.Width(50), GUILayout.Height(20));
                    storeInfo.mark = EditorGUILayout.TextArea(storeInfo.mark + "", GUILayout.Width(20), GUILayout.Height(20));
                    GUILayout.Label("w", GUILayout.Width(20), GUILayout.Height(20));
                    storeInfo.mark_x = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_x + "", GUILayout.Width(20), GUILayout.Height(20)));
                    GUILayout.Label("h", GUILayout.Width(20), GUILayout.Height(20));
                    storeInfo.mark_y = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_y + "", GUILayout.Width(20), GUILayout.Height(20)));
                }
                break;
            case StoreTypeEnum.Grocery:
                storeInfo.store_goods_type = (int)(StoreForGroceryTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForGroceryTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                break;
            case StoreTypeEnum.Dress:
                storeInfo.store_goods_type = (int)(StoreForDressTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForDressTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                break;
            case StoreTypeEnum.Pharmacy:
                storeInfo.store_goods_type = (int)(StoreForPharmacyTypeEnum)EditorGUILayout.EnumPopup("商品类型", (StoreForPharmacyTypeEnum)storeInfo.store_goods_type, GUILayout.Width(300), GUILayout.Height(20));
                break;
        }
        GUILayout.Label("备用名字", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.name_language = EditorGUILayout.TextArea(storeInfo.name_language + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("备用描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content_language = EditorGUILayout.TextArea(storeInfo.content_language + "", GUILayout.Width(100), GUILayout.Height(20));
    }

    private void GUIStoreItemForArenaInfo(StoreInfoBean storeInfo)
    {
        GUILayout.Label("竞赛等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = (int)(TrophyTypeEnum)EditorGUILayout.EnumPopup("职业", $"{storeInfo.mark_type}".GetEnum<TrophyTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20));
        if (storeInfo.pre_data.IsNull())
            storeInfo.pre_data = "Chef";
        storeInfo.pre_data = ((WorkerEnum)EditorGUILayout.EnumPopup("职业", $"{storeInfo.pre_data}".GetEnum<WorkerEnum>(), GUILayout.Width(300), GUILayout.Height(20))) + "";

        GUILayout.Label("报名费LMS：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.price_l = long.Parse(EditorGUILayout.TextArea(storeInfo.price_l + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_m = long.Parse(EditorGUILayout.TextArea(storeInfo.price_m + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_s = long.Parse(EditorGUILayout.TextArea(storeInfo.price_s + "", GUILayout.Width(100), GUILayout.Height(20)));
        GUILayout.Label("消耗时间(小时)", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark = EditorGUILayout.TextArea(storeInfo.mark + "", GUILayout.Width(100), GUILayout.Height(20));
        GUIPreForMiniGame(storeInfo);
        GUIReward(storeInfo);
    }

    private void GUIStoreItemForImprove(StoreInfoBean storeInfo)
    {
        GUILayout.Label("考试等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(100), GUILayout.Height(20)));
        GUILayout.Label("价格LMS：", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.price_l = long.Parse(EditorGUILayout.TextArea(storeInfo.price_l + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_m = long.Parse(EditorGUILayout.TextArea(storeInfo.price_m + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.price_s = long.Parse(EditorGUILayout.TextArea(storeInfo.price_s + "", GUILayout.Width(100), GUILayout.Height(20)));
        GUILayout.Label("消耗时间(小时)", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark = EditorGUILayout.TextArea(storeInfo.mark + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("名字", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.name_language = EditorGUILayout.TextArea(storeInfo.name_language + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content_language = EditorGUILayout.TextArea(storeInfo.content_language + "", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.pre_data_minigame = EditorUI.GUIListData<PreTypeForMiniGameEnum>("小游戏数据", storeInfo.pre_data_minigame);
    }

    private void GUIStoreItemForInnLevel(StoreInfoBean storeInfo)
    {
        GUILayout.Label("客栈等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(100), GUILayout.Height(20)));
        GUILayout.Label("名字", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.name_language = EditorGUILayout.TextArea(storeInfo.name_language + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content_language = EditorGUILayout.TextArea(storeInfo.content_language + "", GUILayout.Width(100), GUILayout.Height(20));
        GUIPre(storeInfo);
        GUIReward(storeInfo);
    }

    private void GUICreateItem()
    {
        GUILayout.Label("创建物品");
        GUILayout.BeginHorizontal();

        createItemType = EditorUI.GUIEnum<GeneralEnum>("物品类型", (int)createItemType, 200);
        createItemsInfo.items_type = (int)createItemType;

        EditorUI.GUIText("名字：", 50);
        createItemsInfo.name_language = EditorUI.GUIEditorText(createItemsInfo.name_language + "", 150);

        if (createItemType == GeneralEnum.Hat)
        {
            if (!createItemsInfo.name_language.Contains("-头"))
                createItemsInfo.name_language += "-头";
        }
        else if (createItemType == GeneralEnum.Clothes)
        {
            if (!createItemsInfo.name_language.Contains("-衣"))
                createItemsInfo.name_language += "-衣";
        }
        else if (createItemType == GeneralEnum.Shoes)
        {
            if (!createItemsInfo.name_language.Contains("-鞋"))
                createItemsInfo.name_language += "-鞋";
        }
        spriteCreateIcon = EditorGUILayout.ObjectField(new GUIContent("选择图片", ""), spriteCreateIcon, typeof(Sprite), true, GUILayout.Width(250)) as Sprite;
        if (spriteCreateIcon)
        {
            EditorUI.GUIText("icon_key：", 60);
            createItemsInfo.icon_key = EditorGUILayout.TextArea(spriteCreateIcon.name + "", GUILayout.Width(150), GUILayout.Height(20));
        }

        long autoId = createItemsInfo.items_type * 100000;
        if (createItemType == GeneralEnum.Hat || createItemType == GeneralEnum.Clothes || createItemType == GeneralEnum.Shoes)
        {
            if (spriteCreateIcon)
            {
                if (spriteCreateIcon.name.Contains("normal")) autoId += 0 * 10000;
                else if (spriteCreateIcon.name.Contains("special")) autoId += 1 * 10000;
                else if (spriteCreateIcon.name.Contains("work")) autoId += 3 * 10000;
                else if (spriteCreateIcon.name.Contains("team")) autoId += 4 * 10000;
                else if (spriteCreateIcon.name.Contains("anim")) autoId += 5 * 10000;
            }
        }
        else if (createItemType == GeneralEnum.Chef
            || createItemType == GeneralEnum.Waiter
            || createItemType == GeneralEnum.Accoutant
            || createItemType == GeneralEnum.Accost
            || createItemType == GeneralEnum.Beater)
        {
            if (spriteCreateIcon && spriteCreateIcon.name.Contains("special"))
                autoId += 1 * 10000;
        }
        if (createItemType == GeneralEnum.Menu)
        {
            GUILayout.Label("增加的菜谱ID：");
            createItemsInfo.add_id = long.Parse(EditorGUILayout.TextArea(createItemsInfo.add_id + "", GUILayout.Width(150), GUILayout.Height(20)));
            autoId += createItemsInfo.add_id;
            inputId = 0;
        }
        else
        {
            EditorUI.GUIText("输入ID：", 50);
            inputId = long.Parse(EditorGUILayout.TextArea(inputId + "", GUILayout.Width(100), GUILayout.Height(20)));
        }

        GUILayout.Label("物品ID：", GUILayout.Width(50), GUILayout.Height(20));
        EditorGUILayout.TextArea((inputId + autoId) + "", GUILayout.Width(100), GUILayout.Height(20));
        createItemsInfo.id = (int)(inputId + autoId);

        GUILayout.Label("描述：");
        createItemsInfo.content_language = EditorGUILayout.TextArea(createItemsInfo.content_language + "", GUILayout.Width(150), GUILayout.Height(20));

        GUILayout.Label("增加属性：");
        GUILayout.Label("命");
        createItemsInfo.add_life = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_life + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("厨");
        createItemsInfo.add_cook = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_cook + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("速");
        createItemsInfo.add_speed = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_speed + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("算");
        createItemsInfo.add_account = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_account + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("魅");
        createItemsInfo.add_charm = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_charm + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("武");
        createItemsInfo.add_force = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_force + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("运");
        createItemsInfo.add_lucky = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_lucky + "", GUILayout.Width(150), GUILayout.Height(20)));
        GUILayout.Label("忠");
        createItemsInfo.add_loyal = int.Parse(EditorGUILayout.TextArea(createItemsInfo.add_loyal + "", GUILayout.Width(150), GUILayout.Height(20)));

        GUILayout.EndHorizontal();
    }

    private string findIds = "";

    private void GUIFindItem()
    {
        GUILayout.Label("查询物品");
        GUILayout.BeginHorizontal();
        GUILayout.Label("物品ID：", GUILayout.Width(50), GUILayout.Height(20));
        findIds = EditorGUILayout.TextArea(findIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] idArray = findIds.SplitForArrayLong(',');
            listFindItem = gameItemsManager.GetItemsByIds(idArray);
        }
        if (GUILayout.Button("查询所有", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetAllItems();
        if (GUILayout.Button("查询礼物", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Gift);
        if (GUILayout.Button("查询读物", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Read);
        if (GUILayout.Button("查询书籍", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Book);
        if (GUILayout.Button("查询菜谱", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Menu);
        if (GUILayout.Button("查询技能书", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.SkillBook);
        if (GUILayout.Button("查询所有药", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetMedicineList();
        if (GUILayout.Button("查询厨师用道具", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Chef);
        if (GUILayout.Button("查询伙计用道具", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Waiter);
        if (GUILayout.Button("查询账房用道具", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accoutant);
        if (GUILayout.Button("查询接待用道具", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accost);
        if (GUILayout.Button("查询打手用道具", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Beater);
        if (GUILayout.Button("查询所有服装", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetClothesList();
        if (GUILayout.Button("查询其他", GUILayout.Width(100), GUILayout.Height(20)))
            listFindItem = gameItemsManager.GetOtherList();
        GUILayout.EndHorizontal();

        for (int i = 0; i < listFindItem.Count; i++)
        {
            ItemsInfoBean itemInfo = listFindItem[i];
            GUILayout.BeginHorizontal();
            GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));

            EditorUI.GUIText("物品ID：");
            itemInfo.id = int.Parse(EditorGUILayout.TextArea(itemInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
            EditorUI.GUIText("物品稀有度：");
            itemInfo.rarity = EditorUI.GUIEditorText(itemInfo.rarity, 20);

            EditorUI.GUIText("动画key：");
            itemInfo.anim_key = EditorUI.GUIEditorText(itemInfo.anim_key, 50);

            string path = "Assets/Texture/";
            switch ((GeneralEnum)itemInfo.items_type)
            {
                case GeneralEnum.Hat: path += "Character/Dress/Hat/"; break;
                case GeneralEnum.Clothes: path += "Character/Dress/Clothes/"; break;
                case GeneralEnum.Shoes: path += "Character/Dress/Shoes/"; break;
                case GeneralEnum.Mask: path += "Character/Dress/Mask/"; break;
                case GeneralEnum.Medicine: path += "Items/Medicine/"; break;
                case GeneralEnum.Chef: path += "Items/Chef/"; break;
                case GeneralEnum.Waiter: path += "Items/Waiter/"; break;
                case GeneralEnum.Accoutant: path += "Items/Accountant/"; break;
                case GeneralEnum.Accost: path += "Items/Accost/"; break;
                case GeneralEnum.Beater: path += "Items/Beater/"; break;
                case GeneralEnum.Book:
                case GeneralEnum.SkillBook:
                case GeneralEnum.Menu:
                case GeneralEnum.Read:
                case GeneralEnum.Gift: path += "Common/UI/"; break;
                default: path += "Items/"; break;
            }
            path += (itemInfo.icon_key + ".png");
            Texture2D iconTex = EditorGUIUtility.FindTexture(path);
            if (iconTex)
                GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
            EditorUI.GUIText("icon_key：");
            itemInfo.icon_key = EditorGUILayout.TextArea(itemInfo.icon_key + "", GUILayout.Width(150), GUILayout.Height(20));
            itemInfo.items_type = (int)EditorUI.GUIEnum<GeneralEnum>("物品类型", itemInfo.items_type);
            EditorUI.GUIText("名字：");
            itemInfo.name_language = EditorGUILayout.TextArea(itemInfo.name_language + "", GUILayout.Width(150), GUILayout.Height(20));
            EditorUI.GUIText("描述：");
            itemInfo.content_language = EditorGUILayout.TextArea(itemInfo.content_language + "", GUILayout.Width(150), GUILayout.Height(20));
            GeneralEnum itemType = (GeneralEnum)itemInfo.items_type;
            if (itemType != GeneralEnum.Menu && itemType != GeneralEnum.Medicine
                && itemType != GeneralEnum.SkillBook && itemType != GeneralEnum.Read)
            {
                EditorUI.GUIText("增加属性：");
                EditorUI.GUIText("|");
                EditorUI.GUIText("命");
                itemInfo.add_life = int.Parse(EditorGUILayout.TextArea(itemInfo.add_life + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("厨");
                itemInfo.add_cook = int.Parse(EditorGUILayout.TextArea(itemInfo.add_cook + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("速");
                itemInfo.add_speed = int.Parse(EditorGUILayout.TextArea(itemInfo.add_speed + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("算");
                itemInfo.add_account = int.Parse(EditorGUILayout.TextArea(itemInfo.add_account + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("魅");
                itemInfo.add_charm = int.Parse(EditorGUILayout.TextArea(itemInfo.add_charm + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("武");
                itemInfo.add_force = int.Parse(EditorGUILayout.TextArea(itemInfo.add_force + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("运");
                itemInfo.add_lucky = int.Parse(EditorGUILayout.TextArea(itemInfo.add_lucky + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("忠");
                itemInfo.add_loyal = int.Parse(EditorGUILayout.TextArea(itemInfo.add_loyal + "", GUILayout.Width(150), GUILayout.Height(20)));
                EditorUI.GUIText("|");
                EditorUI.GUIText("旋转角度");
                itemInfo.rotation_angle = EditorUI.GUIEditorText(itemInfo.rotation_angle);
            }
            if (itemType == GeneralEnum.Medicine)
            {
                itemInfo.effect = EditorUI.GUIListData<EffectTypeEnum>("效果", itemInfo.effect);
                itemInfo.effect_details = EditorUI.GUIListData<EffectDetailsEnum>("效果详情", itemInfo.effect_details);
            }
            else if (itemType == GeneralEnum.Menu)
            {
                EditorUI.GUIText("绑定菜谱ID：");
                itemInfo.add_id = long.Parse(EditorGUILayout.TextArea(itemInfo.add_id + "", GUILayout.Width(150), GUILayout.Height(20)));
            }
            else if (itemType == GeneralEnum.SkillBook)
            {
                EditorUI.GUIText("绑定技能ID：");
                itemInfo.add_id = long.Parse(EditorGUILayout.TextArea(itemInfo.add_id + "", GUILayout.Width(150), GUILayout.Height(20)));
            }
            else if (itemType == GeneralEnum.Read)
            {
                EditorUI.GUIText("绑定textlook markID：");
                itemInfo.add_id = long.Parse(EditorGUILayout.TextArea(itemInfo.add_id + "", GUILayout.Width(150), GUILayout.Height(20)));
            }
            GUILayout.EndHorizontal();
        }
    }

    private void GUIReward(StoreInfoBean storeInfo)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("奖励：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加奖励", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfo.reward_data += (RewardTypeEnum.AddItems.GetEnumName() + ":" + "1|");
        List<string> listRewardData = storeInfo.reward_data.SplitForListStr('|');
        storeInfo.reward_data = "";
        for (int i = 0; i < listRewardData.Count; i++)
        {
            string itemRewardData = listRewardData[i];
            if (itemRewardData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listRewardData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemRewardData = itemRewardData.SplitForListStr(':');
            listItemRewardData[0] = EditorGUILayout.EnumPopup("奖励类型", listItemRewardData[0].GetEnum<RewardTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20)).GetEnumName();
            listItemRewardData[1] = EditorGUILayout.TextArea(listItemRewardData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storeInfo.reward_data += (listItemRewardData[0] + ":" + listItemRewardData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }

    private void GUIPre(StoreInfoBean storeInfo)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfo.pre_data += ("|" + PreTypeEnum.PayMoneyL.GetEnumName() + ":" + "1|");
        List<string> listPreData = storeInfo.pre_data.SplitForListStr('|');
        storeInfo.pre_data = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (itemPreData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listPreData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemPreData = itemPreData.SplitForListStr(':');
            listItemPreData[0] = EditorGUILayout.EnumPopup("前置类型", listItemPreData[0].GetEnum<PreTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20)).GetEnumName();
            listItemPreData[1] = EditorGUILayout.TextArea(listItemPreData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storeInfo.pre_data += (listItemPreData[0] + ":" + listItemPreData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }

    private void GUIPreForMiniGame(StoreInfoBean storeInfo)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("小游戏前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
            storeInfo.pre_data_minigame += ("|" + PreTypeForMiniGameEnum.WinSurvivalTime.GetEnumName() + ":" + "1|");
        List<string> listPreData = storeInfo.pre_data_minigame.SplitForListStr('|');
        storeInfo.pre_data_minigame = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (itemPreData.IsNull()) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listPreData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemPreData = itemPreData.SplitForListStr(':');
            listItemPreData[0] = EditorGUILayout.EnumPopup("前置类型", listItemPreData[0].GetEnum<PreTypeForMiniGameEnum>(), GUILayout.Width(300), GUILayout.Height(20)).GetEnumName();
            listItemPreData[1] = EditorGUILayout.TextArea(listItemPreData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storeInfo.pre_data_minigame += (listItemPreData[0] + ":" + listItemPreData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }

    public static void GUIBuildItemCreate(BuildItemBean buildItem)
    {
        GUILayout.BeginVertical();
        GUILayout.Label("建造物品创建：[只读，请编辑Excel文件]", GUILayout.Width(300), GUILayout.Height(20));
        GUIBuildItem(buildItem);
        GUILayout.EndVertical();
    }

    public static void GUIBuildItemFind(List<BuildItemBean> listBuildItem, out List<BuildItemBean> outListBuildItem)
    {
        GUILayout.Label("建造物品查询：", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("查询所有"))
        {
            listBuildItem = new List<BuildItemBean>();
            var dic = BuildItemCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listBuildItem.Add(item.Value);
        }
        if (EditorUI.GUIButton("查询地板"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Floor);
        if (EditorUI.GUIButton("查询墙壁"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Wall);
        if (EditorUI.GUIButton("查询桌椅"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Table);
        if (EditorUI.GUIButton("查询灶台"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Stove);
        if (EditorUI.GUIButton("查询柜台"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Counter);
        if (EditorUI.GUIButton("查询装饰"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Decoration);
        if (EditorUI.GUIButton("查询正门"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Door);
        if (EditorUI.GUIButton("查询楼梯"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.Stairs);
        if (EditorUI.GUIButton("查询床-基础"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedBase);
        if (EditorUI.GUIButton("查询床-床栏"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedBar);
        if (EditorUI.GUIButton("查询床-床单"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedSheets);
        if (EditorUI.GUIButton("查询床-枕头"))
            listBuildItem = GetBuildItemByType(BuildItemTypeEnum.BedPillow);
        GUILayout.EndHorizontal();
        if (listBuildItem != null)
        {
            foreach (BuildItemBean itemData in listBuildItem)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUIBuildItem(itemData);
            }
        }
        outListBuildItem = listBuildItem;
    }

    private static List<BuildItemBean> GetBuildItemByType(BuildItemTypeEnum type)
    {
        List<BuildItemBean> result = new List<BuildItemBean>();
        var dic = BuildItemCfg.GetAllData();
        if (dic == null) return result;
        foreach (var item in dic)
            if (item.Value.build_type == (int)type)
                result.Add(item.Value);
        return result;
    }

    public static void GUIBuildItem(BuildItemBean buildItem)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("id：");
        buildItem.id = EditorUI.GUIEditorText(buildItem.id);
        buildItem.build_type = (int)EditorUI.GUIEnum<BuildItemTypeEnum>("类型：", buildItem.build_type);
        EditorUI.GUIText("模型ID：");
        buildItem.model_name = EditorUI.GUIEditorText(buildItem.model_name);
        EditorUI.GUIText(" 图标：", 50);
        buildItem.icon_key = EditorUI.GUIEditorText(buildItem.icon_key, 200);
        string picPath = "";
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor: picPath = "Assets/Texture/Tile/Floor/"; break;
            case BuildItemTypeEnum.Wall: picPath = "Assets/Texture/Tile/Wall/"; break;
            case BuildItemTypeEnum.Table: picPath = "Assets/Texture/InnBuild/TableAndChair/"; break;
            case BuildItemTypeEnum.Stove: picPath = "Assets/Texture/InnBuild/Stove/"; break;
            case BuildItemTypeEnum.Counter: picPath = "Assets/Texture/InnBuild/Counter/"; break;
            case BuildItemTypeEnum.Decoration: picPath = "Assets/Texture/InnBuild/Decoration/"; break;
            case BuildItemTypeEnum.Door: picPath = "Assets/Texture/InnBuild/Door/"; break;
            case BuildItemTypeEnum.BedBar:
            case BuildItemTypeEnum.BedBase:
            case BuildItemTypeEnum.BedPillow:
            case BuildItemTypeEnum.BedSheets: picPath = "Assets/Texture/InnBuild/Bed/"; break;
        }
        EditorUI.GUIPic(picPath + "/" + buildItem.icon_key);

        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Table:
            case BuildItemTypeEnum.Counter:
            case BuildItemTypeEnum.Stove:
            case BuildItemTypeEnum.Door:
            case BuildItemTypeEnum.Decoration:
                EditorUI.GUIText("icon_list");
                buildItem.icon_list = EditorUI.GUIEditorText(buildItem.icon_list, 300);
                break;
            case BuildItemTypeEnum.Floor:
            case BuildItemTypeEnum.Wall:
                EditorUI.GUIText("tile名字：");
                buildItem.tile_name = EditorUI.GUIEditorText(buildItem.tile_name, 200);
                break;
        }
        EditorUI.GUIText("美观：", 50);
        buildItem.aesthetics = EditorUI.GUIEditorText(buildItem.aesthetics);
        EditorUI.GUIText("名称：", 50);
        buildItem.name = EditorUI.GUIEditorText(buildItem.name, 200);
        EditorUI.GUIText("形容：", 50);
        buildItem.content = EditorUI.GUIEditorText(buildItem.content, 300);
        GUILayout.EndHorizontal();
    }

    public static void GUIMenuCreate(MenuInfoBean menuInfo)
    {
        EditorUI.GUIText("菜谱创建 [只读，请编辑Excel文件]");
        GUIMenuItem(menuInfo);
        GUILayout.Space(20);
    }

    public static void GUIMenuFind(string findIds, List<MenuInfoBean> listData, out string outFindIds, out List<MenuInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("查询IDs");
        findIds = EditorUI.GUIEditorText(findIds);
        if (EditorUI.GUIButton("查询指定ID"))
        {
            long[] ids = findIds.SplitForArrayLong(',');
            listData = new List<MenuInfoBean>();
            foreach (long id in ids)
            {
                MenuInfoBean item = MenuInfoCfg.GetItemData(id);
                if (item != null) listData.Add(item);
            }
        }
        if (EditorUI.GUIButton("查询所有菜单"))
        {
            listData = new List<MenuInfoBean>();
            var dic = MenuInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listData.Add(item.Value);
        }
        GUILayout.EndHorizontal();
        if (!listData.IsNull())
        {
            for (int i = 0; i < listData.Count; i++)
            {
                MenuInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUIMenuItem(itemData);
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    public static void GUIMenuItem(MenuInfoBean menuInfo)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("ID:");
        menuInfo.id = EditorUI.GUIEditorText(menuInfo.id);

        EditorUI.GUIText("名称:");
        menuInfo.name = EditorUI.GUIEditorText(menuInfo.name, 150);
        EditorUI.GUIText("内容:");
        menuInfo.content = EditorUI.GUIEditorText(menuInfo.content, 300);

        EditorUI.GUIText("烹饪时间:");
        menuInfo.cook_time = EditorUI.GUIEditorText(menuInfo.cook_time);

        EditorUI.GUIText("当前利润:");
        long getmoney = menuInfo.price_s - (
             menuInfo.ing_oilsalt * 5 + menuInfo.ing_meat * 10 +
             menuInfo.ing_riverfresh * 10 + menuInfo.ing_seafood * 50 +
             menuInfo.ing_vegetables * 5 + menuInfo.ing_melonfruit * 5 +
             menuInfo.ing_waterwine * 10 + menuInfo.ing_flour * 5);
        EditorUI.GUIText(getmoney + "");
        EditorUI.GUIText("输入利润:", 50);
        long setGetmoney = EditorUI.GUIEditorText(getmoney, 50);
        if (getmoney != setGetmoney)
        {
            menuInfo.price_s = (int)setGetmoney +
             menuInfo.ing_oilsalt * 5 + menuInfo.ing_meat * 10 +
             menuInfo.ing_riverfresh * 10 + menuInfo.ing_seafood * 50 +
             menuInfo.ing_vegetables * 5 + menuInfo.ing_melonfruit * 5 +
             menuInfo.ing_waterwine * 10 + menuInfo.ing_flour * 5;
        }
        EditorUI.GUIText("利润:");
        EditorUI.GUIText("价格LMS:");
        menuInfo.price_l = EditorUI.GUIEditorText(menuInfo.price_l, 50);
        menuInfo.price_m = EditorUI.GUIEditorText(menuInfo.price_m, 50);
        menuInfo.price_s = EditorUI.GUIEditorText(menuInfo.price_s, 50);
        EditorUI.GUIText("动画名称:");
        menuInfo.anim_key = EditorUI.GUIEditorText(menuInfo.anim_key, 150);
        EditorUI.GUIText("图片名称:");
        menuInfo.icon_key = EditorUI.GUIEditorText(menuInfo.icon_key, 150);
        string menuPicPath = "Assets/Texture/Food/";
        EditorUI.GUIPic(menuPicPath + "/" + menuInfo.icon_key);
        EditorUI.GUIText("稀有度:");
        menuInfo.rarity = EditorUI.GUIEditorText(menuInfo.rarity);
        EditorUI.GUIText("材料 油盐:");
        menuInfo.ing_oilsalt = EditorUI.GUIEditorText(menuInfo.ing_oilsalt, 50);
        EditorUI.GUIText("材料 鲜肉:");
        menuInfo.ing_meat = EditorUI.GUIEditorText(menuInfo.ing_meat, 50);
        EditorUI.GUIText("材料 河鲜:");
        menuInfo.ing_riverfresh = EditorUI.GUIEditorText(menuInfo.ing_riverfresh, 50);
        EditorUI.GUIText("材料 海鲜:");
        menuInfo.ing_seafood = EditorUI.GUIEditorText(menuInfo.ing_seafood, 50);
        EditorUI.GUIText("材料 蔬菜:");
        menuInfo.ing_vegetables = EditorUI.GUIEditorText(menuInfo.ing_vegetables, 50);
        EditorUI.GUIText("材料 瓜果:");
        menuInfo.ing_melonfruit = EditorUI.GUIEditorText(menuInfo.ing_melonfruit, 50);
        EditorUI.GUIText("材料 酒水:");
        menuInfo.ing_waterwine = EditorUI.GUIEditorText(menuInfo.ing_waterwine, 50);
        EditorUI.GUIText("材料 面粉:");
        menuInfo.ing_flour = EditorUI.GUIEditorText(menuInfo.ing_flour, 50);

        GUILayout.EndHorizontal();
    }

    public static void GUISkillCreate(SkillInfoBean skillInfo)
    {
        EditorUI.GUIText("技能创建 [只读，请编辑Excel文件]");
        GUISkillItem(skillInfo);
        GUILayout.Space(20);
    }

    public static void GUISkillFind(string findIds, List<SkillInfoBean> listData, out string outFindIds, out List<SkillInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("查询IDs");
        findIds = EditorUI.GUIEditorText(findIds);
        if (EditorUI.GUIButton("查询指定ID"))
        {
            long[] ids = findIds.SplitForArrayLong(',');
            listData = new List<SkillInfoBean>();
            foreach (long id in ids)
            {
                SkillInfoBean item = SkillInfoCfg.GetItemData(id);
                if (item != null) listData.Add(item);
            }
        }
        if (EditorUI.GUIButton("查询所有技能"))
        {
            listData = new List<SkillInfoBean>();
            var dic = SkillInfoCfg.GetAllData();
            if (dic != null) foreach (var item in dic) listData.Add(item.Value);
        }
        GUILayout.EndHorizontal();
        if (!listData.IsNull())
        {
            for (int i = 0; i < listData.Count; i++)
            {
                SkillInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("[只读]", GUILayout.Width(50), GUILayout.Height(20));
                GUISkillItem(itemData);
                GUILayout.EndHorizontal();
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    public static void GUISkillItem(SkillInfoBean skillInfo)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("ID:");
        skillInfo.id = EditorUI.GUIEditorText(skillInfo.id);
        EditorUI.GUIText("名称");
        skillInfo.name = EditorUI.GUIEditorText(skillInfo.name);
        EditorUI.GUIText("介绍");
        skillInfo.content = EditorUI.GUIEditorText(skillInfo.content, 200);
        EditorUI.GUIText("图片名称:");
        skillInfo.icon_key = EditorUI.GUIEditorText(skillInfo.icon_key, 150);
        string menuPicPath = "Assets/Texture/Common/UI/";
        EditorUI.GUIPic(menuPicPath + "/" + skillInfo.icon_key);
        EditorUI.GUIText("使用数量");
        skillInfo.use_number = EditorUI.GUIEditorText(skillInfo.use_number);
        skillInfo.effect = EditorUI.GUIListData<EffectTypeEnum>("效果", skillInfo.effect);
        skillInfo.effect_details = EditorUI.GUIListData<EffectDetailsEnum>("效果详情", skillInfo.effect_details);
        skillInfo.pre_data = EditorUI.GUIListData<PreTypeEnum>("解锁条件", skillInfo.pre_data);
        GUILayout.EndHorizontal();
    }
}
