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
        StoreInfoManager storeInfoManager = GetUIMananger<UIGameManager>().storeInfoManager;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForArenaInfo();
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
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        NpcInfoManager npcInfoManager = GetUIMananger<UIGameManager>().npcInfoManager;
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
            PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, storeInfo.pre_data_minigame, gameItemsManager, npcInfoManager);
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
            //添加对应的奖杯
            switch (type)
            {
                case 1:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyElementary, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyS, "1000"));
                    break;
                case 2:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyIntermediate, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyS, "5000"));
                    break;
                case 3:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyAdvanced, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyM, "10"));
                    break;
                case 4:
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddArenaTrophyLegendary, "1"));
                    miniGameData.listReward.Add(new RewardTypeBean(RewardTypeEnum.AddMoneyL, "10"));
                    break;
            }
            if (miniGameData != null)
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
        return miniGameData;
    }

    private MiniGameBarrageBean CreateBarrageGameData(MiniGameBarrageBean miniGameData, StoreInfoBean storeInfo)
    {
        return miniGameData;
    }

    private MiniGameAccountBean CreateAccountGameData(MiniGameAccountBean miniGameData, StoreInfoBean storeInfo)
    {
        return miniGameData;
    }

    private MiniGameDebateBean CreateDebateGameData(MiniGameDebateBean miniGameData, StoreInfoBean storeInfo)
    {
        return miniGameData;
    }

    private MiniGameCombatBean CreateCombatGameData(MiniGameCombatBean miniGameData, StoreInfoBean storeInfo)
    {
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