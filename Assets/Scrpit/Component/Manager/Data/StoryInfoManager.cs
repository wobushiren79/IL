using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoManager : BaseManager, IStoryInfoView
{
    public StoryInfoController storyInfoController;
    public Dictionary<long, StoryInfoBean> mapStory = new Dictionary<long, StoryInfoBean>();
    //剧情道具
    public Dictionary<string, GameObject> dicStoryProp =new Dictionary<string, GameObject>();
    //剧情所用人物模型
    public GameObject objNpcModel;

    private void Awake()
    {
        objNpcModel = LoadAssetUtil.SyncLoadAsset<GameObject>("character/character", "CharacterForStory");
        storyInfoController = new StoryInfoController(this, this);
        storyInfoController.GetAllStoryInfo(null);
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

    public void GetStoryDetailsById(long id, Action<List<StoryInfoDetailsBean>> action)
    {
        storyInfoController.GetStoryDetailsById(id, action);
    }

    /// <summary>
    /// 获取故事道具模型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetStoryPropModelByName(string name)
    {
        GameObject objModel = GetModel(dicStoryProp, "", name);
        return objModel;
    }

    /// <summary>
    /// 检测故事是否触发
    /// </summary>
    /// 
    public StoryInfoBean CheckStory(GameDataBean gameData,ScenesEnum scenesEnum)
    {
        return CheckStory(gameData, scenesEnum,TownBuildingEnum.Town, 2);
    }
    public StoryInfoBean CheckStory(GameDataBean gameData,ScenesEnum scenesEnum, TownBuildingEnum positionType, int outOrIn)
    {
        if (mapStory == null)
            return null;
        foreach (long key in mapStory.Keys)
        {
            StoryInfoBean storyInfo = mapStory[key];
            //TODO 检测条件
            //判断场景是否符合
            if (scenesEnum != storyInfo.GetStoryScene())
            {
                continue;
            }
            //判断该事件是否可重复触发
            if (storyInfo.trigger_loop == 0)
            {
                //如果已经触发过该事件
                if (gameData.CheckTriggeredEvent(storyInfo.id))
                    continue;
            }
            //检测触发条件
            if (!EventTriggerEnumTools.CheckIsAllTrigger(gameData, storyInfo.trigger_condition))
            {
                continue;
            }
            //如果是小镇
            if (storyInfo.story_scene == (int)ScenesEnum.GameTownScene)
            {
                //判断地点是否正确
                if (positionType != (TownBuildingEnum)storyInfo.location_type || outOrIn != storyInfo.out_in)
                    continue;
            }
            return storyInfo;
        }
        return null;
    }
    #region 故事数据回调
    public void GetStoryInfoFail()
    {
    }

    public void GetStoryDetailsFail()
    {
    }

    public void GetStoryInfoSuccess(List<StoryInfoBean> listData, Action<List<StoryInfoBean>> action)
    {
        mapStory = new Dictionary<long, StoryInfoBean>();
        foreach (StoryInfoBean itemData in listData)
        {
            mapStory.Add(itemData.id, itemData);
        }
        action?.Invoke(listData);
    }

    public void GetStoryDetailsByIdSuccess(List<StoryInfoDetailsBean> listData, Action<List<StoryInfoDetailsBean>> action)
    {
        action?.Invoke(listData);
    }
    #endregion

}