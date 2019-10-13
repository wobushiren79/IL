using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIMiniGameCombat : UIBaseMiniGame
{
    public GameObject objOurCharacterContainer;
    public GameObject objEnemyCharacterContainer;

    public GameObject objOurCharacterModel;
    public GameObject objEnemyCharacterModel;

    public MiniGameCombatBean gameCombatData;

    /// <summary>
    /// 刷新UI
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="miniGameCombatData"></param>
    public void SetData(MiniGameCombatBean gameCombatData)
    {
        if (gameCombatData == null)
            return;
        this.gameCombatData = gameCombatData;
        CreateOurCharacterList(gameCombatData.listUserGameData);
        CreateEnemyCharacterList(gameCombatData.listEnemyGameData);
    }

    /// <summary>
    /// 创建友方信息列表
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateOurCharacterList(List<MiniGameCharacterBean> listCharacterData)
    {
        if (listCharacterData == null)
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            GameObject objItem = Instantiate(objOurCharacterContainer, objOurCharacterModel);
        }
    }

    /// <summary>
    /// 创建敌方信息列表
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateEnemyCharacterList(List<MiniGameCharacterBean> listCharacterData)
    {
        if (listCharacterData == null)
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            GameObject objItem = Instantiate(objEnemyCharacterContainer, objEnemyCharacterModel);
        }
    }

}