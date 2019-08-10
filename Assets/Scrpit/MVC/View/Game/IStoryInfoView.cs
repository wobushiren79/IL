using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IStoryInfoView
{
    void GetAllStoryInfoSuccess(List<StoryInfoBean> listData);

    void GetAllStoryInfoFail();

    void GetStoryDetailsByIdSuccess(List<StoryInfoDetailsBean> listData);

    void GetStoryDetailsFail();
}