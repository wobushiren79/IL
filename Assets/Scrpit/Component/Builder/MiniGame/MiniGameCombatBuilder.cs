using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MiniGameCombatBuilder : BaseMonoBehaviour
{
    public GameObject objPlayerContainer;
    public GameObject objPlayerModel;

    //我方角色
    public List<NpcAIMiniGameCombatCpt> listOurCharacter = new List<NpcAIMiniGameCombatCpt>();
    //地方角色
    public List<NpcAIMiniGameCombatCpt> listEnemyCharacter = new List<NpcAIMiniGameCombatCpt>();

    /// <summary>
    /// 创建所有角色
    /// </summary>
    /// <param name="listUserGameData"></param>
    /// <param name="listEnemyGameData"></param>
    public void CreateAllPlaer(Vector3 combatPosition, List<MiniGameCharacterBean> listUserGameData, List<MiniGameCharacterBean> listEnemyGameData)
    {
        CreateOurCharacter(combatPosition, listUserGameData);
        CreateEnemyCharacter(combatPosition, listEnemyGameData);
    }

    /// <summary>
    /// 创建友方角色
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateOurCharacter(Vector3 combatPosition, List<MiniGameCharacterBean> listCharacterData)
    {
        listOurCharacter.Clear();
        float characterPositionY = (listCharacterData.Count - 1) / 2f;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            Vector3 characterPosition = new Vector3(combatPosition.x - 5, characterPositionY);
            characterPositionY--;
            MiniGameCharacterBean itemData = listCharacterData[i];
            NpcAIMiniGameCombatCpt npcCpt = CreateCharacter(characterPosition, itemData);
            listOurCharacter.Add(npcCpt);
        }
    }

    /// <summary>
    /// 创建敌方角色
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateEnemyCharacter(Vector3 combatPosition, List<MiniGameCharacterBean> listCharacterData)
    {
        listEnemyCharacter.Clear();
        float characterPositionY = (listCharacterData.Count - 1) / 2f;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            Vector3 characterPosition = new Vector3(combatPosition.x + 5, characterPositionY);
            characterPositionY--;
            MiniGameCharacterBean itemData = listCharacterData[i];
            NpcAIMiniGameCombatCpt npcCpt = CreateCharacter(characterPosition, itemData);
            listEnemyCharacter.Add(npcCpt);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="miniGameCharacterData"></param>
    public NpcAIMiniGameCombatCpt CreateCharacter(Vector3 combatPosition, MiniGameCharacterBean miniGameCharacterData)
    {
        GameObject objPlayer = Instantiate(objPlayerContainer, objPlayerModel, combatPosition);
        NpcAIMiniGameCombatCpt npcCpt = objPlayer.GetComponent<NpcAIMiniGameCombatCpt>();
        npcCpt.SetData(miniGameCharacterData);
        return npcCpt;
    }
}