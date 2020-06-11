using UnityEngine;
using UnityEditor;

public class SteamHandler : BaseMonoBehaviour
{
    public SteamUserStatsImpl steamUserStats;

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
}