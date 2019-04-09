using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IBuildDataView 
{
    void GetAllBuildItemsSuccess(List<BuildItemBean> listData);

    void GetAllBuildItemsFail();

    void GetBuildItemsByTypeSuccess(BuildItemBean.BuildType type,List<BuildItemBean> listData);

    void GetBuildItemsByTypeFail();
}