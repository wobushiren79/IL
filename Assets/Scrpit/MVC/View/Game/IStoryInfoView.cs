using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IStoryInfoView
{
    void GetStoryInfoSuccess(List<StoryInfoBean> listData);

    void GetStoryInfoFail();

    void GetStoryDetailsByIdSuccess(List<StoryInfoDetailsBean> listData);

    void GetStoryDetailsFail();
}