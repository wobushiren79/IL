using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;
using Pathfinding;
public class SceneGameInnInit : BaseSceneInit, DialogView.IDialogCallBack
{
    protected SceneInnManager sceneInnManager;

    protected NpcCustomerBuilder npcCustomerBuilder;
    protected NpcWorkerBuilder npcWorkerBuilder;
    protected NpcEventBuilder npcEventBuilder;

    public override void Awake()
    {
        base.Awake();

        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
        npcWorkerBuilder = Find<NpcWorkerBuilder>(ImportantTypeEnum.NpcBuilder);
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    public override void Start()
    {
        base.Start();
        //初始化场景
        if (sceneInnManager != null)
        {
            InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
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
        if (weatherHandler != null)
        {
            weatherHandler.SetWeahter(GameCommonInfo.CurrentDayData.weatherToday);
        }

        if (gameDataManager != null)
        {
            //增加回调
            GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
            TimeBean timeData = gameDataManager.gameData.gameTime;
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
                gameTimeHandler.SetTime(timeData.hour, timeData.minute);
                gameTimeHandler.SetTimeStatus(false);
                //建造NPC
                RefreshScene();
                //设置位置
                Vector3 startPosition = sceneInnManager.GetTownEntranceLeft();
                BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
                baseControl.SetFollowPosition(startPosition);
            }
        }




        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 刷新场景
    /// </summary>
    public override void RefreshScene()
    {
        base.RefreshScene();
        //建造NPC
        npcCustomerBuilder.BuilderCustomerForInit(20);
    }

    /// <summary>
    /// 初始化新的一天
    /// </summary>
    public void InitNewDay()
    {
        //随机化一个天气
        if (weatherHandler != null)
        {
            WeatherBean weatherData = null;
            if (gameDataManager.gameData.gameTime.month == 4 && gameDataManager.gameData.gameTime.day == 1)
            {
                //冬月的第一天必定下大雪
                weatherData = new WeatherBean(WeatherTypeEnum.Snow);
                weatherHandler.SetWeahter(weatherData);
            }
            else
            {
                weatherData = weatherHandler.RandomWeather();

            }
            GameCommonInfo.CurrentDayData.weatherToday = weatherData;
        }
    }

    /// <summary>
    /// 结束一天
    /// </summary>
    public void EndDay()
    {
        //停止时间
        gameTimeHandler.SetTimeStatus(true);

        if (GameCommonInfo.CurrentDayData.dayStatus != GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏则不增加经验
            //如果有菜谱研究 增加研究点数
            int addHour = 24 - gameDataManager.gameData.gameTime.hour;
            gameDataHandler.AddTimeProcess(addHour * 60);
        }

        //重置游戏时间
        GameCommonInfo.GameData.gameTime.hour = 0;
        GameCommonInfo.GameData.gameTime.minute = 0;

        if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.None)
        {
            //重新进入游戏
            //打开日历
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作状态结束一天 则进入结算画面
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameSettle));
            //保存数据
            gameDataManager.SaveGameData(InnHandler.Instance.GetInnRecord());
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Rest
            || GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.End)
        {
            //打开日历
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
            //保存数据
            gameDataManager.SaveGameData(InnHandler.Instance.GetInnRecord());
        }
    }

    public void CleanInnData()
    {
        //结算所有客户
        InnHandler.Instance.SettlementAllCustomer();
        InnHandler.Instance.CloseInn();
        //停止控制
        if (controlHandler != null)
            controlHandler.StopControl();
        //清楚所有NPC
        if (npcCustomerBuilder != null)
            npcCustomerBuilder.ClearNpc();
        //清楚所有NPC
        if (npcWorkerBuilder != null)
            npcWorkerBuilder.ClearAllWork();
        //清楚所有NPC
        if (npcEventBuilder != null)
            npcEventBuilder.ClearNpc();
        //停止控制
        if (controlHandler != null)
            controlHandler.EndControl();
        //停止时间
        gameTimeHandler.SetTimeStatus(true);

        DialogBean dialogBean = new DialogBean();
        if (gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            dialogBean.content = GameCommonInfo.GetUITextById(3006);
        }
        else if (gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
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