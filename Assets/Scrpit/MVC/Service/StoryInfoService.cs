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
        string[] colKeys = new string[] { "story_id", "story_order" };
        string[] operations = new string[] { "=", "=", };
        string[] colValues = new string[] { id + "", order + "", };
        SQliteHandle.DeleteTableData(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, colKeys, operations, colValues);
        //插入数据
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            Dictionary<string,object> mapData= ReflexUtil.GetAllNameAndValue(itemData);
            string[] insertKeys = new string[mapData.Count];
            string[] insertValues = new string[mapData.Count];
            int i = 0;
            foreach (var item in mapData)
            {
                if (item.Key.Equals("id"))
                {
                    insertKeys[i] = item.Key;
                    insertValues[i] = Convert.ToString(item.Value);
                }
                i++;
            }
         SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, insertKeys, insertValues);
        }

    }
}