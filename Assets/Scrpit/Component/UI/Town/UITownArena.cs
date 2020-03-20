using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;

public class UITownArena : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public GameObject objArenaContainer;
    public GameObject objArenaModel;

    public RadioGroupView rgType;
    private List<StoreInfoBean> listArenaInfo;

    public override void Start()
    {
        base.Start();
        if (rgType != null)
        {
            rgType.SetCallBack(this);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForArenaInfo();
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    /// <param name="type"></param>
    public void InitData(int type)
    {
        CptUtil.RemoveChildsByActive(objArenaContainer);
        List<MiniGameBaseBean> listMiniGameData = null;
        switch (type)
        {
            case 1:
                listMiniGameData = GameCommonInfo.DailyLimitData.listArenaDataFor1;
                break;
            case 2:
                listMiniGameData = GameCommonInfo.DailyLimitData.listArenaDataFor2;
                break;
            case 3:
                listMiniGameData = GameCommonInfo.DailyLimitData.listArenaDataFor3;
                break;
            case 4:
                listMiniGameData = GameCommonInfo.DailyLimitData.listArenaDataFor4;
                break;
        }
        if (listMiniGameData == null)
        {
            listMiniGameData = CreateMiniGameData(type);
            GameCommonInfo.DailyLimitData.AddArenaDataByType(type, listMiniGameData);
        }
        for (int i = 0; i < listMiniGameData.Count; i++)
        {
            MiniGameBaseBean itemMiniGameData = listMiniGameData[i];
            GameObject objItem = Instantiate(objArenaContainer, objArenaModel);
            ItemTownArenaCpt arenaItem = objItem.GetComponent<ItemTownArenaCpt>();
            arenaItem.SetData(itemMiniGameData);
            GameUtil.RefreshRectViewHight((RectTransform)objItem.transform, true);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 创建迷你游戏数据
    /// </summary>
    /// <returns></returns>
    private List<MiniGameBaseBean> CreateMiniGameData(int type)
    {
        List<MiniGameBaseBean> listMiniGameData = new List<MiniGameBaseBean>();
        int arenaNumber = Random.Range(1, 5);
        for (int i = 0; i < arenaNumber; i++)
        {
            MiniGameEnum gameType = RandomUtil.GetRandomEnum<MiniGameEnum>();
            StoreInfoBean storeInfo = null;

            MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(gameType);
            switch (gameType)
            {
                case MiniGameEnum.Cooking:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Chef);
                    miniGameData = CreateCookingGameData((MiniGameCookingBean)miniGameData, storeInfo);
                    break;
                case MiniGameEnum.Barrage:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Waiter);
                    miniGameData = CreateBarrageGameData((MiniGameBarrageBean)miniGameData, storeInfo);
                    break;
                case MiniGameEnum.Account:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Accountant);
                    miniGameData = CreateAccountGameData((MiniGameAccountBean)miniGameData, storeInfo);
                    break;
                case MiniGameEnum.Debate:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Accost);
                    miniGameData = CreateDebateGameData((MiniGameDebateBean)miniGameData, storeInfo);
                    break;
                case MiniGameEnum.Combat:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Beater);
                    miniGameData = CreateCombatGameData((MiniGameCombatBean)miniGameData, storeInfo);
                    break;
            }
            PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, storeInfo.pre_data_minigame, uiGameManager.gameItemsManager, uiGameManager.npcInfoManager);
            miniGameData.preMoneyL = storeInfo.price_l;
            miniGameData.preMoneyM = storeInfo.price_m;
            miniGameData.preMoneyS = storeInfo.price_s;
            //奖励添加
            miniGameData.listReward = new List<RewardTypeBean>();
            List<RewardTypeBean> listReward = RewardTypeEnumTools.GetListRewardData(storeInfo.reward_data);
            if (!CheckUtil.ListIsNull(listReward))
            {
                RewardTypeBean randomReward = RandomUtil.GetRandomDataByList(listReward);
                miniGameData.listReward.Add(randomReward);
            }
            int gameTime = 1;
            //添加对应的奖杯
            switch (type)
            {
                case 1:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyElementary, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyS, "1000"));
                    gameTime = 1;
                    break;
                case 2:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyIntermediate, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyS, "5000"));
                    gameTime = 2;
                    break;
                case 3:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyAdvanced, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyM, "10"));
                    gameTime = 3;
                    break;
                case 4:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyLegendary, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyL, "10"));
                    gameTime = 4;
                    break;
            }
            //设置游戏时间
            miniGameData.preGameTime = gameTime;
            //添加游戏数据
            listMiniGameData.Add(miniGameData);
        }
        return listMiniGameData;
    }

    /// <summary>
    /// 根据职业和类型获取竞技场数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="workerEnum"></param>
    /// <returns></returns>
    private StoreInfoBean GetStoreInfoByTypeAndWorker(int type, WorkerEnum workerEnum)
    {
        foreach (StoreInfoBean storeInfo in listArenaInfo)
        {
            if (storeInfo.mark_type == type && EnumUtil.GetEnum<WorkerEnum>(storeInfo.pre_data) == workerEnum)
            {
                return storeInfo;
            }
        }
        return null;
    }

    private MiniGameCookingBean CreateCookingGameData(MiniGameCookingBean miniGameData, StoreInfoBean storeInfo)
    {
        miniGameData.winScore = 60;
        miniGameData.storyGameStartId = 30000001;
        miniGameData.storyGameAuditId = 30000002;
        //随机生成敌人
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        for (int i = 0; i < Random.Range(1, 16); i++)
        {
            CharacterBean randomEnemy = CharacterBean.CreateRandomWorkerData(uiGameManager.characterBodyManager);
            listEnemyData.Add(randomEnemy);
        }
        //主持由东方姑娘主持
        List<CharacterBean> listCompereData = new List<CharacterBean>();
        CharacterBean compereData = uiGameManager.npcInfoManager.GetCharacterDataById(110051);
        listCompereData.Add(compereData);
        //评审人员
        List<long> listAuditerIds = new List<long>() { 100011, 100021, 100031, 100041, 100051, 100061, 100071, 100081, 100091 };
        List<CharacterBean> listAuditerData = new List<CharacterBean>();
        listAuditerIds = RandomUtil.GetRandomDataByListForNumberNR(listAuditerIds, 5);
        foreach (long itemId in listAuditerIds)
        {
            CharacterBean auditerData = uiGameManager.npcInfoManager.GetCharacterDataById(itemId);
            listAuditerData.Add(auditerData);
        }
        miniGameData.InitData(uiGameManager.gameItemsManager, null, listEnemyData, listAuditerData, listCompereData);
        return miniGameData;
    }

    private MiniGameBarrageBean CreateBarrageGameData(MiniGameBarrageBean miniGameData, StoreInfoBean storeInfo)
    {
        return miniGameData;
    }

    private MiniGameAccountBean CreateAccountGameData(MiniGameAccountBean miniGameData, StoreInfoBean storeInfo)
    {
        miniGameData.winMoneyL = 0;
        miniGameData.winMoneyM = 1;
        miniGameData.winMoneyS = 10;
        return miniGameData;
    }

    private MiniGameDebateBean CreateDebateGameData(MiniGameDebateBean miniGameData, StoreInfoBean storeInfo)
    {
        miniGameData.winLife = 1;
        CharacterBean enemyData = uiGameManager.npcInfoManager.GetCharacterDataById(110111);
        miniGameData.InitData(uiGameManager.gameItemsManager, null, enemyData);
        return miniGameData;
    }

    private MiniGameCombatBean CreateCombatGameData(MiniGameCombatBean miniGameData, StoreInfoBean storeInfo)
    {
        miniGameData.winBringDownNumber = 1;
        miniGameData.winSurvivalNumber = 1;
        CharacterBean enemyData = uiGameManager.npcInfoManager.GetCharacterDataById(110111);
        miniGameData.InitData(uiGameManager.gameItemsManager, null, enemyData);
        return miniGameData;
    }

    #region 等级选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        InitData(position + 1);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listArenaInfo = listData;
        InitData(1);
    }
    #endregion
}