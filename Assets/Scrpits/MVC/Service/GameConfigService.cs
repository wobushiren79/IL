using UnityEngine;
using UnityEditor;

public class GameConfigService : BaseDataStorageImpl<GameConfigBean>
{
    private readonly string mSaveFileName;

    public GameConfigService()
    {
        mSaveFileName = "GameConfig";
    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public GameConfigBean QueryData()
    {
       return BaseLoadData(mSaveFileName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(GameConfigBean gameConfig)
    {
        BaseSaveData(mSaveFileName,gameConfig);
    }
}