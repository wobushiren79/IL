﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemCreateWindowsEditor : EditorWindow
{
    GameItemsManager gameItemsManager;
    StoreInfoManager storeInfoManager;
    InnBuildManager innBuildManager;
    ItemsInfoService itemsInfoService;
    StoreInfoService storeInfoService;
    BuildItemService buildItemService;
    MenuInfoService menuInfoService;
    SkillInfoService skillInfoService;
    AchievementInfoService achievementInfoService;
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
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        innBuildManager.buildDataController.GetAllBuildItemsData();
        itemsInfoService = new ItemsInfoService();
        storeInfoService = new StoreInfoService();
        buildItemService = new BuildItemService();
        menuInfoService = new MenuInfoService();
        skillInfoService = new SkillInfoService();
        achievementInfoService = new AchievementInfoService();
    }

    public void RefreshData()
    {
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
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
    //建造物品创建数据
    public BuildItemBean createBuildItem = new BuildItemBean();

    public AchievementInfoBean createAchInfo = new AchievementInfoBean();
    long inputId = 0;

    //创建菜单数据
    public MenuInfoBean createMenuInfo = new MenuInfoBean();
    public string menuFindIds = "";

    //技能创建数据
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
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
        {
            RefreshData();
        }

        GUICreateItem();
        GUIFindItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUIBuildItemCreate(buildItemService, createBuildItem);
        GUIBuildItemFind(buildItemService, listFindBuildItem, out listFindBuildItem);
        GUILayout.Label("------------------------------------------------------------------------------------------------");

        GUICreateStoreItem();
        GUIFindStoreItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUICreateAchItem();
        GUIFindAchItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUIMenuCreate(menuInfoService, createMenuInfo);
        GUIMenuFind(menuInfoService, menuFindIds, listFindMenuItem, out menuFindIds, out listFindMenuItem);
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUISkillCreate(skillInfoService, createSkillInfo);
        GUISkillFind(skillInfoService, skillFindIds, listFindSkillItem, out skillFindIds, out listFindSkillItem);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();


    }

    /// <summary>
    /// 创建成就UI
    /// </summary>
    private void GUICreateAchItem()
    {
        GUILayout.Label("创建成就");
        GUIAchItem(createAchInfo, true);
    }

    private string findAchIds = "";

    /// <summary>
    /// 成就查询UI
    /// </summary>
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
            listFindAchItem = achievementInfoService.QueryAllData();
        }
        if (GUILayout.Button("查询通用成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Normal);
        }
        if (GUILayout.Button("查询厨师成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Chef);
        }
        if (GUILayout.Button("查询伙计成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Waiter);
        }
        if (GUILayout.Button("查询账房成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Account);
        }
        if (GUILayout.Button("查询接待成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Accost);
        }
        if (GUILayout.Button("查询打手成就", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindAchItem = achievementInfoService.QueryDataByType((int)AchievementTypeEnum.Beater);
        }
        GUILayout.EndHorizontal();
        if (listFindAchItem == null)
            return;
        foreach (AchievementInfoBean itemAchInfo in listFindAchItem)
        {
            GUIAchItem(itemAchInfo, false);
        }
    }

    /// <summary>
    /// 成就UI item
    /// </summary>
    /// <param name="achievementInfo"></param>
    /// <param name="isCreate"></param>
    private void GUIAchItem(AchievementInfoBean achievementInfo, bool isCreate)
    {
        EditorGUILayout.BeginHorizontal();
        if (isCreate)
        {
            if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
            {
                achievementInfoService.InsertData(achievementInfo);
            }
        }
        else
        {
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                achievementInfoService.DeleteDataById(achievementInfo.id);
                listFindAchItem.Remove(achievementInfo);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                achievementInfoService.Update(achievementInfo);
            }
        }

        GUILayout.Label("成就ID：", GUILayout.Width(50), GUILayout.Height(20));
        achievementInfo.id = int.Parse(EditorGUILayout.TextArea(achievementInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        achievementInfo.ach_id = achievementInfo.id;
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
        achievementInfo.name = EditorGUILayout.TextArea(achievementInfo.name + "", GUILayout.Width(150), GUILayout.Height(20));
        GUILayout.Label("内容：", GUILayout.Width(100), GUILayout.Height(20));
        achievementInfo.content = EditorGUILayout.TextArea(achievementInfo.content + "", GUILayout.Width(150), GUILayout.Height(20));
        EditorUI.GUIText("前置成就:");
        achievementInfo.pre_ach_ids = EditorUI.GUIEditorText(achievementInfo.pre_ach_ids);
        //前置相关
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
            if (itemPreData.IsNull())
            {
                continue;
            }
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

        //奖励相关
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
            if (itemRewardData.IsNull())
            {
                continue;
            }
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


        if (!isCreate)
        {
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                achievementInfoService.Update(achievementInfo);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
    }

    /// <summary>
    /// 创建商品UI
    /// </summary>
    private void GUICreateStoreItem()
    {
        GUILayout.Label("创建商品");
        GUIStoreItem(createStoreInfo, true);
    }

    private string findStoreIds = "";

    /// <summary>
    /// 查询商品UI
    /// </summary>
    private void GUIFindStoreItem()
    {
        GUILayout.Label("查询物品");
        GUILayout.BeginHorizontal();
        GUILayout.Label("商品ID：", GUILayout.Width(50), GUILayout.Height(20));
        findStoreIds = EditorGUILayout.TextArea(findStoreIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {

        }
        if (GUILayout.Button("查询百宝斋商品", GUILayout.Width(100), GUILayout.Height(20)))
        {

            storeInfoManager.GetStoreInfoForGrocery((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询绸缎庄商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForDress((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询建造商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForCarpenter((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询药店商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForPharmacy((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询公会商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildGoods((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询职业升级", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildImprove((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询客栈升级", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildInnLevel((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询斗技场", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForArenaInfo((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询斗技场商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForArenaGoods((listData) => { listFindStoreItem = listData; });
        }
        if (GUILayout.Button("查询床商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForCarpenterBed((listData) => { listFindStoreItem = listData; });
        }
        GUILayout.EndHorizontal();
        if (listFindStoreItem == null)
            return;
        foreach (StoreInfoBean itemStoreInfo in listFindStoreItem)
        {
            GUIStoreItem(itemStoreInfo, false);
        }
    }

    /// <summary>
    /// 商品UI
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUIStoreItem(StoreInfoBean storeInfo, bool isCreate)
    {
        EditorGUILayout.BeginHorizontal();
        if (isCreate)
        {
            if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
            {
                storeInfoService.InsertData(storeInfo);
            }
        }
        else
        {
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                storeInfoService.DeleteDataById(storeInfo.id);
                listFindStoreItem.Remove(storeInfo);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                storeInfoService.Update(storeInfo);
            }
        }

        GUILayout.Label("商品ID：", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.id = int.Parse(EditorGUILayout.TextArea(storeInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        storeInfo.store_id = storeInfo.id;
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

        if (!isCreate)
        {
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                storeInfoService.Update(storeInfo);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
    }

    private void GUIStoreItemForBuild()
    {

    }

    /// <summary>
    /// 商品
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUIStoreItemForGoods(StoreInfoBean storeInfo)
    {
        //if (storeInfo.mark.IsNull())
        //{
        //    storeInfo.mark = "1";
        //}
        //storeInfo.mark = (int)(GeneralEnum)EditorGUILayout.EnumPopup("商品类型", (GeneralEnum)int.Parse(storeInfo.mark), GUILayout.Width(300), GUILayout.Height(20))+"";
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
        storeInfo.name = EditorGUILayout.TextArea(storeInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("备用描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content = EditorGUILayout.TextArea(storeInfo.content + "", GUILayout.Width(100), GUILayout.Height(20));
    }

    private void GUIStoreItemForArenaInfo(StoreInfoBean storeInfo)
    {
        GUILayout.Label("竞赛等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = (int)(TrophyTypeEnum)EditorGUILayout.EnumPopup("职业", $"{storeInfo.mark_type}".GetEnum<TrophyTypeEnum>(), GUILayout.Width(300), GUILayout.Height(20));
        if (storeInfo.pre_data.IsNull())
        {
            storeInfo.pre_data = "Chef";
        }
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
        storeInfo.name = EditorGUILayout.TextArea(storeInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content = EditorGUILayout.TextArea(storeInfo.content + "", GUILayout.Width(100), GUILayout.Height(20));

        storeInfo.pre_data_minigame = EditorUI.GUIListData<PreTypeForMiniGameEnum>("小游戏数据", storeInfo.pre_data_minigame);
    }

    private void GUIStoreItemForInnLevel(StoreInfoBean storeInfo)
    {
        GUILayout.Label("客栈等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(100), GUILayout.Height(20)));
        GUILayout.Label("名字", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.name = EditorGUILayout.TextArea(storeInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("描述", GUILayout.Width(50), GUILayout.Height(20));
        storeInfo.content = EditorGUILayout.TextArea(storeInfo.content + "", GUILayout.Width(100), GUILayout.Height(20));
        //前置
        GUIPre(storeInfo);
        //奖励相关
        GUIReward(storeInfo);
    }

    /// <summary>
    /// 创建Item的UI
    /// </summary>
    private void GUICreateItem()
    {
        GUILayout.Label("创建物品");
        GUILayout.BeginHorizontal();

        createItemType = EditorUI.GUIEnum<GeneralEnum>("物品类型", (int)createItemType, 200);
        createItemsInfo.items_type = (int)createItemType;

        EditorUI.GUIText("名字：", 50);
        createItemsInfo.name = EditorUI.GUIEditorText(createItemsInfo.name + "", 150);

        if (createItemType == GeneralEnum.Hat)
        {
            if (!createItemsInfo.name.Contains("-头"))
            {
                createItemsInfo.name += "-头";
            }
        }
        else if (createItemType == GeneralEnum.Clothes)
        {
            if (!createItemsInfo.name.Contains("-衣"))
            {
                createItemsInfo.name += "-衣";
            }
        }
        else if (createItemType == GeneralEnum.Shoes)
        {
            if (!createItemsInfo.name.Contains("-鞋"))
            {
                createItemsInfo.name += "-鞋";
            }
        }
        spriteCreateIcon = EditorGUILayout.ObjectField(new GUIContent("选择图片", ""), spriteCreateIcon, typeof(Sprite), true, GUILayout.Width(250)) as Sprite;
        if (spriteCreateIcon)
        {
            EditorUI.GUIText("icon_key：", 60);
            createItemsInfo.icon_key = EditorGUILayout.TextArea(spriteCreateIcon.name + "", GUILayout.Width(150), GUILayout.Height(20));
        }

        //设置ID
        long autoId = createItemsInfo.items_type * 100000;
        if (createItemType == GeneralEnum.Hat || createItemType == GeneralEnum.Clothes || createItemType == GeneralEnum.Shoes)
        {
            if (spriteCreateIcon)
            {
                if (spriteCreateIcon.name.Contains("normal"))
                {
                    autoId += 0 * 10000;
                }
                else if (spriteCreateIcon.name.Contains("special"))
                {
                    autoId += 1 * 10000;
                }
                else if (spriteCreateIcon.name.Contains("work"))
                {
                    autoId += 3 * 10000;
                }
                else if (spriteCreateIcon.name.Contains("team"))
                {
                    autoId += 4 * 10000;
                }
                else if (spriteCreateIcon.name.Contains("anim"))
                {
                    autoId += 5 * 10000;
                }
            }
        }
        else if (createItemType == GeneralEnum.Chef
          || createItemType == GeneralEnum.Waiter
          || createItemType == GeneralEnum.Accoutant
          || createItemType == GeneralEnum.Accost
          || createItemType == GeneralEnum.Beater)
        {
            if (spriteCreateIcon)
            {
                if (spriteCreateIcon.name.Contains("special"))
                {
                    autoId += 1 * 10000;
                }
            }
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
        createItemsInfo.items_id = createItemsInfo.id;

        GUILayout.Label("描述：");
        createItemsInfo.content = EditorGUILayout.TextArea(createItemsInfo.content + "", GUILayout.Width(150), GUILayout.Height(20));

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
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            createItemsInfo.valid = 1;
            itemsInfoService.InsertData(createItemsInfo);
        }
    }

    private string findIds = "";
    /// <summary>
    /// 查询Item的UI
    /// </summary>
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
        {
            listFindItem = gameItemsManager.GetAllItems();
        }
        if (GUILayout.Button("查询礼物", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Gift);
        }
        if (GUILayout.Button("查询读物", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Read);
        }
        if (GUILayout.Button("查询书籍", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Book);
        }
        if (GUILayout.Button("查询菜谱", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Menu);
        }
        if (GUILayout.Button("查询技能书", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.SkillBook);
        }
        if (GUILayout.Button("查询所有药", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetMedicineList();
        }
        if (GUILayout.Button("查询厨师用道具", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Chef);
        }
        if (GUILayout.Button("查询伙计用道具", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Waiter);
        }
        if (GUILayout.Button("查询账房用道具", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accoutant);
        }
        if (GUILayout.Button("查询接待用道具", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Accost);
        }
        if (GUILayout.Button("查询打手用道具", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetItemsListByType(GeneralEnum.Beater);
        }
        if (GUILayout.Button("查询所有服装", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetClothesList();
        }
        if (GUILayout.Button("查询其他", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetOtherList();
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < listFindItem.Count; i++)
        {
            ItemsInfoBean itemInfo = listFindItem[i];
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("保存", GUILayout.Width(100), GUILayout.Height(20)))
            {
                itemsInfoService.UpdateData(itemInfo);
                LogUtil.Log("保存成功");
            }
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                itemsInfoService.DeleteData(itemInfo);
                listFindItem.Remove(itemInfo);
                LogUtil.Log("删除成功");
            }

            EditorUI.GUIText("物品ID：");
            itemInfo.id = int.Parse(EditorGUILayout.TextArea(itemInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
            EditorUI.GUIText("物品稀有度：");
            itemInfo.rarity = EditorUI.GUIEditorText(itemInfo.rarity, 20);

            EditorUI.GUIText("动画key：");
            itemInfo.anim_key = EditorUI.GUIEditorText(itemInfo.anim_key, 50);

            string path = "Assets/Texture/";
            switch ((GeneralEnum)itemInfo.items_type)
            {
                case GeneralEnum.Hat:
                    path += "Character/Dress/Hat/";
                    break;
                case GeneralEnum.Clothes:
                    path += "Character/Dress/Clothes/";
                    break;
                case GeneralEnum.Shoes:
                    path += "Character/Dress/Shoes/";
                    break;
                case GeneralEnum.Mask:
                    path += "Character/Dress/Mask/";
                    break;
                case GeneralEnum.Medicine:
                    path += "Items/Medicine/";
                    break;
                case GeneralEnum.Chef:
                    path += "Items/Chef/";
                    break;
                case GeneralEnum.Waiter:
                    path += "Items/Waiter/";
                    break;
                case GeneralEnum.Accoutant:
                    path += "Items/Accountant/";
                    break;
                case GeneralEnum.Accost:
                    path += "Items/Accost/";
                    break;
                case GeneralEnum.Beater:
                    path += "Items/Beater/";
                    break;
                case GeneralEnum.Book:
                case GeneralEnum.SkillBook:
                case GeneralEnum.Menu:
                case GeneralEnum.Read:
                case GeneralEnum.Gift:
                    path += "Common/UI/";

                    break;
                default:
                    path += "Items/";
                    break;

            }
            path += (itemInfo.icon_key + ".png");
            Texture2D iconTex = EditorGUIUtility.FindTexture(path);
            if (iconTex)
            {
                GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
                //EditorGUI.DrawPreviewTexture(new Rect(0, 0, 100, 100), iconTex);
                //spIcon = EditorGUILayout.ObjectField(new GUIContent("图标", ""), spIcon, typeof(Sprite), true) as Sprite;
            }
            EditorUI.GUIText("icon_key：");
            itemInfo.icon_key = EditorGUILayout.TextArea(itemInfo.icon_key + "", GUILayout.Width(150), GUILayout.Height(20));
            itemInfo.items_type = (int)EditorUI.GUIEnum<GeneralEnum>("物品类型", itemInfo.items_type);
            EditorUI.GUIText("名字：");
            itemInfo.name = EditorGUILayout.TextArea(itemInfo.name + "", GUILayout.Width(150), GUILayout.Height(20));
            EditorUI.GUIText("描述：");
            itemInfo.content = EditorGUILayout.TextArea(itemInfo.content + "", GUILayout.Width(150), GUILayout.Height(20));
            GeneralEnum itemType = (GeneralEnum)itemInfo.items_type;
            if (itemType != GeneralEnum.Menu
                && itemType != GeneralEnum.Medicine
                && itemType != GeneralEnum.SkillBook
                 && itemType != GeneralEnum.Read)
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

    /// <summary>
    /// 奖励UI
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUIReward(StoreInfoBean storeInfo)
    {
        //奖励相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("奖励：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加奖励", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfo.reward_data += (RewardTypeEnum.AddItems.GetEnumName() + ":" + "1|");
        }
        List<string> listRewardData = storeInfo.reward_data.SplitForListStr('|');
        storeInfo.reward_data = "";
        for (int i = 0; i < listRewardData.Count; i++)
        {
            string itemRewardData = listRewardData[i];
            if (itemRewardData.IsNull())
            {
                continue;
            }
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

    /// <summary>
    /// 前置条件UI
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUIPre(StoreInfoBean storeInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfo.pre_data += ("|" + PreTypeEnum.PayMoneyL.GetEnumName() + ":" + "1|");
        }
        List<string> listPreData = storeInfo.pre_data.SplitForListStr('|');
        storeInfo.pre_data = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (itemPreData.IsNull())
            {
                continue;
            }
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

    /// <summary>
    /// 迷你游戏前置条件
    /// </summary>
    /// <param name="storeInfo"></param>
    private void GUIPreForMiniGame(StoreInfoBean storeInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("小游戏前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfo.pre_data_minigame += ("|" + PreTypeForMiniGameEnum.WinSurvivalTime.GetEnumName() + ":" + "1|");
        }
        List<string> listPreData = storeInfo.pre_data_minigame.SplitForListStr('|');
        storeInfo.pre_data_minigame = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (itemPreData.IsNull())
            {
                continue;
            }
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


    /// <summary>
    /// 建造物品创建
    /// </summary>
    /// <param name="buildItemService"></param>
    /// <param name="buildItem"></param>
    public static void GUIBuildItemCreate(BuildItemService buildItemService, BuildItemBean buildItem)
    {
        GUILayout.BeginVertical();
        GUILayout.Label("建造物品创建：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("创建", GUILayout.Width(120), GUILayout.Height(20)))
        {
            buildItem.valid = 1;
            buildItemService.InsertData(buildItem);
        }
        GUIBuildItem(buildItem);
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 建造物品查询
    /// </summary>
    /// <param name="buildItemService"></param>
    /// <param name="listBuildItem"></param>
    /// <param name="outListBuildItem"></param>
    public static void GUIBuildItemFind(BuildItemService buildItemService,
        List<BuildItemBean> listBuildItem,
        out List<BuildItemBean> outListBuildItem)
    {
        GUILayout.Label("建造物品查询：", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("查询所有"))
        {
            listBuildItem = buildItemService.QueryAllData();
        }
        if (EditorUI.GUIButton("查询地板"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Floor);
        }
        if (EditorUI.GUIButton("查询墙壁"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Wall);
        }
        if (EditorUI.GUIButton("查询桌椅"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Table);
        }
        if (EditorUI.GUIButton("查询灶台"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Stove);
        }
        if (EditorUI.GUIButton("查询柜台"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Counter);
        }
        if (EditorUI.GUIButton("查询装饰"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Decoration);
        }
        if (EditorUI.GUIButton("查询正门"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Door);
        }
        if (EditorUI.GUIButton("查询楼梯"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Stairs);
        }
        if (EditorUI.GUIButton("查询床-基础"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.BedBase);
        }
        if (EditorUI.GUIButton("查询床-床栏"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.BedBar);
        }
        if (EditorUI.GUIButton("查询床-床单"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.BedSheets);
        }
        if (EditorUI.GUIButton("查询床-枕头"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.BedPillow);
        }
        GUILayout.EndHorizontal();
        if (listBuildItem != null)
        {
            BuildItemBean removeData = null;
            foreach (BuildItemBean itemData in listBuildItem)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                if (EditorUI.GUIButton("更新"))
                {
                    buildItemService.Update(itemData);
                }
                if (EditorUI.GUIButton("删除"))
                {
                    if (buildItemService.DeleteData(itemData.id))
                    {
                        removeData = itemData;
                    };
                }
                GUILayout.EndHorizontal();
                GUIBuildItem(itemData);
            }
            if (removeData != null)
            {
                listBuildItem.Remove(removeData);
            }
        }
        outListBuildItem = listBuildItem;
    }

    /// <summary>
    /// 建造物品展示
    /// </summary>
    /// <param name="buildItem"></param>
    public static void GUIBuildItem(BuildItemBean buildItem)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("id：");
        buildItem.id = EditorUI.GUIEditorText(buildItem.id);
        buildItem.build_id = buildItem.id;
        buildItem.build_type = (int)EditorUI.GUIEnum<BuildItemTypeEnum>("类型：", buildItem.build_type);
        EditorUI.GUIText("模型ID：");
        buildItem.model_name = EditorUI.GUIEditorText(buildItem.model_name);
        EditorUI.GUIText(" 图标：", 50);
        buildItem.icon_key = EditorUI.GUIEditorText(buildItem.icon_key, 200);
        string picPath = "";
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor:
                picPath = "Assets/Texture/Tile/Floor/";
                break;
            case BuildItemTypeEnum.Wall:
                picPath = "Assets/Texture/Tile/Wall/";
                break;
            case BuildItemTypeEnum.Table:
                picPath = "Assets/Texture/InnBuild/TableAndChair/";
                break;
            case BuildItemTypeEnum.Stove:
                picPath = "Assets/Texture/InnBuild/Stove/";
                break;
            case BuildItemTypeEnum.Counter:
                picPath = "Assets/Texture/InnBuild/Counter/";
                break;
            case BuildItemTypeEnum.Decoration:
                picPath = "Assets/Texture/InnBuild/Decoration/";
                break;
            case BuildItemTypeEnum.Door:
                picPath = "Assets/Texture/InnBuild/Door/";
                break;
            case BuildItemTypeEnum.BedBar:
            case BuildItemTypeEnum.BedBase:
            case BuildItemTypeEnum.BedPillow:
            case BuildItemTypeEnum.BedSheets:
                picPath = "Assets/Texture/InnBuild/Bed/";
                break;
            default:
                break;
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
            default:
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

    /// <summary>
    /// 创建菜单
    /// </summary>
    public static void GUIMenuCreate(MenuInfoService menuInfoService, MenuInfoBean menuInfo)
    {
        EditorUI.GUIText("菜谱创建");
        if (EditorUI.GUIButton("创建"))
        {
            menuInfo.valid = 1;
            menuInfoService.InsertData(menuInfo);
        }
        GUIMenuItem(menuInfo);
        GUILayout.Space(20);
    }

    /// <summary>
    /// 菜单查询
    /// </summary>
    /// <param name="menuInfoService"></param>
    /// <param name="listData"></param>
    public static void GUIMenuFind(MenuInfoService menuInfoService, string findIds, List<MenuInfoBean> listData, out string outFindIds, out List<MenuInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("查询IDs");
        findIds = EditorUI.GUIEditorText(findIds);
        if (EditorUI.GUIButton("查询指定ID"))
        {
            long[] ids = findIds.SplitForArrayLong(',');
            listData = menuInfoService.QueryDataByIds(ids);
        }
        if (EditorUI.GUIButton("查询所有菜单"))
        {
            listData = menuInfoService.QueryAllData();
        }
        GUILayout.EndHorizontal();
        if (!listData.IsNull())
        {

            for (int i = 0; i < listData.Count; i++)
            {
                MenuInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (EditorUI.GUIButton("更新"))
                {
                    menuInfoService.Update(itemData);
                }
                if (EditorUI.GUIButton("删除"))
                {
                    menuInfoService.DeleteData(itemData.id);
                }
                GUILayout.EndHorizontal();
                GUIMenuItem(itemData);
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    /// <summary>
    /// 菜单展示
    /// </summary>
    public static void GUIMenuItem(MenuInfoBean menuInfo)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("ID:");
        menuInfo.id = EditorUI.GUIEditorText(menuInfo.id);
        menuInfo.menu_id = menuInfo.id;

        EditorUI.GUIText("名称:");
        menuInfo.name = EditorUI.GUIEditorText(menuInfo.name, 150);
        EditorUI.GUIText("内容:");
        menuInfo.content = EditorUI.GUIEditorText(menuInfo.content, 300);

        EditorUI.GUIText("烹饪时间:");
        menuInfo.cook_time = EditorUI.GUIEditorText(menuInfo.cook_time);

        EditorUI.GUIText("当前利润:");
        long getmoney = menuInfo.price_s - (
             menuInfo.ing_oilsalt * 5 +
             menuInfo.ing_meat * 10 +
             menuInfo.ing_riverfresh * 10 +
             menuInfo.ing_seafood * 50 +
             menuInfo.ing_vegetables * 5 +
             menuInfo.ing_melonfruit * 5 +
             menuInfo.ing_waterwine * 10 +
             menuInfo.ing_flour * 5);
        EditorUI.GUIText(getmoney + "");
        EditorUI.GUIText("输入利润:", 50);
        long setGetmoney = EditorUI.GUIEditorText(getmoney, 50);
        if (getmoney != setGetmoney)
        {
            //自定义了利润
            menuInfo.price_s =
                setGetmoney +
             menuInfo.ing_oilsalt * 5 +
             menuInfo.ing_meat * 10 +
             menuInfo.ing_riverfresh * 10 +
             menuInfo.ing_seafood * 50 +
             menuInfo.ing_vegetables * 5 +
             menuInfo.ing_melonfruit * 5 +
             menuInfo.ing_waterwine * 10 +
             menuInfo.ing_flour * 5;
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

    /// <summary>
    /// 技能创建
    /// </summary>
    /// <param name="skillInfoService"></param>
    /// <param name="skillInfo"></param>
    public static void GUISkillCreate(SkillInfoService skillInfoService, SkillInfoBean skillInfo)
    {
        if (EditorUI.GUIButton("创建"))
        {
            skillInfo.valid = 1;
            skillInfoService.InsertData(skillInfo);
        }
        GUISkillItem(skillInfo);
        GUILayout.Space(20);
    }

    /// <summary>
    /// 技能查询
    /// </summary>
    /// <param name="skillInfoService"></param>
    /// <param name="findIds"></param>
    /// <param name="listData"></param>
    /// <param name="outFindIds"></param>
    /// <param name="outListData"></param>
    public static void GUISkillFind(SkillInfoService skillInfoService, string findIds, List<SkillInfoBean> listData, out string outFindIds, out List<SkillInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("查询IDs");
        findIds = EditorUI.GUIEditorText(findIds);
        if (EditorUI.GUIButton("查询指定ID"))
        {
            long[] ids = findIds.SplitForArrayLong(',');
            listData = skillInfoService.QueryDataByIds(ids);
        }
        if (EditorUI.GUIButton("查询所有技能"))
        {
            listData = skillInfoService.QueryAllData();
        }
        GUILayout.EndHorizontal();
        if (!listData.IsNull())
        {

            for (int i = 0; i < listData.Count; i++)
            {
                SkillInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (EditorUI.GUIButton("更新"))
                {
                    skillInfoService.Update(itemData);
                }
                if (EditorUI.GUIButton("删除"))
                {
                    skillInfoService.DeleteData(itemData.id);
                }
                GUISkillItem(itemData);
                GUILayout.EndHorizontal();
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    /// <summary>
    /// 技能展示
    /// </summary>
    /// <param name="skillInfo"></param>
    public static void GUISkillItem(SkillInfoBean skillInfo)
    {
        GUILayout.BeginHorizontal();
        EditorUI.GUIText("ID:");
        skillInfo.id = EditorUI.GUIEditorText(skillInfo.id);
        skillInfo.skill_id = skillInfo.id;
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

    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listFindStoreItem = listData;
    }
    #endregion
}