using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoryInfoService
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;

    public StoryInfoService()
    {
        mTableName = "story_info";
        mLeftDetailsTableName = "story_info_details";
    }

    /// <summary>
    ///  查询故事数据
    /// </summary>
    /// <returns></returns>
    public List<StoryInfoBean> QueryAllStoryData()
    {
        return SQliteHandle.LoadTableData<StoryInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName);
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
        return SQliteHandle.LoadTableDataByCol<StoryInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, colName, operations, colValue);

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
        return SQliteHandle.LoadTableDataByCol<StoryInfoDetailsBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, colName, operations, colValue);
    }

}