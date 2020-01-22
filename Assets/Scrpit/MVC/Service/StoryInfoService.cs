using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoService : BaseMVCService
{

    public StoryInfoService() : base("story_info", "story_info_details")
    {

    }

    /// <summary>
    ///  查询故事数据
    /// </summary>
    /// <returns></returns>
    public List<StoryInfoBean> QueryAllStoryData()
    {
        return BaseQueryAllData<StoryInfoBean>();
    }

    public List<StoryInfoBean> QueryStoryData(long id)
    {
        return BaseQueryData<StoryInfoBean>("id", id + "");
    }

    public List<StoryInfoBean> QueryStoryData(int scene)
    {
        return BaseQueryData<StoryInfoBean>("story_scene", scene + "");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="storyInfoBean"></param>
    public void UpdateStoryData(StoryInfoBean storyInfoBean)
    {
        BaseDeleteDataById(storyInfoBean.id);
        BaseInsertData(tableNameForMain, storyInfoBean);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    public void DeleteDataById(long id)
    {
        BaseDeleteDataById(id);
        BaseDeleteData(tableNameForLeft, "story_id", "" + id);
    }

    /// <summary>
    /// 根据场景查询故事数据
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public List<StoryInfoBean> QueryStoryInfoByScene(int scene)
    {
        string[] colName = new string[] { "story_scene" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { scene + "" };
        return SQliteHandle.LoadTableDataByCol<StoryInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, colName, operations, colValue);

    }

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="storyInfo"></param>
    public void CreateStoryInfo(StoryInfoBean storyInfo)
    {
        BaseInsertData(tableNameForMain, storyInfo);
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<StoryInfoDetailsBean> QueryStoryDetailsById(long id)
    {
        string[] colName = new string[] { "story_id" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { id + "" };
        return SQliteHandle.LoadTableDataByCol<StoryInfoDetailsBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForLeft, colName, operations, colValue);
    }

    /// <summary>
    /// 根据ID和序号修改数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order"></param>
    public void UpdateStoryDetailsByIdAndOrder(long id, int order, List<StoryInfoDetailsBean> listData)
    {
        if (listData == null)
            return;
        //先删除该ID和序号下的所有数据
        BaseDeleteData(tableNameForLeft, "story_id", id + "", "story_order", order + "");
        //插入数据
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            BaseInsertData(tableNameForLeft, itemData);
        }
    }
}