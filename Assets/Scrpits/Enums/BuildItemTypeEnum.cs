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
    public static Sprite GetBuildItemSprite(BuildItemBean buildItem)
    {
        if (buildItem == null)
            return null;
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor:
                return InnBuildHandler.Instance.manager.GetFloorSpriteByName(buildItem.icon_key);
            case BuildItemTypeEnum.Wall:
                return InnBuildHandler.Instance.manager.GetWallSpriteByName(buildItem.icon_key);
            case BuildItemTypeEnum.Table:
                if (buildItem.model_name.Equals(BuildItemModelTypeEnum.Table_1.GetEnumName()))
                {
                    return InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(buildItem.icon_key);
                }
                else if (buildItem.model_name.Equals(BuildItemModelTypeEnum.Table_2.GetEnumName()))
                {
                    return InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(buildItem.icon_key + "_2");
                }
                else
                {
                    return InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(buildItem.icon_key);
                }
            default:
                return InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(buildItem.icon_key);
        }
    }

    public static string GetBuildItemName(BuildItemTypeEnum buildItemType)
    {
        string name = "???";
        switch (buildItemType)
        {
            case BuildItemTypeEnum.BedBase:
                name = TextHandler.Instance.manager.GetTextById(801);
                break;
            case BuildItemTypeEnum.BedBar:
                name = TextHandler.Instance.manager.GetTextById(802);
                break;
            case BuildItemTypeEnum.BedSheets:
                name = TextHandler.Instance.manager.GetTextById(803);
                break;
            case BuildItemTypeEnum.BedPillow:
                name = TextHandler.Instance.manager.GetTextById(804);
                break;
            default:
                break;
        }
        return name;
    }
}