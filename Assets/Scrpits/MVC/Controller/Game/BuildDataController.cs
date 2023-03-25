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
        if (listData.IsNull())
        {
            GetView().GetAllBuildItemsFail();
        }
        else
        {
            GetView().GetAllBuildItemsSuccess(listData);
        }
    }

    public void GetBuildItemsDataByType(BuildItemTypeEnum type)
    {
        List<BuildItemBean> listData = GetModel().GetBuildItemsByType((int)type);
        if (listData.IsNull())
        {
            GetView().GetBuildItemsByTypeFail();
        }
        else
        {
            GetView().GetBuildItemsByTypeSuccess(type,listData);
        }
    }
}