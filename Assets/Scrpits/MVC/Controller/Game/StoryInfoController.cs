using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoController : BaseMVCController<StoryInfoModel, IStoryInfoView>
{
    public StoryInfoController(BaseMonoBehaviour content, IStoryInfoView view) : base(content, view)
    {
    }

    public override void InitData()
    {

    }

    public void GetStoryInfoByScene(ScenesEnum scene, Action<List<StoryInfoBean>> action)
    {
        List<StoryInfoBean> listData = GetModel().GetStoryInfoByScene(scene);
        if (listData != null)
            GetView().GetStoryInfoSuccess(listData, action);
        else
            GetView().GetStoryInfoFail();
    }

    public void GetAllStoryInfo(Action<List<StoryInfoBean>> action)
    {
        List<StoryInfoBean> listData = GetModel().GetAllStoryInfo();
        if (listData != null)
            GetView().GetStoryInfoSuccess(listData, action);
        else
            GetView().GetStoryInfoFail();
    }

    public void GetStoryDetailsById(long id, Action<List<StoryInfoDetailsBean>> action)
    {
        List<StoryInfoDetailsBean> listData = GetModel().GetStoryDetailsById(id);
        if (listData != null)
            GetView().GetStoryDetailsByIdSuccess(listData, action);
        else
            GetView().GetStoryDetailsFail();
    }
}