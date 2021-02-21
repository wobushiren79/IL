using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 事件触发条件
/// </summary>
public enum EventTriggerEnum
{
    Year,//年月日
    Month,
    Day,
    Favorability,//好感
    EventIds,//需触发事件ID
}

public class EventTriggerBean
{
    public EventTriggerEnum triggerType;
    public string triggerData;
    public bool isTrigger = false;

    public EventTriggerBean(EventTriggerEnum triggerType, string triggerData)
    {
        this.triggerType = triggerType;
        this.triggerData = triggerData;
    }
}

public class EventTriggerEnumTools
{
    /// <summary>
    /// 检测是否全部准备就绪
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckIsAllTrigger(GameDataBean gameData, string data)
    {
        if (CheckUtil.StringIsNull(data))
            return false;
        List<EventTriggerBean> listTriggerData = GetListTriggerData(data);
        foreach (var itemTriggerData in listTriggerData)
        {
            GetTriggerDetails(itemTriggerData, gameData);
            if (!itemTriggerData.isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<EventTriggerBean> GetListTriggerData(string data)
    {
        List<EventTriggerBean> listTriggerData = new List<EventTriggerBean>();
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            EventTriggerEnum triggerType = EnumUtil.GetEnum<EventTriggerEnum>(itemListData[0]);
            string triggerValue = itemListData[1];
            listTriggerData.Add(new EventTriggerBean(triggerType, triggerValue));
        }
        return listTriggerData;
    }

    /// <summary>
    /// 获取触发条件详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static EventTriggerBean GetTriggerDetails(EventTriggerBean triggerData, GameDataBean gameData)
    {
        switch (triggerData.triggerType)
        {
            case EventTriggerEnum.Year:
                GetTriggerDetailsForDate(triggerData, gameData.gameTime.year);
                break;
            case EventTriggerEnum.Month:
                GetTriggerDetailsForDate(triggerData, gameData.gameTime.month);
                break;
            case EventTriggerEnum.Day:
                GetTriggerDetailsForDate(triggerData, gameData.gameTime.day);
                break;
            case EventTriggerEnum.Favorability:
                GetTriggerDetailsForFavorability(triggerData, gameData);
                break;
            case EventTriggerEnum.EventIds:
                GetTriggerDetailsForEventIds(triggerData, gameData);
                break;
        }
        return triggerData;
    }

    /// <summary>
    /// 获取触发条件详情 好感
    /// </summary>
    /// <param name="triggerData"></param>
    /// <param name="gameData"></param>
    private static void GetTriggerDetailsForFavorability(EventTriggerBean triggerData, GameDataBean gameData)
    {
        long[] favorabilityData = StringUtil.SplitBySubstringForArrayLong(triggerData.triggerData, ',');
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(favorabilityData[0]);
        if (characterFavorability == null)
        {
            triggerData.isTrigger = false;
        }
        else
        {
            if (characterFavorability.favorabilityLevel < favorabilityData[1])
            {
                triggerData.isTrigger = false;
            }
            else
            {
                triggerData.isTrigger = true;
            }
        }
    }

    /// <summary>
    ///  获取触发条件详情 日期
    /// </summary>
    /// <param name="triggerData"></param>
    /// <param name="date"></param>
    private static void GetTriggerDetailsForDate(EventTriggerBean triggerData, int date)
    {
        int triggerDate = int.Parse(triggerData.triggerData);
        //如果没有设置日期 则默认触发
        if (triggerDate == 0)
        {
            triggerData.isTrigger = true;
            return;
        }
        if (triggerDate == date)
        {
            triggerData.isTrigger = true;
        }
        else
        {
            triggerData.isTrigger = false;
        }
    }

    /// <summary>
    /// 获取触发条件详情 已经触发过的事件ID
    /// </summary>
    /// <param name="triggerData"></param>
    /// <param name="gameData"></param>
    private static void GetTriggerDetailsForEventIds(EventTriggerBean triggerData, GameDataBean gameData)
    {
        long[] idsData = StringUtil.SplitBySubstringForArrayLong(triggerData.triggerData, ',');
        if (gameData.CheckTriggeredEvent(idsData))
        {
            triggerData.isTrigger = true;
        }
        else
        {
            triggerData.isTrigger = false;
        }
    }
}