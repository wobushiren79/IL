using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class UIMiniGameEnd : BaseUIComponent
{
    //游戏结果
    public GameObject objHalo;
    public Text tvGameResult;
    public Image ivGameResult;

    public Button btClose;
    public Image ivClose;

    public GameObject objContent;
    public Image ivContent;

    public Sprite spTitleWin;
    public Sprite spTitleLose;

    public Sprite spBackgroundWin;
    public Sprite spBackgroundLose;

    public Sprite spCloseWin;
    public Sprite spCloseLose;

    public GameObject objResultContainer;
    public GameObject objResultWinModel;
    public GameObject objResultLoseModel;

    public ICallBack callBack;

    public MiniGameBaseBean miniGameData;
    private void Start()
    {
        if (btClose != null)
            btClose.onClick.AddListener(OnClickClose);
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="isWin"></param>
    public void SetData(MiniGameBaseBean miniGameData)
    {
        this.miniGameData = miniGameData;
        if (miniGameData.gameResult == 1)
        {
            SetWin();
        }
        else
        {
            SetLose();
        }
        objContent.transform.localScale = new Vector3(1, 1, 1);
        objContent.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 设置胜利
    /// </summary>
    public void SetWin()
    {
        if (tvGameResult != null)
        {
            tvGameResult.text = GameCommonInfo.GetUITextById(41);
            tvGameResult.color = new Color(0.5f, 0, 0);
        }

        if (ivGameResult != null)
            ivGameResult.sprite = spTitleWin;
        if (objHalo != null)
            objHalo.SetActive(true);
        if (ivClose != null)
            ivClose.sprite = spCloseWin;
        if (ivContent != null)
            ivContent.sprite = spBackgroundWin;

        CreateWinItems();
    }

    /// <summary>
    /// 设置失败
    /// </summary>
    public void SetLose()
    {
        if (tvGameResult != null)
        {
            tvGameResult.text = GameCommonInfo.GetUITextById(42);
            tvGameResult.color = new Color(1f, 1f, 1f);
        }

        if (ivGameResult != null)
            ivGameResult.sprite = spTitleLose;
        if (objHalo != null)
            objHalo.SetActive(false);
        if (ivClose != null)
            ivClose.sprite = spCloseLose;
        if (ivContent != null)
            ivContent.sprite = spBackgroundLose;

        CreateLoseItems();
    }

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public void OnClickClose()
    {
        CloseUI();
        if (callBack != null)
            callBack.OnClickClose();
    }

    /// <summary>
    /// 创建胜利数据
    /// </summary>
    private void CreateWinItems()
    {
        CptUtil.RemoveChildsByActive(objResultContainer.transform);
        if (miniGameData == null)
            return;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        IconDataManager iconDataManager = GetUIManager<UIGameManager>().iconDataManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;

        CharacterDressManager characterDressManager = GetUIManager<UIGameManager>().characterDressManager;
        NpcInfoManager npcInfoManager = GetUIManager<UIGameManager>().npcInfoManager;
        //通常列表
        string reasonStr = "";
        GameObject objReasonItem = Instantiate(objResultContainer, objResultWinModel);
        ItemMiniGameEndResultWinCpt itemReasonWin = objReasonItem.GetComponent<ItemMiniGameEndResultWinCpt>();
        switch (miniGameData.gameReason)
        {
            case MiniGameReasonEnum.Improve:
                //晋升成功
                reasonStr = GameCommonInfo.GetUITextById(43);
                //数据添加
                Sprite attributeIcon = null;
                string attributeRewardContent = "";
                foreach (MiniGameCharacterBean miniGameCharacterData in miniGameData.listUserGameData)
                {
                    switch (miniGameData.gameType)
                    {
                        case MiniGameEnum.Cooking:
                            attributeRewardContent = GameCommonInfo.GetUITextById(1) + " +5";
                            miniGameCharacterData.characterData.baseInfo.chefInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Barrage:
                            attributeRewardContent = GameCommonInfo.GetUITextById(2) + " +5";
                            miniGameCharacterData.characterData.baseInfo.waiterInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Account:
                            attributeRewardContent = GameCommonInfo.GetUITextById(3) + " +5";
                            miniGameCharacterData.characterData.baseInfo.accountantInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Debate:
                            attributeRewardContent = GameCommonInfo.GetUITextById(4) + " +5";
                            miniGameCharacterData.characterData.baseInfo.accostInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Combat:
                            attributeRewardContent = GameCommonInfo.GetUITextById(5) + " +5";
                            miniGameCharacterData.characterData.baseInfo.beaterInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                    }
                }
                //创建属性奖励
                attributeIcon = iconDataManager.GetIconSpriteByName("keyboard_button_up_1");
                CreateWinReward(attributeIcon,null, attributeRewardContent);
                break;
            case MiniGameReasonEnum.Fight:
                //决斗胜利
                reasonStr = GameCommonInfo.GetUITextById(44);
                break;
            case MiniGameReasonEnum.Recruit:
                //决斗胜利
                reasonStr = GameCommonInfo.GetUITextById(45);
                break;
        }
        itemReasonWin.SetContent(reasonStr);
        itemReasonWin.SetIcon(null);
        //添加奖励
        RewardTypeEnumTools.CompleteReward(miniGameData.listReward, gameDataManager.gameData);
        //遍历奖励列表
        foreach (var itemReward in miniGameData.listReward)
        {
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager);
            CreateWinReward(itemReward.spRewardIcon, itemReward.workerCharacterData, itemReward.rewardDescribe);
        }
    }

    /// <summary>
    /// 创建奖励列表
    /// </summary>
    /// <param name="rewardIcon"></param>
    /// <param name="rewardCharacterData"></param>
    /// <param name="rewardContent"></param>
    private void CreateWinReward(Sprite rewardIcon,CharacterBean rewardCharacterData,string rewardContent)
    {
        //生成奖励列表
        GameObject objItem = Instantiate(objResultContainer, objResultWinModel);
        ItemMiniGameEndResultWinCpt itemWin = objItem.GetComponent<ItemMiniGameEndResultWinCpt>();
        if (rewardIcon != null)
            itemWin.SetIcon(rewardIcon);
        if (rewardCharacterData != null)
            itemWin.SetCharacterUI(rewardCharacterData);
        itemWin.SetContent(rewardContent);
        //设置动画
        objItem.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 创建失败数据
    /// </summary>
    private void CreateLoseItems()
    {
        CptUtil.RemoveChildsByActive(objResultContainer.transform);
        GameObject objItem = Instantiate(objResultContainer, objResultLoseModel);
        ItemMiniGameEndResultLoseCpt itemLose = objItem.GetComponent<ItemMiniGameEndResultLoseCpt>();
        //获取失败文本
        string content = GameCommonInfo.GetUITextById(UnityEngine.Random.Range(99011, 99015));
        if (itemLose != null)
            itemLose.SetContent(content);
        //设置动画
        objItem.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.7f);
    }

    public interface ICallBack
    {
        void OnClickClose();
    }
}