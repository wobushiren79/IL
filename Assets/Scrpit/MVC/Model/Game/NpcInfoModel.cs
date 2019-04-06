using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoModel : BaseMVCModel
{
    private NpcInfoService mNpcInfoService;

    public override void InitData()
    {
        mNpcInfoService = new NpcInfoService();
    }

    /// <summary>
    /// 获取所有NPC信息
    /// </summary>
    /// <returns></returns>
    public List<NpcInfoBean> GetAllNpcInfo()
    {
        return mNpcInfoService.QueryAllData();
    }

    /// <summary>
    /// 通过NPC类型获取NPC
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public List<NpcInfoBean> GetNpcInfoByType(int[] types)
    {
        return mNpcInfoService.QueryDataByType(types);
    }

    /// <summary>
    /// 通过NPC类型获取NPC
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public List<NpcInfoBean> GetNpcInfoByType(List<int> types)
    {
        return mNpcInfoService.QueryDataByType(TypeConversionUtil.ListToArray(types));
    }
}