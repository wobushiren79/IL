using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneForInfiniteTowersHandler : BaseHandler, IBaseObserver
{
    protected MiniGameCombatHandler combatHandler;
    protected SceneInfiniteTowersManager infiniteTowersManager;

    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected NpcInfoManager npcInfoManager;
    protected NpcTeamManager npcTeamManager;
    protected UIGameManager uiGameManager;
    protected CharacterBodyManager characterBodyManager;

    protected UserInfiniteTowersBean infiniteTowersData;
    public void Awake()
    {
        combatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        infiniteTowersManager = Find<SceneInfiniteTowersManager>(ImportantTypeEnum.SceneManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        characterBodyManager = Find<CharacterBodyManager>(ImportantTypeEnum.CharacterManager);
    }
    public void Start()
    {
        combatHandler.AddObserver(this);
    }

    public MiniGameCombatBean InitCombat(UserInfiniteTowersBean infiniteTowersData)
    {
        //设置标牌
        infiniteTowersManager.SetSignForLayer(infiniteTowersData.layer);
        //设置战斗数据
        MiniGameCombatBean miniGameCombat = (MiniGameCombatBean)MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Combat);
        //设置战斗地点
        miniGameCombat.miniGamePosition = infiniteTowersManager.GetCombatPostionByLayer(infiniteTowersData.layer);
        //获取战斗成员数据
        //获取友方数据
        List<CharacterBean> listUserData = new List<CharacterBean>();
        int totalLucky = 0;
        foreach (string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean itemCharacterData = gameDataManager.gameData.GetCharacterDataById(memberId);
            if (itemCharacterData != null)
            {
                listUserData.Add(itemCharacterData);
                itemCharacterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
                totalLucky += characterAttributes.lucky;
            }
        }

        //如果没有就启动测试数据
        if (listUserData.Count == 0)
        {
            CharacterBean itemCharacterData = npcInfoManager.GetCharacterDataById(10001);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
        }

        //获取敌方数据
        List<CharacterBean> listEnemyData = infiniteTowersManager.GetCharacterDataByInfiniteTowersLayer(npcTeamManager,npcInfoManager, characterBodyManager, infiniteTowersData.layer);
        //设置敌方能力
        foreach (CharacterBean itemEnemyData in listEnemyData)
        {
            CharacterAttributesBean enemyAttributes = infiniteTowersManager.GetEnemyAttributesByLayer(itemEnemyData,infiniteTowersData.layer);
            if (enemyAttributes != null)
                itemEnemyData.attributes.InitAttributes(enemyAttributes);
        }

        //初始化战斗数据
        miniGameCombat.gameReason = MiniGameReasonEnum.Fight;
        miniGameCombat.winSurvivalNumber = 1;
        miniGameCombat.winBringDownNumber = listEnemyData.Count;
        miniGameCombat.InitData(gameItemsManager, listUserData, listEnemyData);

        //添加奖励装备
        List<RewardTypeBean> listRewardEquip = RewardTypeEnumTools.GetRewardEnemyEquipForInfiniteTowers(listEnemyData, infiniteTowersData.layer, totalLucky);
        if(!CheckUtil.ListIsNull(listRewardEquip))
            miniGameCombat.listReward.AddRange(listRewardEquip);
        //添加奖励物品
        List<RewardTypeBean> listRewardItems = RewardTypeEnumTools.GetRewardItemsForInfiniteTowers(listEnemyData, infiniteTowersData.layer, totalLucky);
        if (!CheckUtil.ListIsNull(listRewardItems))
            miniGameCombat.listReward.AddRange(listRewardItems);
        return miniGameCombat;
    }


    public void StartCombat(MiniGameCombatBean combatData, UserInfiniteTowersBean infiniteTowersData)
    {
        combatHandler.InitGame(combatData);

        UIMiniGameCountDown uiCountDown = (UIMiniGameCountDown)uiGameManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.MiniGameCountDown));
        //设置标题
        uiCountDown.SetTitle(infiniteTowersData.layer + GameCommonInfo.GetUITextById(83));
    }

    public void NextLayer(UserInfiniteTowersBean infiniteTowersData)
    {
        //清理一下系统
        SystemUtil.GCCollect();

        this.infiniteTowersData = infiniteTowersData;
        //获取战斗数据
        MiniGameCombatBean gameCombatData = InitCombat(infiniteTowersData);
        //开始战斗
        StartCombat(gameCombatData, infiniteTowersData);
    }

    protected void GameEndHandle()
    {
        MiniGameCombatBean miniGameCombatData = combatHandler.miniGameData;
        if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Win)
        {
            //战斗胜利
            //层数+1
            infiniteTowersData.layer += 1;
            //记录
            UserAchievementBean userAchievement = gameDataManager.gameData.userAchievement;
            userAchievement.SetMaxInfiniteTowersLayer(infiniteTowersData.layer);
            //开始下一层
            NextLayer(infiniteTowersData);
        }
        else if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Lose)
        {
            //战斗失败
            //删除记录
            gameDataManager.gameData.RemoveInfiniteTowersData(infiniteTowersData);
            //跳转场景
            SceneUtil.SceneChange(ScenesEnum.GameMountainScene);
        }
        else if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Escape)
        {
            //战斗逃跑
            //跳转场景
            SceneUtil.SceneChange(ScenesEnum.GameMountainScene);
        }
    }

    #region 结果回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {

        if (observable as MiniGameCombatHandler)
        {
            switch ((MiniGameStatusEnum)type)
            {
                case MiniGameStatusEnum.Gameing:
                    break;
                case MiniGameStatusEnum.GameEnd:
                    break;
                case MiniGameStatusEnum.GameClose:
                    GameEndHandle();
                    break;
            }
        }
    }
    #endregion
}