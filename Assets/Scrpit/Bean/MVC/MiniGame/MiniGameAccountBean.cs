using UnityEngine;
using UnityEditor;

public class MiniGameAccountBean : MiniGameBaseBean
{
    //选手位置
    public Vector3 playerPosition;
    //摄像头位置
    public Vector3 cameraPosition;
    //金钱生成位置
    public Transform tfMoneyPosition;

    //当前金钱
    public int currentMoneyL;
    public int currentMoneyM;
    public int currentMoneyS;
    //当前时间
    public float currentTime;

    public MiniGameAccountBean()
    {
        gameType = MiniGameEnum.Account;
    }

    public override void InitData(GameItemsManager gameItemsManager, CharacterBean userData)
    {
        base.InitData(gameItemsManager, userData);
        userData.GetAttributes(gameItemsManager,out CharacterAttributesBean attributesData);
        //设置游戏时间
        winSurvivalTime = attributesData.account + winSurvivalTime;
        if (winSurvivalTime < 30)
        {
            winSurvivalTime = 30;
        }
        //当前时间
        currentTime = winSurvivalTime;
    }
}