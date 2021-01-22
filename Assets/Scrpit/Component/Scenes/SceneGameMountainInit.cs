using UnityEngine;
using UnityEditor;

public class SceneGameMountainInit : BaseNormalSceneInit
{
    public SceneMountainManager sceneMountainManager;
    public NpcImportantBuilder npcImportantBuilder;

    public override void Awake()
    {
        base.Awake();
        sceneMountainManager = Find<SceneMountainManager>(ImportantTypeEnum.SceneManager);
        npcImportantBuilder = Find<NpcImportantBuilder>(ImportantTypeEnum.NpcBuilder);
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
        if (npcImportantBuilder != null)
            npcImportantBuilder.BuildImportantForMountain();
    }

    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt moveControl = base.InitUserPosition();

        //位置控制
        switch (GameCommonInfo.ScenesChangeData.beforeScene)
        {
            case ScenesEnum.GameTownScene:
                moveControl.SetPosition(sceneMountainManager.GetExitDoor());
                break;
            case ScenesEnum.GameInfiniteTowersScene:
                Vector3 doorPosition = sceneMountainManager.GetInfiniteTowersStairs();
                moveControl.SetPosition(doorPosition);
                break;
        }
        return moveControl;
    }
}