using UnityEngine;
using System.Collections;

public class SceneGameInfiniteTowersInit : BaseNormalSceneInit
{
    protected SceneForInfiniteTowersHandler infiniteTowersHandler;
    protected SceneInfiniteTowersManager infiniteTowersManager;

    protected UserInfiniteTowersBean infiniteTowersData;
    public override void Awake()
    {
        base.Awake();
        infiniteTowersHandler = Find<SceneForInfiniteTowersHandler>(ImportantTypeEnum.SceneHandler);
        infiniteTowersManager = Find<SceneInfiniteTowersManager>(ImportantTypeEnum.SceneManager);
    }

    public override void Start()
    {
        base.Start();
        infiniteTowersData = GameCommonInfo.InfiniteTowersData;

        //测试
        if (infiniteTowersData == null)
        {
            infiniteTowersData = new UserInfiniteTowersBean();
            infiniteTowersData.layer = 1;
            infiniteTowersData.listMembers.Add("");
        }

        //获取战斗数据
        MiniGameCombatBean gameCombatData =  infiniteTowersHandler.InitCombat(infiniteTowersData);
        //开始战斗
        infiniteTowersHandler.StartCombat(gameCombatData);
    }



}