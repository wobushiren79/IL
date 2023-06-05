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
        //构建重要的NPC
        //NpcHandler.Instance.buildForImportant.BuildImportantForMountain();
        //改变四季
        GameSeasonsHandler.Instance.ChangeSeasons();

        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();

        //设置天气
        GameWeatherHandler.Instance.SetWeather(GameCommonInfo.CurrentDayData.weatherToday);
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