using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildItemBean : BaseBean
{
    public int build_type;
    public long model_id;
    public string icon_key;
    public string icon_list;
    public string name;
    public string content;
    public int aesthetics;//美观值

    /// <summary>
    /// 获取图标数据
    /// </summary>
    /// <returns></returns>
    public List<string> GetIconList()
    {
        return StringUtil.SplitBySubstringForListStr(icon_list, ',');
    }
}