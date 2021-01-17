using UnityEngine;
using UnityEditor;
using System.Collections;

public class MiniGameAccountHandler : BaseMiniGameHandler<MiniGameAccountBuilder, MiniGameAccountBean>,
    ControlForMiniGameAccountCpt.ICallBack,
    MiniGameAccountEjectorCpt.ICallBack
{


    public override void InitGame(MiniGameAccountBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建玩家
        miniGameBuilder.CreateUserCharacter(miniGameData.listUserGameData, miniGameData.playerPosition);
        //摄像头初始化
        ControlForMiniGameAccountCpt gameControl = (ControlForMiniGameAccountCpt)controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameAccount);
        gameControl.SetCameraOrthographicSize(8);
        gameControl.SetCameraPosition(miniGameData.cameraPosition);
        gameControl.SetCallBack(this);
        //生成金钱
        miniGameBuilder.CreateMoney(miniGameData.winMoneyL, miniGameData.winMoneyM, miniGameData.winMoneyS, miniGameData.tfMoneyPosition);
        //打开倒计时UI
        OpenCountDownUI(miniGameData);
    }

    public override void StartGame()
    {
        base.StartGame();
        //发射器开始旋转
        MiniGameAccountEjectorCpt ejectorCpt = miniGameBuilder.GetEjector();
        MiniGameCharacterForAccountBean userCharacterData =(MiniGameCharacterForAccountBean) miniGameData.GetUserGameData();
        userCharacterData.characterData.GetAttributes( out CharacterAttributesBean characterAttributes);
        ejectorCpt.SetData(5 + characterAttributes.account / 20f,   1.8f + characterAttributes.account / 20f);
        ejectorCpt.SetCallBack(this);
        ejectorCpt.StartRotate();
        //打开游戏UI
        UIMiniGameAccount uiMiniGameAccount = (UIMiniGameAccount)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameAccount));
        uiMiniGameAccount.SetData(miniGameData.winSurvivalTime, miniGameData.winMoneyL, miniGameData.winMoneyM, miniGameData.winMoneyS);
        //开始倒计时
        StartCoroutine(StartCountDown());
    }

    /// <summary>
    /// 检测游戏结果
    /// </summary>
    /// <returns></returns>
    public MiniGameResultEnum CheckGameResults()
    {
        if (miniGameData.currentMoneyL >= miniGameData.winMoneyL
            && miniGameData.currentMoneyM >= miniGameData.winMoneyM
             && miniGameData.currentMoneyS >= miniGameData.winMoneyS)
        {
            return MiniGameResultEnum.Win;
        }
        return MiniGameResultEnum.Lose;
    }

    /// <summary>
    /// 开始倒计时
    /// </summary>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public IEnumerator StartCountDown()
    {
        UIMiniGameAccount uiMiniGameAccount = (UIMiniGameAccount)uiGameManager.GetOpenUI();
        while (true)
        {
            //设置游戏UI时间
            uiMiniGameAccount.SetTime(miniGameData.currentTime);
            yield return new WaitForSeconds(1);
            miniGameData.currentTime--;
            if (miniGameData.currentTime <= 0)
                break;
        }
        EndGame(CheckGameResults());
    }



    #region 倒计时回调
    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        StartGame();
    }

    #endregion

    #region 操作监听
    public void OnClickLaunch()
    {
        if (GetMiniGameStatus() == MiniGameStatusEnum.Gameing)
        {
            MiniGameAccountEjectorCpt ejectorCpt = miniGameBuilder.GetEjector();
            ejectorCpt.Launch();
        }
    }
    #endregion

    #region 发射机回调
    public void AccountEjectorSettlement(MiniGameAccountEjectorCpt ejector, int moneyL, int moneyM, int moneyS)
    {
        miniGameData.currentMoneyL += moneyL;
        miniGameData.currentMoneyM += moneyM;
        miniGameData.currentMoneyS += moneyS;

        //展示特效
        MiniGameAccountEjectorCpt ejectorCpt = miniGameBuilder.GetEjector();
        UIMiniGameAccount uiMiniGameAccount = (UIMiniGameAccount)uiGameManager.GetOpenUI();
        uiMiniGameAccount.ShowMoneyGet(ejectorCpt.transform.position, moneyL, moneyM, moneyS);


        //如果分满了直接结束游戏
        MiniGameResultEnum gameResultEnum = CheckGameResults();
        if (gameResultEnum == MiniGameResultEnum.Win)
        {
            StopAllCoroutines();
            EndGame(MiniGameResultEnum.Win);
        }  
    }
    #endregion
}