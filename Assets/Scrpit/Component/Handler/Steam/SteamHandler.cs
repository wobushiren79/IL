using UnityEngine;
using UnityEditor;

public class SteamHandler : BaseMonoBehaviour
{
    public SteamUserStatsImpl steamUserStats;
    public SteamLeaderboardImpl steamLeaderboard;

    private void Start()
    {
        steamUserStats = new SteamUserStatsImpl();
        steamUserStats.InitUserStats();
    }

    /// <summary>
    /// 解锁用户成就
    /// </summary>
    public void UnLockAchievement(long achId)
    {
        if (steamUserStats == null)
        {
            steamUserStats = new SteamUserStatsImpl();
            steamUserStats.InitUserStats();
        }
        steamUserStats.UserCompleteAchievement(achId + "");
    }

    /// <summary>
    /// 根据名字查询排行榜ID
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="callBack"></param>
    public void GetLeaderboardId(string leaderboardName,ISteamLeaderboardFindCallBack callBack)
    {
        if (steamLeaderboard == null)
        {
            steamLeaderboard = new SteamLeaderboardImpl();
        }
        steamLeaderboard.FindLeaderboard(leaderboardName, callBack);
    }

    /// <summary>
    /// 查询全球排名
    /// </summary>
    /// <param name="leaderboardId"></param>
    /// <param name="startRank"></param>
    /// <param name="endRank"></param>
    public void GetLeaderboardDataForGlobal(ulong leaderboardId,int startRank,int endRank,ISteamLeaderboardEntriesCallBack callBack)
    {
        if (steamLeaderboard == null)
        {
            steamLeaderboard = new SteamLeaderboardImpl();
        }
        steamLeaderboard.FindLeaderboardEntries(leaderboardId, startRank, endRank, Steamworks.ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, callBack);
    }

}