using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnAttributesBean
{
    public string innName;//客栈名称
    public int innLevel;//客栈等级 （天地人1-5星  3 2 1）

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
        List<InnResBean> listFurnitureData = innBuildData.GetFurnitureList();
        List<InnResBean> listFloorData = innBuildData.GetFloorList();
        List<InnResBean> listWallData = innBuildData.GetWallList();

        List<InnResBean> listData = new List<InnResBean>();
        listData.AddRange(listFurnitureData);
        listData.AddRange(listFloorData);
        listData.AddRange(listWallData);

        foreach (InnResBean itemData in listData)
        {
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(itemData.id);
            aesthetics += buildItem.aesthetics;
        }
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
        maxAesthetics = levelTitle * 300 + levelStar * 50 + 100;
        //string level = "???";
        //if (aesthetics <= 100)
        //{
        //    level = GameCommonInfo.GetUITextById(120);
        //}
        //else if (aesthetics > 100&& aesthetics <= 200)
        //{
        //    level = GameCommonInfo.GetUITextById(121);
        //}
        //else if (aesthetics > 200 && aesthetics <= 300)
        //{
        //    level = GameCommonInfo.GetUITextById(122);
        //}
        //else if (aesthetics > 300 && aesthetics <= 400)
        //{
        //    level = GameCommonInfo.GetUITextById(123);
        //}
        //else if (aesthetics > 400 && aesthetics <= 500)
        //{
        //    level = GameCommonInfo.GetUITextById(124);
        //}
        //else if (aesthetics > 500 && aesthetics <= 1000)
        //{
        //    level = GameCommonInfo.GetUITextById(125);
        //}
        //else if (aesthetics > 1000 && aesthetics <= 2000)
        //{
        //    level = GameCommonInfo.GetUITextById(126);
        //}
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
        maxRichness = levelTitle * 15 + levelStar * 2 + 5;
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
        GetInnLevel(out int levelTitle, out int levelStar);
        float maxAesthetics = levelTitle * 300 + levelStar * 50 + 100;
        if (this.aesthetics > maxAesthetics)
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
        GetInnLevel(out int levelTitle, out int levelStar);
        int maxRichNess = levelTitle * 15 + levelStar * 2 + 5;
        if (this.richness > maxRichNess)
        {
            this.richness = maxRichNess;
        }
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
        float ratePraise = praise / (float)1000;
        if (ratePraise > 1)
            ratePraise = 1;
        //菜品丰富度所占比重
        float rateRichness = (richness * 0.02f);
        if (rateRichness > 1)
            rateRichness = 1;
        rate = 0.32f * rateRichness + 0.32f * ratePraise + 0.32f * rateAesthetics + 0.04f;
        rate = rate / 2f;
        return rate;
    }
}