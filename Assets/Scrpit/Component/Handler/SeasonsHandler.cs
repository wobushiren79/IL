using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SeasonsHandler : BaseHandler
{
    public List<GameObject> listTreeContainer;

    protected SceneBuilder sceneBuilder;

    private void Awake()
    {
        sceneBuilder = Find<SceneBuilder>(ImportantTypeEnum.SceneBuilder);
    }

    private void Start()
    {
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
        ChangeSeasons();
    }

    /// <summary>
    /// 改变季节
    /// </summary>
    /// <param name="seasons"></param>
    public void ChangeSeasons(SeasonsEnum seasons)
    {
        switch (seasons)
        {
            case SeasonsEnum.Spring:
                break;
            case SeasonsEnum.Summer:
                break;
            case SeasonsEnum.Autumn:
                break;
            case SeasonsEnum.Winter:
                break;
        }
        //所有植被修改
        foreach (GameObject itemTreeContainer in listTreeContainer)
        {
            TreeCpt[] listTree = itemTreeContainer.GetComponentsInChildren<TreeCpt>();
            foreach (TreeCpt itemTree in listTree)
            {
                itemTree.SetData(seasons);
            }
        }
        //地面和草地修改
        sceneBuilder.BuildScene(seasons);
    }

    public void ChangeSeasons()
    {
        GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);
        if (month >= 1 && month <= 4)
        {
            ChangeSeasons((SeasonsEnum)month);
        }
        else
        {
            ChangeSeasons(SeasonsEnum.Spring);
        }
    }

    #region 时间回调
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ChangeSeasons();
        }
    }
    #endregion
}