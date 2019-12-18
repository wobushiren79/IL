using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserDailyLimitBean 
{
    //银行换金上限
    public int exchangeMoneyL=5;
    //当前对话过的NPC
    public List<long> listNpcTalk = new List<long>();

    public void InitData()
    {
        exchangeMoneyL = 5;
        listNpcTalk.Clear();
    }

    /// <summary>
    /// 检测NPC是否对话过
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool CheckIsTalkNpc(long npcId)
    {
       return listNpcTalk.Contains(npcId);
    }

    /// <summary>
    /// 增加NPC对话
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool AddTalkNpc(long npcId)
    {
        if(CheckIsTalkNpc(npcId))
        {
            return false;
        }
        else
        {
            listNpcTalk.Add(npcId);
            return true;
        }
    }
}