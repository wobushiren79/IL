using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnAttributesBean
{
    public long aesthetics;//客栈美观数
    public float praise;//好评率
    public long richness;//菜品丰富度

    /// <summary>
    /// 刷新菜品数据
    /// </summary>
    /// <param name="listOwnMenu"></param>
    public void RefreshRichNess(List<MenuOwnBean> listOwnMenu)
    {
        //菜品丰富度数据刷新
        richness = 0;
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemData = listOwnMenu[i];
            if (itemData.isSell)
            {
                richness += 1;
            }
        }
    }

    /// <summary>
    /// 设置美观值
    /// </summary>
    public void SetAesthetics(long aesthetics)
    {
        this.aesthetics = aesthetics;
    }
    public void SetAesthetics(InnBuildManager innBuildManager, InnBuildBean innBuildData)
    {
        if (innBuildManager == null || innBuildData == null)
            return;
        this.aesthetics = 0;
        List<InnResBean> listData = innBuildData.GetFurnitureList();
        foreach (InnResBean itemData in listData)
        {
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(itemData.id);
            aesthetics += buildItem.aesthetics;
        }
    }

    /// <summary>
    /// 增加好评
    /// </summary>
    /// <param name="addPraise"></param>
    public void AddPraise(float addPraise)
    {
        praise += addPraise;
        if (praise < 0)
        {
            praise = 0;
        }
        if (praise > 100)
        {
            praise = 100;
        }
    }

    /// <summary>
    /// 获取美观值
    /// </summary>
    /// <returns></returns>
    public long GetAesthetics()
    {
        return aesthetics;
    }

    /// <summary>
    /// 获取好评
    /// </summary>
    /// <returns></returns>
    public float GetPraise()
    {
        return praise;
    }

    /// <summary>
    /// 计算顾客想要吃饭的概率
    /// </summary>
    /// <returns></returns>
    public float CalculationCustomerWantRate()
    {
        float rate = 0;
        //美观所占比重
        float rateAesthetics = aesthetics * 0.001f;
        if (rateAesthetics > 1)
            rateAesthetics = 1;
        //点赞率所占比重
        float ratePraise = (praise);
        if (ratePraise > 1)
            ratePraise = 1;
        //菜品丰富度所占比重
        float rateRichness = (richness * 0.01f);
        if (rateRichness > 1)
            rateRichness = 1;
        rate = 0.32f * rateRichness + 0.32f* ratePraise + 0.32f* rateAesthetics + 0.04f;
        return rate;
    }
}