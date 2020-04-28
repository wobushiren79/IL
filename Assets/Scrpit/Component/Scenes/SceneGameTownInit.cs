using UnityEngine;
using UnityEditor;
using System;

public class SceneGameTownInit : BaseNormalSceneInit
{
    public NpcImportantBuilder npcImportantBuilder;
    public NpcPasserBuilder npcPasserBuilder;

    protected InnBuildManager innBuildManager;
    protected SceneTownManager sceneTownManager;

    public override void Awake()
    {
        base.Awake();
        sceneTownManager = Find<SceneTownManager>(ImportantTypeEnum.SceneManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }


    public override void Start()
    {
        base.Start();
        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();
        //构建重要的NPC
        if (npcImportantBuilder != null)
            npcImportantBuilder.BuildImportant();
        //构建普通路人NPC
        if (npcPasserBuilder != null)
            npcPasserBuilder.BuilderPasserForInit(20);
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
        }
        return moveControl;
    }
}