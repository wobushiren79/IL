using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterFavorabilityBean
{
    //角色ID
    public long characterId;
    //好感度
    public int favorability = 0;
    //是否是第一次见面
    public bool firstMeet = true;

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
    public void AddFavorability(int favorability)
    {
        favorability += favorability;
    }

    /// <summary>
    /// 设置第一次见面
    /// </summary>
    public void SetIsFirstMeet()
    {
        firstMeet = false;
    }

}