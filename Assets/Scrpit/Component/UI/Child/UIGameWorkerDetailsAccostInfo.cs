using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class UIGameWorkerDetailsAccostInfo : BaseMonoBehaviour
{
    public Text tvAccostTotalNumber;
    public Text tvAccostSuccessNumber;
    public Text tvAccostFailNumber;

    [Header("数据")]
    public CharacterWorkerForAccostBean accostInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="accostInfo"></param>
    public void SetData(CharacterWorkerForAccostBean accostInfo)
    {
        this.accostInfo = accostInfo;
        SetAccostTotalNumber(accostInfo.accostTotalNumber);
        SetAccostSuccessNumber(accostInfo.accostSuccessNumber);
        SetAccostFailNumber(accostInfo.accostFailNumber);
    }

    /// <summary>
    /// 设置成功招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void SetAccostSuccessNumber(long number)
    {
        if (tvAccostSuccessNumber != null)
            tvAccostSuccessNumber.text = number + "";
    }

    /// <summary>
    /// 设置失败招揽次数
    /// </summary>
    /// <param name="number"></param>
    public void SetAccostFailNumber(long number)
    {
        if (tvAccostFailNumber != null)
            tvAccostFailNumber.text = number + "";
    }

    /// <summary>
    /// 设置总共数量 
    /// </summary>
    /// <param name="nunber"></param>
    public void SetAccostTotalNumber(long nunber)
    {
        if (tvAccostTotalNumber != null)
            tvAccostTotalNumber.text = nunber + "";
    }
}