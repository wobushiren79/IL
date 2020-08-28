using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserDailyLimitBean
{
    //银行换金上限
    public int exchangeMoneyL = 0;

    //当前对话过的NPC
    public List<long> listNpcTalk = new List<long>();
    //当前送过礼物的NPC
    public List<long> listNpcGift = new List<long>();
    //当天招募者数据
    public List<CharacterBean> listRecruitmentCharacter;
    //当天参加过竞技场游戏的人物
    public List<string> listArenaAttendedCharacter = new List<string>();
    //当天竞技场数据
    public List<MiniGameBaseBean> listArenaDataForElementary;
    public List<MiniGameBaseBean> listArenaDataForIntermediate;
    public List<MiniGameBaseBean> listArenaDataForAdvanced;
    public List<MiniGameBaseBean> listArenaDataForLegendary;
    //每日事件数量上限
    public int numberForEvent = 0;

    public void InitData(GameDataBean gameData)
    {
        gameData.GetInnAttributesData().GetInnLevel(out int levelTitle, out int levelStar);

        //交换金钱限额
        if (levelTitle == 0)
            exchangeMoneyL = 0;
        else
            exchangeMoneyL = (levelStar + (levelTitle - 1) * 5 ) * 5;
        //每日事件数量
        numberForEvent = (levelTitle==0 ? 3 :  (levelTitle - 1) * 5 + levelStar + 3);

        listNpcGift.Clear();
        listNpcTalk.Clear();
        listArenaAttendedCharacter.Clear();
        listRecruitmentCharacter = null;
        listArenaDataForElementary = null;
        listArenaDataForIntermediate = null;
        listArenaDataForAdvanced = null;
        listArenaDataForLegendary = null;
    }



    /// <summary>
    /// 增加参加过竞技场的名单
    /// </summary>
    /// <param name="listCharacter"></param>
    public void AddArenaAttendedCharacter(List<CharacterBean> listCharacter)
    {
        List<string> listId = new List<string>();
        foreach (CharacterBean itemData in listCharacter)
        {
            listId.Add(itemData.baseInfo.characterId);
        }
        AddArenaAttendedCharacter(listId);
    }
    public void AddArenaAttendedCharacter(List<string> listCharacter)
    {
        listArenaAttendedCharacter.AddRange(listCharacter);
    }

    /// <summary>
    /// 获取参加过竞技场的名单
    /// </summary>
    /// <returns></returns>
    public List<string> GetArenaAttendedCharacter()
    {
        return listArenaAttendedCharacter;
    }

    /// <summary>
    /// 获取竞技场数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<MiniGameBaseBean> GetArenaDataByType(TrophyTypeEnum type)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                return listArenaDataForElementary;
            case TrophyTypeEnum.Intermediate:
                return listArenaDataForIntermediate;
            case TrophyTypeEnum.Advanced:
                return listArenaDataForAdvanced;
            case TrophyTypeEnum.Legendary:
                return listArenaDataForLegendary;
        }
        return null;
    }

    /// <summary>
    /// 增加竞技场数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listMiniGameData"></param>
    public void AddArenaDataByType(TrophyTypeEnum type, List<MiniGameBaseBean> listMiniGameData)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                if (listArenaDataForElementary == null)
                    listArenaDataForElementary = new List<MiniGameBaseBean>();
                listArenaDataForElementary.AddRange(listMiniGameData);
                break;
            case TrophyTypeEnum.Intermediate:
                if (listArenaDataForIntermediate == null)
                    listArenaDataForIntermediate = new List<MiniGameBaseBean>();
                listArenaDataForIntermediate.AddRange(listMiniGameData);
                break;
            case TrophyTypeEnum.Advanced:
                if (listArenaDataForAdvanced == null)
                    listArenaDataForAdvanced = new List<MiniGameBaseBean>();
                listArenaDataForAdvanced.AddRange(listMiniGameData);
                break;
            case TrophyTypeEnum.Legendary:
                if (listArenaDataForLegendary == null)
                    listArenaDataForLegendary = new List<MiniGameBaseBean>();
                listArenaDataForLegendary.AddRange(listMiniGameData);
                break;
        }
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="miniGameData"></param>
    public void RemoveArenaDataByType(TrophyTypeEnum type, MiniGameBaseBean miniGameData)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                if(listArenaDataForElementary!=null)
                    listArenaDataForElementary.Remove(miniGameData);
                break;
            case TrophyTypeEnum.Intermediate:
                if (listArenaDataForIntermediate != null)
                    listArenaDataForIntermediate.Remove(miniGameData);
                break;
            case TrophyTypeEnum.Advanced:
                if (listArenaDataForAdvanced != null)
                    listArenaDataForAdvanced.Remove(miniGameData);
                break;
            case TrophyTypeEnum.Legendary:
                if (listArenaDataForLegendary != null)
                    listArenaDataForLegendary.Remove(miniGameData);
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
    /// 检测是否还有事件数量
    /// </summary>
    /// <returns></returns>
    public bool CheckEventNumber(int number)
    {
        if(numberForEvent - number < 0)
        {
            return false;
        }
        else
        {
            numberForEvent -= number;
            return true;
        }
    }

    /// <summary>
    /// 增加NPC对话
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool AddTalkNpc(long npcId)
    {
        if (CheckIsTalkNpc(npcId))
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