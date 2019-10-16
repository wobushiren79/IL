using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseMiniGameHandler : BaseHandler, UIMiniGameCountDown.ICallBack
{
    public enum MiniGameStatusEnum
    {
        GamePre = 0,//游戏准备中
        Gameing = 1,//游戏进行中
        GameEnd = 2,//游戏结束
        GameClose = 3,//游戏关闭
    }

    //UI管理
    public UIGameManager uiGameManager;
    //控制器
    public ControlHandler controlHandler;

    //迷你游戏状态
    private MiniGameStatusEnum mMiniGameStatus = MiniGameStatusEnum.GamePre;

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="status"></param>
    public void SetMiniGameStatus(MiniGameStatusEnum status)
    {
        mMiniGameStatus = status;
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public MiniGameStatusEnum GetMiniGameStatus()
    {
        return mMiniGameStatus;
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