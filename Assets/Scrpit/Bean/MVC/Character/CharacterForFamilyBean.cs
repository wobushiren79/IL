using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class CharacterForFamilyBean : CharacterBean
{
    public int familyType;
    public CharacterForFamilyBean()
    {

    }

    public CharacterForFamilyBean(NpcInfoBean npcInfo) : base(npcInfo)
    {

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