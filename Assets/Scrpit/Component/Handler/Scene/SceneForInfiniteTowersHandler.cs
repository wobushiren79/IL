using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneForInfiniteTowersHandler : SceneBaseHandler
{
    protected UserInfiniteTowersBean infiniteTowersData;

    public MiniGameCombatBean InitCombat(UserInfiniteTowersBean infiniteTowersData)
    {
        SceneInfiniteTowersManager infiniteTowersManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInfiniteTowersManager>();
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        foreach (string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean itemCharacterData = gameData.GetCharacterDataById(memberId);
            if (itemCharacterData != null)
            {
                listUserData.Add(itemCharacterData);
                itemCharacterData.GetAttributes( out CharacterAttributesBean characterAttributes);
                totalLucky += characterAttributes.lucky;
            }
        }

        //如果没有就启动测试数据
        if (listUserData.Count == 0)
        {
            CharacterBean itemCharacterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(10001);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
        }

        //获取敌方数据
        List<CharacterBean> listEnemyData = infiniteTowersManager.GetCharacterDataByInfiniteTowersLayer(infiniteTowersData.layer);
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
        miniGameCombat.InitData(listUserData, listEnemyData);

        //添加奖励装备
        List<RewardTypeBean> listRewardEquip = RewardTypeEnumTools.GetRewardEnemyEquipForInfiniteTowers(listEnemyData, infiniteTowersData.layer, totalLucky);
        if(!CheckUtil.ListIsNull(listRewardEquip))
            miniGameCombat.listReward.AddRange(listRewardEquip);
        //添加奖励物品
        List<RewardTypeBean> listRewardItems = RewardTypeEnumTools.GetRewardItemsForInfiniteTowers(listEnemyData, infiniteTowersData.layer, totalLucky,false);
        if (!CheckUtil.ListIsNull(listRewardItems))
            miniGameCombat.listReward.AddRange(listRewardItems);
        return miniGameCombat;
    }


    public void StartCombat(MiniGameCombatBean combatData, UserInfiniteTowersBean infiniteTowersData)
    {
        MiniGameHandler.Instance.handlerForCombat.InitGame(combatData);

        UIMiniGameCountDown uiCountDown = UIHandler.Instance.manager.GetUI<UIMiniGameCountDown>(UIEnum.MiniGameCountDown);
        //设置标题
        uiCountDown.SetTitle(infiniteTowersData.layer + TextHandler.Instance.manager.GetTextById(83));
    }

    public IEnumerator NextLayer(UserInfiniteTowersBean infiniteTowersData)
    {
        GameTimeHandler.Instance.SetTimeStop();
        //延迟开始。
        yield return new WaitForSeconds(0.1f);
        //清理一下系统
        SystemUtil.GCCollect();

        this.infiniteTowersData = infiniteTowersData;
        //添加通知
        MiniGameHandler.Instance.handlerForCombat.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        //获取战斗数据
        MiniGameCombatBean gameCombatData = InitCombat(infiniteTowersData);
        //开始战斗
        StartCombat(gameCombatData, infiniteTowersData);
    }

    protected void GameEndHandle()
    {
        MiniGameCombatBean miniGameCombatData = MiniGameHandler.Instance.handlerForCombat.miniGameData;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Win)
        {
            //战斗胜利
            //层数+1
            infiniteTowersData.layer += 1;
            //记录 
            UserAchievementBean userAchievement = gameData.userAchievement;
            userAchievement.SetMaxInfiniteTowersLayer(infiniteTowersData.layer);
            //开始下一层
            StartCoroutine(NextLayer(infiniteTowersData));
        }
        else if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Lose)
        {
            //战斗失败
            //删除记录
            gameData.RemoveInfiniteTowersData(infiniteTowersData);
            //跳转场景
            GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameMountainScene);
        }
        else if (miniGameCombatData.GetGameResult() == MiniGameResultEnum.Escape)
        {
            //战斗逃跑
            //跳转场景
            GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameMountainScene);
        }
    }

    #region 结果回调
    public void NotifyForMiniGameStatus(MiniGameStatusEnum type, params object[] obj)
    {
        switch (type)
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
    #endregion
}