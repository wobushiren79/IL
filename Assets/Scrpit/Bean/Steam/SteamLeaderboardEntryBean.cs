using UnityEngine;
using UnityEditor;
using System;
using Steamworks;

[Serializable]
public class SteamLeaderboardEntryBean 
{
    //排行榜分数
    public int score;
    //排行榜排名
    public int rank;
    //steam用户ID
    public CSteamID steamID;

}