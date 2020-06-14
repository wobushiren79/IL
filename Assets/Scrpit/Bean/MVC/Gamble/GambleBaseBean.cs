using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GambleBaseBean
{
    public GambleTypeEnum gambleType;
    public GambleStatusType gambleStatus;

    //下注金额
    public long betForMoneyL;
    public long betForMoneyM;
    public long betForMoneyS;

    //下注上限
    public long betMaxForMoneyL;
    public long betMaxForMoneyM;
    public long betMaxForMoneyS;

    public bool isWin = false;

    //奖励倍率
    public float winRewardRate = 2;



    public GambleBaseBean(GambleTypeEnum gambleType)
    {
        this.gambleType = gambleType;
        ResetData();
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        betForMoneyL = 0;
        betForMoneyM = 0;
        betForMoneyS = 0;
        this.gambleStatus = GambleStatusType.Prepare;
        isWin = false;
    }

    /// <summary>
    /// 设置输赢
    /// </summary>
    /// <param name="isWin"></param>
    public void SetIsWin(bool isWin)
    {
        this.isWin = isWin;
    }

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="gambleStatus"></param>
    public void SetGambleStatus(GambleStatusType gambleStatus)
    {
        this.gambleStatus = gambleStatus;
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public GambleStatusType GetGambleStatus()
    {
        return gambleStatus;
    }

    /// <summary>
    /// 设置下注上限
    /// </summary>
    /// <param name="betMaxForMoneyL"></param>
    /// <param name="betMaxForMoneyM"></param>
    /// <param name="betMaxForMoneyS"></param>
    public void SetBetMax(long betMaxForMoneyL, long betMaxForMoneyM, long betMaxForMoneyS)
    {
        this.betMaxForMoneyL = betMaxForMoneyL;
        this.betMaxForMoneyM = betMaxForMoneyM;
        this.betMaxForMoneyS = betMaxForMoneyS;
    }


    /// <summary>
    /// 获取赌博名称
    /// </summary>
    /// <param name="gambleName"></param>
    public void GetGambleName(out string gambleName)
    {
        gambleName = "";
        switch (gambleType)
        {
            case GambleTypeEnum.TrickyCup:
                gambleName = GameCommonInfo.GetUITextById(601);
                break;
            case GambleTypeEnum.TrickySize:
                gambleName = GameCommonInfo.GetUITextById(602);
                break;
        }
    }


}