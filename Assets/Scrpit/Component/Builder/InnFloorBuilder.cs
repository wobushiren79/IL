using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFloorBuilder : BaseTilemapBuilder
{
    public InnBuildManager innBuildManager;
    public GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.InnBuilder);
    }

    public  void StartBuild()
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
            InnResBean itemData = listData[i];
            BuildItemBean buildItemData= innBuildManager.GetBuildDataById(itemData.id);
            //todo
            Build(buildItemData.icon_key, (int)itemData.startPosition.x, (int)itemData.startPosition.y);
        }
    }
}