using UnityEngine;
using UnityEditor;

public class InnLightCpt : LightCpt
{

    public override void OpenLight()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        int with = gameData.innBuildData.innWidth;
        int high = gameData.innBuildData.innHeight;
        SetInnLightSize(with, high);
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