﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemGameCalendarCpt : BaseMonoBehaviour
{
    public Image ivBackground;
    public Text tvDay;
    public Text tvDetails;

    public DateInfoBean dateInfo;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="dateInfo"></param>
    public void SetData(DateInfoBean dateInfo)
    {
        if (dateInfo == null)
            return;
        this.dateInfo = dateInfo;
        SetDay(dateInfo.day);
        SetDetails(dateInfo.content);
    }

    public void SetItemStatus(bool isCurrentDay)
    {
        if (isCurrentDay)
        {
            ivBackground.DOKill();
            ivBackground.color = Color.white;
            ivBackground.DOColor(new Color(1f, 0.5f, 0.5f),1);
            tvDay.DOKill();
            tvDay.transform.localScale=new Vector3(1,1,1);
            tvDay.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1).From().SetEase(Ease.OutBack);
        }
        else
        {
            ivBackground.color = Color.white;
        }
    }



    /// <summary>
    /// 设置天数
    /// </summary>
    /// <param name="day"></param>
    public void SetDay(int day)
    {
        if (tvDay != null)
            tvDay.text = day+"";
    }

    /// <summary>
    /// 设置详情说明
    /// </summary>
    /// <param name="content"></param>
    public void SetDetails(string content)
    {
        if (tvDetails != null)
            tvDetails.text = content + "";
    }

    
}

