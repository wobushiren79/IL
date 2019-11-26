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

    public GameObject objCombatUserPosition;
    public GameObject objCombatEnemyPosition;

    [Header("数据")]
    public MiniGameCharacterForDebateBean userGameData;
    public MiniGameCharacterForDebateBean enemyGameData;

    public List<ItemMiniGameDebateCardCpt> listUserCard = new List<ItemMiniGameDebateCardCpt>();
    public List<ItemMiniGameDebateCardCpt> listEnemyCard = new List<ItemMiniGameDebateCardCpt>();

    private void Update()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_Space))
        {
            ClearCard();
            DrawCard();
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="userGameData"></param>
    /// <param name="enemyGameData"></param>
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
    /// 抽卡
    /// </summary>
    public void DrawCard()
    {
        if (listUserCard.Count >= 5)
            return;
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listUserDebate = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listEnemyDebate = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        for (int i = 0; i < 5 - listUserCard.Count; i++)
        {
            listUserDebate.Add(RandomUtil.GetRandomEnum<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>());
        }
        for (int i = 0; i < 5 - listUserCard.Count; i++)
        {
            listEnemyDebate.Add(RandomUtil.GetRandomEnum<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>());
        }
        CreateCardItemList(listUserDebate, listEnemyDebate);

    }

    /// <summary>
    /// 选择一张卡
    /// </summary>
    public void SelectCard(ItemMiniGameDebateCardCpt selectCard)
    {
        ItemMiniGameDebateCardCpt enemyCard = RandomUtil.GetRandomDataByList(listEnemyCard);
        CreateCombatCard(selectCard, enemyCard);

        listUserCard.Remove(selectCard);
        listEnemyCard.Remove(enemyCard);
        //Destroy(enemyCard.gameObject);
        //Destroy(selectCard.gameObject);
    }

    /// <summary>
    /// 创建战斗卡牌
    /// </summary>
    /// <param name="userCard"></param>
    /// <param name="enemyCard"></param>
    public void CreateCombatCard(ItemMiniGameDebateCardCpt userCard, ItemMiniGameDebateCardCpt enemyCard)
    {
        userCard.transform.SetParent(objCombatContainer.transform);
        userCard.ClosePointerListener();
        userCard.transform.DOMove(objCombatUserPosition.transform.position, 1);

        enemyCard.transform.SetParent(objCombatContainer.transform);
        enemyCard.ClosePointerListener();
        enemyCard.transform.DOMove(objCombatEnemyPosition.transform.position, 1);
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
            GameObject objItem = CreateCardItem(i, itemType, objUserDebateCardContainer);
        }
        for (int i = 0; i < listEnemyDebate.Count; i++)
        {
            ItemMiniGameDebateCardCpt.DebateCardTypeEnun itemType = listEnemyDebate[i];
            GameObject objItem = CreateCardItem(i, itemType, objEnemyDebateCardContainer);
        }
    }

    public GameObject CreateCardItem(int position, ItemMiniGameDebateCardCpt.DebateCardTypeEnun itemType, GameObject objContainer)
    {
        GameObject objItem = Instantiate(objContainer, objDebateCardModel);
        RectTransform rtfItem = (RectTransform)objItem.transform;

        //初始默认不可点击
        ItemMiniGameDebateCardCpt cardItem = objItem.GetComponent<ItemMiniGameDebateCardCpt>();
        cardItem.ClosePointerListener();
        //设置初始透明度
        CanvasGroup cgItem = objItem.GetComponent<CanvasGroup>();
        cgItem.alpha = 0;

        if (objContainer == objUserDebateCardContainer)
        {
            cardItem.SetData(itemType, 1);
            rtfItem.DOLocalMoveY(120, 1).From().SetEase(Ease.OutBack).SetDelay(position * 0.2f).OnStart(delegate () { cgItem.alpha = 1; }).OnComplete(delegate () { cardItem.OpenPointerListener(); });
            //rtfItem.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(position * 0.1f);
            listUserCard.Add(cardItem);
        }
        else if (objContainer == objEnemyDebateCardContainer)
        {
            cardItem.SetData(itemType, 2);
            rtfItem.DOLocalMoveY(120, 1).From().SetEase(Ease.OutBack).SetDelay(position * 0.2f).OnStart(delegate () { cgItem.alpha = 1; }).OnComplete(delegate () { cardItem.OpenPointerListener(); });
            //rtfItem.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(position * 0.1f);
            listEnemyCard.Add(cardItem);
        }
        return objItem;
    }

    /// <summary>
    /// 清空卡
    /// </summary>
    public void ClearCard()
    {
        listUserCard.Clear();
        listEnemyCard.Clear();
        CptUtil.RemoveChildsByActive(objUserDebateCardContainer);
        CptUtil.RemoveChildsByActive(objEnemyDebateCardContainer);
    }
}