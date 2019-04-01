using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameListDataService : BaseDataStorageImpl<GameDataSimpleBean>
{
    private readonly string mSaveFileName;

    public GameListDataService()
    {
        mSaveFileName = "GameDataSimpleList";
    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public List<GameDataSimpleBean> QueryData()
    {
        return BaseStartLoadDataForList(mSaveFileName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(List<GameDataSimpleBean> gameDataList)
    {
        BaseStartSaveDataForList(mSaveFileName, gameDataList);
    }
}