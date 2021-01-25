using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InnWallBuilder : BaseTilemapBuilder
{
    public void StartBuild()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData != null && gameData.innBuildData != null)
        {
            ClearAllTiles();
            BuildWall(gameData.innBuildData.listWall);
            BuildWall(gameData.innBuildData.listSecondWall);
        }
    }

    /// <summary>
    /// 开始修建墙壁
    /// </summary>
    /// <param name="listData"></param>
    public void BuildWall(List<InnResBean> listData)
    {
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemData.id);
            TileBase wallTile = InnBuildHandler.Instance.manager.GetWallTileByName(buildItemData.tile_name);
            Build(wallTile, new Vector3Int((int)itemData.startPosition.x, (int)itemData.startPosition.y, 0));
        }
    }


    /// <summary>
    /// 修建墙壁
    /// </summary>
    /// <param name="itemData"></param>
    public void BuildWall(InnResBean itemData)
    {
        if (itemData == null)
            return;
        BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemData.id);
        TileBase wallTile = InnBuildHandler.Instance.manager.GetWallTileByName(buildItemData.tile_name);
        Build(wallTile, new Vector3Int((int)itemData.startPosition.x, (int)itemData.startPosition.y, 0));
    }

    /// <summary>
    /// 改变墙壁
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="changeTileName"></param>
    public void ChangeWall(Vector3Int changePosition, string changeTileName)
    {
        TileBase floorTile = InnBuildHandler.Instance.manager.GetWallTileByName(changeTileName);
        Build(floorTile, changePosition);
    }

    /// <summary>
    /// 清理墙壁
    /// </summary>
    /// <param name="position"></param>
    public void ClearWall(Vector3Int position)
    {
        ClearTile(position);
    }
}