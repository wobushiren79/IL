using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;
using Pathfinding;
public class SceneGameInnInit : BaseSceneInit, IBaseObserver, DialogView.IDialogCallBack
{
    protected SceneInnManager sceneInnManager;

    protected InnFloorBuilder innFloorBuilder;
    protected InnWallBuilder innWallBuilder;
    protected InnFurnitureBuilder innFurnitureBuilder;
    protected InnHandler innHandler;
    protected NpcCustomerBuilder npcCustomerBuilder;


    public override void Awake()
    {
        base.Awake();

        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        innFloorBuilder = Find<InnFloorBuilder>(ImportantTypeEnum.InnBuilder);
        innWallBuilder = Find<InnWallBuilder>(ImportantTypeEnum.InnBuilder);
        innFurnitureBuilder = Find<InnFurnitureBuilder>(ImportantTypeEnum.InnBuilder);
        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    public override void Start()
    {
        base.Start();
        //获取团队NPC信息
        if (npcTeamManager != null)
        {
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Customer);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Friend);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Rascal);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Sundry);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Entertain);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Disappointed);
        }
        //故事数据
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(ScenesEnum.GameInnScene);
        //初始化场景
        if (sceneInnManager != null)
        {
            InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
            sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);

            innBuildData.ChangeInnSize(2, innBuildManager, 30, 30);
        }

        //构建地板
        if (innFloorBuilder != null)
            innFloorBuilder.StartBuild();
        //构建墙壁
        if (innWallBuilder != null)
            innWallBuilder.StartBuild();
        //构建建筑
        if (innFurnitureBuilder != null)
            innFurnitureBuilder.StartBuild();

        //初始化客栈处理
        if (innHandler != null)
        {
            innHandler.InitInn();
            innHandler.InitRecord();
        }

        if (gameDataManager != null && gameTimeHandler != null)
        {
            //增加回调
            gameTimeHandler.AddObserver(this);
            TimeBean timeData = gameDataManager.gameData.gameTime;
            if (timeData.hour >= 24 || timeData.hour < 6)
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

        //设置天气
        if (weatherHandler != null)
        {
            weatherHandler.SetWeahter(GameCommonInfo.CurrentDayData.weatherToday);
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
            gameDataHandler.AddMenuResearch(addHour * 60);
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
            gameDataManager.SaveGameData(innHandler.GetInnRecord());
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Rest)
        {
            //打开日历
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
            //保存数据
            gameDataManager.SaveGameData(innHandler.GetInnRecord());
        }
        //关闭店面
        if (innHandler != null)
            innHandler.CloseInn();
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                InitNewDay();
            }
            else if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {
                //结算所有客户
                if (innHandler != null)
                    innHandler.SettlementAllCustomer();
                //清楚所有NPC
                if (npcCustomerBuilder != null)
                    npcCustomerBuilder.ClearNpc();
                //停止控制
                if (controlHandler != null)
                    controlHandler.StopControl();
                if (dialogManager != null)
                {   //停止时间
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
                    dialogManager.CreateDialog(DialogEnum.Text, this, dialogBean);
                }
                else
                {
                    EndDay();
                }
            }
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