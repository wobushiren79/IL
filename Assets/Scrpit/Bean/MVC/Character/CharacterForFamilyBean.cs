using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class CharacterForFamilyBean : CharacterBean
{
    public int familyType;

    public TimeBean birthTime;
    public CharacterForFamilyBean(TimeBean birthTime)
    {
        this.birthTime = birthTime;
    }

    public CharacterForFamilyBean(NpcInfoBean npcInfo, TimeBean birthTime) : base(npcInfo)
    {
        this.birthTime = birthTime;
    }

    public FamilyTypeEnum GetFamilyType()
    {
        return (FamilyTypeEnum)familyType;
    }

    public void SetFamilyType(FamilyTypeEnum familyType)
    {
        this.familyType = (int)familyType;
    }

    /// <summary>
    /// 检测是否长大
    /// </summary>
    /// <returns></returns>
    public bool CheckIsGrowUp(TimeBean currentTime)
    {
        int days = (currentTime.year - birthTime.year) * 42 * 4 + (currentTime.month - birthTime.month) * 42 + (currentTime.day - birthTime.day);
        if (days >= 504)
        {
            return true;
        }
        return false;
    }
}