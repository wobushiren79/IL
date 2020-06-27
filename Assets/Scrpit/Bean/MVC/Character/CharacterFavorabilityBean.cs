﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterFavorabilityBean
{
    //角色ID
    public long characterId;
    //好感度
    public int favorability = 0;
    public int favorabilityLevel = 0;
    //是否是第一次见面
    public bool firstMeet = true;

    //送礼次数
    public int giftNumber = 0;
    //谈话次数
    public int talkNumber = 0;

    public CharacterFavorabilityBean(long characterId) : this(characterId, 0)
    {

    }

    public CharacterFavorabilityBean(long characterId, int favorability)
    {
        this.characterId = characterId;
        this.favorability = favorability;
    }

    /// <summary>
    /// 增加好感
    /// </summary>
    /// <param name="favorability"></param>
    public void AddFavorability(int addFavorability)
    {
        favorability += addFavorability;
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.NpcFavorabilityFor1, out int love1);
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.NpcFavorabilityFor2, out int love2);
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.NpcFavorabilityFor3, out int love3);
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.NpcFavorabilityFor4, out int love4);
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.NpcFavorabilityFor5, out int love5);
        if (favorability >= 0 && favorability <= love1)
        {
            favorabilityLevel = 0;
        }
        else if (favorability > love1 && favorability <= love2)
        {
            favorabilityLevel = 1;
        }
        else if (favorability > love2 && favorability <= love3)
        {
            favorabilityLevel = 2;
        }
        else if (favorability > love3 && favorability <= love4)
        {
            favorabilityLevel = 3;
        }
        else if (favorability > love4 && favorability <= love5)
        {
            favorabilityLevel = 4;
        }
        else if (favorability > love5)
        {
            favorabilityLevel = 5;
        }
        if (favorability < 0)
        {
            favorability = 0;
        }
    }

    /// <summary>
    /// 设置第一次见面
    /// </summary>
    public void SetIsFirstMeet()
    {
        firstMeet = false;
    }

    /// <summary>
    /// 增加普通礼物赠送次数
    /// </summary>
    /// <param name="number"></param>
    public void AddGiftNumber(int number)
    {
        giftNumber += number;
    }


    /// <summary>
    /// 增加谈话次数
    /// </summary>
    /// <param name="number"></param>
    public void AddTalkNumber(int number)
    {
        talkNumber += number;
    }
}