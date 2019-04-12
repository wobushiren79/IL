using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFloorBuilder : BaseTilemapBuilder
{
    public InnBuildManager InnBuildManager;
    public GameDataManager gameDataManager;

    private void Start()
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
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildItemBean buildItemData= InnBuildManager.GetFloorDataById(itemData.id);
            Build(buildItemData.icon_key, (int)itemData.startPosition.x, (int)itemData.startPosition.y);
        }
    }
}