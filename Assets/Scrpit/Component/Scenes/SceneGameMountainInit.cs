using UnityEngine;
using UnityEditor;

public class SceneGameMountainInit : BaseNormalSceneInit
{
    public SceneMountainManager sceneMountainManager;

    public override void Awake()
    {
        base.Awake();
        sceneMountainManager = Find<SceneMountainManager>(ImportantTypeEnum.SceneManager);
    }

    public override void Start()
    {
        base.Start();
        //故事数据
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(ScenesEnum.GameMountainScene);
    }


    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt controlForMove = base.InitUserPosition();
        //位置控制
        Vector3 exitPosition = sceneMountainManager.GetExitDoor();
        controlForMove.SetPosition(exitPosition);
        return controlForMove;
    }

}