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
            levelStr = TextHandler.Instance.manager.GetTextById(104);
        }
        else if (level == LevelTypeEnum.Star)
        {
            levelStr = TextHandler.Instance.manager.GetTextById(101);
        }
        else if (level == LevelTypeEnum.Moon)
        {
            levelStr = TextHandler.Instance.manager.GetTextById(102);
        }
        else if (level == LevelTypeEnum.Sun)
        {
            levelStr = TextHandler.Instance.manager.GetTextById(103);
        }
        else
        {

        }
        return levelStr;
    }

    public static Sprite GetLevelIcon(LevelTypeEnum level)
    {
        Sprite spIcon = null;
        if (level == LevelTypeEnum.Init)
        {
        }
        else if (level == LevelTypeEnum.Star)
        {
            spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("reputation_level_1_1");
        }
        else if (level == LevelTypeEnum.Moon)
        {
            spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("reputation_level_2_1");
        }
        else if (level == LevelTypeEnum.Sun)
        {
            spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("reputation_level_3_1");
        }
        return spIcon;
    }
}