using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoManager : BaseManager
{
    public Dictionary<long, StoryInfoBean> mapStory = new Dictionary<long, StoryInfoBean>();
    //剧情道具
    public Dictionary<string, GameObject> dicStoryProp =new Dictionary<string, GameObject>();
    //剧情所用人物模型
    public GameObject objNpcModel;
    protected StoryInfoService storyInfoService;

    public static string pathStoryItems = "Assets/Prefabs/Stroy";
    public void Awake()
    {
        objNpcModel = LoadAddressablesUtil.LoadAssetSync<GameObject>("Assets/Prefabs/Character/CharacterForStory.prefab");
        storyInfoService = new StoryInfoService();
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
        List<StoryInfoDetailsBean> listData = storyInfoService.QueryStoryDetailsById(id);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取故事道具模型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetStoryPropModelByName(string name)
    {
        GameObject objModel = GetModelForAddressablesSync(dicStoryProp, $"{pathStoryItems}/{name}.prefab");
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
}