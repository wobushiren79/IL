using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIMiniGameDebate : UIGameComponent
{
    [Header("控件")]
    public CharacterUICpt characterUser;
    public CharacterUICpt characterEnemy;
    public Text tvUserName;
    public Text tvEnemyName;
    public Text tvUserLife;
    public Text tvEnemyLife;
    public Text tvUserCharm;
    public Text tvEnemyCharm;

    public GameObject objUserDebateCardContainer;
    public GameObject objEnemyDebateCardContainer;
    public GameObject objDebateCardModel;

    public GameObject objCombatContainer;

    public GameObject objCombatUserPosition;
    public GameObject objCombatEnemyPosition;
    public GameObject objCombatUserEndPosition;
    public GameObject objCombatEnemyEndPosition;

    public ParticleSystem psCombat;

    [Header("数据")]
    public MiniGameCharacterForDebateBean userGameData;
    public MiniGameCharacterForDebateBean enemyGameData;

    public List<ItemMiniGameDebateCardCpt> listUserCard = new List<ItemMiniGameDebateCardCpt>();
    public List<ItemMiniGameDebateCardCpt> listEnemyCard = new List<ItemMiniGameDebateCardCpt>();

    private ICallBack mCallBack;

    public bool isCombat = false;//是否正在战斗

    public override void RefreshUI()
    {
        base.RefreshUI();
        GameItemsManager gameItemsManager=  GetUIManager<UIGameManager>().gameItemsManager;
        SetCharacter(userGameData.characterData, enemyGameData.characterData);
        SetCharacterName(userGameData.characterData.baseInfo.name, enemyGameData.characterData.baseInfo.name);
        SetLife(userGameData.characterCurrentLife, userGameData.characterMaxLife, enemyGameData.characterCurrentLife, enemyGameData.characterMaxLife);

        userGameData.characterData.GetAttributes( gameItemsManager, out CharacterAttributesBean userAttributes);
        enemyGameData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean enemyAttributes);
        SetCharm(userAttributes.charm, enemyAttributes.charm);
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="userGameData"></param>
    /// <param name="enemyGameData"></param>
    public void SetData(MiniGameCharacterForDebateBean userGameData, MiniGameCharacterForDebateBean enemyGameData)
    {
        ClearCard();
        this.userGameData = userGameData;
        this.enemyGameData = enemyGameData;
        RefreshUI();
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacter(CharacterBean userCharacterData, CharacterBean enemyCharacterData)
    {
        if (characterUser != null)
            characterUser.SetCharacterData(userCharacterData.body, userCharacterData.equips);
        if (characterEnemy != null)
            characterEnemy.SetCharacterData(enemyCharacterData.body, enemyCharacterData.equips);
    }

    /// <summary>
    /// 设置角色姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetCharacterName(string userName, string enemyName)
    {
        if (tvUserName != null)
            tvUserName.text = userName;
        if (tvEnemyName != null)
            tvEnemyName.text = enemyName;
    }

    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="life"></param>
    /// <param name="maxLife"></param>
    public void SetLife(int userLife, int userMaxLife, int enemyLife, int enemyMaxLife)
    {
        if (tvUserLife != null)
        {
            tvUserLife.text = userLife + "/" + userMaxLife;
        }
        if (tvEnemyLife != null)
        {
            tvEnemyLife.text = enemyLife + "/" + enemyMaxLife;
        }
    }

    /// <summary>
    /// 设置魅力
    /// </summary>
    /// <param name="userCharam"></param>
    /// <param name="enemyCharm"></param>
    public void SetCharm(int userCharam, int enemyCharm)
    {
        if (tvUserCharm != null)
        {
            tvUserCharm.text = GameCommonInfo.GetUITextById(4) + ":" + userCharam;
        }
        if (tvEnemyCharm != null)
        {
            tvEnemyCharm.text = GameCommonInfo.GetUITextById(4) + ":" + enemyCharm;
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
        if (isCombat)
            return;
        isCombat = true;
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
        CheckWinner(userCard, enemyCard, out ItemMiniGameDebateCardCpt winner, out ItemMiniGameDebateCardCpt loser);

        enemyCard.transform.SetParent(objCombatContainer.transform);
        enemyCard.ClosePointerListener();
        enemyCard.transform.DOMove(objCombatEnemyPosition.transform.position, 0.5f).OnComplete(delegate ()
        {

            enemyCard.transform.DOMove(objCombatEnemyEndPosition.transform.position, 1).SetEase(Ease.InOutBack);

        });

        userCard.transform.SetParent(objCombatContainer.transform);
        userCard.ClosePointerListener();
        userCard.transform.DOMove(objCombatUserPosition.transform.position, 0.5f).OnComplete(delegate ()
        {

            userCard.transform.DOMove(objCombatUserEndPosition.transform.position, 1).SetEase(Ease.InOutBack).OnComplete(delegate ()
            {

                //败者删除动画
                if (winner == null || loser == null)
                {
                    CardDestroyAnim(userCard);
                    CardDestroyAnim(enemyCard);
                }
                else
                {
                    CardDestroyAnim(loser);
                    if (mCallBack != null)
                        mCallBack.DamageForCharacter(loser.ownType);
                    psCombat.Play();
                    CardDestroyAnim(winner, 1f);
                }
                //开始新的回合
                StartCoroutine(CoroutineForStartNewRound());
            });

        });
    }

    /// <summary>
    /// 检测胜利者
    /// </summary>
    /// <param name="userCard"></param>
    /// <param name="enemyCard"></param>
    /// <returns></returns>
    public void CheckWinner(
        ItemMiniGameDebateCardCpt userCard, ItemMiniGameDebateCardCpt enemyCard,
        out ItemMiniGameDebateCardCpt winnerCard, out ItemMiniGameDebateCardCpt loserCard)
    {
        ItemMiniGameDebateCardCpt.DebateCardTypeEnun userCardType = userCard.debateCardType;
        ItemMiniGameDebateCardCpt.DebateCardTypeEnun enemyCardType = enemyCard.debateCardType;
        int result = (int)userCardType - (int)enemyCardType;
        if (result == -1 || result == 2)
        {
            winnerCard = userCard;
            loserCard = enemyCard;
        }
        else if (result == 0)
        {
            winnerCard = null;
            loserCard = null;
        }
        else
        {
            winnerCard = enemyCard;
            loserCard = userCard;
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
            rtfItem
                .DOLocalMoveY(120, 1)
                .From()
                .SetEase(Ease.OutBack)
                .SetDelay(position * 0.2f)
                .OnStart(
                    delegate () 
                        {
                            cgItem.alpha = 1;
                            uiGameManager.audioHandler.PlaySound(AudioSoundEnum.GetCard);
                        })
                .OnComplete(
                    delegate () 
                        {
                            cardItem.OpenPointerListener();
                        });
            listUserCard.Add(cardItem);
        }
        else if (objContainer == objEnemyDebateCardContainer)
        {
            cardItem.SetData(itemType, 2);
            rtfItem
                .DOLocalMoveY(120, 1)
                .From()
                .SetEase(Ease.OutBack)
                .SetDelay(position * 0.2f)
                .OnStart(
                    delegate () 
                        {
                            cgItem.alpha = 1;
                        })
                .OnComplete(
                    delegate () 
                        {
                            cardItem.OpenPointerListener();
                        });
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

    /// <summary>
    /// 卡片删除动画
    /// </summary>
    /// <param name="card"></param>
    private void CardDestroyAnim(ItemMiniGameDebateCardCpt card, float delayTime)
    {
        CanvasGroup cgLoser = card.GetComponent<CanvasGroup>();
        card.transform.DOShakePosition(0.2f, 5).SetDelay(delayTime);
        cgLoser.DOFade(0, 0.2f).SetDelay(delayTime); ;
        card.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f).SetDelay(delayTime).OnComplete(delegate ()
        {
            Destroy(card.gameObject);
        });
    }
    private void CardDestroyAnim(ItemMiniGameDebateCardCpt card)
    {
        CardDestroyAnim(card, 0);
    }

    /// <summary>
    /// 开始新的回合
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForStartNewRound()
    {
        yield return new WaitForSeconds(0.5f);
        if (listUserCard.Count <= 2)
        {
            DrawCard();
        }
        yield return new WaitForSeconds(0.5f);
        isCombat = false;
    }

    public interface ICallBack
    {
        /// <summary>
        /// 对角色造成伤害 
        /// </summary>
        /// <param name="characterType">1己方 2敌方</param>
        void DamageForCharacter(int characterType);
    }
}