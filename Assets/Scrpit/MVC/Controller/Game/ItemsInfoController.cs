using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsInfoController : BaseMVCController<ItemsInfoModel, IItemsInfoView>
{
    public ItemsInfoController(BaseMonoBehaviour content, IItemsInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 查询所有装备信息
    /// </summary>
    public void GetAllItemsInfo()
    {
        List<ItemsInfoBean> listData = GetModel().GetAllEquipInfo();
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetItemsInfoSuccess(listData);
        }
        else
        {
            GetView().GetItemsInfoFail();
        }
    }

    /// <summary>
    /// 根据ID获取装备
    /// </summary>
    /// <param name="ids"></param>
    public void GetItemsInfoByIds(long[] ids)
    {
        List<ItemsInfoBean> listData = GetModel().GetItemsInfoByIds(ids);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetItemsInfoSuccess(listData);
        }
        else
        {
            GetView().GetItemsInfoFail();
        }
    }

    /// <summary>
    /// 根据ID获取装备
    /// </summary>
    /// <param name="ids"></param>
    public void GetItemsInfoByIds(List<long> ids)
    {
        List<ItemsInfoBean> listData = GetModel().GetItemsInfoByIds(ids);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetItemsInfoSuccess(listData);
        }
        else
        {
            GetView().GetItemsInfoFail();
        }
    }


}