using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserDailyLimitBean 
{
    //银行换金上限
    public int exchangeMoneyL=5;
    //当前对话过的NPC
    public List<long> listNpcTalk = new List<long>();
    //当前送过礼物的NPC
    public List<long> listNpcGift = new List<long>();
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
    /// 检测NPC是否送过礼物
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool CheckIsGiftNpc(long npcId)
    {
        return listNpcGift.Contains(npcId);
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

    /// <summary>
    /// 增加NPC对话
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool AddGiftNpc(long npcId)
    {
        if (CheckIsGiftNpc(npcId))
        {
            return false;
        }
        else
        {
            listNpcGift.Add(npcId);
            return true;
        }
    }
}