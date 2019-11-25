using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIMiniGameDebate : BaseUIComponent
{
    [Header("控件")]
    public CharacterUICpt characterUser;
    public CharacterUICpt characterEnemy;
    public Text tvUserName;
    public Text tvEnemyName;
    public Text tvUserLife;
    public Text tvEnemyLife;

    public GameObject objUserDebateCardContainer;
    public GameObject objEnemyDebateCardContainer;
    public GameObject objDebateCardModel;

    public GameObject objCombatContainer;
    public GameObject objCombatCardModel;

    [Header("数据")]
    public MiniGameCharacterForDebateBean userGameData;
    public MiniGameCharacterForDebateBean enemyGameData;

    public void SetData(MiniGameCharacterForDebateBean userGameData, MiniGameCharacterForDebateBean enemyGameData)
    {
        this.userGameData = userGameData;
        this.enemyGameData = enemyGameData;
        SetUserCharacterName(userGameData.characterData.baseInfo.name);
        SetEnemyCharacterName(enemyGameData.characterData.baseInfo.name);
        SetUserLife(userGameData.characterCurrentLife, userGameData.characterMaxLife);
        SetEnemyLife(userGameData.characterCurrentLife, userGameData.characterMaxLife);
        SetUserCharacter(userGameData.characterData);
        SetEnemyCharacter(enemyGameData.characterData);
    }

    /// <summary>
    /// 设置友方角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetUserCharacter(CharacterBean characterData)
    {
        if (characterUser != null)
            characterUser.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置敌方角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetEnemyCharacter(CharacterBean characterData)
    {
        if (characterEnemy != null)
            characterEnemy.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置友方角色姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetUserCharacterName(string name)
    {
        if (tvUserName != null)
            tvUserName.text = name;
    }

    /// <summary>
    /// 设置敌方角色姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetEnemyCharacterName(string name)
    {
        if (tvEnemyName != null)
            tvEnemyName.text = name;
    }

    /// <summary>
    /// 设置友方血量
    /// </summary>
    /// <param name="life"></param>
    /// <param name="maxLife"></param>
    public void SetUserLife(int life, int maxLife)
    {
        if (tvUserLife != null)
        {
            tvUserLife.text = life + "/" + maxLife;
        }
    }

    /// <summary>
    /// 设置敌方血量
    /// </summary>
    /// <param name="life"></param>
    /// <param name="maxLife"></param>
    public void SetEnemyLife(int life, int maxLife)
    {
        if (tvEnemyLife != null)
        {
            tvEnemyLife.text = life + "/" + maxLife;
        }
    }

    /// <summary>
    /// 创建卡片
    /// </summary>
    /// <param name="listUserDebate"></param>
    /// <param name="listEnemyDebate"></param>
    public void CreateCardItemList(List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listUserDebate, List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listEnemyDebate)
    {
        for (int i = 0; i < listUserDebate.Count; i++)
        {
            ItemMiniGameDebateCardCpt.DebateCardTypeEnun itemType = listUserDebate[i];
            GameObject objItem = CreateCardItem(itemType, objUserDebateCardContainer);
            objItem.transform.DOScale(new Vector3(0,0,0), 1).From().SetEase(Ease.OutBack).SetDelay(i * 0.1f);
        }
        for (int i = 0; i < listEnemyDebate.Count; i++)
        {
            ItemMiniGameDebateCardCpt.DebateCardTypeEnun itemType = listEnemyDebate[i];
            GameObject objItem = CreateCardItem(itemType, objEnemyDebateCardContainer);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(i * 0.1f);
            Button bt= objItem.GetComponent<Button>();
            bt.enabled = false;
        }
    }

    public GameObject CreateCardItem(ItemMiniGameDebateCardCpt.DebateCardTypeEnun itemType, GameObject objContainer)
    {
        GameObject objItem = Instantiate(objContainer, objDebateCardModel);
        ItemMiniGameDebateCardCpt cardItem = objItem.GetComponent<ItemMiniGameDebateCardCpt>();
        cardItem.SetData(itemType);
        return objItem;
    }

}