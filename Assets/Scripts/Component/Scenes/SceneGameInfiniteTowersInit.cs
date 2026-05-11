using UnityEngine;
using System.Collections;

public class SceneGameInfiniteTowersInit : BaseNormalSceneInit
{

    protected UserInfiniteTowersBean infiniteTowersData;


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
        SceneForInfiniteTowersHandler infiniteTowersHandler = GameScenesHandler.Instance.manager.GetSceneHandler<SceneForInfiniteTowersHandler>();
        infiniteTowersHandler.NextLayer(infiniteTowersData);
    }
}