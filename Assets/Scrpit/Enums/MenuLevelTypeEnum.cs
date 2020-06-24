using UnityEngine;
using UnityEditor;

public enum MenuLevelTypeEnum
{
    Init = 0,
    Star = 1,
    Moon = 2,
    Sun = 3,
}

public class MenuLevelTypeEnumTools
{
    public static string GetMenuLevelStr(MenuLevelTypeEnum menuLevel)
    {
        string levelStr = "";

        if (menuLevel == MenuLevelTypeEnum.Init)
        {
            levelStr = GameCommonInfo.GetUITextById(104);
        }
        else if (menuLevel == MenuLevelTypeEnum.Star)
        {
            levelStr = GameCommonInfo.GetUITextById(101);
        }
        else if (menuLevel == MenuLevelTypeEnum.Moon)
        {
            levelStr = GameCommonInfo.GetUITextById(102);
        }
        else if (menuLevel == MenuLevelTypeEnum.Sun)
        {
            levelStr = GameCommonInfo.GetUITextById(103);
        }
        else
        {

        }
        return levelStr;
    }
}