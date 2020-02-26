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
    Door = 9,
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
            default:
                return innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
        }
    }
}