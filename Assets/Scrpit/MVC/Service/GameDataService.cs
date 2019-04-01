using UnityEngine;
using UnityEditor;

public class GameDataService : BaseDataStorageImpl<GameDataBean>
{
    public GameDataService()
    {
      
    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public GameDataBean QueryDataByUserId(string userId)
    {
        return BaseStartLoadData(userId);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateDataByUserId(string userId, GameDataBean gameData)
    {
        BaseStartSaveData(userId, gameData);
    }
}