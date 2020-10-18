using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

public class SceneForInnHandler : BaseHandler, IBaseObserver
{
    protected GameTimeHandler gameTimeHandler;
    protected SceneInnManager sceneInnManager;
    protected GameDataManager gameDataManager;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>( ImportantTypeEnum.TimeHandler);
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);

        gameTimeHandler.AddObserver(this);
    }


    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if(observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                //初始化场景
                if (sceneInnManager != null)
                {
                    InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
                    sceneInnManager.InitScene(innBuildData.innWidth, innBuildData.innHeight);
                }
                //StartCoroutine(CoroutineForBuildNavMesh());
            }
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