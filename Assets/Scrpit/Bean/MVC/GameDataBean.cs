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
    public List<CharacterBean> workCharacterList=new List<CharacterBean>();//员工
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
    /// 修改建筑材料数量
    /// </summary>
    public void ChangeBuildItem(long buildId, long number)
    {
        ChangeItem(buildId, number, buildItemList);
    }

    public void ChangeItem(long buildId , long number, List<ItemBean> list)
    {
        bool hasData = false;
        for(int i = 0; i < list.Count; i++)
        {
            ItemBean item = list[i];
            if (item.itemId == buildId)
            {
                hasData = true;
                item.itemNumber += number;
                if (item.itemNumber <=0)
                {
                    item.itemNumber = 0;
                    list.RemoveAt(i);
                    i--;
                }
                break;
            }
        }
        if (!hasData&& number>0)
        {
            list.Add(new ItemBean(buildId,number));
        }
    }

}