using UnityEngine;
using UnityEditor;
using System.IO;

public class GameDataService : BaseDataStorageImpl<GameDataBean>
{

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public GameDataBean QueryDataByUserId(string userId)
    {
        return BaseLoadData(userId + "/Base");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateDataByUserId(string userId, GameDataBean gameData)
    {
        FileUtil.CreateDirectory(dataStoragePath + "/" + userId);
        BaseSaveData(userId + "/Base", gameData);
    }

    /// <summary>
    /// 删除用户数据
    /// </summary>
    public void DeleteDataByUserId(string userId)
    {
        BaseDeleteFolder(userId);
    }
}