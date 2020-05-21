using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserAchievementBean
{
    public List<long> listAchievement = new List<long>();//解锁成就列表

    public long ownMoneyL;
    public long ownMoneyM;
    public long ownMoneyS;

    public long ownTrophyElementary; //竞技场初级奖杯
    public long ownTrophyIntermediate;//竞技场中级奖杯
    public long ownTrophyAdvanced;//竞技场高级奖杯
    public long ownTrophyLegendary;//竞技场传说奖杯

    public long ownIngOilsalt;//油盐
    public long ownIngMeat;//肉类
    public long ownIngRiverfresh;//河鲜
    public long ownIngSeafood;//海鲜
    public long ownIngVegetables;//蔬菜
    public long ownIngMelonfruit;//瓜果
    public long ownIngWaterwine;//酒水
    public long ownIngFlour;//面粉

    //公会硬币
    public long ownGuildCoin;

    //顾客数量
    public long numberForNormalCustomer;
    public long numberForTeamCustomer;
    public long numberForFriendsCustomer;

    //评价
    public long praiseForExcited;
    public long praiseForHappy;
    public long praiseForOkay;
    public long praiseForOrdinary;
    public long praiseForDisappointed;
    public long praiseForAnger;
    


    /// <summary>
    /// 是否包含该成就
    /// </summary>
    /// <param name="achId"></param>
    /// <returns></returns>
    public bool CheckHasAchievement(long achId)
    {
        return listAchievement.Contains(achId);
    }

    /// <summary>
    /// 添加成就
    /// </summary>
    /// <param name="achId"></param>
    public void AddAchievement(long achId)
    {
        listAchievement.Add(achId);
    }

    /// <summary>
    /// 增加好评
    /// </summary>
    /// <param name="praiseType"></param>
    /// <param name="number"></param>
    public void AddPraise(PraiseTypeEnum praiseType,int number)
    {
        switch (praiseType)
        {
            case PraiseTypeEnum.Excited:
                praiseForExcited += number;
                break;
            case PraiseTypeEnum.Happy:
                praiseForHappy += number;
                break;
            case PraiseTypeEnum.Okay:
                praiseForOkay += number;
                break;
            case PraiseTypeEnum.Ordinary:
                praiseForOrdinary += number;
                break;
            case PraiseTypeEnum.Disappointed:
                praiseForDisappointed += number;
                break;
            case PraiseTypeEnum.Anger:
                praiseForAnger += number;
                break;
        }
    }

    /// <summary>
    /// 记录顾客
    /// </summary>
    public void AddNumberForCustomer(CustomerTypeEnum customerType,int number)
    {
        switch (customerType)
        {
            case CustomerTypeEnum.Normal:
                numberForNormalCustomer += number;
                break;
            case CustomerTypeEnum.Team:
                numberForTeamCustomer += number;
                break;
            case CustomerTypeEnum.Friend:
                numberForFriendsCustomer += number;
                break;
        }
    }


    /// <summary>
    /// 返回所有顾客数量
    /// </summary>
    /// <returns></returns>
    public long GetNumberForAllCustomer()
    {
        return numberForNormalCustomer + numberForTeamCustomer + numberForFriendsCustomer;
    }

    /// <summary>
    /// 根据类型返回顾客数量
    /// </summary>
    /// <param name="customerType"></param>
    /// <returns></returns>
    public long GetNumberForCustomerByType(CustomerTypeEnum customerType)
    {
        switch(customerType)
        {
            case CustomerTypeEnum.Normal:
                return numberForNormalCustomer;
            case CustomerTypeEnum.Team:
                return numberForTeamCustomer;
            case CustomerTypeEnum.Friend:
                return numberForFriendsCustomer;
        }
        return 0;
    }
}