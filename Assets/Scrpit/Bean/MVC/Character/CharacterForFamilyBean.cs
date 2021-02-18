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
}