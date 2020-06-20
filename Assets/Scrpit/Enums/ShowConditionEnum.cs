using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public enum ShowConditionEnum
{
    InnLevel,
    TimeForYear,
    TimeForYearAfter,
    TimeForMonth,
    TimeForDay,
    TimeForHour,
    NpcFavorability,//NPC好感
}

public class ShowConditionBean : DataBean<ShowConditionEnum>
{
    public bool isCondition;

    public ShowConditionBean() : base(ShowConditionEnum.InnLevel, "")
    {

    }
}

public class ShowConditionTools : DataTools
{

    /// <summary>
    /// 检测是否全部满足
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckIsMeetAllCondition(GameDataBean gameData, string data)
    {
        List<ShowConditionBean> listConditionData = GetListConditionData(data);
        foreach (var itemConditionData in listConditionData)
        {
            GetConditionDetails(gameData, itemConditionData);
            if (!itemConditionData.isCondition)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取出现条件
    /// </summary>
    /// <returns></returns>
    public static List<ShowConditionBean> GetListConditionData(string data)
    {
        List<ShowConditionBean> listConditionData = GetListData<ShowConditionBean, ShowConditionEnum>(data);
        return listConditionData;
    }

    /// <summary>
    /// 获取出现详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static ShowConditionBean GetConditionDetails(GameDataBean gameData, ShowConditionBean conditionData)
    {
        switch (conditionData.dataType)
        {
            case ShowConditionEnum.InnLevel:
                GetConditionDetailsForInnLevel(gameData, conditionData);
                break;
            case ShowConditionEnum.TimeForYear:
            case ShowConditionEnum.TimeForMonth:
            case ShowConditionEnum.TimeForDay:
            case ShowConditionEnum.TimeForHour:
            case ShowConditionEnum.TimeForYearAfter:
                GetConditionDetailsForTime(gameData, conditionData);
                break;
            case ShowConditionEnum.NpcFavorability:
                GetConditionDetailsForNpcFavorability(gameData, conditionData);
                break;
        }
        return conditionData;
    }

    /// <summary>
    /// 获取出现详情-等级判断
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="conditionData"></param>
    /// <returns></returns>
    private static ShowConditionBean GetConditionDetailsForInnLevel(GameDataBean gameData, ShowConditionBean conditionData)
    {
        gameData.GetInnAttributesData().GetInnLevel(out int levelTitle, out int levelStar);
        int innConditionLevel = int.Parse(conditionData.data);
        int innLevel = levelTitle * 10 + levelStar;
        if (innLevel < innConditionLevel)
        {
            conditionData.isCondition = false;
        }
        else
        {
            conditionData.isCondition = true;
        }
        return conditionData;
    }

    /// <summary>
    /// 获取出现详情-时间
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="conditionData"></param>
    /// <returns></returns>
    protected static ShowConditionBean GetConditionDetailsForTime(GameDataBean gameData, ShowConditionBean conditionData)
    {
        List<int> listTime = StringUtil.SplitBySubstringForArrayInt(conditionData.data, ',').ToList();
        TimeBean gameTime = gameData.gameTime;
        switch (conditionData.dataType)
        {
            case ShowConditionEnum.TimeForYear:
                if (listTime.Contains(gameTime.year))
                {
                    conditionData.isCondition = true;
                }
                break;
            case ShowConditionEnum.TimeForYearAfter:
                int year = listTime[0];
                if (gameTime.year>= year)
                {
                    conditionData.isCondition = true;
                }
                break;
            case ShowConditionEnum.TimeForMonth:
                if (listTime.Contains(gameTime.month))
                {
                    conditionData.isCondition = true;
                }
                break;
            case ShowConditionEnum.TimeForDay:
                if (listTime.Contains(gameTime.day))
                {
                    conditionData.isCondition = true;
                }
                break;
            case ShowConditionEnum.TimeForHour:
                if (listTime.Contains(gameTime.hour))
                {
                    conditionData.isCondition = true;
                }
                break;
        }
        return conditionData;
    }

    /// <summary>
    /// 获取出现详情  NPC好感
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="conditionData"></param>
    /// <returns></returns>
    protected static ShowConditionBean GetConditionDetailsForNpcFavorability(GameDataBean gameData, ShowConditionBean conditionData)
    {
        long[] listData = StringUtil.SplitBySubstringForArrayLong(conditionData.data, ',');
        long npcId = listData[0];
        long npcFavorability = listData[1];
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(npcId);
        if (characterFavorability.favorability >= npcFavorability)
        {
            conditionData.isCondition = true;
        }
        else
        {
            conditionData.isCondition = false;
        }
        return conditionData;
    }

}