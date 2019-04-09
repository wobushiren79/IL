using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildDataController : BaseMVCController<BuildDataModel, IBuildDataView>
{
    public BuildDataController(BaseMonoBehaviour content, IBuildDataView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    public void GetAllBuildItemsData()
    {
        List<BuildItemBean> listData = GetModel().GetAllBuildItems();
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetAllBuildItemsFail();
        }
        else
        {
            GetView().GetAllBuildItemsSuccess(listData);
        }
    }

    public void GetBuildItemsDataByType(BuildItemBean.BuildType type)
    {
        List<BuildItemBean> listData = GetModel().GetBuildItemsByType((int)type);
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBuildItemsByTypeFail();
        }
        else
        {
            GetView().GetBuildItemsByTypeSuccess(type,listData);
        }
    }
}