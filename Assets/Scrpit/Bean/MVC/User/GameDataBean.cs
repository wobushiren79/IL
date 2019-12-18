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

    public string innName;//客栈名称
    public int innLevel;//客栈等级 （天地人1-5星  3 2 1）
    public InnAttributesBean innAttributes;//客栈属性

    public CharacterBean userCharacter = new CharacterBean();// 老板
    public List<CharacterBean> listWorkerCharacter = new List<CharacterBean>();//员工

    public InnBuildBean innBuildData;//客栈建筑数据
    public TimeBean gameTime = new TimeBean();//游戏时间
    public UserAchievementBean userAchievement;

    public List<ItemBean> listBuild = new List<ItemBean>();//所拥有的建筑材料
    public List<ItemBean> listItems = new List<ItemBean>();//所拥有的装备
    public List<MenuOwnBean> listMenu = new List<MenuOwnBean>();//所拥有的菜单

    public List<CharacterFavorabilityBean> listCharacterFavorability = new List<CharacterFavorabilityBean>();//角色好感度
    public List<long> listTriggeredEvent = new List<long>();//触发过的事件

    public long ingOilsalt;//油盐
    public long ingMeat;//肉类
    public long ingRiverfresh;//河鲜
    public long ingSeafood;//海鲜
    public long ingVegetables;//蔬菜
    public long ingMelonfruit;//瓜果
    public long ingWaterwine;//酒水
    public long ingFlour;//面粉

    public int workerNumberLimit = 3;//员工人员招聘上限

    public WeatherBean weatherToday;//当天天气

    /// <summary>
    /// 增加工作员工
    /// </summary>
    /// <param name="characterData"></param>
    public void AddWorkCharacter(CharacterBean characterData)
    {
        listWorkerCharacter.Add(characterData);
    }

    /// <summary>
    /// 添加成就和奖励
    /// </summary>
    /// <param name="achievementInfo"></param>
    public void AddAchievement(AchievementInfoBean achievementInfo)
    {
        //扣除需要支付的数据
        PayMoney(achievementInfo.achieve_pay_l, achievementInfo.achieve_pay_m, achievementInfo.achieve_pay_s);
        //添加成就ID
        GetAchievementData().AddAchievement(achievementInfo.id);
        //添加奖励
        if (achievementInfo.reward_guildcoin != 0)
        {
            guildCoin += achievementInfo.reward_guildcoin;
        }
        //添加装备
        if (!CheckUtil.StringIsNull(achievementInfo.reward_items_ids))
        {
            foreach (long id in achievementInfo.GetRewardItems())
            {
                AddNewItems(id, 1);
            }
        }
        //添加建筑材料
        if (!CheckUtil.StringIsNull(achievementInfo.reward_build_ids))
        {
            foreach (long id in achievementInfo.GetRewardBuild())
            {
                ChangeBuildNumber(id, 1);
            }
        }
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
        //检测是否已经学过
        foreach (MenuOwnBean itemData in listMenu)
        {
            if (itemData.menuId == menuId)
            {
                return false;
            }
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
        ItemBean itemBean = new ItemBean(id, 1);
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
        moneyM += priceM;
        moneyS += priceS;
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
    /// 获取正在出售的食物
    /// </summary>
    /// <returns></returns>
    public List<MenuOwnBean> GetSellMenuList()
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
    /// 获取客栈等级
    /// </summary>
    /// <param name="levelTitle"></param>
    /// <param name="levelStar"></param>
    /// <returns></returns>
    public string GetInnLevel(out int levelTitle, out int levelStar)
    {
        levelStar = (innLevel % 10);
        levelTitle = (innLevel % 100) / 10;
        string levelTitleStr = "";
        string levelStarStr = "";
        switch (levelTitle)
        {
            case 1:
                levelTitleStr = GameCommonInfo.GetUITextById(2007);
                break;
            case 2:
                levelTitleStr = GameCommonInfo.GetUITextById(2008);
                break;
            case 3:
                levelTitleStr = GameCommonInfo.GetUITextById(2009);
                break;
        }

        switch (levelStar)
        {
            case 1:
                levelStarStr = GameCommonInfo.GetUITextById(2010);
                break;
            case 2:
                levelStarStr = GameCommonInfo.GetUITextById(2011);
                break;
            case 3:
                levelStarStr = GameCommonInfo.GetUITextById(2012);
                break;
            case 4:
                levelStarStr = GameCommonInfo.GetUITextById(2013);
                break;
            case 5:
                levelStarStr = GameCommonInfo.GetUITextById(2014);
                break;
        }
        return levelTitleStr + levelStarStr;
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
            if(itemData.characterId == characterId)
            {
                return itemData;
            }
        }
        CharacterFavorabilityBean characterFavorability = new CharacterFavorabilityBean(characterId);
        listCharacterFavorability.Add(characterFavorability);
        return characterFavorability;
    }

    /// <summary>
    /// 检测是否拥有该ID的角色
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public bool CheckHasWorker(long characterId)
    {
        foreach (CharacterBean characterData in listWorkerCharacter)
        {
            if(characterData.baseInfo.characterId.Equals(characterId+""))
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

    /// <summary>
    /// 检测是否能做出食物
    /// </summary>
    /// <param name="foodData"></param>
    /// <returns></returns>
    public bool CheckCookFood(MenuInfoBean foodData)
    {
        if (foodData.ing_oilsalt != 0 && ingOilsalt < foodData.ing_oilsalt)
        {
            return false;
        }
        if (foodData.ing_meat != 0 && ingMeat < foodData.ing_meat)
        {
            return false;
        }
        if (foodData.ing_riverfresh != 0 && ingRiverfresh < foodData.ing_riverfresh)
        {
            return false;
        }
        if (foodData.ing_seafood != 0 && ingSeafood < foodData.ing_seafood)
        {
            return false;
        }
        if (foodData.ing_vegetables != 0 && ingVegetables < foodData.ing_vegetables)
        {
            return false;
        }
        if (foodData.ing_melonfruit != 0 && ingMelonfruit < foodData.ing_melonfruit)
        {
            return false;
        }
        if (foodData.ing_waterwine != 0 && ingWaterwine < foodData.ing_waterwine)
        {
            return false;
        }
        if (foodData.ing_flour != 0 && ingFlour < foodData.ing_flour)
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
        if ( listWorkerCharacter.Count>= workerNumberLimit)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 修改食物销售数量
    /// </summary>
    /// <param name="number"></param>
    /// <param name="menuId"></param>
    public void ChangeMenuSellNumber(long number, long menuId)
    {
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            if (itemData.menuId == menuId)
            {
                itemData.sellNumber += number;
                return;
            }
        }
    }

    /// <summary>
    /// 修改建筑材料数量
    /// </summary>
    public void ChangeBuildNumber(long buildId, long number)
    {
        ChangeItem(buildId, number, listBuild);
    }

    /// <summary>
    /// 修改道具数量
    /// </summary>
    public void ChangeItemsNumber(long itemsId, long number)
    {
        ChangeItem(itemsId, number, listItems);
    }

    public void ChangeItem(long buildId, long number, List<ItemBean> list)
    {
        bool hasData = false;
        for (int i = 0; i < list.Count; i++)
        {
            ItemBean item = list[i];
            if (item.itemId == buildId)
            {
                hasData = true;
                item.itemNumber += number;
                if (item.itemNumber <= 0)
                {
                    item.itemNumber = 0;
                    list.RemoveAt(i);
                    i--;
                }
                break;
            }
        }
        if (!hasData && number > 0)
        {
            list.Add(new ItemBean(buildId, number));
        }
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


}