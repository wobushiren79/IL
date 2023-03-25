using UnityEngine;
using UnityEditor;

public class SceneGameMountainInit : BaseNormalSceneInit
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
        NpcHandler.Instance.buildForImportant.BuildImportantForMountain();
        //改变四季
        GameSeasonsHandler.Instance.ChangeSeasons();

        UIHandler.Instance.OpenUI<UIGameMain>();
    }

    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt moveControl = base.InitUserPosition();
        SceneMountainManager sceneMountainManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneMountainManager>();
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