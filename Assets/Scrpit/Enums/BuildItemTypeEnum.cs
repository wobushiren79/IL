using UnityEngine;
using UnityEditor;

public enum BuildItemTypeEnum
{
    Other = 0,
    Floor = 1,
    Wall = 2,
    Table = 3,
    Stove = 4,
    Counter = 5,
    Decoration = 6,
    Door = 9,
    Bed = 10,
    Stairs=11,

    BedBase=101,
    BedBar=102,
    BedSheets=103,
    BedPillow=104,
}

public class BuildItemTypeEnumTools
{
    public static Sprite GetBuildItemSprite(InnBuildManager innBuildManager, BuildItemBean buildItem)
    {
        if (buildItem == null)
            return null;
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor:
                return innBuildManager.GetFloorSpriteByName(buildItem.icon_key);
            case BuildItemTypeEnum.Wall:
                return innBuildManager.GetWallSpriteByName(buildItem.icon_key);
            case BuildItemTypeEnum.Table:
                if (buildItem.model_name.Equals(EnumUtil.GetEnumName(BuildItemModelTypeEnum.Table_1)))
                {
                    return innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
                }
                else if (buildItem.model_name.Equals(EnumUtil.GetEnumName(BuildItemModelTypeEnum.Table_2)))
                {
                    return innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key + "_2");
                }
                else
                {
                    return innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
                }
            default:
                return innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
        }
    }

    public static string GetBuildItemName(BuildItemTypeEnum buildItemType)
    {
        string name = "???";
        switch (buildItemType)
        {
            case BuildItemTypeEnum.BedBase:
                name = GameCommonInfo.GetUITextById(801);
                break;
            case BuildItemTypeEnum.BedBar:
                name = GameCommonInfo.GetUITextById(802);
                break;
            case BuildItemTypeEnum.BedSheets:
                name = GameCommonInfo.GetUITextById(803);
                break;
            case BuildItemTypeEnum.BedPillow:
                name = GameCommonInfo.GetUITextById(804);
                break;
            default:
                break;
        }
        return name;
    }
}