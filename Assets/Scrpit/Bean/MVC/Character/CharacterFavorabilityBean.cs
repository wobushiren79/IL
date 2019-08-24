using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterFavorabilityBean
{
    public long characterId;
    public int favorability;

    public CharacterFavorabilityBean(long characterId, int favorability)
    {
        this.characterId = characterId;
        this.favorability = favorability;
    }
}