using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SeasonsHandler : BaseHandler, IBaseObserver
{
    public List<GameObject> listTreeContainer;

    protected GameTimeHandler gameTimeHandler;
    protected SceneBuilder sceneBuilder;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        sceneBuilder = Find<SceneBuilder>(ImportantTypeEnum.SceneBuilder);
    }

    private void Start()
    {
        if (gameTimeHandler != null)
        {
            gameTimeHandler.AddObserver(this);
            ChangeSeasons();
        }
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
            TreeCpt[] listTree= itemTreeContainer.GetComponentsInChildren<TreeCpt>();
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
        gameTimeHandler.GetTime(out int year, out int month, out int day);
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
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                ChangeSeasons();
            }
        }
    }
    #endregion
}