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
        AddAccostSolicitTotalNumber(accostInfo.accostTotalNumber);
        AddAccostSolicitSuccessNumber(accostInfo.accostSuccessNumber);
        AddAccostSolicitFailNumber(accostInfo.accostFailNumber);

        AddAccostGuideNumber(accostInfo.guideNumber);
        
    }

    /// <summary>
    /// 设置总共数量 
    /// </summary>
    /// <param name="nunber"></param>
    public void AddAccostSolicitTotalNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accost_pro_0");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(323), number + "");
    }

    /// <summary>
    /// 设置成功招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostSolicitSuccessNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accost_pro_0");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(324), number + "");
    }


    /// <summary>
    /// 设置失败招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostSolicitFailNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accost_pro_0");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(325), number + "");
    }

    /// <summary>
    /// 设置引路次数
    /// </summary>
    public void AddAccostGuideNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_accost_guide_pro_0");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(347), number + "");
    }
}