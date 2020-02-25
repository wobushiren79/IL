using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InnFloorBuilder : BaseTilemapBuilder
{
    protected InnBuildManager innBuildManager;
    protected GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    public void StartBuild()
    {
        if (gameDataManager != null && gameDataManager.gameData != null && gameDataManager.gameData.innBuildData != null)
        {
            BuildFloor(gameDataManager.gameData.innBuildData.listFloor);
        }
    }

    /// <summary>
    /// 开始修建地板
    /// </summary>
    /// <param name="listData"></param>
    public void BuildFloor(List<InnResBean> listData)
    {
        ClearAllTiles();
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
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(itemData.id);
        TileBase floorTile = innBuildManager.GetFloorTileByName(buildItemData.tile_name);
        Build(floorTile, new Vector3Int((int)itemData.startPosition.x, (int)itemData.startPosition.y, 0));
    }

    /// <summary>
    /// 改变地板
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="changeTileName"></param>
    public void ChangeFloor(Vector3Int changePosition,string changeTileName)
    {
        TileBase floorTile = innBuildManager.GetFloorTileByName(changeTileName);
        Build(floorTile, changePosition);
    }
}