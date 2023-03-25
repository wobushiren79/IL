using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildDataModel : BaseMVCModel
{
    private BuildItemService mBuildItemService;

    public override void InitData()
    {
        mBuildItemService = new BuildItemService();
    }

    /// <summary>
    /// 查询所有建筑信息
    /// </summary>
    /// <returns></returns>
    public List<BuildItemBean> GetAllBuildItems()
    {
       return mBuildItemService.QueryAllData();
    }

    /// <summary>
    /// 查询指定建筑信息
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<BuildItemBean> GetBuildItemsByType(int type)
    {
        return mBuildItemService.QueryDataByType(type);
    }
}