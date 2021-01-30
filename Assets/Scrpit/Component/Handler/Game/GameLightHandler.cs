using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameLightHandler : BaseHandler<GameLightHandler,GameLightManager>
{
    //灯光容器列表
    protected List<GameObject> _listLightContainer = new List<GameObject>();
    //开灯时间
    public int openLightTime = 19;

    public List<GameObject> listLightContainer
    {
        get
        {
            if (CheckUtil.ListIsNull(_listLightContainer))
            {
                _listLightContainer = FindListWithTag<GameObject>(TagInfo.Tag_LightContainer);
            }
            return _listLightContainer;
        }
    }

    private void Start()
    {
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
        CheckTime();
    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }
    public void CheckTime()
    {
        //转换场景检测时间
        GameTimeHandler.Instance.GetTime(out float hour, out float min);
        if (hour >= openLightTime)
        {
            SetAllLightStatus(true);
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
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            SetAllLightStatus(false);
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            if (timeHour == openLightTime)
            {
                SetAllLightStatus(true);
            }
        }
    }
    #endregion
}