﻿using UnityEngine;
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

    public static Sprite GetLevelIcon(IconDataManager iconDataManager, LevelTypeEnum level)
    {
        Sprite spIcon = null;
        if (level == LevelTypeEnum.Init)
        {
        }
        else if (level == LevelTypeEnum.Star)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_1_1");
        }
        else if (level == LevelTypeEnum.Moon)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_2_1");
        }
        else if (level == LevelTypeEnum.Sun)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_3_1");
        }
        return spIcon;
    }
}