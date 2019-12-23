using UnityEngine;
using UnityEditor;

public class GameCommonInfo
{
    //游戏用户ID
    public static string GameUserId;
    //游戏设置
    public static GameConfigBean GameConfig;
    //世界时间
    public static GameDataBean GameData;
    //进入竞技场准备数据
    public static ArenaPrepareBean ArenaPrepareData;
    //随机种子
    public static int RandomSeed = 1564;
    //每日限制数据
    public static UserDailyLimitBean DailyLimitData = new UserDailyLimitBean();
    // 预加载场景名字
    public static ScenesChangeBean ScenesChangeData = new ScenesChangeBean();

    private static GameConfigController mGameConfigController;
    private static UITextController mUITextController;

    static GameCommonInfo()
    {
        GameConfig = new GameConfigBean();

        mGameConfigController = new GameConfigController(null, new GameConfigCallBack());
        mUITextController = new UITextController(null, null);
        mGameConfigController.GetGameConfigData();
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