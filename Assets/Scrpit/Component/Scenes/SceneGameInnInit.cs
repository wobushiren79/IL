using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneGameInnInit : BaseSceneInit, IBaseObserver, DialogView.IDialogCallBack
{
    protected SceneInnManager sceneInnManager;
    public InnBuildManager innBuildManager;

    protected InnFloorBuilder innFloorBuilder;
    protected InnWallBuilder innWallBuilder;
    protected InnFurnitureBuilder innFurnitureBuilder;

    public InnHandler innHandler;


    public NavMeshSurface navMesh;

    public NpcCustomerBuilder npcCustomerBuilder;

    public override void Awake()
    {
        base.Awake();
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        innFloorBuilder = Find<InnFloorBuilder>(ImportantTypeEnum.InnBuilder);
        innWallBuilder = Find<InnWallBuilder>(ImportantTypeEnum.InnBuilder);
        innFurnitureBuilder = Find<InnFurnitureBuilder>(ImportantTypeEnum.InnBuilder);
    }

    public override void Start()
    {
        base.Start();
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (npcTeamManager != null)
        {
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Customer);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Friend);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Rascal);
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Sundry);
        }

        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();
        //故事数据
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(ScenesEnum.GameInnScene);
        //初始化场景
        if (sceneInnManager != null)
        {
            InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
            sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);
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


        StartCoroutine(BuildNavMesh());

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
                npcCustomerBuilder.BuilderCustomerForInit(20);

                //设置位置
                Vector3 startPosition = sceneInnManager.GetTownEntranceLeft();
                BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
                baseControl.SetFollowPosition(startPosition);
            }
        }
    }

    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        navMesh.BuildNavMesh();
    }

    /// <summary>
    /// 初始化新的一天
    /// </summary>
    public void InitNewDay()
    {
        //随机化一个天气
        if (weatherHandler != null)
        {
            WeatherBean weatherData = weatherHandler.RandomWeather();
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
            //保存数据
            gameDataManager.SaveGameData(innHandler.GetInnRecord());
            //如果是工作状态结束一天 则进入结算画面
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameSettle));
        }
        else if (GameCommonInfo.CurrentDayData.dayStatus == GameTimeHandler.DayEnum.Rest)
        {
            //保存数据
            gameDataManager.SaveGameData(innHandler.GetInnRecord());
            //打开日历
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
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
                //停止控制
                if (controlHandler != null)
                    controlHandler.StopControl();
                if (dialogManager != null)
                {   //停止时间
                    gameTimeHandler.SetTimeStatus(true);
                    DialogBean dialogBean = new DialogBean();
                    dialogBean.content = GameCommonInfo.GetUITextById(3006);
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

    }
    #endregion
}