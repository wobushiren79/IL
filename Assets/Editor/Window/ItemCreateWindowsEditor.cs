﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemCreateWindowsEditor : EditorWindow, StoreInfoManager.ICallBack
{
    GameItemsManager gameItemsManager;
    StoreInfoManager storeInfoManager;
    ItemsInfoService itemsInfoService;
    StoreInfoService storeInfoService;
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
        storeInfoManager.SetCallBack(this);
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        itemsInfoService = new ItemsInfoService();
        storeInfoService = new StoreInfoService();
        achievementInfoService = new AchievementInfoService();
    }

    public void RefreshData()
    {
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        listFindItem.Clear();
        listFindStoreItem.Clear();
    }

    private Vector2 scrollPosition = Vector2.zero;

    public ItemsInfoBean createItemsInfo = new ItemsInfoBean();
    public Sprite spriteCreateIcon;
    public GeneralEnum createItemType;

    public StoreInfoBean createStoreInfo = new StoreInfoBean();
    public StoreTypeEnum createStoreItemType;

    public AchievementInfoBean createAchInfo = new AchievementInfoBean();
    long inputId = 0;



    public List<ItemsInfoBean> listFindItem = new List<ItemsInfoBean>();
    public List<StoreInfoBean> listFindStoreItem = new List<StoreInfoBean>();
    public List<AchievementInfoBean> listFindAchItem = new List<AchievementInfoBean>();
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
        GUICreateStoreItem();
        GUIFindStoreItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUICreateAchItem();
        GUIFindAchItem();
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
        achievementInfo.id = long.Parse(EditorGUILayout.TextArea(achievementInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
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

        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
        {
            achievementInfo.pre_data += ("|" + EnumUtil.GetEnumName(PreTypeEnum.PayMoneyL) + ":" + "1|");
        }
        List<string> listPreData = StringUtil.SplitBySubstringForListStr(achievementInfo.pre_data, '|');
        achievementInfo.pre_data = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (CheckUtil.StringIsNull(itemPreData))
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
            List<string> listItemPreData = StringUtil.SplitBySubstringForListStr(itemPreData, ':');
            listItemPreData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("前置类型", EnumUtil.GetEnum<PreTypeEnum>(listItemPreData[0]), GUILayout.Width(300), GUILayout.Height(20)));
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
            achievementInfo.reward_data += (EnumUtil.GetEnumName(RewardTypeEnum.AddWorkerNumber) + ":" + "1|");
        }
        List<string> listRewardData = StringUtil.SplitBySubstringForListStr(achievementInfo.reward_data, '|');
        achievementInfo.reward_data = "";
        for (int i = 0; i < listRewardData.Count; i++)
        {
            string itemRewardData = listRewardData[i];
            if (CheckUtil.StringIsNull(itemRewardData))
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
            List<string> listItemRewardData = StringUtil.SplitBySubstringForListStr(itemRewardData, ':');
            listItemRewardData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("奖励类型", EnumUtil.GetEnum<RewardTypeEnum>(listItemRewardData[0]), GUILayout.Width(300), GUILayout.Height(20)));
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
        if (GUILayout.Button("查询公会商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildGoods();
        }
        if (GUILayout.Button("查询职业升级", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildImprove();
        }
        if (GUILayout.Button("查询客栈升级", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForGuildInnLevel();
        }
        if (GUILayout.Button("查询斗技场", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForArenaInfo();
        }
        if (GUILayout.Button("查询商品", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfoManager.GetStoreInfoForArenaGoods();
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
        storeInfo.id = long.Parse(EditorGUILayout.TextArea(storeInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
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



    private void GUIStoreItemForArenaInfo(StoreInfoBean storeInfo)
    {
        GUILayout.Label("竞赛等级（1初级 2中级，3高级，4传说）：", GUILayout.Width(250), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(100), GUILayout.Height(20)));
        if(CheckUtil.StringIsNull(storeInfo.pre_data))
        {
            storeInfo.pre_data = "Chef";
        }
        storeInfo.pre_data = ((WorkerEnum)EditorGUILayout.EnumPopup("职业", EnumUtil.GetEnum<WorkerEnum>(storeInfo.pre_data), GUILayout.Width(300), GUILayout.Height(20)))+"";
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
    }

    private void GUIStoreItemForInnLevel(StoreInfoBean storeInfo)
    {
        GUILayout.Label("客栈等级：", GUILayout.Width(100), GUILayout.Height(20));
        storeInfo.mark_type = int.Parse(EditorGUILayout.TextArea(storeInfo.mark_type + "", GUILayout.Width(100), GUILayout.Height(20)));
        //GUILayout.Label("升级所需价格LMS：", GUILayout.Width(100), GUILayout.Height(20));
        //storeInfo.price_l = long.Parse(EditorGUILayout.TextArea(storeInfo.price_l + "", GUILayout.Width(100), GUILayout.Height(20)));
        //storeInfo.price_m = long.Parse(EditorGUILayout.TextArea(storeInfo.price_m + "", GUILayout.Width(100), GUILayout.Height(20)));
        //storeInfo.price_s = long.Parse(EditorGUILayout.TextArea(storeInfo.price_s + "", GUILayout.Width(100), GUILayout.Height(20)));

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

        createItemType = (GeneralEnum)EditorGUILayout.EnumPopup("物品类型", createItemType);
        createItemsInfo.items_type = (int)createItemType;

        spriteCreateIcon = EditorGUILayout.ObjectField(new GUIContent("选择图片", ""), spriteCreateIcon, typeof(Sprite), true) as Sprite;
        if (spriteCreateIcon)
        {
            GUILayout.Label("icon_key：");
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
            GUILayout.Label("输入ID");
            inputId = long.Parse(EditorGUILayout.TextArea(inputId + "", GUILayout.Width(100), GUILayout.Height(20)));
        }

        GUILayout.Label("物品ID：", GUILayout.Width(50), GUILayout.Height(20));
        EditorGUILayout.TextArea((inputId + autoId) + "", GUILayout.Width(100), GUILayout.Height(20));

        createItemsInfo.id = (inputId + autoId);
        createItemsInfo.items_id = createItemsInfo.id;

        GUILayout.Label("名字：");
        createItemsInfo.name = EditorGUILayout.TextArea(createItemsInfo.name + "", GUILayout.Width(150), GUILayout.Height(20));

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
            long[] idArray = StringUtil.SplitBySubstringForArrayLong(findIds, ',');
            listFindItem = gameItemsManager.GetItemsByIds(idArray);
        }
        if (GUILayout.Button("查询所有", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetAllItems();
        }
        if (GUILayout.Button("查询所有服装", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindItem = gameItemsManager.GetClothesList();
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

            GUILayout.Label("物品ID：");
            itemInfo.id = long.Parse(EditorGUILayout.TextArea(itemInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));

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
                case GeneralEnum.Chef:
                    path += "Items/Chef/";
                    break;
                case GeneralEnum.Waiter:
                    path += "Items/Waiter/";
                    break;
                case GeneralEnum.Accouting:
                    path += "Items/Accouting/";
                    break;
                case GeneralEnum.Accost:
                    path += "Items/Accost/";
                    break;
                case GeneralEnum.Beater:
                    path += "Items/Beater/";
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
            GUILayout.Label("icon_key：");
            itemInfo.icon_key = EditorGUILayout.TextArea(itemInfo.icon_key + "", GUILayout.Width(150), GUILayout.Height(20));

            itemInfo.items_type = (int)(GeneralEnum)EditorGUILayout.EnumPopup("物品类型", (GeneralEnum)itemInfo.items_type);

            GUILayout.Label("名字：");
            itemInfo.name = EditorGUILayout.TextArea(itemInfo.name + "", GUILayout.Width(150), GUILayout.Height(20));
            GUILayout.Label("描述：");
            itemInfo.content = EditorGUILayout.TextArea(itemInfo.content + "", GUILayout.Width(150), GUILayout.Height(20));
            GUILayout.Label("增加属性：");
            GUILayout.Label("命");
            itemInfo.add_life = int.Parse(EditorGUILayout.TextArea(itemInfo.add_life + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("厨");
            itemInfo.add_cook = int.Parse(EditorGUILayout.TextArea(itemInfo.add_cook + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("速");
            itemInfo.add_speed = int.Parse(EditorGUILayout.TextArea(itemInfo.add_speed + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("算");
            itemInfo.add_account = int.Parse(EditorGUILayout.TextArea(itemInfo.add_account + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("魅");
            itemInfo.add_charm = int.Parse(EditorGUILayout.TextArea(itemInfo.add_charm + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("武");
            itemInfo.add_force = int.Parse(EditorGUILayout.TextArea(itemInfo.add_force + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("运");
            itemInfo.add_lucky = int.Parse(EditorGUILayout.TextArea(itemInfo.add_lucky + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("忠");
            itemInfo.add_loyal = int.Parse(EditorGUILayout.TextArea(itemInfo.add_loyal + "", GUILayout.Width(150), GUILayout.Height(20)));


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
            storeInfo.reward_data += (EnumUtil.GetEnumName(RewardTypeEnum.AddItems) + ":" + "1|");
        }
        List<string> listRewardData = StringUtil.SplitBySubstringForListStr(storeInfo.reward_data, '|');
        storeInfo.reward_data = "";
        for (int i = 0; i < listRewardData.Count; i++)
        {
            string itemRewardData = listRewardData[i];
            if (CheckUtil.StringIsNull(itemRewardData))
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
            List<string> listItemRewardData = StringUtil.SplitBySubstringForListStr(itemRewardData, ':');
            listItemRewardData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("奖励类型", EnumUtil.GetEnum<RewardTypeEnum>(listItemRewardData[0]), GUILayout.Width(300), GUILayout.Height(20)));
            listItemRewardData[1] = EditorGUILayout.TextArea(listItemRewardData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storeInfo.reward_data += (listItemRewardData[0] + ":" + listItemRewardData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }

    private void GUIPre(StoreInfoBean storeInfo)
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label("前置：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加前置", GUILayout.Width(100), GUILayout.Height(20)))
        {
            storeInfo.pre_data += ("|" + EnumUtil.GetEnumName(PreTypeEnum.PayMoneyL) + ":" + "1|");
        }
        List<string> listPreData = StringUtil.SplitBySubstringForListStr(storeInfo.pre_data, '|');
        storeInfo.pre_data = "";
        for (int i = 0; i < listPreData.Count; i++)
        {
            string itemPreData = listPreData[i];
            if (CheckUtil.StringIsNull(itemPreData))
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
            List<string> listItemPreData = StringUtil.SplitBySubstringForListStr(itemPreData, ':');
            listItemPreData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup("前置类型", EnumUtil.GetEnum<PreTypeEnum>(listItemPreData[0]), GUILayout.Width(300), GUILayout.Height(20)));
            listItemPreData[1] = EditorGUILayout.TextArea(listItemPreData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            storeInfo.pre_data += (listItemPreData[0] + ":" + listItemPreData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
    }
    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listFindStoreItem = listData;
    }
    #endregion
}