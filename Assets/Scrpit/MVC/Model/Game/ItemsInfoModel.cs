using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsInfoModel : BaseMVCModel
{
    private ItemsInfoService mEquipInfoService;

    public override void InitData()
    {
        mEquipInfoService = new ItemsInfoService();
    }

    /// <summary>
    /// 获取所有装备信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetAllEquipInfo()
    {
        List<ItemsInfoBean> listData = mEquipInfoService.QueryAllData();
        return listData;
    }


    /// <summary>
    /// 根据ID获取装备数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsInfoByIds(long[] ids)
    {
        List<ItemsInfoBean> listData = mEquipInfoService.QueryDataByIds(ids);
        return listData;
    }

    public List<ItemsInfoBean> GetItemsInfoByIds(List<long> ids)
    {
        List<ItemsInfoBean> listData = GetItemsInfoByIds(TypeConversionUtil.ListToArray(ids));
        return listData;
    }
}