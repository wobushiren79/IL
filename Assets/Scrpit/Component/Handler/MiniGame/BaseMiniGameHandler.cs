using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseMiniGameHandler : BaseHandler, UIMiniGameCountDown.ICallBack
{    
    //UI管理
    public UIGameManager uiGameManager;
    //控制器
    public ControlHandler controlHandler;

    public enum NotifyMiniGameEnum
    {
        GameStart = 1,
        GameEnd = 2,
        GameClose = 3,
    }

    /// <summary>
    /// 打开倒计时UI
    /// </summary>
    /// <param name="miniGameData"></param>
    public void OpenCountDownUI(MiniGameBaseBean miniGameData)
    {
        //打开游戏准备倒计时UI
        UIMiniGameCountDown uiCountDown = (UIMiniGameCountDown)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCountDown));
        uiCountDown.SetCallBack(this);
        //设置胜利条件
        List<string> listWinConditions = miniGameData.GetListWinConditions();
        string targetTitleStr = "???";
        switch (miniGameData.gameType)
        {
            case MiniGameEnum.Barrage:
                targetTitleStr = GameCommonInfo.GetUITextById(202);
                break;
            case MiniGameEnum.Combat:
                targetTitleStr = GameCommonInfo.GetUITextById(205);
                break;
        }
        //设置准备UI的数据
        uiCountDown.SetData(targetTitleStr, listWinConditions);
    }


    #region 倒计时UI回调
    public virtual void GamePreCountDownStart()
    {

    }

    public virtual void GamePreCountDownEnd()
    {

    }
    #endregion
}