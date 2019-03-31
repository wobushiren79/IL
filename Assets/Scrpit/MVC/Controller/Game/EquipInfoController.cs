using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EquipInfoController : BaseMVCController<EquipInfoModel, IEquipInfoView>
{
    public EquipInfoController(BaseMonoBehaviour content, IEquipInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 查询所有装备信息
    /// </summary>
    public void GetAllEquipInfo()
    {
        List<EquipInfoBean> listData = GetModel().GetAllEquipInfo();
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetEquipInfoSuccess(listData);
        }
        else
        {
            GetView().GetEquipInfoFail();
        }
    }

    /// <summary>
    /// 根据ID获取装备
    /// </summary>
    /// <param name="ids"></param>
    public void GetEquipInfoByIds(long[] ids)
    {
        List<EquipInfoBean> listData = GetModel().GetEquipInfoByIds(ids);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetEquipInfoSuccess(listData);
        }
        else
        {
            GetView().GetEquipInfoFail();
        }
    }

    /// <summary>
    /// 根据ID获取装备
    /// </summary>
    /// <param name="ids"></param>
    public void GetEquipInfoByIds(List<long> ids)
    {
        List<EquipInfoBean> listData = GetModel().GetEquipInfoByIds(ids);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetEquipInfoSuccess(listData);
        }
        else
        {
            GetView().GetEquipInfoFail();
        }
    }
}