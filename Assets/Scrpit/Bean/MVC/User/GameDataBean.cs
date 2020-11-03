using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBean
{
    public string userId;//用户ID

    public long moneyS;//1黄金=10白银  1白银=1000文
    public long moneyM;
    public long moneyL;

    public long guildCoin;//公会硬币

    public long trophyElementary; //竞技场初级奖杯
    public long trophyIntermediate;//竞技场中级奖杯
    public long trophyAdvanced;//竞技场高级奖杯
    public long trophyLegendary;//竞技场传说奖杯

    public InnAttributesBean innAttributes = new InnAttributesBean();//客栈属性
    public CharacterBean userCharacter = new CharacterBean();// 老板
    public List<CharacterBean> listWorkerCharacter = new List<CharacterBean>();//员工

    public InnBuildBean innBuildData;//客栈建筑数据
    public TimeBean gameTime = new TimeBean();//游戏时间
    public TimeBean playTime = new TimeBean();//游玩时间
    public UserAchievementBean userAchievement = new UserAchievementBean();//成就相关

    public List<ItemBean> listBuild = new List<ItemBean>();//所拥有的建筑材料
    public List<ItemBean> listItems = new List<ItemBean>();//所拥有的装备
    public List<MenuOwnBean> listMenu = new List<MenuOwnBean>();//所拥有的菜单
    public List<BuildBedBean> listBed = new List<BuildBedBean>();//所拥有的菜单

    public List<CharacterFavorabilityBean> listCharacterFavorability = new List<CharacterFavorabilityBean>();//角色好感度
    public List<long> listTriggeredEvent = new List<long>();//触发过的事件
    public List<UserLoansBean> listLoans = new List<UserLoansBean>();//贷款
    public List<UserInfiniteTowersBean> listInfinteTowers = new List<UserInfiniteTowersBean>();//爬塔数据


    public long ingOilsalt;//油盐
    public long ingMeat;//肉类
    public long ingRiverfresh;//河鲜
    public long ingSeafood;//海鲜
    public long ingVegetables;//蔬菜
    public long ingMelonfruit;//瓜果
    public long ingWaterwine;//酒水
    public long ingFlour;//面粉

    public int workerNumberLimit = 2;//员工人员招聘上限
    public int loansNumberLimit = 3;//贷款上限

    /// <summary>
    /// 增加无尽之塔数据
    /// </summary>
    /// <param name="infiniteTowersData"></param>
    public void AddInfinteTowersData(UserInfiniteTowersBean infiniteTowersData)
    {
        listInfinteTowers.Add(infiniteTowersData);
    }

    /// <summary>
    /// 增加一个床
    /// </summary>
    /// <param name="buildBedData"></param>
    public void AddBed(BuildBedBean buildBedData)
    {
        listBed.Add(buildBedData);
    }

    /// <summary>
    /// 增加贷款
    /// </summary>
    /// <param name="loans"></param>
    /// <returns></returns>
    public bool AddLoans(UserLoansBean loans)
    {
        if (listLoans.Count >= loansNumberLimit)
        {
            return false;
        }
        else
        {
            listLoans.Add(loans);
            return true;
        }
    }

    /// <summary>
    /// 增加竞技场奖杯
    /// </summary>
    /// <param name="arenaTrophy1"></param>
    /// <param name="arenaTrophy2"></param>
    /// <param name="arenaTrophy3"></param>
    /// <param name="arenaTrophy4"></param>
    /// <param name="isRecord">是否记录</param>
    public void AddArenaTrophy(long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary,bool isRecord)
    {
        this.trophyElementary += trophyElementary;
        this.trophyIntermediate += trophyIntermediate;
        this.trophyAdvanced += trophyAdvanced;
        this.trophyLegendary += trophyLegendary;

        if (isRecord)
        {
            userAchievement.ownTrophyElementary += trophyElementary;
            userAchievement.ownTrophyIntermediate += trophyIntermediate;
            userAchievement.ownTrophyAdvanced += trophyAdvanced;
            userAchievement.ownTrophyLegendary += trophyLegendary;
        }

        if (this.trophyElementary < 0)
            this.trophyElementary = 0;
        if (this.trophyIntermediate < 0)
            this.trophyIntermediate = 0;
        if (this.trophyAdvanced < 0)
            this.trophyAdvanced = 0;
        if (this.trophyLegendary < 0)
            this.trophyLegendary = 0;
    }
    public void AddArenaTrophy(long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        AddArenaTrophy(trophyElementary, trophyIntermediate, trophyAdvanced, trophyLegendary, true);
    }

    /// <summary>
    /// 增加食材
    /// </summary>
    public void AddIng(IngredientsEnum ingType, int number)
    {
        switch (ingType)
        {
            case IngredientsEnum.Oilsalt:
                ingOilsalt += number;
                userAchievement.ownIngOilsalt += number;
                if (ingOilsalt < 0)
                    ingOilsalt = 0;
                break;
            case IngredientsEnum.Meat:
                ingMeat += number;
                userAchievement.ownIngMeat += number;
                if (ingMeat < 0)
                    ingMeat = 0;
                break;
            case IngredientsEnum.Riverfresh:
                ingRiverfresh += number;
                userAchievement.ownIngRiverfresh += number;
                if (ingRiverfresh < 0)
                    ingRiverfresh = 0;
                break;
            case IngredientsEnum.Seafood:
                ingSeafood += number;
                userAchievement.ownIngSeafood += number;
                if (ingSeafood < 0)
                    ingSeafood = 0;
                break;
            case IngredientsEnum.Vegetables:
                ingVegetables += number;
                userAchievement.ownIngVegetables += number;
                if (ingVegetables < 0)
                    ingVegetables = 0;
                break;
            case IngredientsEnum.Melonfruit:
                ingMelonfruit += number;
                userAchievement.ownIngMelonfruit += number;
                if (ingMelonfruit < 0)
                    ingMelonfruit = 0;
                break;
            case IngredientsEnum.Waterwine:
                ingWaterwine += number;
                userAchievement.ownIngWaterwine += number;
                if (ingWaterwine < 0)
                    ingWaterwine = 0;
                break;
            case IngredientsEnum.Flour:
                ingFlour += number;
                userAchievement.ownIngFlour += number;
                if (ingFlour < 0)
                    ingFlour = 0;
                break;
        }
    }

    /// <summary>
    /// 增加工作员工
    /// </summary>
    /// <param name="characterData"></param>
    public void AddWorkCharacter(CharacterBean characterData)
    {
        listWorkerCharacter.Add(characterData);
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="eventId"></param>
    public void AddTraggeredEvent(long eventId)
    {
        if (!CheckTriggeredEvent(eventId))
        {
            listTriggeredEvent.Add(eventId);
        }
    }

    /// <summary>
    /// 增加菜谱
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    public bool AddFoodMenu(long menuId)
    {
        // 检测是否已经学过
        bool hasMenu =  CheckHasMenu(menuId);
        if (hasMenu)
        {
            //已经有了 添加失败
            return false;
        }
        MenuOwnBean menuOwn = new MenuOwnBean
        {
            menuId = menuId
        };
        listMenu.Add(menuOwn);
        return true;
    }



    /// <summary>
    /// 增加一个新的道具
    /// </summary>
    /// <param name="id"></param>
    public void AddNewItems(long id, long number)
    {
        ItemBean itemBean = new ItemBean(id, number);
        listItems.Add(itemBean);
    }

    /// <summary>
    /// 增加金钱
    /// </summary>
    /// <param name="getMoneyL"></param>
    /// <param name="getMoneyM"></param>
    /// <param name="getMoneyS"></param>
    public void AddMoney(long priceL, long priceM, long priceS)
    {
        moneyL += priceL;
        userAchievement.ownMoneyL += priceL;
        moneyM += priceM;
        userAchievement.ownMoneyM += priceM;
        moneyS += priceS;
        userAchievement.ownMoneyS += priceS;
    }

    /// <summary>
    /// 增加公会勋章
    /// </summary>
    /// <param name="coin"></param>
    public void AddGuildCoin(long coin)
    {
        guildCoin += coin;
        userAchievement.ownGuildCoin += coin;
        if (guildCoin < 0)
            guildCoin = 0;
    }

    /// <summary>
    /// 修改建筑材料数量
    /// </summary>
    public ItemBean AddBuildNumber(long buildId, long number)
    {
        return AddItem(buildId, number, listBuild);
    }

    /// <summary>
    /// 修改道具数量
    /// </summary>
    public ItemBean AddItemsNumber(long itemsId, long number)
    {
        return AddItem(itemsId, number, listItems);
    }

    public ItemBean AddItem(long id, long number, List<ItemBean> list)
    {
        bool hasData = false;
        ItemBean targetItem = null;
        for (int i = 0; i < list.Count; i++)
        {
            ItemBean item = list[i];
            if (item.itemId == id)
            {
                hasData = true;
                item.itemNumber += number;
                if (item.itemNumber <= 0)
                {
                    item.itemNumber = 0;
                    list.RemoveAt(i);
                    i--;
                }
                targetItem = item;
                break;
            }
        }
        if (!hasData && number > 0)
        {
            ItemBean itemData = new ItemBean(id, number);
            list.Add(itemData);
            targetItem = itemData;
        }
        return targetItem;
    }


    /// <summary>
    /// 修改食物销售数量
    /// </summary>
    /// <param name="innFoodManager"></param>
    /// <param name="number"></param>
    /// <param name="menuId"></param>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    /// <param name="isMenuLevelUp"></param>
    public void AddMenuSellNumber(InnFoodManager innFoodManager, long number, long menuId, long priceL, long priceM, long priceS, out bool isMenuLevelUp)
    {
        isMenuLevelUp = false;
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.menuId == menuId)
            {
                itemData.SellMenu(innFoodManager,number, priceL, priceM, priceS, out isMenuLevelUp);
                return;
            }
        }
    }

    /// <summary>
    /// 修改床单的销售数量
    /// </summary>
    /// <param name="remarkId"></param>
    /// <param name="number"></param>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    /// <param name="isBedLevelUp"></param>
    public void AddBedSellNumber(string remarkId, long number,long sleeptime, long priceL, long priceM, long priceS, out bool isBedLevelUp)
    {
        isBedLevelUp = false;
        BuildBedBean buildBedData=  GetBedByRemarkId(remarkId);
        if (buildBedData!=null)
        {
            buildBedData.SellBed(number, sleeptime, priceL, priceM, priceS, out isBedLevelUp);
        }
    }

    /// <summary>
    /// 获取所有人员信息
    /// </summary>
    /// <returns></returns>
    public List<CharacterBean> GetAllCharacterData()
    {
        List<CharacterBean> listData = new List<CharacterBean>();
        if (userCharacter != null)
            listData.Add(userCharacter);
        if (listWorkerCharacter != null)
            listData.AddRange(listWorkerCharacter);
        return listData;
    }

    /// <summary>
    /// 通过ID获取员工信息
    /// </summary>
    /// <returns></returns>
    public  CharacterBean GetCharacterDataById(string characterId) 
    {
        List<CharacterBean> listData = GetAllCharacterData();
        for (int i = 0;i< listData.Count;i++)
        {
            CharacterBean itemCharacter = listData[i];
            if (itemCharacter.baseInfo.characterId == null)
            {
                continue;
            }
            if (itemCharacter.baseInfo.characterId.Equals(characterId))
            {
                return itemCharacter;
            }
        }
        return null;
    }


    /// <summary>
    /// 通过IDs获取员工信息
    /// </summary>
    /// <returns></returns>
    public List<CharacterBean> GetCharacterDataByIds(List<string> listCharacterId)
    {
        List<CharacterBean> listData = GetAllCharacterData();
        List<CharacterBean> listTempData = new List<CharacterBean>();
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemCharacter = listData[i];
            for (int f = 0; f < listCharacterId.Count; f++)
            {
                string itemCharacterId = listCharacterId[f];
                if (itemCharacter.baseInfo.characterId == null)
                {
                    continue;
                }
                if (itemCharacter.baseInfo.characterId.Equals(itemCharacterId))
                {
                    listTempData.Add(itemCharacter);
                    break;
                }
            }
        }
        return listTempData;
    }

    /// <summary>
    /// 获取成就数据
    /// </summary>
    /// <returns></returns>
    public UserAchievementBean GetAchievementData()
    {
        if (userAchievement == null)
            userAchievement = new UserAchievementBean();
        return userAchievement;
    }

    /// <summary>
    /// 获取客栈属性数据
    /// </summary>
    /// <returns></returns>
    public InnAttributesBean GetInnAttributesData()
    {
        if (innAttributes == null)
            innAttributes = new InnAttributesBean();
        innAttributes.RefreshRichNess(listMenu);
        return innAttributes;
    }

    /// <summary>
    /// 获取建筑数据
    /// </summary>
    /// <returns></returns>
    public InnBuildBean GetInnBuildData()
    {
        if (innBuildData == null)
            innBuildData = new InnBuildBean();
        return innBuildData;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="innBuildManager"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemBean> GetBuildDataByType(InnBuildManager innBuildManager, BuildItemTypeEnum type)
    {
        List<ItemBean> listData = new List<ItemBean>();
        foreach (ItemBean itemData in listBuild)
        {
            BuildItemBean buildItemData= innBuildManager.GetBuildDataById(itemData.itemId);
            if (buildItemData.GetBuildType() == type)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 获取正在出售的菜品
    /// </summary>
    /// <returns></returns>
    public List<MenuOwnBean> GetMenuListForSell()
    {
        List<MenuOwnBean> listData = new List<MenuOwnBean>();
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.isSell)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 获取研究中的菜品
    /// </summary>
    /// <returns></returns>
    public List<MenuOwnBean> GetMenuListForResearching()
    {
        List<MenuOwnBean> listData = new List<MenuOwnBean>();
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.GetMenuStatus() == ResearchStatusEnum.Researching)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 获取研究中的床
    /// </summary>
    /// <returns></returns>
    public List<BuildBedBean> GetBedListForResearching()
    {
        List<BuildBedBean> listData = new List<BuildBedBean>();
        for (int i = 0; i < listBed.Count; i++)
        {
            BuildBedBean itemData = listBed[i];
            if (itemData.GetBedStatus() == ResearchStatusEnum.Researching)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 通过备注ID获取床
    /// </summary>
    /// <param name="remarkId"></param>
    /// <returns></returns>
    public BuildBedBean GetBedByRemarkId(string remarkId)
    {
        for (int i = 0; i < listBed.Count; i++)
        {
            BuildBedBean itemData = listBed[i];
            if (itemData.remarkId.Equals(remarkId))
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 通过等级获取床单数量
    /// </summary>
    /// <param name="levelType"></param>
    /// <returns></returns>
    public int GetBedNumberByLevel(LevelTypeEnum levelType)
    {
        int number = 0;
        foreach (BuildBedBean itemBed in listBed)
        {
            if (itemBed.GetBedLevel() >= levelType)
            {
                number++;
            }
        }
        return number;
    }

    /// <summary>
    /// 获取指定菜品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MenuOwnBean GetMenuById(long id)
    {
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.menuId == id)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取所有菜品
    /// </summary>
    /// <returns></returns>
    public List<MenuOwnBean> GetMenuList()
    {
        return listMenu;
    }

    /// <summary>
    /// 通过等级获取菜品数量
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetMenuNumberByLevel(LevelTypeEnum menuLevel)
    {
        int number = 0;
        foreach (MenuOwnBean itemMenu in listMenu)
        {
            if(itemMenu.GetMenuLevel() >= menuLevel)
            {
                number++;
            }
        }
        return number;
    }



    /// <summary>
    /// 获取某一物品数量
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    /// 
    public long GetItemsNumber(long itemId)
    {
        return GetNumber(itemId, listItems);
    }

    /// <summary>
    /// 获取建筑材料数量
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public long GetBuildNumber(long itemId)
    {
        return GetNumber(itemId, listBuild);
    }

    public long GetNumber(long itemId, List<ItemBean> listData)
    {
        long number = 0;
        for (int i = 0; i < listData.Count; i++)
        {
            ItemBean itemData = listData[i];
            if (itemId == itemData.itemId)
            {
                number += itemData.itemNumber;
            }
        }
        return number;
    }

    /// <summary>
    /// 获取角色好感数据
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public CharacterFavorabilityBean GetCharacterFavorability(long characterId)
    {
        foreach (CharacterFavorabilityBean itemData in listCharacterFavorability)
        {
            if (itemData.characterId == characterId)
            {
                return itemData;
            }
        }
        CharacterFavorabilityBean characterFavorability = new CharacterFavorabilityBean(characterId);
        listCharacterFavorability.Add(characterFavorability);
        return characterFavorability;
    }
    public CharacterFavorabilityBean GetCharacterFavorabilityNoAdd(long characterId)
    {
        foreach (CharacterFavorabilityBean itemData in listCharacterFavorability)
        {
            if (itemData.characterId == characterId)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 检测是否拥有该ID的角色
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public bool CheckHasWorker(string characterId)
    {
        foreach (CharacterBean characterData in listWorkerCharacter)
        {
            if (characterData.baseInfo.characterId.Equals(characterId))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否已经触发过该事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public bool CheckTriggeredEvent(long eventId)
    {
        return listTriggeredEvent.Contains(eventId);
    }

    public bool CheckTriggeredEvent(long[] eventIds)
    {
        foreach (long itemId in eventIds)
        {
            if (!CheckTriggeredEvent(itemId))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测是否已经学过
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    public bool CheckHasMenu(long menuId)
    {
        foreach (MenuOwnBean itemData in listMenu)
        {
            if (itemData.menuId == menuId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否能做出食物
    /// </summary>
    /// <param name="foodData"></param>
    /// <returns></returns>
    public bool CheckCookFood(MenuInfoBean foodData)
    {
        if (!HasEnoughIng(IngredientsEnum.Oilsalt, foodData.ing_oilsalt))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Meat, foodData.ing_meat))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Riverfresh, foodData.ing_riverfresh))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Seafood, foodData.ing_seafood))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Vegetables, foodData.ing_vegetables))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Melonfruit, foodData.ing_melonfruit))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Waterwine, foodData.ing_waterwine))
        {
            return false;
        }
        if (!HasEnoughIng(IngredientsEnum.Flour, foodData.ing_flour))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检测是否超过最大限制工作人员
    /// </summary>
    /// <returns></returns>
    public bool CheckIsMaxWorker()
    {
        if (listWorkerCharacter.Count >= workerNumberLimit)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否正在出售
    /// </summary>
    /// <returns></returns>
    public bool CheckIsSellMenu(long menuId)
    {
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.menuId == menuId && itemData.isSell)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否有items
    /// </summary>
    /// <param name="id"></param>
    /// <param name="hasItems"></param>
    /// <param name="number"></param>
    public void CheckHasItems(long id, out bool hasItems, out long number)
    {
        hasItems = false;
        number = 0;
        foreach (ItemBean itemData in listItems)
        {
            if (itemData.itemId == id)
            {
                hasItems = true;
                number = itemData.itemNumber;
                return;
            }
        }
    }

    /// <summary>
    /// 检测是否有指定的菜品
    /// </summary>
    /// <param name="loveMenus"></param>
    /// <param name="ownLoveMenus"></param>
    /// <returns></returns>
    public bool CheckHasLoveMenus(List<long> loveMenus, out List<MenuOwnBean> ownLoveMenus)
    {
        bool hasLove = false;
        ownLoveMenus = new List<MenuOwnBean>();
        foreach (MenuOwnBean itemMenu in listMenu)
        {
            if (loveMenus.Contains(itemMenu.menuId) && itemMenu.isSell)
            {
                ownLoveMenus.Add(itemMenu);
                hasLove = true;
            }
        }
        return hasLove;
    }

    /// <summary>
    /// 扣除食材
    /// </summary>
    /// <param name="foodData"></param>
    public void DeductIng(MenuInfoBean foodData)
    {
        ingOilsalt -= foodData.ing_oilsalt;
        ingMeat -= foodData.ing_meat;
        ingRiverfresh -= foodData.ing_riverfresh;
        ingSeafood -= foodData.ing_seafood;
        ingVegetables -= foodData.ing_vegetables;
        ingMelonfruit -= foodData.ing_melonfruit;
        ingWaterwine -= foodData.ing_waterwine;
        ingFlour -= foodData.ing_flour;
        if (ingOilsalt < 0)
            ingOilsalt = 0;
        if (ingMeat < 0)
            ingMeat = 0;
        if (ingRiverfresh < 0)
            ingRiverfresh = 0;
        if (ingSeafood < 0)
            ingSeafood = 0;
        if (ingVegetables < 0)
            ingVegetables = 0;
        if (ingMelonfruit < 0)
            ingMelonfruit = 0;
        if (ingWaterwine < 0)
            ingWaterwine = 0;
        if (ingFlour < 0)
            ingFlour = 0;
    }

    /// <summary>
    /// 扣除材料
    /// </summary>
    /// <param name="ingredients"></param>
    /// <param name="number"></param>
    public void DeductIng(IngredientsEnum ingredients, long number)
    {
        switch (ingredients)
        {
            case IngredientsEnum.Oilsalt:
                ingOilsalt -= number;
                break;
            case IngredientsEnum.Meat:
                ingMeat -= number;
                break;
            case IngredientsEnum.Riverfresh:
                ingRiverfresh -= number;
                break;
            case IngredientsEnum.Seafood:
                ingSeafood -= number;
                break;
            case IngredientsEnum.Vegetables:
                ingVegetables -= number;
                break;
            case IngredientsEnum.Melonfruit:
                ingMelonfruit -= number;
                break;
            case IngredientsEnum.Waterwine:
                ingWaterwine -= number;
                break;
            case IngredientsEnum.Flour:
                ingFlour -= number;
                break;
        }
        if (ingOilsalt < 0)
            ingOilsalt = 0;
        if (ingMeat < 0)
            ingMeat = 0;
        if (ingRiverfresh < 0)
            ingRiverfresh = 0;
        if (ingSeafood < 0)
            ingSeafood = 0;
        if (ingVegetables < 0)
            ingVegetables = 0;
        if (ingMelonfruit < 0)
            ingMelonfruit = 0;
        if (ingWaterwine < 0)
            ingWaterwine = 0;
        if (ingFlour < 0)
            ingFlour = 0;
    }

    /// <summary>
    /// 是否有足够的钱
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public bool HasEnoughMoney(long priceL, long priceM, long priceS)
    {
        if (moneyL < priceL || moneyM < priceM || moneyS < priceS)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否1足够的公会勋章
    /// </summary>
    /// <param name="coin"></param>
    /// <returns></returns>
    public bool HasEnoughGuildCoin(long coin)
    {
        if (guildCoin < coin)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否有足够的奖杯
    /// </summary>
    /// <param name="trophyElementary"></param>
    /// <param name="trophyIntermediate"></param>
    /// <param name="trophyAdvanced"></param>
    /// <param name="trophyLegendary"></param>
    /// <returns></returns>
    public bool HasEnoughTrophy(long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        if (this.trophyElementary < trophyElementary
            || this.trophyIntermediate < trophyIntermediate
            || this.trophyAdvanced < trophyAdvanced
            || this.trophyLegendary < trophyLegendary)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否有足够的材料
    /// </summary>
    /// <param name="ingredients"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool HasEnoughIng(IngredientsEnum ingredients, long number)
    {
        long hasNumber = 0;
        switch (ingredients)
        {
            case IngredientsEnum.Oilsalt:
                hasNumber = ingOilsalt;
                break;
            case IngredientsEnum.Meat:
                hasNumber = ingMeat;
                break;
            case IngredientsEnum.Riverfresh:
                hasNumber = ingRiverfresh;
                break;
            case IngredientsEnum.Seafood:
                hasNumber = ingSeafood;
                break;
            case IngredientsEnum.Vegetables:
                hasNumber = ingVegetables;
                break;
            case IngredientsEnum.Melonfruit:
                hasNumber = ingMelonfruit;
                break;
            case IngredientsEnum.Waterwine:
                hasNumber = ingWaterwine;
                break;
            case IngredientsEnum.Flour:
                hasNumber = ingFlour;
                break;
        }
        if (hasNumber >= number)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 支付金钱
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void PayMoney(long priceL, long priceM, long priceS)
    {
        moneyL -= priceL;
        moneyM -= priceM;
        moneyS -= priceS;
        if (moneyL < 0)
            moneyL = 0;
        if (moneyM < 0)
            moneyM = 0;
        if (moneyS < 0)
            moneyS = 0;
        userAchievement.payMoneyL += moneyL;
        userAchievement.payMoneyM += moneyM;
        userAchievement.payMoneyS += moneyS;
    }

    /// <summary>
    /// 支付公会勋章
    /// </summary>
    /// <param name="coin"></param>
    public void PayGuildCoin(long coin)
    {
        guildCoin -= coin;
        if (guildCoin < 0)
            guildCoin = 0;
    }

    /// <summary>
    /// 还贷
    /// </summary>
    /// <returns></returns>
    public void PayLoans(out List<UserLoansBean> listPayLoans)
    {
        listPayLoans = new List<UserLoansBean>();
        if (CheckUtil.ListIsNull(listLoans))
        {
            return;
        }
        for (int i = 0; i < listLoans.Count; i++)
        {
            UserLoansBean itemLoans = listLoans[i];
            if (HasEnoughMoney(0, 0, itemLoans.moneySForDay))
            {
                //支付金钱
                PayMoney(0, 0, itemLoans.moneySForDay);
                //剩余的还贷日期-1
                itemLoans.residueDays -= 1;
                //已经还过的列表+1
                listPayLoans.Add(itemLoans);
                //如果已经还完
                if (itemLoans.residueDays <= 0)
                {
                    listLoans.Remove(itemLoans);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// 支付奖杯
    /// </summary>
    /// <param name="trophyElementary"></param>
    /// <param name="trophyIntermediate"></param>
    /// <param name="trophyAdvanced"></param>
    /// <param name="trophyLegendary"></param>
    public void PayTrophy(long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        this.trophyElementary -= trophyElementary;
        this.trophyIntermediate -= trophyIntermediate;
        this.trophyAdvanced -= trophyAdvanced;
        this.trophyLegendary -= trophyLegendary;
        if (trophyElementary < 0)
            trophyElementary = 0;
        if (trophyIntermediate < 0)
            trophyIntermediate = 0;
        if (trophyAdvanced < 0)
            trophyAdvanced = 0;
        if (trophyLegendary < 0)
            trophyLegendary = 0;
    }

    /// <summary>
    /// 移除员工
    /// </summary>
    /// <param name="characterData"></param>
    /// <returns></returns>
    public bool RemoveWorker(CharacterBean characterData)
    {
        if (listWorkerCharacter != null && characterData != null)
            return listWorkerCharacter.Remove(characterData);
        return false;
    }

    /// <summary>
    /// 移除一张床
    /// </summary>
    /// <param name="buildBedData"></param>
    /// <returns></returns>
    public bool RemoveBed(BuildBedBean buildBedData)
    {
        return listBed.Remove(buildBedData);
    }

    /// <summary>
    /// 移除无尽之塔数据
    /// </summary>
    /// <param name="infiniteTowersData"></param>
    /// <returns></returns>
    public bool RemoveInfiniteTowersData(UserInfiniteTowersBean infiniteTowersData)
    {
        return listInfinteTowers.Remove(infiniteTowersData);
    }
}