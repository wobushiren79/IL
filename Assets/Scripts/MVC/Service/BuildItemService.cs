using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildItemService : BaseMVCService
{
    public BuildItemService() : base("build_item", "build_item_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {
    }

    /// <summary>
    /// 查询所有装修物品数据
    /// </summary>
    /// <returns></returns>
    public List<BuildItemBean> QueryAllData()
    {
        return BaseQueryAllData<BuildItemBean>("build_id");
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<BuildItemBean> QueryDataByType(int type)
    {
        return BaseQueryData<BuildItemBean>("build_id", tableNameForMain + ".build_type", "" + type);
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="buildItem"></param>
    /// <returns></returns>
    public bool InsertData(BuildItemBean buildItem)
    {
        List<string> listLeftKey = new List<string>
        {
            "build_id",
            "name",
            "content"
        };
        return BaseInsertDataWithLeft(buildItem, listLeftKey);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteData(long id)
    {
        return BaseDeleteDataWithLeft("id", "build_id", id+"");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void Update(BuildItemBean buildItem)
    {
        if(DeleteData(buildItem.id))
        {
            InsertData(buildItem);
        }
    }
}