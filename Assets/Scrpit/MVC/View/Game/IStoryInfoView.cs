using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface IStoryInfoView
{
    void GetStoryInfoSuccess(List<StoryInfoBean> listData, Action<List<StoryInfoBean>> action);

    void GetStoryInfoFail();

    void GetStoryDetailsByIdSuccess(List<StoryInfoDetailsBean> listData, Action<List<StoryInfoDetailsBean>> action);

    void GetStoryDetailsFail();
}