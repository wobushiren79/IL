using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

public class SceneForInnHandler : BaseHandler
{
    protected SceneInnManager sceneInnManager;

    private void Awake()
    {
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);

        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
    }


    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType ==  GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            //初始化场景
            if (sceneInnManager != null)
            {
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                InnBuildBean innBuildData = gameData.GetInnBuildData();
                sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);
            }
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