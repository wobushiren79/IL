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