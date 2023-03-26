using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

public class GameCommonInfo
{
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
        baseDataController = new BaseDataController(null, null);

        IconHandler.Instance.InitData();
    }

    /// <summary>
    /// 随机化种子
    /// </summary>
    public static void InitRandomSeed()
    {
        Random.InitState(RandomSeed);
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
}