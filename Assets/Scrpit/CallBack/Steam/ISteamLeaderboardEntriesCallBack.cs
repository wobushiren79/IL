using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface ISteamLeaderboardEntriesCallBack 
{
    /// <summary>
    /// 获取数据成功
    /// </summary>
    /// <param name="listData"></param>
    void GetEntriesSuccess(List<SteamLeaderboardEntryBean> listData);

    /// <summary>
    /// 获取数据失败
    /// </summary>
    /// <param name="msg"></param>
    void GetEntriesFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg);
}