using UnityEditor;
using UnityEngine;

public class SceneGameCourtyardInit : BaseNormalSceneInit
{
    public override void Start()
    {
        base.Start();
        RefreshScene();
    }

    public override void RefreshScene()
    {
        base.RefreshScene();

        InitTestData();

        //构建重要的NPC
        //NpcHandler.Instance.buildForImportant.BuildImportantForMountain();
        //改变四季
        GameSeasonsHandler.Instance.ChangeSeasons();

        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();

        //设置天气
        GameWeatherHandler.Instance.SetWeather(GameCommonInfo.CurrentDayData.weatherToday);

        //构建地板
        InnBuildHandler.Instance.builderForCourtyard.StartBuild();
    }

    /// <summary>
    /// 初始化测试数据
    /// </summary>
    public void InitTestData()
    {
        //测试数据
        var gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.userId.IsNull())
        {
            gameData.AddNewItems(9900001, 999);
            gameData.AddNewItems(9901001, 999);
            gameData.AddNewItems(9901002, 999);
            gameData.AddNewItems(9902001, 999);
            gameData.AddNewItems(9902002, 999);
            gameData.AddNewItems(9910001, 999);
            GameTimeHandler.Instance.SetTime(9, 0);
            InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
            innCourtyardData.courtyardLevel = 9;
        }
    }

    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt moveControl = base.InitUserPosition();
        //位置控制
        Vector3 startPosition;
        SceneCourtyardManager sceneCourtyardManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneCourtyardManager>();
        switch (GameCommonInfo.ScenesChangeData.beforeScene)
        {
            default:
                startPosition = sceneCourtyardManager.GetCourtyardEntrance();
                break;
        }
        moveControl.SetPosition(startPosition);
        return moveControl;
    }
}