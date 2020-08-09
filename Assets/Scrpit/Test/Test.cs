using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

public class Test : BaseMonoBehaviour, ISteamLeaderboardFindCallBack
{

    private void Awake()
    {

    }


    public void Start()
    {
        SteamHandler steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
        steamHandler.GetLeaderboardId("Get_Money_S", this);
    }

    public void FindLeaderboardSuccess(ulong leaderboard)
    {
        LogUtil.Log("leaderboard:"+ leaderboard);
    }

    public void FindLeaderboardFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {

    }
}
