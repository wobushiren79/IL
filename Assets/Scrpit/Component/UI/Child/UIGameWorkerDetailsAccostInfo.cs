using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class UIGameWorkerDetailsAccostInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{
    [Header("数据")]
    public CharacterWorkerForAccostBean accostInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="accostInfo"></param>
    public void SetData(CharacterWorkerForAccostBean accostInfo)
    {
        this.accostInfo = accostInfo;
        CptUtil.RemoveChildsByActive(objItemContent);
        AddAccostTotalNumber(accostInfo.accostTotalNumber);
        AddAccostSuccessNumber(accostInfo.accostSuccessNumber);
        AddAccostFailNumber(accostInfo.accostFailNumber);
        //AddAccostTime(accostInfo.accostTotalTime);
    }




    /// <summary>
    /// 设置总共数量 
    /// </summary>
    /// <param name="nunber"></param>
    public void AddAccostTotalNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(323), number + "");
    }

    /// <summary>
    /// 设置成功招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostSuccessNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(324), number + "");
    }


    /// <summary>
    /// 设置失败招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostFailNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(325), number + "");
    }



    /// <summary>
    /// 设置总共数量 
    /// </summary>
    /// <param name="nunber"></param>
    public void AddAccostTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(326), time + GameCommonInfo.GetUITextById(38));

    }
}