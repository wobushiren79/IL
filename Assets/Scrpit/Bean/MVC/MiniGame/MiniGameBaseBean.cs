using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBaseBean 
{
    public int gameLevel;//游戏等级
    //胜利条件
    public float winSurvivalTime;//生存时间(秒)
    public float winLife;//生命值多少以上

    /// <summary>
    /// 获取胜利条件列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetListWinConditions()
    {
        List<string> listWinConditions = new List<string>();
        if (winSurvivalTime != 0)
        {
            string winSurvivalTimeStr = string.Format(GameCommonInfo.GetUITextById(211), winSurvivalTime + GameCommonInfo.GetUITextById(39));
            listWinConditions.Add(winSurvivalTimeStr);
        }
        if (winLife != 0)
        {
            string winLifeStr = string.Format(GameCommonInfo.GetUITextById(212), winLife + "");
            listWinConditions.Add(winLifeStr);
        }
        return listWinConditions;
    }

}