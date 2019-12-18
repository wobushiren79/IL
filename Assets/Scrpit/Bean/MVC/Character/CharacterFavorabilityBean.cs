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
    public int favorabilityLevel = 0;
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
    public void AddFavorability(int addFavorability)
    {
        favorability += addFavorability;
        if (favorability >=0 && favorability<=100)
        {
            favorabilityLevel = 0;
        }
        else if (favorability > 100 && favorability <= 200)
        {
            favorabilityLevel = 1;
        }
        else if (favorability > 200 && favorability <= 300)
        {
            favorabilityLevel = 2;
        }
        else if (favorability > 300 && favorability <= 400)
        {
            favorabilityLevel = 3;
        }
        else if (favorability > 400 && favorability <= 500)
        {
            favorabilityLevel = 4;
        }
        else if (favorability > 500)
        {
            favorabilityLevel = 5;
        }
    }

    /// <summary>
    /// 设置第一次见面
    /// </summary>
    public void SetIsFirstMeet()
    {
        firstMeet = false;
    }

}