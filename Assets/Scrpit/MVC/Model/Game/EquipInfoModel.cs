using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EquipInfoModel : BaseMVCModel
{
    private EquipInfoService mEquipInfoService;

    public override void InitData()
    {
        mEquipInfoService = new EquipInfoService();
    }

    /// <summary>
    /// 获取所有装备信息
    /// </summary>
    /// <returns></returns>
    public List<EquipInfoBean> GetAllEquipInfo()
    {
        List<EquipInfoBean> listData = mEquipInfoService.QueryAllData();
        return listData;
    }


    /// <summary>
    /// 根据ID获取装备数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<EquipInfoBean> GetEquipInfoByIds(long[] ids)
    {
        List<EquipInfoBean> listData = mEquipInfoService.QueryDataByIds(ids);
        return listData;
    }

    public List<EquipInfoBean> GetEquipInfoByIds(List<long> ids)
    {
        List<EquipInfoBean> listData = GetEquipInfoByIds(TypeConversionUtil.ListToArray(ids));
        return listData;
    }
}