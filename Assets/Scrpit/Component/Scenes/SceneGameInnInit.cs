using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;
using Pathfinding;
public class SceneGameInnInit : BaseSceneInit, DialogView.IDialogCallBack
{
    protected SceneInnManager sceneInnManager;

    public override void Awake()
    {
        base.Awake();

        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
    }

    public override void Start()
    {
        base.Start();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //初始化场景
        if (sceneInnManager != null)
        {
            InnBuildBean innBuildData = gameData.GetInnBuildData();
            sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);

            //innBuildData.ChangeInnSize(2, innBuildManager, 9, 9);
        }

        //构建地板
        InnBuildHandler.Instance.builderForFloor.StartBuild();
        //构建墙壁
        InnBuildHandler.Instance.builderForWall.StartBuild();
        //构建建筑
        InnBuildHandler.Instance.builderForFurniture.StartBuild();

        //初始化客栈处理
        InnHandler.Instance.InitInn();
        InnHandler.Instance.InitRecord();

        //设置天气
        GameWeatherHandler.Instance.SetWeahter(GameCommonInfo.CurrentDayData.weatherToday);

        //增加回调
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
        TimeBean timeData = gameData.gameTime;
        if (timeData.hour >= 24
            || timeData.hour < 6
            || GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.End)
        {
            //如果是需要切换第二天
            EndDay();
        }
        else
        {
            //如果是其他场景切换过来
            GameTimeHandler.Instance.SetTime(timeData.hour, timeData.minute);
            GameTimeHandler.Instance.SetTimeStatus(false);
            //建造NPC
            RefreshScene();
            //设置位置
            Vector3 startPosition = sceneInnManager.GetTownEntranceLeft();
            BaseControl baseControl = GameControlHandler.Instance.StartControl<BaseControl>(GameControlHandler.ControlEnum.Normal);
            baseControl.SetFollowPosition(startPosition);
        }
        //改变四季
        GameSeasonsHandler.Instance.ChangeSeasons();

        StartCoroutine(BuildNavMesh());
    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }

    /// <summary>
    /// 刷新场景
    /// </summary>
    public override void RefreshScene()
    {
        base.RefreshScene();
        //建造NPC
        NpcHandler.Instance.builderForCustomer.BuilderCustomerForInit(20);
    }

    /// <summary>
    /// 初始化新的一天
    /// </summary>
    public void InitNewDay()
    {
        //随机化一个天气

        WeatherBean weatherData = null;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.gameTime.month == 4 && gameData.gameTime.day == 1)
        {
            //冬月的第一天必定下大雪
            weatherData = new WeatherBean(WeatherTypeEnum.Snow);
            GameWeatherHandler.Instance.SetWeahter(weatherData);
        }
        else
        {
            weatherData = GameWeatherHandler.Instance.RandomWeather();

        }
        GameCommonInfo.CurrentDayData.weatherToday = weatherData;
    }

    /// <summary>
    /// 结束一天
    /// </summary>
    public void EndDay()
    {
        //停止时间
        GameTimeHandler.Instance.SetTimeStatus(true);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (GameCommonInfo.CurrentDayData.dayStatus != GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏则不增加经验
            //如果有菜谱研究 增加研究点数
            int addHour = 24 - gameData.gameTime.hour;
            GameDataHandler.Instance.AddTimeProcess(addHour * 60);
        }

        if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏
            //打开日历
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameDate>(UIEnum.GameDate);
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作状态结束一天 则进入结算画面
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameSettle>(UIEnum.GameSettle);
            //保存数据
            GameDataHandler.Instance.manager.SaveGameData(InnHandler.Instance.GetInnRecord());
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Rest
            || GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.End)
        {
            //打开日历
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameDate>(UIEnum.GameDate);
            //保存数据
            GameDataHandler.Instance.manager.SaveGameData(InnHandler.Instance.GetInnRecord());
        }
    }

    public void CleanInnData()
    {
        //结算所有客户
        InnHandler.Instance.SettlementAllCustomer();
        InnHandler.Instance.CloseInn();
        //停止控制
        GameControlHandler.Instance.StopControl();
        //清楚所有NPC
        NpcHandler.Instance.builderForCustomer.ClearNpc();
        //清楚所有NPC
        NpcHandler.Instance.builderForWorker.ClearAllWork();
        //清楚所有NPC
        NpcHandler.Instance.builderForEvent.ClearNpc();
        //停止控制
        GameControlHandler.Instance.EndControl();

        //停止时间
        GameTimeHandler.Instance.SetTimeStatus(true);

        DialogBean dialogBean = new DialogBean();
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            dialogBean.content = GameCommonInfo.GetUITextById(3006);
        }
        else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
        {
            dialogBean.content = GameCommonInfo.GetUITextById(3014);
        }
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Text, this, dialogBean);
    }

    #region 通知回调
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            InitNewDay();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            CleanInnData();
        }
    }
    #endregion


    #region  弹窗通知回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        EndDay();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {
        EndDay();
    }
    #endregion
}