using UnityEngine;
using UnityEditor;
using System.Collections;

public class BaseSceneInit : BaseMonoBehaviour
{
    protected UIGameManager uiGameManager;
    protected GameDataManager gameDataManager;
    protected GameTimeHandler gameTimeHandler;

    protected WeatherHandler weatherHandler;
    protected GameDataHandler gameDataHandler;


    public virtual void Awake()
    {
        weatherHandler = Find<WeatherHandler>(ImportantTypeEnum.WeatherHandler);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataHandler = Find<GameDataHandler>(ImportantTypeEnum.GameDataHandler);
    }

    public virtual void Start()
    {
        if (gameDataManager != null)
        {
            if (GameCommonInfo.GameData == null || CheckUtil.StringIsNull(GameCommonInfo.GameData.userId))
            {
                gameDataManager.GetGameDataByUserId(GameCommonInfo.GameUserId);
            }
            else
            {
                gameDataManager.gameData = GameCommonInfo.GameData;
            }
        }
        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        if (AstarPath.active != null)
            AstarPath.active.Scan();
    }

    public virtual void RefreshScene()
    {

    }

}