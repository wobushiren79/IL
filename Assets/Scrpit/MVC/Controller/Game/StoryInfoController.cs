using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryInfoController : BaseMVCController<StoryInfoModel, IStoryInfoView>
{
    public StoryInfoController(BaseMonoBehaviour content, IStoryInfoView view) : base(content, view)
    {
    }

    public override void InitData()
    {

    }

    public void GetStoryInfoByScene(ScenesEnum scene)
    {
        List<StoryInfoBean> listData = GetModel().GetStoryInfoByScene(scene);
        if (listData != null)
            GetView().GetStoryInfoSuccess(listData);
        else
            GetView().GetStoryInfoFail();
    }

    public void GetAllStoryInfo()
    {
        List<StoryInfoBean> listData = GetModel().GetAllStoryInfo();
        if (listData != null)
            GetView().GetStoryInfoSuccess(listData);
        else
            GetView().GetStoryInfoFail();
    }

    public void GetStoryDetailsById(long id)
    {
        List<StoryInfoDetailsBean> listData = GetModel().GetStoryDetailsById(id);
        if (listData != null)
            GetView().GetStoryDetailsByIdSuccess(listData);
        else
            GetView().GetStoryDetailsFail();
    }
}