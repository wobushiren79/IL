using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownArenaCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Text tvTitle;
    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;
    public Text tvGameTime;

    public Text tvRule;
    public Button btJoin;

    public GameObject objRewardContainer;
    public GameObject objRewardModel;
    public MiniGameBaseBean miniGameData;

    private void Start()
    {
        if (btJoin != null)
            btJoin.onClick.AddListener(OnClickJoin);
    }

    public void SetData(MiniGameBaseBean miniGameData)
    {
        InitDataForType(miniGameData.gameType, miniGameData);
    }

    public void InitDataForType(MiniGameEnum gameType, MiniGameBaseBean miniGameData)
    {
        this.miniGameData = miniGameData;
        SetTitle(miniGameData.GetGameName());
        SetReward(miniGameData.listReward);
        SetPrice(miniGameData.preMoneyL, miniGameData.preMoneyM, miniGameData.preMoneyS);
        SetRuleContent(miniGameData.GetListWinConditions());
        SetGameTime(miniGameData.preGameTime);
        switch (gameType)
        {
            case MiniGameEnum.Cooking:
                break;
            case MiniGameEnum.Barrage:
                break;
            case MiniGameEnum.Account:
                break;
            case MiniGameEnum.Debate:
                break;
            case MiniGameEnum.Combat:
                break;
        }
    }

    public void InitDataForCooking(MiniGameBaseBean miniGameData)
    {
        MiniGameCookingBean miniGameCookingData = (MiniGameCookingBean)miniGameData;
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title)
    {
        if (tvTitle != null)
        {
            tvTitle.text = title;
        }
    }

    /// <summary>
    /// 设置奖励
    /// </summary>
    /// <param name="listReward"></param>
    public void SetReward(List<RewardTypeBean> listReward)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        InfoItemsPopupShow infoItemsPopupShow = uiGameManager.infoItemsPopup;
        foreach (RewardTypeBean itemReward in listReward)
        {
            GameObject objReward = Instantiate(objRewardContainer, objRewardModel);
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            Text tvNumber = CptUtil.GetCptInChildrenByName<Text>(objReward, "Text");
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager);
            ivIcon.sprite = itemReward.spRewardIcon;
            tvNumber.text = "x" + itemReward.rewardNumber;
            if (itemReward.dataType == RewardTypeEnum.AddItems)
            {
                InfoItemsPopupButton infoItemsPopup = objReward.GetComponent<InfoItemsPopupButton>();
                infoItemsPopup.SetPopupShowView(infoItemsPopupShow);
                ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemReward.rewardId);
                infoItemsPopup.SetData(itemsInfo, itemReward.spRewardIcon);
            }
        }
    }

    /// <summary>
    /// 设置游戏时间
    /// </summary>
    /// <param name="hour"></param>
    public void SetGameTime(int hour)
    {
        if (tvGameTime != null)
            tvGameTime.text = GameCommonInfo.GetUITextById(40) + ":" + hour + GameCommonInfo.GetUITextById(37);
    }

    /// <summary>
    /// 设置规则说明
    /// </summary>
    /// <param name="content"></param>
    public void SetRuleContent(string content)
    {
        if (tvRule != null)
            tvRule.text = content;
    }

    public void SetRuleContent(List<string> listRule)
    {
        string ruleStr = "";
        for (int i = 0; i < listRule.Count; i++)
        {
            string itemRule = listRule[i];
            ruleStr += ((i + 1) + "." + itemRule + "\n");
        }
        SetRuleContent(ruleStr);
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="pirceM"></param>
    /// <param name="priceS"></param>
    public void SetPrice(long priceL, long pirceM, long priceS)
    {
        if (priceL == 0)
            objPriceL.SetActive(false);
        if (pirceM == 0)
            objPriceM.SetActive(false);
        if (priceS == 0)
            objPriceS.SetActive(false);
        tvPriceL.text = "" + priceL;
        tvPriceM.text = "" + pirceM;
        tvPriceS.text = "" + priceS;
    }

    /// <summary>
    /// 点击加入
    /// </summary>
    public void OnClickJoin()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        DialogManager dialogManager = uiGameManager.dialogManager;

        if (!gameDataManager.gameData.HasEnoughMoney(miniGameData.preMoneyL, miniGameData.preMoneyM, miniGameData.preMoneyS))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1020));
            return;
        }
        DialogBean dialogData = new DialogBean();
        string gameName = tvTitle.text;
        string gameTime = miniGameData.preGameTime + GameCommonInfo.GetUITextById(37);
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3021), gameName, gameTime);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        ControlHandler controlHandler = uiGameManager.controlHandler;
        DialogManager dialogManager = uiGameManager.dialogManager;
        if (dialogView as PickForCharacterDialogView)
        {
            //支付金钱
            gameDataManager.gameData.PayMoney(miniGameData.preMoneyL, miniGameData.preMoneyM, miniGameData.preMoneyS);
            //设置参赛人员
            PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)dialogView;
            List<CharacterBean> listCharacter = pickForCharacterDialog.GetPickCharacter();
            miniGameData.InitData(gameItemsManager, listCharacter);
            //设置竞技场数据
            GameCommonInfo.SetAreanPrepareData(miniGameData);
            //保存之前的位置
            GameCommonInfo.ScenesChangeData.beforeUserPosition = controlHandler.GetControl(ControlHandler.ControlEnum.Normal).transform.position;
            //跳转到竞技场
            SceneUtil.SceneChange(ScenesEnum.GameArenaScene);
        }
        else
        {
            DialogBean dialogData = new DialogBean();
            PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)dialogManager.CreateDialog(DialogEnum.PickForCharacter, this, dialogData);
            pickForCharacterDialog.SetPickCharacterMax(1);
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
