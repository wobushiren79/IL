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
                name = TextHandler.Instance.manager.GetTextById(21);
                break;
            case IngredientsEnum.Meat:
                name = TextHandler.Instance.manager.GetTextById(22);
                break;
            case IngredientsEnum.Riverfresh:
                name = TextHandler.Instance.manager.GetTextById(23);
                break;
            case IngredientsEnum.Seafood:
                name = TextHandler.Instance.manager.GetTextById(24);
                break;
            case IngredientsEnum.Vegetables:
                name = TextHandler.Instance.manager.GetTextById(25);
                break;
            case IngredientsEnum.Melonfruit:
                name = TextHandler.Instance.manager.GetTextById(26);
                break;
            case IngredientsEnum.Waterwine:
                name = TextHandler.Instance.manager.GetTextById(27);
                break;
            case IngredientsEnum.Flour:
                name = TextHandler.Instance.manager.GetTextById(28);
                break;
        }
        return name;
    }

    public static Sprite GetIngredientIcon( IngredientsEnum ingredients)
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
       return IconDataHandler.Instance.manager.GetIconSpriteByName(iconKey);
    }
}