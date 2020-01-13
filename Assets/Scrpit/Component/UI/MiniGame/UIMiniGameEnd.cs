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
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        IconDataManager iconDataManager = GetUIMananger<UIGameManager>().iconDataManager;
        InnBuildManager innBuildManager = GetUIMananger<UIGameManager>().innBuildManager;

        CharacterDressManager characterDressManager = GetUIMananger<UIGameManager>().characterDressManager;
        NpcInfoManager npcInfoManager = GetUIMananger<UIGameManager>().npcInfoManager;
        //通常列表
        string reasonStr = "";
        switch (miniGameData.gameReason)
        {
            case MiniGameReasonEnum.Improve:
                //晋升成功
                reasonStr = GameCommonInfo.GetUITextById(43);
                //数据添加
                foreach (MiniGameCharacterBean miniGameCharacterData in miniGameData.listUserGameData)
                {
                    switch (miniGameData.gameType)
                    {
                        case MiniGameEnum.Cooking:
                            miniGameCharacterData.characterData.baseInfo.chefInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Barrage:
                            miniGameCharacterData.characterData.baseInfo.waiterInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                    }
                }
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
        GameObject objReasonItem = Instantiate(objResultContainer, objResultWinModel);
        ItemMiniGameEndResultWinCpt itemReasonWin = objReasonItem.GetComponent<ItemMiniGameEndResultWinCpt>();
        itemReasonWin.SetContent(reasonStr);
        itemReasonWin.SetIcon(null);
        //添加奖励
        RewardTypeEnumTools.CompleteReward(miniGameData.listReward, gameDataManager.gameData);
        //遍历奖励列表
        foreach (var itemReward in miniGameData.listReward)
        {
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager);
            //生成奖励列表
            GameObject objItem = Instantiate(objResultContainer, objResultWinModel);
            ItemMiniGameEndResultWinCpt itemWin = objItem.GetComponent<ItemMiniGameEndResultWinCpt>();
            if (itemReward.spRewardIcon != null)
                itemWin.SetIcon(itemReward.spRewardIcon);
            if (itemReward.workerCharacterData != null)
                itemWin.SetCharacterUI(itemReward.workerCharacterData);
            itemWin.SetContent(itemReward.rewardDescribe);
            //设置动画
            objItem.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.7f);
        }
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