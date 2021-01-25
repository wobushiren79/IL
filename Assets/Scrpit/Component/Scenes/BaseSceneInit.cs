using UnityEngine;
using UnityEditor;
using System.Collections;

public class BaseSceneInit : BaseMonoBehaviour
{
    protected WeatherHandler weatherHandler;


    public virtual void Awake()
    {
        weatherHandler = Find<WeatherHandler>(ImportantTypeEnum.WeatherHandler);
    }

    public virtual void Start()
    {
        GameDataBean gameData =  GameDataHandler.Instance.manager.GetGameData();
        if (GameCommonInfo.GameData == null || CheckUtil.StringIsNull(GameCommonInfo.GameData.userId))
        {
            GameDataHandler.Instance.manager.GetGameDataByUserId(GameCommonInfo.GameUserId);
        }
        else
        {
            gameData = GameCommonInfo.GameData;
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