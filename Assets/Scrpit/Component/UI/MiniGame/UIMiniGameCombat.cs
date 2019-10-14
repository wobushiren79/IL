using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIMiniGameCombat : UIBaseMiniGame
{
    //进度条
    public RectTransform rtfRoundContainer;

    //友方角色信息容器
    public GameObject objOurCharacterContainer;
    //敌方角色信息容器
    public GameObject objEnemyCharacterContainer;
    //角色回合信息容器
    public GameObject objRoundCharacterContainer;

    public GameObject objOurCharacterModel;
    public GameObject objEnemyCharacterModel;
    public GameObject objRoundCharacterModel;

    public MiniGameCombatBean gameCombatData;

    public List<ItemMiniGameCombatCharacterInfoCpt> mListCharacterInfo = new List<ItemMiniGameCombatCharacterInfoCpt>();
    public List<ItemMiniGameCombatCharacterRoundCpt> mListCharacterRound = new List<ItemMiniGameCombatCharacterRoundCpt>();

    private ICallBack mCallBack;

    private float mRoundContainerW = 0;

    private void Awake()
    {
        if (rtfRoundContainer != null)
            mRoundContainerW = rtfRoundContainer.rect.width;
    }

    private void Update()
    {
        //排序
        if (mListCharacterRound != null)
        {
            mListCharacterRound= mListCharacterRound.OrderByDescending(i=>((RectTransform)i.transform).anchoredPosition.x).ToList();
            for (int i = 0; i < mListCharacterRound.Count; i++)
            {
                RectTransform itemRTF =(RectTransform)mListCharacterRound[i].transform;
                itemRTF.SetAsFirstSibling();
            }
        }
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
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
    /// <param name="miniGameCombatData"></param>
    public void SetData(MiniGameCombatBean gameCombatData)
    {
        if (gameCombatData == null)
            return;
        this.gameCombatData = gameCombatData;
        //清空数据
        mListCharacterInfo.Clear();
        mListCharacterRound.Clear();
        CptUtil.RemoveChildsByActive(objRoundCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objOurCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objEnemyCharacterContainer.transform);

        //创建Item
        CreateOurCharacterList(gameCombatData.listUserGameData);
        CreateEnemyCharacterList(gameCombatData.listEnemyGameData);
    }

    /// <summary>
    /// 设置角色开始新的回合计时
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void StartNewRoundForCharacter(MiniGameCharacterBean gameCharacterData)
    {
        for (int i = 0; i < mListCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = mListCharacterRound[i];
            if (gameCharacterData == itemCpt.gameCharacterData)
            {
                //恢复状态
                itemCpt.SetStatus(false);
                //重新设置位置
                RectTransform itemRTF= (RectTransform)itemCpt.transform;
                itemRTF.anchoredPosition = Vector2.zero;
                return;
            }
        }
    }

    /// <summary>
    /// 开始回合计时
    /// </summary>
    public void StartRound()
    {
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        for (int i = 0; i < mListCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = mListCharacterRound[i];
            RectTransform itemRTF = (RectTransform)itemCpt.transform;

            itemCpt.gameCharacterData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
            //计算角色的回合的速度和到达时间
            float characterSpeed = (1 + characterAttributes.speed) * 5;
            float roundTime = (mRoundContainerW - itemRTF.anchoredPosition.x) / characterSpeed;
            itemRTF.DOAnchorPosX((mRoundContainerW), roundTime)
                .SetEase(Ease.Linear)
                .OnComplete(
                    delegate ()
                    {
                        //停止当前其他回合
                        foreach (ItemMiniGameCombatCharacterRoundCpt tempItemCpt in mListCharacterRound)
                        {
                            DOTween.Kill(tempItemCpt.transform);
                        }
                     
                        //通知轮到角色回合
                        if (mCallBack != null)
                            mCallBack.CharacterRoundCombat(itemCpt.gameCharacterData);
                        //设置为选中状态
                        itemCpt.SetStatus(true);
                    }
             );
        }
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
            MiniGameCharacterBean itemData = listCharacterData[i];
            CreateCharacterItem(i, itemData, objOurCharacterContainer, objOurCharacterModel);
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
            MiniGameCharacterBean itemData = listCharacterData[i];
            CreateCharacterItem(i, itemData, objEnemyCharacterContainer, objEnemyCharacterModel);
        }
    }

    /// <summary>
    /// 创建角色信息
    /// </summary>
    /// <param name="position"></param>
    /// <param name="gameCharacterData"></param>
    /// <param name="objContainer"></param>
    /// <param name="objModel"></param>
    public void CreateCharacterItem(int position, MiniGameCharacterBean gameCharacterData, GameObject objContainer, GameObject objModel)
    {
        //创建角色信息
        GameObject objItem = Instantiate(objContainer, objModel);
        ItemMiniGameCombatCharacterInfoCpt characterInfoCpt = objItem.GetComponent<ItemMiniGameCombatCharacterInfoCpt>();
        characterInfoCpt.SetData(gameCharacterData);
        mListCharacterInfo.Add(characterInfoCpt);
        //创建回合条信息
        GameObject objRoundItem = Instantiate(objRoundCharacterContainer, objRoundCharacterModel);
        RectTransform rtfItemRound = objRoundItem.GetComponent<RectTransform>();
        ItemMiniGameCombatCharacterRoundCpt characterRoundCpt = objRoundItem.GetComponent<ItemMiniGameCombatCharacterRoundCpt>();
        characterRoundCpt.SetData(gameCharacterData);
        mListCharacterRound.Add(characterRoundCpt);
        //添加动画
        objItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
        objRoundItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
    }

    public interface ICallBack
    {
        /// <summary>
        /// 轮到一个角色的回合
        /// </summary>
        /// <param name="gameCharacterData"></param>
        void CharacterRoundCombat(MiniGameCharacterBean gameCharacterData);
    }

}