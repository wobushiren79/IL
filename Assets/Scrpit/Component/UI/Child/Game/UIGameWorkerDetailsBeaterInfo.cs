﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsBeaterInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{
    [Header("数据")]
    public CharacterWorkerForBeaterBean beaterInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="beaterInfo"></param>
    public void SetData(CharacterWorkerForBeaterBean beaterInfo)
    {
        this.beaterInfo = beaterInfo;
        CptUtil.RemoveChildsByActive(objItemContent);
        AddFightNumber(beaterInfo.fightTotalNumber);
        //AddFightTime(beaterInfo.fightTotalTime);
        AddFightWinNumber(beaterInfo.fightWinNumber);
        AddFightLoseNumber(beaterInfo.fightLoseNumber);
        //AddRestTime(beaterInfo.restTotalTime);
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void AddFightNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_beater_pro_2");
        CreateTextItem(spIcon,TextHandler.Instance.manager.GetTextById(327), number+"");
    }

    /// <summary>
    /// 设置打架时间
    /// </summary>
    /// <param name="time"></param>
    public void AddFightTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(328), time + TextHandler.Instance.manager.GetTextById(38));
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void AddFightWinNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("expression_love");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(329), number + "");
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void AddFightLoseNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("expression_dead");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(330), number + "");
    }


    /// <summary>
    /// 设置休息时间
    /// </summary>
    /// <param name="time"></param>
    public void AddRestTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(331), time + TextHandler.Instance.manager.GetTextById(38));
    }
}