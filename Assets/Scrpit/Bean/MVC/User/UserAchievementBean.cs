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

    //团队顾客数据
    public List<UserCustomerBean> listForTeamCustomerData = new List<UserCustomerBean>();
    public List<UserCustomerBean> listForFriendCustomerData = new List<UserCustomerBean>();

    //评价
    public long praiseForExcited;
    public long praiseForHappy;
    public long praiseForOkay;
    public long praiseForOrdinary;
    public long praiseForDisappointed;
    public long praiseForAnger;

    //是否打开过帮助
    public bool isOpenedHelp = false;

    //单日最高收入
    public long maxDayGetMoneyL;
    public long maxDayGetMoneyM;
    public long maxDayGetMoneyS;

    //单日最高完成定完
    public long maxDayCompleteOrder;

    /// <summary>
    /// 设置每日最大赚取金钱
    /// </summary>
    /// <param name="maxDayGetMoneyL"></param>
    /// <param name="maxDayGetMoneyM"></param>
    /// <param name="maxDayGetMoneyS"></param>
    public void SetMaxDayGetMoney(long maxDayGetMoneyL, long maxDayGetMoneyM, long maxDayGetMoneyS)
    {
        if (this.maxDayGetMoneyL == 0 || maxDayGetMoneyL > this.maxDayGetMoneyL)
        {
            this.maxDayGetMoneyL = maxDayGetMoneyL;
        }
        if (this.maxDayGetMoneyM == 0 || maxDayGetMoneyM > this.maxDayGetMoneyM)
        {
            this.maxDayGetMoneyM = maxDayGetMoneyM;
        }
        if (this.maxDayGetMoneyS == 0 || maxDayGetMoneyS > this.maxDayGetMoneyS)
        {
            this.maxDayGetMoneyS = maxDayGetMoneyS;
        }
    }

    /// <summary>
    /// 设置每日最大完成订单
    /// </summary>
    /// <param name="completeOrder"></param>
    public void SetMaxDayCompleteOrder(long completeOrder)
    {
        if (maxDayCompleteOrder == 0 || completeOrder > maxDayCompleteOrder)
        {
            maxDayCompleteOrder = completeOrder;
        }
    }

    /// <summary>
    /// 获取团队顾客数据
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public UserCustomerBean GetCustomerData(CustomerTypeEnum customerType, string teamId)
    {
        List<UserCustomerBean> listData = null;
        switch (customerType)
        {
            case CustomerTypeEnum.Friend:
                listData = listForFriendCustomerData;
                break;
            case CustomerTypeEnum.Team:
                listData = listForTeamCustomerData;
                break;
        }
        if (listData == null)
            return null;
        foreach (UserCustomerBean itemData in listData)
        {
            if (teamId.Equals(itemData.id))
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 是否包含成就
    /// </summary>
    /// <param name="achId"></param>
    /// <returns></returns>
    public bool CheckHasAchievement(long[]  achIds)
    {
        foreach (long achId in  achIds)
        {
            if (!listAchievement.Contains(achId))
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckHasAchievement(long achId)
    {
        return CheckHasAchievement(new long[] { achId });
    }

    /// <summary>
    /// 检测是否有指定团队顾客
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public bool CheckHasTeamCustomer(string teamId)
    {
        if (listForTeamCustomerData == null)
            listForTeamCustomerData = new List<UserCustomerBean>();
        foreach (UserCustomerBean itemCustomer in listForTeamCustomerData)
        {
            if (teamId.Equals(itemCustomer.id))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckHasTeamCustomerLoveMenu(long teamId, long menuId)
    {
        if (listForTeamCustomerData == null)
            listForTeamCustomerData = new List<UserCustomerBean>();
        foreach (UserCustomerBean itemCustomer in listForTeamCustomerData)
        {
            if (itemCustomer.id.Equals(teamId + "") && itemCustomer.CheckHasMenu(menuId))
            {
                return true;
            }
        }
        return false;
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
    public void AddPraise(PraiseTypeEnum praiseType, int number)
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

    public long GetPraiseNumber(PraiseTypeEnum praiseType)
    {
        switch (praiseType)
        {
            case PraiseTypeEnum.Excited:
                return praiseForExcited;
            case PraiseTypeEnum.Happy:
                return praiseForHappy;
            case PraiseTypeEnum.Okay:
                return praiseForOkay;
            case PraiseTypeEnum.Ordinary:
                return praiseForOrdinary;
            case PraiseTypeEnum.Disappointed:
                return praiseForDisappointed;
            case PraiseTypeEnum.Anger:
                return praiseForAnger;
        }
        return 0;
    }

    /// <summary>
    /// 记录顾客
    /// </summary>
    public void AddNumberForCustomer(CustomerTypeEnum customerType, string id, int number)
    {
        List<UserCustomerBean> listData = null;
        switch (customerType)
        {
            case CustomerTypeEnum.Normal:
                numberForNormalCustomer += number;
                break;
            case CustomerTypeEnum.Team:
                numberForTeamCustomer += number;
                listData = listForTeamCustomerData;
                break;
            case CustomerTypeEnum.Friend:
                numberForFriendsCustomer += number;
                listData = listForFriendCustomerData;
                break;
        }
        if (listData == null)
            return;
        bool hasData = false;
        foreach (UserCustomerBean itemCustomerData in listData)
        {
            if (itemCustomerData.id.Equals(id))
            {
                itemCustomerData.AddNumber(number);
                hasData = true;
                break;
            }
        }
        if (!hasData)
        {
            UserCustomerBean userCustomerData = new UserCustomerBean();
            userCustomerData.id = id;
            userCustomerData.AddNumber(number);
            listData.Add(userCustomerData);
        }
    }

    public void AddMenuForCustomer(CustomerTypeEnum customerType, string id, long menuId)
    {
        switch (customerType)
        {
            case CustomerTypeEnum.Normal:
                break;
            case CustomerTypeEnum.Team:
                foreach (UserCustomerBean itemCustomerData in listForTeamCustomerData)
                {
                    if (itemCustomerData.id.Equals(id))
                    {
                        itemCustomerData.AddMenu(menuId);
                        break;
                    }
                }
                break;
            case CustomerTypeEnum.Friend:
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


        switch (customerType)
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