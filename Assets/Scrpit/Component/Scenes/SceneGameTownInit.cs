using UnityEngine;
using UnityEditor;
using System;

public class SceneGameTownInit : BaseNormalSceneInit
{
    protected SceneTownManager sceneTownManager;

    public override void Awake()
    {
        base.Awake();
        sceneTownManager = Find<SceneTownManager>(ImportantTypeEnum.SceneManager);
    }

    public override void Start()
    {
        base.Start();
        RefreshScene();       

    }

    public override void RefreshScene()
    {
        base.RefreshScene();
        //构建重要的NPC
        NpcHandler.Instance.buildForImportant.BuildImportantForTown();
        //构建普通路人NPC
        NpcHandler.Instance.buildForPasser.BuilderPasserForInit(20);

        //改变四季
        GameSeasonsHandler.Instance.ChangeSeasons();
    }

    /// <summary>
    /// 初始化用户位置
    /// </summary>
    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt moveControl = base.InitUserPosition();
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
            case ScenesEnum.GameMountainScene:
                moveControl.SetPosition(sceneTownManager.GetMountainDoorPosition());
                break;
        }
        return moveControl;
    }

}