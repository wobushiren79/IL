using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InnWallBuilder : BaseTilemapBuilder
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
            BuildWall(gameDataManager.gameData.innBuildData.listWall);
        }
    }

    /// <summary>
    /// 开始修建地板
    /// </summary>
    /// <param name="listData"></param>
    public void BuildWall(List<InnResBean> listData)
    {
        ClearAllTiles();
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildItemBean buildItemData = innBuildManager.GetBuildDataById(itemData.id);
            TileBase wallTile = innBuildManager.GetWallTileByName(buildItemData.tile_name);
            Build(wallTile, new Vector3Int((int)itemData.startPosition.x, (int)itemData.startPosition.y, 0));
        }
    }
}