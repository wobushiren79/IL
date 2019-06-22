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

    public string innName;//客栈名称
    public CharacterBean userCharacter;// 老板
    public List<CharacterBean> workCharacterList = new List<CharacterBean>();//员工
    public InnBuildBean innBuildData;//客栈建筑数据
    public TimeBean gameTime;//游戏时间

    public List<ItemBean> buildItemList = new List<ItemBean>();//所拥有的建筑材料
    public List<ItemBean> equipItemList = new List<ItemBean>();//所拥有的装备
    public List<MenuOwnBean> menuList = new List<MenuOwnBean>();//所拥有的菜单

    public long ingOilsalt;//油盐
    public long ingMeat;//肉类
    public long ingRiverfresh;//河鲜
    public long ingSeafood;//海鲜
    public long ingVegetables;//蔬菜
    public long ingMelonfruit;//瓜果
    public long ingWaterwine;//酒水
    public long ingFlour;//面粉

    public int workerNumberLimit=5;//员工人员招聘上限
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
        for (int i = 0; i < menuList.Count; i++)
        {
            MenuOwnBean itemData = menuList[i];
            if (itemData.isSell)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    public static void GetMoneyDetails(long money, out long L, out long M, out long S)
    {
        long temp1 = money % 10;
        long temp2 = money % 100 / 10;
        long temp3 = money % 1000 / 100;
        long temp4 = money % 10000 / 1000;
        S = temp3 * 100 + temp2 * 10 + temp1;
        M = temp4;
        L = money / 10000;
    }

    /// <summary>
    /// 修改食物销售数量
    /// </summary>
    /// <param name="number"></param>
    /// <param name="menuId"></param>
    public void ChangeMenuSellNumber(long number, long menuId)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            MenuOwnBean itemData = menuList[i];
            if (itemData.menuId == menuId)
            {
                itemData.sellNumber += number;
                return;
            }
        }
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
    /// 修改建筑材料数量
    /// </summary>
    public void ChangeBuildItem(long buildId, long number)
    {
        ChangeItem(buildId, number, buildItemList);
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

}