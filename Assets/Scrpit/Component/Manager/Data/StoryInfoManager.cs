using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryInfoManager : BaseManager, IStoryInfoView
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
    public StoryInfoBean CheckStory(GameDataBean gameData)
    {
        if (mapStory == null)
            return null;
        foreach (long key in mapStory.Keys)
        {
            StoryInfoBean storyInfo = mapStory[key];
            //TODO 检测条件
            //判断该事件是否可重复触发
            if (storyInfo.trigger_loop == 0)
            {
                //如果已经触发过该事件
               if(gameData.CheckTriggeredEvent(storyInfo.id))
                    continue;
            }
            //是否触发
            if (storyInfo.trigger_date_year != 0)
            {
                //年是否满足触发条件
                if (storyInfo.trigger_date_year != gameData.gameTime.year)
                    continue;
            }
            if (storyInfo.trigger_date_month != 0)
            {
                //月是否满足触发条件
                if (storyInfo.trigger_date_month != gameData.gameTime.month)
                    continue;
            }
            if (storyInfo.trigger_date_day != 0)
            {
                //日是否满足触发条件
                if (storyInfo.trigger_date_day != gameData.gameTime.day)
                    continue;
            }
            storyInfoController.GetStoryDetailsById(key);
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