using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoManager : BaseManager
{
    public Dictionary<long, StoryInfoBean> mapStory = new Dictionary<long, StoryInfoBean>();
    //剧情道具
    public Dictionary<string, GameObject> dicStoryProp = new Dictionary<string, GameObject>();
    //剧情所用人物模型
    public GameObject objNpcModel;

    public static string pathStoryItems = "Assets/Prefabs/Stroy";
    public void Awake()
    {
        objNpcModel = LoadAddressablesUtil.LoadAssetSync<GameObject>("Assets/Prefabs/Character/CharacterForStory.prefab");
        mapStory = StoryInfoCfg.GetAllData() ?? new Dictionary<long, StoryInfoBean>();
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    public StoryInfoBean GetStoryInfoDataById(long id)
    {
        return GetDataById(id, mapStory);
    }

    public void GetStoryDetailsById(long id, Action<List<StoryInfoDetailsBean>> action)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        StoryInfoDetailsBean[] arrayData = StoryInfoDetailsCfg.GetAllArrayData();
        if (arrayData != null)
        {
            foreach (StoryInfoDetailsBean item in arrayData)
            {
                if (item.id == id)
                    listData.Add(item);
            }
        }
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取故事道具模型
    /// </summary>
    public GameObject GetStoryPropModelByName(string name)
    {
        GameObject objModel = GetModelForAddressablesSync(dicStoryProp, $"{pathStoryItems}/{name}.prefab");
        return objModel;
    }

    /// <summary>
    /// 检测故事是否触发
    /// </summary>
    public StoryInfoBean CheckStory(GameDataBean gameData, ScenesEnum scenesEnum)
    {
        return CheckStory(gameData, scenesEnum, TownBuildingEnum.Town, 2);
    }
    public StoryInfoBean CheckStory(GameDataBean gameData, ScenesEnum scenesEnum, TownBuildingEnum positionType, int outOrIn)
    {
        if (mapStory == null)
            return null;
        foreach (long key in mapStory.Keys)
        {
            StoryInfoBean storyInfo = mapStory[key];
            if (scenesEnum != storyInfo.GetStoryScene())
            {
                continue;
            }
            if (storyInfo.trigger_loop == 0)
            {
                if (gameData.CheckTriggeredEvent(storyInfo.id))
                    continue;
            }
            if (!EventTriggerEnumTools.CheckIsAllTrigger(gameData, storyInfo.trigger_condition))
            {
                continue;
            }
            if (storyInfo.story_scene == (int)ScenesEnum.GameTownScene)
            {
                if (positionType != (TownBuildingEnum)storyInfo.location_type || outOrIn != storyInfo.out_in)
                    continue;
            }
            return storyInfo;
        }
        return null;
    }
}
