using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class GameSeasonsHandler : BaseHandler<GameSeasonsHandler, GameSeasonsManager>
{
    protected List<GameObject> _listTreeContainer;
    protected SceneBuilder _builderForScene;

    public List<GameObject> listTreeContainer
    {
        get
        {
            _listTreeContainer = GameObject.FindGameObjectsWithTag(TagInfo.Tag_SeasonsContainer).ToList();
            return _listTreeContainer;
        }
    }
    public SceneBuilder builderForScene
    {
        get
        {
            if (_builderForScene == null)
            {
                GameObject obj = new GameObject("SceneBuilder");
                obj.transform.SetParent(transform);
                _builderForScene = obj.AddComponent<SceneBuilder>();
            }
            return _builderForScene;
        }
    }

    private void Start()
    {
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
    }

    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }

    /// <summary>
    /// 改变季节
    /// </summary>
    /// <param name="seasons"></param>
    public void ChangeSeasons(SeasonsEnum seasons)
    {
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
        builderForScene.BuildScene(seasons);
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