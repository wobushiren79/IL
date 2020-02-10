using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MenuOwnBean
{
    public long menuId;
    public bool isSell = true;//是否开放售卖
    public long sellNumber;//销售数量

    public long sellMoneyL;
    public long sellMoneyM;
    public long sellMoneyS;

    public MenuOwnBean()
    {
    }

    public MenuOwnBean(long menuId)
    {
        this.menuId = menuId;
    }

    /// <summary>
    /// 获取菜品等级
    /// </summary>
    /// <returns></returns>
    public string GetMenuLevel(out int level)
    {
        string levelStr = "";
        level = 0;
        if (sellNumber >= 100 && sellNumber < 1000)
        {
            levelStr = GameCommonInfo.GetUITextById(101);
            level = 1;
        }
        else if (sellNumber >= 1000 && sellNumber < 10000)
        {
            levelStr = GameCommonInfo.GetUITextById(102);
            level = 2;
        }
        else if (sellNumber >= 10000 && sellNumber < 100000)
        {
            levelStr = GameCommonInfo.GetUITextById(103);
            level = 3;
        }
        else
        {
        }
        return levelStr;
    }
}