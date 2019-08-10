using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryBuilder : BaseMonoBehaviour, StoryInfoManager.CallBack
{
    public StoryInfoManager storyInfoManager;
    public StoryInfoBean storyInfo;
    public List<StoryInfoDetailsBean> listStoryDetails;

    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        storyInfoManager.GetStoryDetailsById(storyInfo.id,this);
    }

    public void GetStoryDetailsSuccess(List<StoryInfoDetailsBean> listData)
    {
        listStoryDetails = listData;
    }
}