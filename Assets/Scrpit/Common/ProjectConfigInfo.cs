using UnityEngine;
using UnityEditor;

public class ProjectConfigInfo
{
    /// <summary>
    /// 游戏版本
    /// </summary>
    public readonly static string GAME_VERSION = "0.3.4";

    /// <summary>
    /// 是否打开日志输出
    /// </summary>
    public static readonly bool IS_OPEN_LOG_MSG = true;

    /// <summary>
    /// steamAppId
    /// </summary>
    public readonly static string STEAM_APP_ID = "983170";

    /// <summary>
    /// steam所有用户群组key
    /// </summary>
    public readonly static string STEAM_KEY_ALL = "B0147AEB59B2D274DBF8BF54AAA7C0AB";

    /// <summary>
    /// 数据库名称
    /// </summary>
    public readonly static string DATA_BASE_INFO_NAME = "ILDB.db";

    /// <summary>
    /// 数据单词刷新个数
    /// </summary>
    public readonly static int ITEM_REFRESH_NUMBER = 10;
}