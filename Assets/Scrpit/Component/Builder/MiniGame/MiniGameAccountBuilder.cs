using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MiniGameAccountBuilder : BaseMiniGameBuilder
{
    public MiniGameAccountEjectorCpt ejectorCpt;
    public NpcAIMiniGameAccountCpt userCharacterAI;

    public GameObject objCharacterContainer;
    public GameObject objCharacterModel;

    public GameObject objMoneyContainer;
    public GameObject objMoneyModel;
    
    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <param name="listUserGameData"></param>
    public void CreateUserCharacter(List<MiniGameCharacterBean> listUserGameData, Vector3 playerPosition)
    {
        foreach (MiniGameCharacterBean miniGameCharacter in listUserGameData)
        {
            GameObject objCharacter = Instantiate(objCharacterContainer, objCharacterModel, playerPosition);
            userCharacterAI = objCharacter.GetComponent<NpcAIMiniGameAccountCpt>();
            userCharacterAI.SetCharacterData(miniGameCharacter.characterData);
        }
    }

    /// <summary>
    /// 生成金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public  void CreateMoney(int moneyL,int moneyM, int moneyS,Transform tfMoneyPosition)
    {
        moneyL *= 2;
        moneyM *= 2;
        moneyS *= 2;
    }

    /// <summary>
    /// 获取发射器
    /// </summary>
    /// <returns></returns>
    public MiniGameAccountEjectorCpt GetEjector()
    {
        return ejectorCpt;
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameAccountCpt GetUserCharacter()
    {
        return userCharacterAI;
    }
}