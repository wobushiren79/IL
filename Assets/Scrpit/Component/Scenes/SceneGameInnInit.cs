using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;

public class SceneGameInnInit : BaseManager,IBaseObserver
{
    public UIGameManager uiGameManager;
    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;
    public NpcInfoManager npcInfoManager;
    public InnBuildManager innBuildManager;
    public StoryInfoManager storyInfoManager;

    public InnFloorBuilder innFloorBuilder;
    public InnWallBuilder innWallBuilder;
    public InnFurnitureBuilder innFurnitureBuilder;

    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public WeatherHandler weatherHandler;

    public NavMeshSurface navMesh;

    public NpcCustomerBuilder leftCustomerBuilder;
    public NpcCustomerBuilder rightCustomerBuilder;

    private void Start()
    {
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (gameDataManager != null)
        {
            if (GameCommonInfo.gameData != null)
            {
                gameDataManager.gameData = GameCommonInfo.gameData;
            }
            else
            {
                gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
            }
        }
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();
        //故事数据
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(1);

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
            innHandler.InitInn();

        //客栈边界生成
        if (leftCustomerBuilder != null)
        {
            leftCustomerBuilder.transform.position = new Vector3(-30, -2.5f, 0);
        }
        if (rightCustomerBuilder != null)
        {
            rightCustomerBuilder.transform.position = new Vector3(30 + gameDataManager.gameData.GetInnBuildData().innWidth, -2.5f, 0);
        }

        StartCoroutine(BuildNavMesh());

        if (gameDataManager != null&& gameTimeHandler != null)
        {
            //增加回调
            gameTimeHandler.AddObserver(this);
            TimeBean timeData = gameDataManager.gameData.gameTime;
            if (timeData.hour == 0 && timeData.minute == 0)
            {
                //如果是需要切换第二天
                gameTimeHandler.SetTimeStatus(true);
                uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameDate));
            }
            else
            {
                //如果是其他场景切换过来
                gameTimeHandler.SetTime(timeData.hour, timeData.minute);
                gameTimeHandler.SetTimeStatus(false);
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
            WeatherBean weatherData=  weatherHandler.RandomWeather();
            gameDataManager.gameData.weatherToday = weatherData;
        }
           
    }


    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params Object[] obj) where T : Object
    {
        if(observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                InitNewDay();
            }
        }
    }
    #endregion

}