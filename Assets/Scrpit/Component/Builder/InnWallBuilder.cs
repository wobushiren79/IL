using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnWallBuilder : BaseTilemapBuilder
{
    public InnBuildManager InnBuildManager;
    public GameDataManager gameDataManager;

    private void Start()
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
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildItemBean buildItemData = InnBuildManager.GetBuildDataById(itemData.id);
            Build(buildItemData.icon_key, (int)itemData.startPosition.x, (int)itemData.startPosition.y);
        }
    }
}