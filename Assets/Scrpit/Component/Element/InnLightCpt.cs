using UnityEngine;
using UnityEditor;

public class InnLightCpt : LightCpt
{
    protected GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public override void OpenLight()
    {
        if (gameDataManager != null)
        {
            int with = gameDataManager.gameData.innBuildData.innWidth;
            int high = gameDataManager.gameData.innBuildData.innHeight;
            SetInnLightSize(with, high);
        }
        base.OpenLight();
    }

    /// <summary>
    /// 设置客栈光源大小
    /// </summary>
    /// <param name="with"></param>
    /// <param name="high"></param>
    public void SetInnLightSize(int with, int high)
    {
        if (light2D != null)
        {
            light2D.transform.position = new Vector3(with / 2f, high / 2f);
            light2D.transform.localScale = new Vector3(with, high);
            //light2D.pointLightOuterRadius = with;
        }
            
    }

}