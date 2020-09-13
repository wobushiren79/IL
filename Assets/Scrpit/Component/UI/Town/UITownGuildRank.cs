using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using Steamworks;

public class UITownGuildRank : UIBaseRank
{
    public override void SetLocalData()
    {
        base.SetLocalData();
        long score = GetScoreByType(rankType);
        CreateLocalItem(score);
    }

    /// <summary>
    /// 按钮-更新数据
    /// </summary>
    public override void OnClickForUpdate()
    {
        base.OnClickForUpdate();
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        long score = GetScoreByType( rankType);
        int intScore = 0;
        if (score > int.MaxValue)
        {
            intScore = int.MaxValue;
        }
        else
        {
            intScore = (int)score;
        }
        string innName = gameData.GetInnAttributesData().innName;
        string playerName = gameData.userCharacter.baseInfo.name;
        steamHandler.SetGetLeaderboardData(rankTypeId, intScore, innName + "-" + playerName , this);
    }

    public long GetScoreByType(RankTypeEnum rankType)
    {
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        long score = 0;
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                score = userAchievement.ownMoneyS;
                break;
            case RankTypeEnum.NumberOrderForFood:
                score = userAchievement.GetNumberForAllCustomerFood();
                break;
            case RankTypeEnum.NumberOrderForHotel:
                score = userAchievement.GetNumberForAllCustomerHotel();
                break;
            case RankTypeEnum.NumberPraiseAnger:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Anger);
                break;
            case RankTypeEnum.NumberPraiseExcited:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Excited);
                break;
            case RankTypeEnum.TimePlay:
                score = gameData.playTime.GetTimeForTotalS();
                break;
            case RankTypeEnum.MaxDayGetMoneyForFoodS:
                score = userAchievement.maxDayGetMoneyS;
                break;
            case RankTypeEnum.MaxDayGetMoneyForHotelS:
                score = userAchievement.maxDayGetMoneyForHotelS;
                break;
            case RankTypeEnum.MaxDayCompleteOrderForFood:
                score = userAchievement.maxDayCompleteOrder;
                break;
            case RankTypeEnum.MaxDayCompleteOrderForHotel:
                score = userAchievement.maxDayCompleteOrderForHotel;
                break;
        }
        return score;
    }

    /// <summary>
    /// 按钮-刷新数据
    /// </summary>
    public override void OnClickForRefresh()
    {
        base.OnClickForRefresh();

        steamHandler.GetLeaderboardDataForUser(rankTypeId, this);
        steamHandler.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }




}