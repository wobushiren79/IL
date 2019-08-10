using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<StoryInfoBean> QueryAllStoryData()
    {
        return SQliteHandle.LoadTableData<StoryInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName);
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
        return SQliteHandle.LoadTableData<StoryInfoDetailsBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, colName, operations, colValue);
    }

}