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
    Medicine = 13,//药
    SkillBook = 14,//技能书
    Read = 15,//读物
    Gift = 16,//礼物
    Mask = 21,//面具

    Other = 999,//其他
}

public class GeneralEnumTools
{
    /// <summary>
    /// 获取相关图标
    /// </summary>
    /// <param name="itemsInfo"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="gameDataManager"></param>
    public static Sprite GetGeneralSprite(ItemsInfoBean itemsInfo)
    {
        return GetGeneralSprite(itemsInfo, false);
    }
    public static Sprite GetGeneralSprite(ItemsInfoBean itemsInfo, bool isHideDres)
    {
        Sprite spIcon = null;
        switch ((GeneralEnum)itemsInfo.items_type)
        {
            case GeneralEnum.Mask:
                spIcon = CharacterDressHandler.Instance.manager.GetMaskSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Hat:
                if (isHideDres)
                    spIcon = IconHandler.Instance.GetIconSpriteByName("unknown_hat_1");
                else
                    spIcon = CharacterDressHandler.Instance.manager.GetHatSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Clothes:
                if (isHideDres)
                    spIcon = IconHandler.Instance.GetIconSpriteByName("unknown_clothes_1");
                else
                    spIcon = CharacterDressHandler.Instance.manager.GetClothesSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Shoes:
                if (isHideDres)
                    spIcon = IconHandler.Instance.GetIconSpriteByName("unknown_shoes_1");
                else
                    spIcon = CharacterDressHandler.Instance.manager.GetShoesSpriteByName(itemsInfo.icon_key);
                break;
            case GeneralEnum.Book:
            case GeneralEnum.Menu:
            case GeneralEnum.SkillBook:
            case GeneralEnum.Read:
            case GeneralEnum.Gift:
            case GeneralEnum.Other:
                spIcon = IconHandler.Instance.GetIconSpriteByName(itemsInfo.icon_key);
                break;
            default:
                spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName(itemsInfo.icon_key);
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
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(401);
                break;
            case GeneralEnum.Hat:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(402);
                break;
            case GeneralEnum.Clothes:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(403);
                break;
            case GeneralEnum.Shoes:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(404);
                break;
            case GeneralEnum.Chef:
            case GeneralEnum.Waiter:
            case GeneralEnum.Accoutant:
            case GeneralEnum.Accost:
            case GeneralEnum.Beater:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(405);
                break;
            case GeneralEnum.Ing:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(406);
                break;
            case GeneralEnum.Book:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(407);
                break;
            case GeneralEnum.Menu:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(408);
                break;
            case GeneralEnum.Medicine:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(409);
                break;
            case GeneralEnum.SkillBook:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(410);
                break;
            case GeneralEnum.Read:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(411);
                break;
            case GeneralEnum.Gift:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(412);
                break;
            case GeneralEnum.Other:
                itemsnTypeName = TextHandler.Instance.manager.GetTextById(413);
                break;
            default:
                break;
        }
        return itemsnTypeName;
    }
}