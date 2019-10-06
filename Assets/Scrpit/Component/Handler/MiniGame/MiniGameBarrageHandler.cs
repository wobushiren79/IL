using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MiniGameBarrageHandler : BaseMiniGameHandler, UIMiniGameCountDown.ICallBack, UIMiniGameEnd.ICallBack
{
    //UI管理
    public UIGameManager uiGameManager;
    //弹幕游戏生成器
    public MiniGameBarrageBuilder miniGameBarrageBuilder;
    //弹幕游戏数据
    public MiniGameBarrageBean gameBarrageData;
    //发射器位置
    public List<Vector3> listEjectorPosition = new List<Vector3>();

    //游戏是否正在游玩
    public bool isGamePlay = false;
    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="arenaPrepareData"></param>
    public void InitGame(MiniGameBarrageBean gameBarrageData)
    {
        if (gameBarrageData == null)
        {
            LogUtil.Log("弹幕游戏数据为NULL，无法初始化弹幕游戏");
            return;
        }
        if (CheckUtil.ListIsNull(gameBarrageData.listEjectorPosition))
        {
            LogUtil.Log("发射台坐标为NULL，无法初始化弹幕游戏");
            return;
        }
        this.gameBarrageData = gameBarrageData;
        //创建所有玩家
        miniGameBarrageBuilder.CreateAllPlayer(gameBarrageData.userGameData, gameBarrageData.listEnemyGameData);
        //创建发射器
        for (int i = 0; i < gameBarrageData.listEjectorPosition.Count;i++)
            miniGameBarrageBuilder.CreateEjector(gameBarrageData.listEjectorPosition[i]);
        //打开游戏准备倒计时UI
        UIMiniGameCountDown uiCountDown = (UIMiniGameCountDown)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCountDown));
        uiCountDown.SetCallBack(this);
        //设置标题
        string targetTitleStr = GameCommonInfo.GetUITextById(202);
        //设置胜利条件
        List<string> listWinConditions = gameBarrageData.GetListWinConditions();
        //设置准备UI的数据
        uiCountDown.SetData(targetTitleStr, listWinConditions);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (gameBarrageData == null)
        {
            LogUtil.Log("弹幕游戏数据为NULL，无法开始弹幕游戏");
            return;
        }
        isGamePlay = true;
        //开始倒计时
        StartCoroutine(StartCountDown(gameBarrageData.winSurvivalTime));
        //开始射击
        StartCoroutine(StartLaunch());
        //通知 游戏开始
        NotifyAllObserver((int)NotifyMiniGameEnum.GameStart);
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="isWinGame">是否赢得游戏</param>
    public void EndGame(bool isWinGame)
    {
        //如果正在游玩 则结束游戏
        if (isGamePlay)
        {
            isGamePlay = false;
            StopAllCoroutines();
            miniGameBarrageBuilder.DestoryPlayer();
            miniGameBarrageBuilder.DestoryEjector();
            //打开游戏结束UI
            UIMiniGameEnd uiMiniGameEnd = (UIMiniGameEnd)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameEnd));
            uiMiniGameEnd.SetData(isWinGame);
            uiMiniGameEnd.SetCallBack(this);
            //通知 游戏结束
            NotifyAllObserver((int)NotifyMiniGameEnum.GameEnd);
        }
    }

    /// <summary>
    /// 开始射击目标
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartLaunch()
    {
        while (true)
        {
            //获取所有发射器
            List<BarrageEjectorCpt> listEjector = miniGameBarrageBuilder.GetEjector();
            if (!CheckUtil.ListIsNull(listEjector))
            {
                foreach (BarrageEjectorCpt itemEjector in listEjector)
                {
                    //获取所有的目标
                    List<NpcAIMiniGameBarrageCpt> listPlayer = miniGameBarrageBuilder.GetPlayerList();
                    //随机获取一个NPC
                    NpcAIMiniGameBarrageCpt npcCpt = RandomUtil.GetRandomDataByList(listPlayer);
                    Vector3 launchTarget = Vector3.zero;
                    if (npcCpt != null)
                        launchTarget = npcCpt.transform.position;
                    //获取发射类型
                    if (CheckUtil.ArrayIsNull(gameBarrageData.launchTypes))
                    {
                        gameBarrageData.launchTypes = new BarrageEjectorCpt.LaunchTypeEnum[]
                        {
                             BarrageEjectorCpt.LaunchTypeEnum.Single
                        };
                    }
                    BarrageEjectorCpt.LaunchTypeEnum launchType = RandomUtil.GetRandomDataByArray(gameBarrageData.launchTypes);


                    itemEjector.StartLaunch(launchType, launchTarget, gameBarrageData.launchSpeed);
                }
            }
            //发射间隔时间
            float launchIntervalTime = gameBarrageData.launchInterval;
            if (launchIntervalTime < 0.1f)
                launchIntervalTime = 0.1f;
            yield return new WaitForSeconds(launchIntervalTime);
        }
    }

    /// <summary>
    /// 开始倒计时
    /// </summary>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public IEnumerator StartCountDown(float totalTime)
    {
        UIMiniGameBarrage uiMiniGame = (UIMiniGameBarrage)uiGameManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.MiniGameBarrage));
        while (true)
        {
            //设置游戏UI时间
            uiMiniGame.SetTime(gameBarrageData.currentTime);
            yield return new WaitForSeconds(1);
            gameBarrageData.currentTime--;
            if (gameBarrageData.currentTime <= 0)
                break;
        }
        EndGame(true);
    }

    #region 倒计时回调
    public void GamePreCountDownEnd()
    {
        //打开弹幕游戏UI
        UIMiniGameBarrage uiMiniGameBarrage = (UIMiniGameBarrage)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameBarrage));
        uiMiniGameBarrage.SetData(gameBarrageData);
        //开始游戏
        StartGame();
    }
    #endregion

    #region 游戏结束按钮回调
    public void OnClickClose()
    {
        //通知 关闭游戏
        NotifyAllObserver((int)NotifyMiniGameEnum.GameClose);
    }
    #endregion

}