using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryInfoModel : BaseMVCModel
{
    private StoryInfoService mStoryInfoService;

    public override void InitData()
    {
        mStoryInfoService = new StoryInfoService();
    }

    public List<StoryInfoBean> GetStoryInfoByScene(int scene)
    {
        return mStoryInfoService.QueryStoryInfoByScene(scene);
    }

    public List<StoryInfoBean> GetAllStoryInfo()
    {
      return  mStoryInfoService.QueryAllStoryData();
    }

    public List<StoryInfoDetailsBean> GetStoryDetailsById(long id)
    {
       return mStoryInfoService.QueryStoryDetailsById(id);
    }

}