using UnityEngine;
using UnityEditor;

public enum SeasonsEnum
{
    Other = 0,
    Spring = 1,//春
    Summer = 2,//夏
    Autumn = 3,//秋
    Winter = 4,//冬
}

public class SeasonsEnumTools
{

    public static Color GetSeasonsColor(SeasonsEnum seasons)
    {
        switch (seasons)
        {
            case SeasonsEnum.Spring:
                return new Color(0, 0.75f, 0, 1);
            case SeasonsEnum.Summer:
                return new Color(0.75f, 0, 0, 1);
            case SeasonsEnum.Autumn:
                return new Color(0.85f, 0.5f, 0, 1);
            case SeasonsEnum.Winter:
                return new Color(0, 0.4f, 0.8f, 1);
            default:
                return Color.black;
        }
    }
}
