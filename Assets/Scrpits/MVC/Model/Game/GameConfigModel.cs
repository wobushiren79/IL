using UnityEngine;
using UnityEditor;

public class GameConfigModel : BaseMVCModel
{
    private GameConfigService mGameConfigService;

    public override void InitData()
    {
        mGameConfigService = new GameConfigService();
    }

    /// <summary>
    /// 获取游戏配置数据
    /// </summary>
    /// <returns></returns>
    public GameConfigBean GetGameConfigData()
    {
        GameConfigBean configBean = mGameConfigService.QueryData();
        if (configBean == null)
            configBean = new GameConfigBean();
        return configBean;
    }

    /// <summary>
    /// 保存游戏配置数据
    /// </summary>
    /// <param name="configBean"></param>
    public void SaveGameConfigData(GameConfigBean configBean)
    {
        mGameConfigService.UpdateData(configBean);
    }
}