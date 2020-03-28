using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MenuInfoService : BaseMVCService
{
    public MenuInfoService():base("menu_info", "menu_info_details_" + GameCommonInfo.GameConfig.language)
    {
    }

    /// <summary>
    /// 查询所有菜单数据
    /// </summary>
    /// <returns></returns>
    public List<MenuInfoBean> QueryAllData()
    {
        return BaseQueryAllData<MenuInfoBean>("menu_id");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<MenuInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<MenuInfoBean>("menu_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <returns></returns>
    public bool InsertData(MenuInfoBean menuInfo)
    {
        List<string> listLeftKey = new List<string>
        {
            "menu_id",
            "name",
            "content"
        };
        return BaseInsertDataWithLeft(menuInfo, listLeftKey);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteData(long id)
    {
        return BaseDeleteDataWithLeft("id", "menu_id", id+"");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <returns></returns>
    public bool Update(MenuInfoBean menuInfo)
    {
        if(DeleteData(menuInfo.id))
        {
           return InsertData(menuInfo);
        }
        return false;
    }
}