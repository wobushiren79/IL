using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InnFloorBuilder : BaseTilemapBuilder
{
    public void StartBuild()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData != null && gameData.innBuildData != null)
        {
            ClearAllTiles();
            BuildFloor(gameData.innBuildData.listFloor);
            BuildFloor(gameData.innBuildData.listSecondFloor);
        }
    }

    /// <summary>
    /// 开始修建地板
    /// </summary>
    /// <param name="listData"></param>
    public void BuildFloor(List<InnResBean> listData)
    {
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            BuildFloor(listData[i]);
        }
    }

    /// <summary>
    /// 修建地板
    /// </summary>
    /// <param name="itemData"></param>
    public void BuildFloor(InnResBean itemData)
    {
        if (itemData == null)
            return;
        BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemData.id);
        TileBase floorTile = InnBuildHandler.Instance.manager.GetFloorTileByName(buildItemData.tile_name);
        Build(floorTile, new Vector3Int((int)itemData.startPosition.x, (int)itemData.startPosition.y, 0));
    }

    /// <summary>
    /// 改变地板
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="changeTileName"></param>
    public void ChangeFloor(Vector3Int changePosition, string changeTileName)
    {
        TileBase floorTile = InnBuildHandler.Instance.manager.GetFloorTileByName(changeTileName);
        Build(floorTile, changePosition);
    }
}