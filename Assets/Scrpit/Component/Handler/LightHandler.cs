using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LightHandler : BaseMonoBehaviour, IBaseObserver
{
    //灯光容器列表
    public List<GameObject> listLightContainer = new List<GameObject>();

    public GameTimeHandler gameTimeHandler;

    private void Start()
    {
        if (gameTimeHandler != null)
        {
            gameTimeHandler.AddObserver(this);
            //转换场景检测时间
            gameTimeHandler.GetTime(out float hour, out float min);
            if (hour >= 18)
            {
                SetAllLightStatus(true);
            }
        }
    }

    /// <summary>
    /// 打开或关闭所有灯光
    /// </summary>
    public void SetAllLightStatus(bool isOpen)
    {
        foreach (GameObject objLightList in listLightContainer)
        {
            LightCpt[] lightList = objLightList.GetComponentsInChildren<LightCpt>();
            if (lightList != null)
            {
                foreach (LightCpt itemLight in lightList)
                {
                    if (isOpen)
                        itemLight.OpenLight();
                    else
                        itemLight.CloseLight();
                }
            }
        }
    }

    #region 时间通知
    public void ObserbableUpdate<T>(T observable, int type, params Object[] obj) where T : Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                SetAllLightStatus(false);
            }
            else if (type == (int)GameTimeHandler.NotifyTypeEnum.Night)
            {
                SetAllLightStatus(true);
            }
        }
    }
    #endregion
}