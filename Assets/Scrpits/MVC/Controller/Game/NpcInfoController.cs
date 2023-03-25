using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoController : BaseMVCController<NpcInfoModel, INpcInfoView>
{
    public NpcInfoController(BaseMonoBehaviour content, INpcInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    public void GetAllNpcInfo()
    {
        List<NpcInfoBean> listData = GetModel().GetAllNpcInfo();
        if (listData.IsNull())
        {
            GetView().GetNpcInfoFail(-1);
        }
        else
        {
            GetView().GetNpcInfoSuccess(-1, listData);
        }
    }

    public void GetNormalNpcInfo()
    {
        List<NpcInfoBean> listData = GetModel().GetNpcInfoByType(new int[] { 0 });
        if (listData.IsNull())
        {
            GetView().GetNpcInfoFail(0);
        }
        else
        {
            GetView().GetNpcInfoSuccess(0, listData);
        }
    }
}