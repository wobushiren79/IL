using UnityEngine;
using UnityEditor;

public enum GeneralEnum
{
    Null = 0,
    Hat = 1,
    Clothes = 2,
    Shoes = 3,

    Chef = 4,
    Waiter = 5,
    Accoutant = 6,
    Accost = 7,
    Beater = 8,

    Ing = 9,
    Book = 11,//属性书
    Menu = 12,//菜谱
    Medicine=13,//药
    SkillBook = 14,//技能书
    Read = 15,//读物
    Mask = 21,//面具

}

public class GeneralEnumTools
{
    /// <summary>
    /// 获取相关图标
    /// </summary>
    /// <param name="itemsInfo"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="gameDataManager"></param>
    public static Sprite GetGeneralSprite(ItemsInfoBean itemsInfo, IconDataManager iconDataManager, GameItemsManager gameItemsManager, CharacterDressManager characterDressManager)
    {
        return GetGeneralSprite(itemsInfo, iconDataManager, gameItemsManager, characterDressManager,false);
    }
    public static Sprite GetGeneralSprite(ItemsInfoBean itemsInfo, IconDataManager iconDataManager, GameItemsManager gameItemsManager, CharacterDressManager characterDressManager, bool isHideDres)
    {
        Sprite spIcon = null;
        switch ((GeneralEnum)itemsInfo.items_type)
        {
            case GeneralEnum.Mask:
                spIcon = characterDressManager.GetMaskSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Hat:
                if (isHideDres)
                    spIcon = iconDataManager.GetIconSpriteByName("unknown_hat_1");
                else
                    spIcon = characterDressManager.GetHatSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Clothes:
                if (isHideDres)
                    spIcon = iconDataManager.GetIconSpriteByName("unknown_clothes_1");
                else
                    spIcon = characterDressManager.GetClothesSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Shoes:
                if (isHideDres)
                    spIcon = iconDataManager.GetIconSpriteByName("unknown_shoes_1");
                else
                    spIcon = characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Book:
            case GeneralEnum.Menu:
            case GeneralEnum.SkillBook:
            case GeneralEnum.Read:
                spIcon = iconDataManager.GetIconSpriteByName(itemsInfo.icon_key);
                break;
            default:
                spIcon = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
                break;
        }
        return spIcon;
    }


    public static string GetGeneralName(GeneralEnum type)
    {
        string itemsnTypeName = "???";
        switch (type)
        {
            case GeneralEnum.Mask:
                itemsnTypeName = GameCommonInfo.GetUITextById(401);
                break;
            case GeneralEnum.Hat:
                itemsnTypeName = GameCommonInfo.GetUITextById(402);
                break;
            case GeneralEnum.Clothes:
                itemsnTypeName = GameCommonInfo.GetUITextById(403);
                break;
            case GeneralEnum.Shoes:
                itemsnTypeName = GameCommonInfo.GetUITextById(404);
                break;
            case GeneralEnum.Chef:
            case GeneralEnum.Waiter:
            case GeneralEnum.Accoutant:
            case GeneralEnum.Accost:
            case GeneralEnum.Beater:
                itemsnTypeName = GameCommonInfo.GetUITextById(405);
                break;
            case GeneralEnum.Ing:
                itemsnTypeName = GameCommonInfo.GetUITextById(406);
                break;
            case GeneralEnum.Book:
                itemsnTypeName = GameCommonInfo.GetUITextById(407);
                break;
            case GeneralEnum.Menu:
                itemsnTypeName = GameCommonInfo.GetUITextById(408);
                break;
            case GeneralEnum.Medicine:
                itemsnTypeName = GameCommonInfo.GetUITextById(409);
                break;
            case GeneralEnum.SkillBook:
                itemsnTypeName = GameCommonInfo.GetUITextById(410);
                break;
            case GeneralEnum.Read:
                itemsnTypeName = GameCommonInfo.GetUITextById(411);
                break;
            default:
                break;
        }
        return itemsnTypeName;
    }
}