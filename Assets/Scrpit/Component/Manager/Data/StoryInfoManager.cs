using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryInfoManager : BaseManager,IStoryInfoView
{
    public StoryInfoController storyInfoController;
    public Dictionary<long, StoryInfoBean> mapStory;

    public CallBack callBack;

    private void Awake()
    {
        storyInfoController = new StoryInfoController(this, this);
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public StoryInfoBean GetStoryInfoDataById(long id)
    {
       return GetDataById(id, mapStory);
    }

    public void GetStoryDetailsById(long id, CallBack callBack)
    {
        this.callBack = callBack;
        storyInfoController.GetStoryDetailsById(id);
    }

    /// <summary>
    /// 检测故事是否触发
    /// </summary>
    public StoryInfoBean CheckStory()
    {
        if (mapStory == null)
            return null;
        foreach (long key in mapStory.Keys)
        {
            StoryInfoBean storyInfo = mapStory[key];
            //TODO 检测条件
            if (key == 1)
            {
                storyInfoController.GetStoryDetailsById(key);
            }
            return storyInfo;
        }
        return null;
    }

    #region 故事数据回调
    public void GetStoryInfoSuccess(List<StoryInfoBean> listData)
    {
        mapStory = new Dictionary<long, StoryInfoBean>();
        foreach (StoryInfoBean itemData in listData)
        {
            mapStory.Add(itemData.id, itemData);
        }
    }

    public void GetStoryDetailsByIdSuccess(List<StoryInfoDetailsBean> listData)
    {
        if (callBack != null)
            callBack.GetStoryDetailsSuccess(listData);
    }

    public void GetStoryInfoFail()
    {
    }

    public void GetStoryDetailsFail()
    {
    }
    #endregion

    public interface CallBack
    {
        void GetStoryDetailsSuccess(List<StoryInfoDetailsBean> listData);
    }
}