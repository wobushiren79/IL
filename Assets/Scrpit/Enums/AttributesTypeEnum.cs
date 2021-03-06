﻿
using UnityEngine;

public enum AttributesTypeEnum 
{
    Null=0,
    Cook=1,
    Speed=2,
    Account=3,
    Charm=4,
    Force=5,
    Life = 11,
    Lucky =12,
    Loyal=13,
}

public static class AttributesTypeEnumTools
{
    public static string GetAttributesName(AttributesTypeEnum attributesType)
    {
        string name = "???";
        switch (attributesType)
        {
            case AttributesTypeEnum.Cook:
                name = TextHandler.Instance.manager.GetTextById(1);
                break;
            case AttributesTypeEnum.Speed:
                name = TextHandler.Instance.manager.GetTextById(2);
                break;
            case AttributesTypeEnum.Account:
                name = TextHandler.Instance.manager.GetTextById(3);
                break;
            case AttributesTypeEnum.Charm:
                name = TextHandler.Instance.manager.GetTextById(4);
                break;
            case AttributesTypeEnum.Force:
                name = TextHandler.Instance.manager.GetTextById(5);
                break;
            case AttributesTypeEnum.Life:
                name = TextHandler.Instance.manager.GetTextById(9);
                break;
            case AttributesTypeEnum.Lucky:
                name = TextHandler.Instance.manager.GetTextById(6);
                break;
            case AttributesTypeEnum.Loyal:
                name = TextHandler.Instance.manager.GetTextById(8);
                break;
        }
        return name;
    }

    public static Sprite GetAttributesIcon( AttributesTypeEnum attributesType)
    {
        string name = "";
        switch (attributesType)
        {
            case AttributesTypeEnum.Cook:
                name = "ui_ability_cook";
                break;
            case AttributesTypeEnum.Speed:
                name = "ui_ability_speed";
                break;
            case AttributesTypeEnum.Account:
                name = "ui_ability_account";
                break;
            case AttributesTypeEnum.Charm:
                name = "ui_ability_charm";
                break;
            case AttributesTypeEnum.Force:
                name = "ui_ability_force";
                break;
            case AttributesTypeEnum.Life:
                name = "ui_ability_life";
                break;
            case AttributesTypeEnum.Lucky:
                name = "ui_ability_lucky";
                break;
            case AttributesTypeEnum.Loyal:
                name = "love_1";
                break;
        }
        return IconDataHandler.Instance.manager.GetIconSpriteByName(name);
    }

}