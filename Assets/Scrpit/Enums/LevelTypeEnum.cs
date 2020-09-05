using UnityEngine;
using UnityEditor;

public enum LevelTypeEnum
{
    Init = 0,
    Star = 1,
    Moon = 2,
    Sun = 3,
}

public class LevelTypeEnumTools
{
    public static string GetLevelStr(LevelTypeEnum level)
    {
        string levelStr = "";

        if (level == LevelTypeEnum.Init)
        {
            levelStr = GameCommonInfo.GetUITextById(104);
        }
        else if (level == LevelTypeEnum.Star)
        {
            levelStr = GameCommonInfo.GetUITextById(101);
        }
        else if (level == LevelTypeEnum.Moon)
        {
            levelStr = GameCommonInfo.GetUITextById(102);
        }
        else if (level == LevelTypeEnum.Sun)
        {
            levelStr = GameCommonInfo.GetUITextById(103);
        }
        else
        {

        }
        return levelStr;
    }
}