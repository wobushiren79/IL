using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIMiniGameDebate : UIBaseMiniGame<MiniGameDebateBean>
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

    private ICallBack mCallBack;

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);

        MiniGameCharacterForDebateBean userGameData =  (MiniGameCharacterForDebateBean)miniGameData.GetUserGameData();
        MiniGameCharacterForDebateBean enemyGameData = (MiniGameCharacterForDebateBean)miniGameData.GetEnemyGameData();

        SetCharacter(userGameData.characterData, enemyGameData.characterData);
        SetCharacterName(userGameData.characterData.baseInfo.name, enemyGameData.characterData.baseInfo.name);
        SetLife(userGameData.characterCurrentLife, userGameData.characterMaxLife, enemyGameData.characterCurrentLife, enemyGameData.characterMaxLife);

        userGameData.characterData.GetAttributes(out CharacterAttributesBean userAttributes);
        enemyGameData.characterData.GetAttributes(out CharacterAttributesBean enemyAttributes);
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
            tvUserCharm.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm) + ":" + userCharam;
        }
        if (tvEnemyCharm != null)
        {
            tvEnemyCharm.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm) + ":" + enemyCharm;
        }
    }
    /// <summary>
    /// 创建卡片集合
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

    /// <summary>
    /// 创建卡片
    /// </summary>
    /// <param name="position"></param>
    /// <param name="itemType"></param>
    /// <param name="objContainer"></param>
    /// <returns></returns>
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
                            AudioHandler.Instance.PlaySound(AudioSoundEnum.GetCard);
                        })
                .OnComplete(
                    delegate () 
                        {
                            cardItem.OpenPointerListener();
                        });
            miniGameData.listUserCard.Add(cardItem);
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
            miniGameData.listEnemyCard.Add(cardItem);
        }
        return objItem;
    }

    /// <summary>
    /// 清空卡
    /// </summary>
    public void ClearAllCard()
    {
        CptUtil.RemoveChildsByActive(objUserDebateCardContainer);
        CptUtil.RemoveChildsByActive(objEnemyDebateCardContainer);
    }

    /// <summary>
    /// 创建战斗卡牌
    /// </summary>
    /// <param name="userCard"></param>
    /// <param name="enemyCard"></param>
    public void CreateCombatAnim(
        ItemMiniGameDebateCardCpt userCard, ItemMiniGameDebateCardCpt enemyCard, 
        ItemMiniGameDebateCardCpt winnerCard, ItemMiniGameDebateCardCpt loserCard,
        float preCombatTime,float combatTime)
    {
        enemyCard.transform.SetParent(objCombatContainer.transform);
        enemyCard.ClosePointerListener();
        enemyCard.transform.DOMove(objCombatEnemyPosition.transform.position, preCombatTime).OnComplete(delegate ()
        {

            enemyCard.transform.DOMove(objCombatEnemyEndPosition.transform.position, combatTime).SetEase(Ease.InOutBack);

        });

        userCard.transform.SetParent(objCombatContainer.transform);
        userCard.ClosePointerListener();
        userCard.transform.DOMove(objCombatUserPosition.transform.position, preCombatTime).OnComplete(delegate ()
        {

            userCard.transform.DOMove(objCombatUserEndPosition.transform.position, combatTime).SetEase(Ease.InOutBack).OnComplete(delegate ()
            {
                if (winnerCard == null || loserCard == null)
                {
                    AudioHandler.Instance.PlaySound(AudioSoundEnum.CardDraw);
                    //平手 都删除
                    CardDestroyAnim(userCard);
                    CardDestroyAnim(enemyCard);
                }
                else
                {
                    //败者先删除动画
                    CardDestroyAnim(loserCard);
                    //卡牌特效
                    psCombat.Play();
                    CardDestroyAnim(winnerCard, 1f);
                    //输赢
                    if (userCard== winnerCard)
                    {
                        AudioHandler.Instance.PlaySound(AudioSoundEnum.CardWin);
                    }
                    else
                    {
                        AudioHandler.Instance.PlaySound(AudioSoundEnum.CardLose);
                    }
                }
                //通知动画结束
                if (mCallBack != null)
                    mCallBack.CombatAnimEnd();
            });

        });
    }

    /// <summary>
    /// 卡片删除动画
    /// </summary>
    /// <param name="card"></param>
    private void CardDestroyAnim(ItemMiniGameDebateCardCpt card, float delayTime)
    {
        if (card == null)
            return;
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

    public interface ICallBack
    {
        /// <summary>
        /// 战斗动画结束
        /// </summary>
        void CombatAnimEnd();
    }
}