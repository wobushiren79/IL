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
    //当天招募者数据
    public List<CharacterBean> listRecruitmentCharacter;
    //当天竞技场数据
    public List<MiniGameBaseBean> listArenaDataFor1;
    public List<MiniGameBaseBean> listArenaDataFor2;
    public List<MiniGameBaseBean> listArenaDataFor3;
    public List<MiniGameBaseBean> listArenaDataFor4;

    public void InitData()
    {
        exchangeMoneyL = 5;
        listNpcGift.Clear();
        listNpcTalk.Clear();
        listRecruitmentCharacter = null;
        listArenaDataFor1 = null;
        listArenaDataFor2 = null;
        listArenaDataFor3 = null;
        listArenaDataFor4 = null;
    }

    public void AddArenaDataByType(int type, List<MiniGameBaseBean> listMiniGameData)
    {
        switch (type)
        {
            case 1:
                if (listArenaDataFor1 == null)
                    listArenaDataFor1 = new List<MiniGameBaseBean>();
                listArenaDataFor1.AddRange(listMiniGameData);
                break;
            case 2:
                if (listArenaDataFor2 == null)
                    listArenaDataFor2 = new List<MiniGameBaseBean>();
                listArenaDataFor2.AddRange(listMiniGameData);
                break;
            case 3:
                if (listArenaDataFor3 == null)
                    listArenaDataFor3 = new List<MiniGameBaseBean>();
                listArenaDataFor3.AddRange(listMiniGameData);
                break;
            case 4:
                if (listArenaDataFor4 == null)
                    listArenaDataFor4 = new List<MiniGameBaseBean>();
                listArenaDataFor4.AddRange(listMiniGameData);
                break;
        }
    }

    /// <summary>
    /// 增加招募者
    /// </summary>
    /// <param name="characterData"></param>
    public void AddRecruitmentCharacter(CharacterBean characterData)
    {
        if (listRecruitmentCharacter == null)
        {
            listRecruitmentCharacter = new List<CharacterBean>();
        }
        listRecruitmentCharacter.Add(characterData);
    }

    /// <summary>
    /// 移除招募者
    /// </summary>
    /// <param name="characterData"></param>
    public void RemoveRecruitmentCharacter(CharacterBean characterData)
    {
        if (listRecruitmentCharacter != null)
        {
            listRecruitmentCharacter.Remove(characterData);
        }
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