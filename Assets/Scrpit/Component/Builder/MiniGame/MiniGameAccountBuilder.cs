using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MiniGameAccountBuilder : BaseMiniGameBuilder
{
    public GameObject objCharacterContainer;
    public GameObject objCharacterModel;

    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <param name="listUserGameData"></param>
    public void CreateUserCharacter(List<MiniGameCharacterBean> listUserGameData, Vector3 playerPosition)
    {
        foreach (MiniGameCharacterBean miniGameCharacter in listUserGameData)
        {
            GameObject objCharacter = Instantiate(objCharacterContainer, objCharacterModel, playerPosition);
            NpcAIMiniGameAccountCpt npcAI = objCharacter.GetComponent<NpcAIMiniGameAccountCpt>();
            npcAI.SetCharacterData(miniGameCharacter.characterData);
        }
    }
}