using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

public class SceneForInnHandler : SceneBaseHandler
{
    private void Awake()
    {
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }

    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType ==  GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            //初始化场景
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            InnBuildBean innBuildData = gameData.GetInnBuildData();
            SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
            sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);
            //StartCoroutine(CoroutineForBuildNavMesh());
        }
    }


    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForBuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        AstarPath.active.Scan();
    }
}