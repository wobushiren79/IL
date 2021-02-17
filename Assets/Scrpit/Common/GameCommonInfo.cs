using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

public class GameCommonInfo
{
    //游戏设置
    public static GameConfigBean GameConfig;
    //进入竞技场准备数据
    public static ArenaPrepareBean ArenaPrepareData;
    //进入无尽之塔准备数据
    public static UserInfiniteTowersBean InfiniteTowersData;
    //随机种子
    public static int RandomSeed = 1564;
    //每日限制数据
    public static UserDailyLimitBean DailyLimitData = new UserDailyLimitBean();
    //当日数据
    public static CurrentDayBean CurrentDayData = new CurrentDayBean();
    // 预加载场景名字
    public static ScenesChangeBean ScenesChangeData = new ScenesChangeBean();

    private static GameConfigController mGameConfigController;
    private static UITextController mUITextController;

    public static BaseDataController baseDataController;

    public static void ClearData()
    {
        ArenaPrepareData = null;
        InfiniteTowersData = null;
        DailyLimitData = new UserDailyLimitBean();
        CurrentDayData = new CurrentDayBean();
        ScenesChangeData = new ScenesChangeBean();
        GameTimeHandler.Instance.SetDayStatus(GameTimeHandler.DayEnum.None);
    }

    static GameCommonInfo()
    {
        GameConfig = new GameConfigBean();
        mGameConfigController = new GameConfigController(null, new GameConfigCallBack());
        baseDataController = new BaseDataController(null, null);
        mUITextController = new UITextController(null, null);
        mGameConfigController.GetGameConfigData();
        baseDataController.InitBaseData();
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    public static void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
    {
        SpriteAtlas sa = LoadAssetUtil.SyncLoadAsset<SpriteAtlas>(ProjectConfigInfo.ASSETBUNDLE_SPRITEATLAS, tag);
        if (sa != null)
            callback?.Invoke(sa);
    }

    /// <summary>
    /// 随机化种子
    /// </summary>
    public static void InitRandomSeed()
    {
        Random.InitState(RandomSeed);
    }

    public static string GetUITextById(long id)
    {
        return mUITextController.GetTextById(id);
    }

    /// <summary>
    /// 设置竞技场数据
    /// </summary>
    /// <param name="miniGameData"></param>
    public static void SetArenaPrepareData(MiniGameBaseBean miniGameData)
    {
        ArenaPrepareData = new ArenaPrepareBean(miniGameData);
    }

    /// <summary>
    /// 设置无尽之塔数据
    /// </summary>
    /// <param name="data"></param>
    public static void SetInfiniteTowersPrepareData(UserInfiniteTowersBean data)
    {
        InfiniteTowersData = data;
    }

    public static void SaveGameConfig()
    {
        mGameConfigController.SaveGameConfigData(GameConfig);
    }

    public class GameConfigCallBack : IGameConfigView
    {
        public void GetGameConfigFail()
        {

        }

        public void GetGameConfigSuccess(GameConfigBean configBean)
        {
            GameConfig = configBean;
            mUITextController.RefreshData();
        }

        public void SetGameConfigFail()
        {

        }

        public void SetGameConfigSuccess(GameConfigBean configBean)
        {

        }
    }

}