using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class CharacterForFamilyBean : CharacterBean 
{
    public int familyType;
 
    
    public FamilyTypeEnum GetFailyType()
    {
        return (FamilyTypeEnum)familyType;
    }
}