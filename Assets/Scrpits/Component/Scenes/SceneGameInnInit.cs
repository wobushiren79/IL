using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;
using Pathfinding;
public class SceneGameInnInit : BaseSceneInit, DialogView.IDialogCallBack
{


    public override void Start()
    {
        base.Start();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //初始化场景

        SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
        InnBuildBean innBuildData = gameData.GetInnBuildData();
        sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);

        //innBuildData.ChangeInnSize(2, innBuildManager, 9, 9);


        //构建地板
        InnBuildHandler.Instance.builderForFloor.StartBuild();
        //构建墙壁
        InnBuildHandler.Instance.builderForWall.StartBuild();
        //构建建筑
        InnBuildHandler.Instance.builderForFurniture.StartBuild();

        //初始化客栈处理
        InnHandler.Instance.InitInn();
        InnHandler.Instance.InitRecord();
        //其他场景切过来需要重置一下客栈数据
        InnHandler.Instance.CloseInn();
        //设置天气
        GameWeatherHandler.Instance.SetWeather(GameCommonInfo.CurrentDayData.weatherToday);
        //打开UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();

        //增加回调
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
        TimeBean timeData = gameData.gameTime;
        if (timeData.hour >= 24
            || timeData.hour < 6
            || GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.End)
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
            Vector3 startPosition;
            switch (GameCommonInfo.ScenesChangeData.beforeScene)
            {
                case ScenesEnum.GameTownScene:
                    startPosition = sceneInnManager.GetCourtyardEntrance();
                    break;
                default:
                    startPosition = sceneInnManager.GetTownEntranceLeft();
                    break;
            }
            ControlForMoveCpt baseControl = GameControlHandler.Instance.StartControl<ControlForMoveCpt>(GameControlHandler.ControlEnum.Normal);
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
        //建筑家庭成员
        NpcHandler.Instance.builderForFamily.BuildFamily();
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
            GameWeatherHandler.Instance.SetWeather(weatherData);
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
        if (GameTimeHandler.Instance.GetDayStatus()!= GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏则不增加经验
            //如果有菜谱研究 增加研究点数
            int addHour = 24 - gameData.gameTime.hour;
            GameDataHandler.Instance.AddTimeProcess(addHour * 60);
        }

        //重置游戏时间
        gameData.gameTime.hour = 0;
        gameData.gameTime.minute = 0;
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏
            //打开日历
            UIHandler.Instance.OpenUIAndCloseOther<UIGameDate>();
        }
        else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作状态结束一天 则进入结算画面
            UIHandler.Instance.OpenUIAndCloseOther<UIGameSettle>();
            //保存数据
            GameDataHandler.Instance.manager.SaveGameData(InnHandler.Instance.GetInnRecord());
        }
        else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest
            || GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.End)
        {
            //打开日历
            UIHandler.Instance.OpenUIAndCloseOther<UIGameDate>();
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
            dialogBean.content = TextHandler.Instance.manager.GetTextById(3006);
        }
        else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
        {
            dialogBean.content = TextHandler.Instance.manager.GetTextById(3014);
        }
        dialogBean.dialogType = DialogEnum.Text;
        dialogBean.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogBean);
    }

    #region 通知回调
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            InitNewDay();
            //重新烘培场景
            StartCoroutine(BuildNavMesh());
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