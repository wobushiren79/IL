using UnityEngine;
using System.Collections;

public class SceneGameInfiniteTowersInit : BaseNormalSceneInit
{
    protected SceneForInfiniteTowersHandler infiniteTowersHandler;

    protected UserInfiniteTowersBean infiniteTowersData;
    public override void Awake()
    {
        base.Awake();
        infiniteTowersHandler = Find<SceneForInfiniteTowersHandler>(ImportantTypeEnum.SceneHandler);

    }

    public override void Start()
    {
        base.Start();

        infiniteTowersData = GameCommonInfo.InfiniteTowersData;

        //测试
        if (infiniteTowersData == null)
        {
            infiniteTowersData = new UserInfiniteTowersBean();
            infiniteTowersData.layer = 10;
            infiniteTowersData.listMembers.Add("");
        }
        
        infiniteTowersHandler.NextLayer(infiniteTowersData);
    }
}