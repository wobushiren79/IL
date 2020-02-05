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
    public UserAchievementBean userAchievement = new UserAchievementBean();//成就相关

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
    /// 增加竞技场奖杯
    /// </summary>
    /// <param name="arenaTrophy1"></param>
    /// <param name="arenaTrophy2"></param>
    /// <param name="arenaTrophy3"></param>
    /// <param name="arenaTrophy4"></param>
    public void AddArenaTrophy(long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        this.trophyElementary += trophyElementary;
        this.trophyIntermediate += trophyIntermediate;
        this.trophyAdvanced += trophyAdvanced;
        this.trophyLegendary += trophyLegendary;
        if (this.trophyElementary < 0)
            this.trophyElementary = 0;
        if (this.trophyIntermediate < 0)
            this.trophyIntermediate = 0;
        if (this.trophyAdvanced < 0)
            this.trophyAdvanced = 0;
        if (this.trophyLegendary < 0)
            this.trophyLegendary = 0;
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
                break;
            case IngredientsEnum.Meat:
                ingMeat += number;
                userAchievement.ownIngMeat += number;
                break;
            case IngredientsEnum.Riverfresh:
                ingRiverfresh += number;
                userAchievement.ownIngRiverfresh += number;
                break;
            case IngredientsEnum.Seafood:
                ingSeafood += number;
                userAchievement.ownIngSeafood += number;
                break;
            case IngredientsEnum.Vegetablest:
                ingVegetables += number;
                userAchievement.ownIngVegetables += number;
                break;
            case IngredientsEnum.Melonfruit:
                ingMelonfruit += number;
                userAchievement.ownIngMelonfruit += number;
                break;
            case IngredientsEnum.Waterwine:
                ingWaterwine += number;
                userAchievement.ownIngWaterwine += number;
                break;
            case IngredientsEnum.Flour:
                ingFlour += number;
                userAchievement.ownIngFlour += number;
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
        moneyM += priceM;
        moneyS += priceS;
    }

    /// <summary>
    /// 增加公会勋章
    /// </summary>
    /// <param name="coin"></param>
    public void AddGuildCoin(long coin)
    {
        guildCoin += coin;
        if (guildCoin < 0)
            guildCoin = 0;
    }

    /// <summary>
    /// 修改建筑材料数量
    /// </summary>
    public void AddBuildNumber(long buildId, long number)
    {
        AddItem(buildId, number, listBuild);
    }

    /// <summary>
    /// 修改道具数量
    /// </summary>
    public void AddItemsNumber(long itemsId, long number)
    {
        AddItem(itemsId, number, listItems);
    }

    public void AddItem(long buildId, long number, List<ItemBean> list)
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

    /// <summary>
    /// 检测是否拥有该ID的角色
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public bool CheckHasWorker(long characterId)
    {
        foreach (CharacterBean characterData in listWorkerCharacter)
        {
            if (characterData.baseInfo.characterId.Equals(characterId + ""))
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
        if (listWorkerCharacter.Count >= workerNumberLimit)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否有指定的菜品
    /// </summary>
    /// <param name="loveMenus"></param>
    /// <param name="ownLoveMenus"></param>
    /// <returns></returns>
    public bool CheckHasLoveMenus(List<long> loveMenus,out List<MenuOwnBean> ownLoveMenus)
    {
        bool hasLove = false;
        ownLoveMenus = new List<MenuOwnBean>();
        foreach (MenuOwnBean itemMenu in listMenu)
        {
            if (loveMenus.Contains(itemMenu.menuId))
            {
                ownLoveMenus.Add(itemMenu);
                hasLove = true;
            }
        }
        return hasLove;
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
        if (listWorkerCharacter != null&& characterData!=null)
           return listWorkerCharacter.Remove(characterData);
        return false;
    }
}