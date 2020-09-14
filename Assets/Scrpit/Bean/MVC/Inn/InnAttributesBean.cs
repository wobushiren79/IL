using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnAttributesBean
{
    public string innName;//客栈名称
    public int innLevel;//客栈等级 （天地人1-3星）

    public float aesthetics;//客栈美观数
    public int richness;//菜品丰富度
    public int praise;//好评

    /// <summary>
    /// 客栈升级
    /// </summary>
    public void SetInnLevelUp()
    {
        GetNextInnLevel(out int nextLevelTitle, out int nextLevelStar);
        innLevel = nextLevelTitle * 10 + nextLevelStar;
    }

    /// <summary>
    /// 获取等级名称
    /// </summary>
    /// <param name="levelTitle"></param>
    /// <param name="levelStar"></param>
    /// <returns></returns>
    public string GetInnLevelStr(int levelTitle, int levelStar)
    {
        string levelTitleStr = "";
        string levelStarStr = "";
        switch (levelTitle)
        {
            case 1:
                levelTitleStr = GameCommonInfo.GetUITextById(2007);
                break;
            case 2:
                levelTitleStr = GameCommonInfo.GetUITextById(2008);
                break;
            case 3:
                levelTitleStr = GameCommonInfo.GetUITextById(2009);
                break;
        }

        switch (levelStar)
        {
            case 1:
                levelStarStr = GameCommonInfo.GetUITextById(2010);
                break;
            case 2:
                levelStarStr = GameCommonInfo.GetUITextById(2011);
                break;
            case 3:
                levelStarStr = GameCommonInfo.GetUITextById(2012);
                break;
            case 4:
                levelStarStr = GameCommonInfo.GetUITextById(2013);
                break;
            case 5:
                levelStarStr = GameCommonInfo.GetUITextById(2014);
                break;
        }
        return levelTitleStr + levelStarStr;
    }

    /// <summary>
    /// 获取客栈等级
    /// </summary>
    /// <param name="levelTitle"></param>
    /// <param name="levelStar"></param>
    /// <returns></returns>
    public string GetInnLevel(out int levelTitle, out int levelStar)
    {
        levelStar = (innLevel % 10);
        levelTitle = (innLevel % 100) / 10;
        return GetInnLevelStr(levelTitle, levelStar);
    }

    /// <summary>
    /// 获取下一客栈等级
    /// </summary>
    /// <param name="levelTitle"></param>
    /// <param name="levelStar"></param>
    /// <returns></returns>
    public string GetNextInnLevel(out int nextLevelTitle, out int nextLevelStar)
    {
        int levelStar = (innLevel % 10);
        int levelTitle = (innLevel % 100) / 10;

        nextLevelStar = 1;
        nextLevelTitle = 1;
        if (levelStar + 1 > 5)
        {
            nextLevelStar = 1;
            nextLevelTitle = levelTitle + 1;
        }
        else
        {
            nextLevelStar = levelStar + 1;
            if (levelTitle == 0)
                levelTitle = 1;
            nextLevelTitle = levelTitle;
        }

        return GetInnLevelStr(nextLevelTitle, nextLevelStar);
    }

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
        LimitRichnessMax();
    }

    /// <summary>
    /// 设置美观值
    /// </summary>
    public void SetAesthetics(long aesthetics)
    {
        this.aesthetics = aesthetics;
        LimitAestheticsMax();
    }

    public void SetAesthetics(InnBuildManager innBuildManager, InnBuildBean innBuildData)
    {
        if (innBuildManager == null || innBuildData == null)
            return;
        this.aesthetics = 0;
        List<InnResBean> listFurnitureData = innBuildData.GetFurnitureList(1);
        List<InnResBean> listFloorData = innBuildData.GetFloorList(1);
        List<InnResBean> listWallData = innBuildData.GetWallList(1);

        List<InnResBean> listData = new List<InnResBean>();
        listData.AddRange(listFurnitureData);
        listData.AddRange(listFloorData);
        listData.AddRange(listWallData);

        foreach (InnResBean itemData in listData)
        {
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(itemData.id);
            aesthetics += buildItem.aesthetics;
        }
        aesthetics = (float)Math.Round(aesthetics, 2);
        LimitAestheticsMax();
    }

    /// <summary>
    /// 增加好评
    /// </summary>
    /// <param name="addPraise"></param>
    public void AddPraise(int addPraise)
    {
        praise += addPraise;
        if (praise < 0)
        {
            praise = 0;
        }
        if (praise > 1000)
        {
            praise = 1000;
        }
    }

    /// <summary>
    /// 获取美观值
    /// </summary>
    /// <returns></returns>
    public void GetAesthetics(out float maxAesthetics, out float aesthetics)
    {
        maxAesthetics = 0;
        aesthetics = this.aesthetics;
        GetInnLevel(out int levelTitle, out int levelStar);
        maxAesthetics = levelTitle == 0 ? (50) : ((levelTitle - 1) * 350 + levelStar * 50 + 50);
    }

    /// <summary>
    /// 获取菜品丰富度
    /// </summary>
    /// <param name="maxRichness"></param>
    /// <param name="richness"></param>
    public void GetRichness(out int maxRichness, out int richness)
    {
        maxRichness = 0;
        richness = this.richness;
        GetInnLevel(out int levelTitle, out int levelStar);
        maxRichness = levelTitle == 0 ? (5) : ((levelTitle - 1) * 20 + levelStar * 4 + 5);
    }

    /// <summary>
    /// 获取好评
    /// </summary>
    /// <returns></returns>
    public void GetPraise(out int maxPraise, out int praise)
    {
        maxPraise = 1000;
        praise = this.praise;
    }

    /// <summary>
    /// 限制美观上线
    /// </summary>
    public void LimitAestheticsMax()
    {
        //限制上限
        GetAesthetics(out float maxAesthetics, out float aesthetics);
        if (aesthetics > maxAesthetics)
        {
            this.aesthetics = maxAesthetics;
        }
    }

    /// <summary>
    /// 限制菜品丰富度上限
    /// </summary>
    public void LimitRichnessMax()
    {
        //限制上限
        GetRichness(out int maxRichness, out int richness);
        if(richness> maxRichness)
        {
            this.richness = maxRichness;
        }
    }

    /// <summary>
    /// 计算顾客想要吃饭的概率
    /// </summary>
    /// <returns></returns>
    public float CalculationCustomerWantRate(CustomerTypeEnum customerType)
    {
        float rate = 0;
        //美观所占比重
        GetAesthetics(out float maxAesthetics,out float aesthetics);
        float rateAesthetics = aesthetics / maxAesthetics;
        if (rateAesthetics > 1)
            rateAesthetics = 1;
        //点赞率所占比重
        GetPraise(out int maxPraise,out int praise);
        float ratePraise = (float)praise / maxPraise;
        if (ratePraise > 1)
            ratePraise = 1;
        //菜品丰富度所占比重
        GetRichness(out int maxRichness,out int richness);
        float rateRichness = richness / maxRichness;
        if (rateRichness > 1)
            rateRichness = 1;

        if (customerType == CustomerTypeEnum.Normal)
        {
            rate = 0.36f * rateRichness + 0.3f * ratePraise + 0.3f * rateAesthetics + 0.04f;
        }
        else if (customerType == CustomerTypeEnum.Team)
        {
            rate = 0.36f * rateRichness + 0.3f * ratePraise + 0.3f * rateAesthetics + 0.04f;
        }
        else
        {
            rate = 0.36f * rateRichness + 0.3f * ratePraise + 0.3f * rateAesthetics + 0.04f;
        }
        rate = rate / 2f;
        return rate;
    }

    /// <summary>
    /// 计算生成团队顾客概率
    /// </summary>
    /// <returns></returns>
    public float CalculationTeamCustomerBuildRate()
    {
        float rate = 0;
        //美观所占比重
        GetAesthetics(out float maxAesthetics, out float aesthetics);
        float rateAesthetics = aesthetics / maxAesthetics;
        if (rateAesthetics > 1)
            rateAesthetics = 1;
        //点赞率所占比重
        GetPraise(out int maxPraise, out int praise);
        float ratePraise = (float)praise / maxPraise;
        if (ratePraise > 1)
            ratePraise = 1;
        //菜品丰富度所占比重
        GetRichness(out int maxRichness, out int richness);
        float rateRichness = (float)richness / maxRichness;
        if (rateRichness > 1)
            rateRichness = 1;
        rate = 0.36f * rateRichness + 0.3f * ratePraise + 0.3f * rateAesthetics + 0.04f;
        rate = rate * 0.1f;
        return rate;
    }

    /// <summary>
    /// 计算生成团队顾客概率
    /// </summary>
    /// <returns></returns>
    public float CalculationCustomerBuildRate()
    {
        float rate = 0;
        //美观所占比重
        GetAesthetics(out float maxAesthetics, out float aesthetics);
        float rateAesthetics = aesthetics / maxAesthetics;
        if (rateAesthetics > 1)
            rateAesthetics = 1;
        //点赞率所占比重
        GetPraise(out int maxPraise, out int praise);
        float ratePraise = (float)praise / maxPraise;
        if (ratePraise > 1)
            ratePraise = 1;
        //菜品丰富度所占比重
        GetRichness(out int maxRichness, out int richness);
        float rateRichness = (float)richness / maxRichness;
        if (rateRichness > 1)
            rateRichness = 1;
        rate = 0.25f * rateRichness + 0.25f * ratePraise + 0.25f * rateAesthetics + 0.25f;
        return rate;
    }

    /// <summary>
    /// 计算生成顾客住宿概率
    /// </summary>
    /// <returns></returns>
    public float CalculationCustomerForHotelRate(InnBuildBean innBuild)
    {
        //如果没有2楼则不生成顾客
        float rate = 0;
        if (innBuild.buildSecondLevel == 0)
        {
            return rate;
        }
        //美观所占比重
        GetAesthetics(out float maxAesthetics, out float aesthetics);
        float rateAesthetics = aesthetics / maxAesthetics;
        if (rateAesthetics > 1)
            rateAesthetics = 1;
        //点赞率所占比重
        GetPraise(out int maxPraise, out int praise);
        float ratePraise = (float)praise / maxPraise;
        if (ratePraise > 1)
            ratePraise = 1;
        rate = 0.5f * ratePraise + 0.5f * rateAesthetics;
        return rate * 0.1f;
    }
}