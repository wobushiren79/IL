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
    public void CreateMoney(int moneyL, int moneyM, int moneyS, Transform tfMoneyPosition)
    {
        moneyL *= 2;
        moneyM *= 2;
        moneyS *= 2;
        while (moneyL > 0)
        {
            int moneyMax = (moneyL > 10 ? 10 : moneyL);
            int money = UnityEngine.Random.Range(3, moneyMax);
            CreateMoneyItem(MoneyEnum.L, money, tfMoneyPosition);
            moneyL -= money;
        }
        while (moneyM > 0)
        {
            int moneyMax = (moneyM > 10 ? 10 : moneyM);
            int money = UnityEngine.Random.Range(3, moneyMax);
            CreateMoneyItem(MoneyEnum.M, money, tfMoneyPosition);
            moneyM -= money;
        }
        while (moneyS > 0)
        {
            int moneyMax = (moneyS > 10 ? 10 : moneyS);
            int money = UnityEngine.Random.Range(3, moneyMax);
            CreateMoneyItem(MoneyEnum.S, money, tfMoneyPosition);
            moneyS -= money;
        }
    }

    private void CreateMoneyItem(MoneyEnum moneyType, int money, Transform tfMoneyPosition)
    {
        Vector3 moneyPosition = GameUtil.GetTransformInsidePosition2D(tfMoneyPosition);
        GameObject objMoney = Instantiate(objMoneyContainer, objMoneyModel, moneyPosition);
        MiniGameAccountMoneyCpt accountMoneyCpt = objMoney.GetComponent<MiniGameAccountMoneyCpt>();
        accountMoneyCpt.InitData(moneyType, money);
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