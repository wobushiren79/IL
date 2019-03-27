using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITextService 
{
    private readonly string mTableName;
    private readonly string mLeftTableName;

    public UITextService()
    {
        mTableName = "ui_text";
        mLeftTableName = "ui_text_details_" + GameCommonInfo.gameConfig.language;
    }

    /// <summary>
    /// 查询所有场景数据
    /// </summary>
    /// <returns></returns>
    public List<UITextBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<UITextBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, new string[] { mLeftTableName }, "id", new string[] { "text_id" });
    }
}