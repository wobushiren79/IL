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
    Accouting = 6,
    Accost = 7,
    Beater = 8,

    Ing = 9,
    Book = 11,
    Menu = 12,//菜谱

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
                spIcon = iconDataManager.GetIconSpriteByName("book_1");
                break;
            case GeneralEnum.Menu:
                spIcon = iconDataManager.GetIconSpriteByName(itemsInfo.icon_key);
                break;
            default:
                spIcon = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
                break;
        }
        return spIcon;
    }

}