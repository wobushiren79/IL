using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MiniGameDebateBuilder : BaseMiniGameBuilder
{
    //角色容器
    public GameObject objCharacterContainer;
    //角色模型
    public GameObject objCharacterModel;

    /// <summary>
    /// 创建全部角色
    /// </summary>
    /// <param name="listUserData"></param>
    /// <param name="listEnemyData"></param>
    /// <param name="debatePosition"></param>
    public void CreateAllCharacter(List<MiniGameCharacterBean> listUserData, List<MiniGameCharacterBean> listEnemyData, Vector3 debatePosition)
    {
        CreateUserCharacter(listUserData[0], debatePosition);
        CreateEnemyCharcater(listEnemyData[0], debatePosition);
    }

    /// <summary>
    /// 创建用户角色
    /// </summary>
    /// <param name="userCharacter"></param>
    /// <param name="debatePosition"></param>
    public void CreateUserCharacter(MiniGameCharacterBean userCharacter, Vector3 debatePosition)
    {
        NpcAIMiniGameDebateCpt npcAI = CreateCharacter(new Vector3(debatePosition.x - 2.5f, debatePosition.y), (MiniGameCharacterForDebateBean)userCharacter);
        npcAI.SetCharacterFace(2);
    }

    /// <summary>
    /// 创建敌人角色
    /// </summary>
    /// <param name="enemyCharacter"></param>
    /// <param name="debatePosition"></param>
    public void CreateEnemyCharcater(MiniGameCharacterBean enemyCharacter, Vector3 debatePosition)
    {
        NpcAIMiniGameDebateCpt npcAI= CreateCharacter(new Vector3(debatePosition.x + 2.5f, debatePosition.y), (MiniGameCharacterForDebateBean)enemyCharacter);
        npcAI.SetCharacterFace(1);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="characterPosition"></param>
    /// <param name="miniGameCharacterData"></param>
    private NpcAIMiniGameDebateCpt CreateCharacter(Vector3 characterPosition, MiniGameCharacterForDebateBean miniGameCharacterData)
    {
        //创建角色
        GameObject objCharacter = Instantiate(objCharacterContainer, objCharacterModel, characterPosition);
        NpcAIMiniGameDebateCpt npcCpt = objCharacter.GetComponent<NpcAIMiniGameDebateCpt>();
        npcCpt.SetData(miniGameCharacterData);
        return npcCpt;
    }
}