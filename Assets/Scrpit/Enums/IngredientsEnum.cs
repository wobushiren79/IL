using UnityEngine;

public enum IngredientsEnum
{
    Oilsalt = 1,//油盐
    Meat = 2,//鲜肉
    Riverfresh = 3,//河鲜
    Seafood = 4,//海鲜
    Vegetables = 5,//蔬菜
    Melonfruit = 6,//水果
    Waterwine = 7,//酒水
    Flour = 8,//面粉
}

public class IngredientsEnumTools
{
    public static string GetIngredientName(IngredientsEnum ingredients)
    {
        string name = "???";
        switch (ingredients)
        {
            case IngredientsEnum.Oilsalt:
                name = GameCommonInfo.GetUITextById(21);
                break;
            case IngredientsEnum.Meat:
                name = GameCommonInfo.GetUITextById(22);
                break;
            case IngredientsEnum.Riverfresh:
                name = GameCommonInfo.GetUITextById(23);
                break;
            case IngredientsEnum.Seafood:
                name = GameCommonInfo.GetUITextById(24);
                break;
            case IngredientsEnum.Vegetables:
                name = GameCommonInfo.GetUITextById(25);
                break;
            case IngredientsEnum.Melonfruit:
                name = GameCommonInfo.GetUITextById(26);
                break;
            case IngredientsEnum.Waterwine:
                name = GameCommonInfo.GetUITextById(27);
                break;
            case IngredientsEnum.Flour:
                name = GameCommonInfo.GetUITextById(28);
                break;
        }
        return name;
    }

    public static Sprite GetIngredientIcon(IconDataManager iconDataManager, IngredientsEnum ingredients)
    {
        string iconKey = "";
        switch (ingredients)
        {
            case IngredientsEnum.Oilsalt:
                iconKey = "ui_ing_oilsalt";
                break;
            case IngredientsEnum.Meat:
                iconKey = "ui_ing_meat";
                break;
            case IngredientsEnum.Riverfresh:
                iconKey = "ui_ing_riverfresh";
                break;
            case IngredientsEnum.Seafood:
                iconKey = "ui_ing_seafood";
                break;
            case IngredientsEnum.Vegetables:
                iconKey = "ui_ing_vegetables";
                break;
            case IngredientsEnum.Melonfruit:
                iconKey = "ui_ing_melonfruit";
                break;
            case IngredientsEnum.Waterwine:
                iconKey = "ui_ing_waterwine";
                break;
            case IngredientsEnum.Flour:
                iconKey = "ui_ing_flour";
                break;
        }
       return iconDataManager.GetIconSpriteByName(iconKey);
    }
}