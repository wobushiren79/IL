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

    //攻击特效
    public GameObject objCombatEffect;

    public NpcAIMiniGameDebateCpt aiUserCharacter;
    public NpcAIMiniGameDebateCpt aiEnemyCharacter;

    /// <summary>
    /// 获取友方角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameDebateCpt GetUserCharacter()
    {
        return aiUserCharacter;
    }

    /// <summary>
    /// 获取地方角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameDebateCpt GetEnemyCharacter()
    {
        return aiEnemyCharacter;
    }
   
    /// <summary>
    /// 创建攻击特效
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject CreateCombatEffect(Vector3 position)
    {
        GameObject objCharacter = Instantiate(objCharacterContainer, objCombatEffect, position);
        return objCharacter;
    }

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
    public void CreateUserCharacter(MiniGameCharacterBean userCharacterData, Vector3 debatePosition)
    {
        aiUserCharacter = CreateCharacter(new Vector3(debatePosition.x - 2.5f, debatePosition.y), (MiniGameCharacterForDebateBean)userCharacterData);
        aiUserCharacter.SetCharacterFace(2);
    }

    /// <summary>
    /// 创建敌人角色
    /// </summary>
    /// <param name="enemyCharacter"></param>
    /// <param name="debatePosition"></param>
    public void CreateEnemyCharcater(MiniGameCharacterBean enemyCharacterData, Vector3 debatePosition)
    {
        aiEnemyCharacter = CreateCharacter(new Vector3(debatePosition.x + 2.5f, debatePosition.y), (MiniGameCharacterForDebateBean)enemyCharacterData);
        aiEnemyCharacter.SetCharacterFace(1);
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

    /// <summary>
    /// 删除所有角色
    /// </summary>
    public void DestroyAllCharacter()
    {
        CptUtil.RemoveChildsByActive(objCharacterContainer);
        aiUserCharacter = null;
        aiEnemyCharacter = null;
    }

    public override void DestroyAll()
    {
        base.DestroyAll();
        DestroyAllCharacter();
    }
}