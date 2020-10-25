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
    public Button btSend;

    public GameObject objRewardContainer;
    public GameObject objRewardModel;

    public MiniGameBaseBean miniGameData;
    public TrophyTypeEnum trophyType;
    public MiniGameEnum gameType;

    private void Start()
    {
        if (btJoin != null)
            btJoin.onClick.AddListener(OnClickForJoin);
        if (btSend != null)
            btSend.onClick.AddListener(OnClickForSend);
    }

    public void SetData(TrophyTypeEnum trophyType, MiniGameBaseBean miniGameData)
    {
        InitDataForType(miniGameData.gameType, trophyType, miniGameData);
    }

    public void InitDataForType(MiniGameEnum gameType, TrophyTypeEnum trophyType, MiniGameBaseBean miniGameData)
    {
        this.gameType = gameType;
        this.miniGameData = miniGameData;
        this.trophyType = trophyType;
        SetTitle(miniGameData);
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
        GameUtil.RefreshRectViewHight((RectTransform)transform, true);
    }

    public void InitDataForCooking(MiniGameBaseBean miniGameData)
    {
        MiniGameCookingBean miniGameCookingData = (MiniGameCookingBean)miniGameData;
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(MiniGameBaseBean miniGameData)
    {
        string title = "";
        if (miniGameData.gameType == MiniGameEnum.Combat)
        {
            if (miniGameData.winBringDownNumber == 1)
            {
                title = miniGameData.GetGameName() + "(" + GameCommonInfo.GetUITextById(92) + ")";
            }
            else
            {
                title = miniGameData.GetGameName() + "(" + string.Format(GameCommonInfo.GetUITextById(91), miniGameData.winBringDownNumber) + ")";
            }
        }
        else
        {
            title = miniGameData.GetGameName();
        }
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
        NpcInfoManager npcInfoManager = uiGameManager.npcInfoManager;
        InfoItemsPopupShow infoItemsPopupShow = uiGameManager.infoItemsPopup;
        foreach (RewardTypeBean itemReward in listReward)
        {
            GameObject objReward = Instantiate(objRewardContainer, objRewardModel);
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            Text tvNumber = CptUtil.GetCptInChildrenByName<Text>(objReward, "Text");
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager, npcInfoManager);
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
        //设置参与等级提示
        string levelToastWorker = "???";
        string levelToastLevel = "???";
        switch (gameType)
        {
            case MiniGameEnum.Cooking:
                levelToastWorker = CharacterWorkerBaseBean.GetWorkerName(WorkerEnum.Chef);
                break;
            case MiniGameEnum.Barrage:
                levelToastWorker = CharacterWorkerBaseBean.GetWorkerName(WorkerEnum.Waiter);
                break;
            case MiniGameEnum.Account:
                levelToastWorker = CharacterWorkerBaseBean.GetWorkerName(WorkerEnum.Accountant);
                break;
            case MiniGameEnum.Debate:
                levelToastWorker = CharacterWorkerBaseBean.GetWorkerName(WorkerEnum.Accost);
                break;
            case MiniGameEnum.Combat:
                levelToastWorker = CharacterWorkerBaseBean.GetWorkerName(WorkerEnum.Beater);
                break;
        }

        switch (trophyType)
        {
            case TrophyTypeEnum.Elementary:
                levelToastLevel = " " + CharacterWorkerBaseBean.GetWorkerLevelName(0) + " " + CharacterWorkerBaseBean.GetWorkerLevelName(1);
                break;
            case TrophyTypeEnum.Intermediate:
                levelToastLevel = " " + CharacterWorkerBaseBean.GetWorkerLevelName(2) + " " + CharacterWorkerBaseBean.GetWorkerLevelName(3);
                break;
            case TrophyTypeEnum.Advanced:
                levelToastLevel = " " + CharacterWorkerBaseBean.GetWorkerLevelName(4) + " " + CharacterWorkerBaseBean.GetWorkerLevelName(5);
                break;
            case TrophyTypeEnum.Legendary:
                levelToastLevel = " " + CharacterWorkerBaseBean.GetWorkerLevelName(6);
                break;
        }
        string levelToast = string.Format(GameCommonInfo.GetUITextById(221), levelToastWorker, levelToastLevel);
        ruleStr += (levelToast + "\n");
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

    protected int arenaJoinType = 1;

    /// <summary>
    /// 点击加入
    /// </summary>
    public void OnClickForJoin()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        DialogManager dialogManager = uiGameManager.dialogManager;
        AudioHandler audioHandler = uiGameManager.audioHandler;
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
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
        arenaJoinType = 1;
    }

    /// <summary>
    /// 点击派遣
    /// </summary>
    public void OnClickForSend()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        DialogManager dialogManager = uiGameManager.dialogManager;
        AudioHandler audioHandler = uiGameManager.audioHandler;

        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (!gameDataManager.gameData.HasEnoughMoney(miniGameData.preMoneyL, miniGameData.preMoneyM, miniGameData.preMoneyS))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1020));
            return;
        }
        DialogBean dialogData = new DialogBean();
        string gameName = tvTitle.text;
        string gameTime = miniGameData.preGameTime + GameCommonInfo.GetUITextById(37);
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3022), gameName, gameTime);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
        arenaJoinType = 2;
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        GameDataHandler gameDataHandler = uiGameManager.gameDataHandler;
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        ControlHandler controlHandler = uiGameManager.controlHandler;
        DialogManager dialogManager = uiGameManager.dialogManager;
        ToastManager toastManager = uiGameManager.toastManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;
        NpcInfoManager npcInfoManager = uiGameManager.npcInfoManager;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        AudioHandler audioHandler = uiGameManager.audioHandler;
        GameTimeHandler gameTimeHandler = uiGameManager.gameTimeHandler;
  
        if (dialogView as PickForCharacterDialogView)
        {
            //判断时间是否过晚
            gameTimeHandler.GetTime(out float hour, out float min);
            if (hour >= 21 || hour < 6)
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1034));
                return;
            }
            //支付金钱
            gameDataManager.gameData.PayMoney(miniGameData.preMoneyL, miniGameData.preMoneyM, miniGameData.preMoneyS);
            //扣除时间
            gameDataManager.gameData.gameTime.hour += miniGameData.preGameTime;
            //如果有研究菜谱 菜谱增加经验
            gameDataHandler.AddResearch(miniGameData.preGameTime*60);
            //设置参赛人员
            PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)dialogView;
            List<CharacterBean> listCharacter = pickForCharacterDialog.GetPickCharacter();
            miniGameData.InitData(gameItemsManager, listCharacter);
            //今日不能再参加
            GameCommonInfo.DailyLimitData.AddArenaAttendedCharacter(listCharacter);
            //删除该条数据
            GameCommonInfo.DailyLimitData.RemoveArenaDataByType(trophyType, miniGameData);

            if (arenaJoinType==1)
            {
                //设置竞技场数据
                GameCommonInfo.SetArenaPrepareData(miniGameData);
                //保存之前的位置
                GameCommonInfo.ScenesChangeData.beforeUserPosition = controlHandler.GetControl(ControlHandler.ControlEnum.Normal).transform.position;
                //跳转到竞技场
                SceneUtil.SceneChange(ScenesEnum.GameArenaScene);
            }
            else if (arenaJoinType == 2)
            {
                //扣除时间
                gameTimeHandler.AddHour(miniGameData.preGameTime);
                //刷新UI
                ((UITownArena)uiComponent).RefreshUI();
                CharacterBean character = listCharacter[0];
                bool isWin= character.CalculationArenaSendWin(gameItemsManager, miniGameData.gameType);
                if (isWin)
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(7011));
                    audioHandler.PlaySound(AudioSoundEnum.Reward);
                    //设置不记录
                    foreach (RewardTypeBean rewardData in miniGameData.listReward)
                    {
                        if (rewardData.GetRewardType() == RewardTypeEnum.AddArenaTrophyAdvanced 
                            || rewardData.GetRewardType() == RewardTypeEnum.AddArenaTrophyElementary
                             || rewardData.GetRewardType() == RewardTypeEnum.AddArenaTrophyIntermediate
                              || rewardData.GetRewardType() == RewardTypeEnum.AddArenaTrophyLegendary)
                        {
                            rewardData.isRecord = false;
                        }
                    }
                    //完成奖励
                    RewardTypeEnumTools.CompleteReward
                        (toastManager, npcInfoManager, iconDataManager, gameItemsManager, innBuildManager, gameDataManager, listCharacter, miniGameData.listReward);     
                }
                else
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(7012));
                    audioHandler.PlaySound(AudioSoundEnum.Passive);
                }
            }
        }
        else
        {
            //弹出选人界面
            DialogBean dialogData = new DialogBean();
            PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)dialogManager.CreateDialog(DialogEnum.PickForCharacter, this, dialogData);
            pickForCharacterDialog.SetPickCharacterMax(1);
            List<string> listExpelCharacterId = new List<string>();
            //排出今日已经参加过的人
            List<string> listAttendedCharacterId = GameCommonInfo.DailyLimitData.GetArenaAttendedCharacter();
            //排出等级不符合的人
            List<CharacterBean> listWorker= gameDataManager.gameData.GetAllCharacterData();
            foreach (CharacterBean itemWorker in listWorker)
            {
                bool isExpel = false;
                CharacterWorkerBaseBean workerInfo = null;
                switch (gameType)
                {
                    case MiniGameEnum.Cooking:
                        workerInfo = itemWorker.baseInfo.GetWorkerInfoByType( WorkerEnum.Chef);
                        break;
                    case MiniGameEnum.Barrage:
                        workerInfo = itemWorker.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                        break;
                    case MiniGameEnum.Account:
                        workerInfo = itemWorker.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
                        break;
                    case MiniGameEnum.Debate:
                        workerInfo = itemWorker.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
                        break;
                    case MiniGameEnum.Combat:
                        workerInfo = itemWorker.baseInfo.GetWorkerInfoByType(WorkerEnum.Beater);
                        break;
                }
                int workLevel = workerInfo.GetLevel();
                switch (trophyType)
                {
                    case TrophyTypeEnum.Elementary:
                        if (workLevel != 0 && workLevel != 1)
                        {
                            isExpel = true;
                        }
                        break;
                    case TrophyTypeEnum.Intermediate:
                        if (workLevel != 2 && workLevel != 3)
                        {
                            isExpel = true;
                        }
                        break;
                    case TrophyTypeEnum.Advanced:
                        if (workLevel != 4 && workLevel != 5)
                        {
                            isExpel = true;
                        }
                        break;
                    case TrophyTypeEnum.Legendary:
                        if (workLevel != 6 )
                        {
                            isExpel = true;
                        }
                        break;
                }
                if (isExpel)
                {
                    listExpelCharacterId.Add(itemWorker.baseInfo.characterId);
                }
            }
            listExpelCharacterId.AddRange(listAttendedCharacterId);
            pickForCharacterDialog.SetExpelCharacter(listExpelCharacterId);
            if (miniGameData.gameType == MiniGameEnum.Combat)
            {
                pickForCharacterDialog.SetPickCharacterMax(miniGameData.winBringDownNumber);
            }
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
