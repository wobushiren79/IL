using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneForInfiniteTowersHandler : BaseHandler
{
    protected MiniGameCombatHandler combatHandler;
    protected SceneInfiniteTowersManager infiniteTowersManager;

    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected NpcInfoManager npcInfoManager;

    public void Awake()
    {
        combatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        infiniteTowersManager = Find<SceneInfiniteTowersManager>(ImportantTypeEnum.SceneManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }


    public MiniGameCombatBean InitCombat(UserInfiniteTowersBean infiniteTowersData)
    {
        //设置标牌
        infiniteTowersManager.SetSignForLayer(infiniteTowersData.layer);
        //设置战斗数据
        MiniGameCombatBean miniGameCombat = (MiniGameCombatBean)MiniGameEnumTools.GetMiniGameData( MiniGameEnum.Combat);
        //设置战斗地点
        miniGameCombat.miniGamePosition = GetCombatPostionByLayer(infiniteTowersData.layer);
        //获取战斗成员数据
        //获取友方数据
        List<CharacterBean> listUserData = new List<CharacterBean>();
        foreach (string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean itemCharacterData = gameDataManager.gameData.GetCharacterDataById(memberId);
            if (itemCharacterData != null)
            {
                listUserData.Add(itemCharacterData);
            }
        }
        //如果没有就启动测试数据
        if (listUserData.Count == 0)
        {
            CharacterBean itemCharacterData=  npcInfoManager.GetCharacterDataById(10001);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
            listUserData.Add(itemCharacterData);
        }

        //获取敌方数据
        List<CharacterBean> listEnemyData = npcInfoManager.GetCharacterDataByInfiniteTowersLayer(infiniteTowersData.layer);
        //初始化战斗数据
        miniGameCombat.gameReason = MiniGameReasonEnum.Fight;
        miniGameCombat.winSurvivalNumber = 1;
        miniGameCombat.winBringDownNumber = listEnemyData.Count;
        miniGameCombat.InitData(gameItemsManager, listUserData, listEnemyData);
        return miniGameCombat;
    }

    /// <summary>
    /// 根据层数获取战斗地点
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Vector3 GetCombatPostionByLayer(long layer)
    {
        if (layer % 10 == 0)
        {
            return infiniteTowersManager.GetBossCombatPosition();
        }
        else
        {
           return  infiniteTowersManager.GetNormalCombatPosition();
        }
    }

    public void StartCombat(MiniGameCombatBean combatData)
    {
        combatHandler.InitGame(combatData);
    }
}