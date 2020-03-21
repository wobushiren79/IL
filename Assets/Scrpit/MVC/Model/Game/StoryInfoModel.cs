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

    public List<StoryInfoBean> GetStoryInfoByScene(ScenesEnum scene)
    {
        return mStoryInfoService.QueryStoryInfoByScene((int)scene);
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