using UnityEngine;
using UnityEditor;
using System;

public class SceneGameTownInit : BaseSceneInit, IBaseObserver, DialogView.IDialogCallBack
{
    public InnBuildManager innBuildManager;
    public StoryInfoManager storyInfoManager;
    public SceneTownManager sceneTownManager;

    public NpcImportantBuilder npcImportantBuilder;
    public NpcPasserBuilder npcPasserBuilder;

    public GameTimeHandler gameTimeHandler;
    public WeatherHandler weatherHandler;



    private new void Start()
    {
        base.Start();
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(2);

        //构建重要的NPC
        if (npcImportantBuilder != null)
            npcImportantBuilder.BuildImportant();
        //构建普通路人NPC
        if (npcPasserBuilder != null)
            npcPasserBuilder.BuilderPasserForInit(20);

        if (gameTimeHandler != null && gameDataManager != null)
        {
            TimeBean timeData = gameDataManager.gameData.gameTime;
            gameTimeHandler.SetTime(timeData.hour, timeData.minute);
            gameTimeHandler.SetTimeStatus(false);
            //增加回调
            gameTimeHandler.AddObserver(this);
        }

        //设置天气
        if (weatherHandler != null)
        {
            weatherHandler.SetWeahter(gameDataManager.gameData.weatherToday);
        }
        //设置角色位置
        InitUserPosition();
    }

    /// <summary>
    /// 结束一天
    /// </summary>
    public void EndDay()
    {
        //停止时间
        if (gameTimeHandler != null)
            gameTimeHandler.SetTimeStatus(true);
        //停止控制
        if (controlHandler != null)
            controlHandler.StopControl();
        //重置游戏时间
        GameCommonInfo.GameData.gameTime.hour = 0;
        GameCommonInfo.GameData.gameTime.minute = 0;

        if (dialogManager != null)
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.content = GameCommonInfo.GetUITextById(3006);
            dialogManager.CreateDialog(1, this, dialogBean, 5);
        }
        else
        {
            SceneUtil.SceneChange(ScenesEnum.GameInnScene);
        }
    }

    /// <summary>
    /// 初始化用户位置
    /// </summary>
    public void InitUserPosition()
    {
        //开始角色控制
        ControlForMoveCpt moveControl = (ControlForMoveCpt)controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
        //位置控制
        switch (GameCommonInfo.ScenesChangeData.beforeScene)
        {
            case ScenesEnum.GameArenaScene:
                moveControl.SetPosition(GameCommonInfo.ScenesChangeData.beforeUserPosition);
                break;
            case ScenesEnum.GameInnScene:
                Vector3 doorPosition = sceneTownManager.GetMainTownDoorPosition();
                moveControl.SetPosition(doorPosition);
                break;
        }
    }

    #region  时间通知回调
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {
                EndDay();
            }
        }
    }
    #endregion

    #region  弹窗通知回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}