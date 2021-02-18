using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITextService 
{
    private readonly string tableNameForMain;
    private readonly string mLeftTableName;

    public UITextService()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        tableNameForMain = "text_ui";
        mLeftTableName = "text_ui_details_" + gameConfig.language;
    }

    /// <summary>
    /// 查询所有场景数据
    /// </summary>
    /// <returns></returns>
    public List<UITextBean> QueryAllData()
    {
        return SQLiteHandle.LoadTableData<UITextBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, new string[] { mLeftTableName }, "id", new string[] { "text_id" });
    }
}