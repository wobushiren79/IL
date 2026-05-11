using System;
using System.Collections.Generic;
public partial class BuildItemBean
{
    /// <summary>
    /// 获取图标数据
    /// </summary>
    /// <returns></returns>
    public List<string> GetIconList()
    {
        return icon_list.SplitForListStr(',');
    }

    /// <summary>
    /// 获取建筑类型
    /// </summary>
    /// <returns></returns>
    public  BuildItemTypeEnum GetBuildType()
    {
        return (BuildItemTypeEnum)build_type;
    }
}
public partial class BuildItemCfg
{
}
