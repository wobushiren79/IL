using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MiniGameBarrageHandler : BaseMiniGameHandler<MiniGameBarrageBuilder, MiniGameBarrageBean>
{
    public override void Awake()
    {
        builderName = "MiniGameBarrageBuilder";
        base.Awake();
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="arenaPrepareData"></param>
    public override void InitGame(MiniGameBarrageBean gameBarrageData)
    {
        base.InitGame(gameBarrageData);
        if (gameBarrageData == null)
        {
            LogUtil.Log("弹幕游戏数据为NULL，无法初始化弹幕游戏");
            return;
        }
        if (gameBarrageData.listEjectorPosition.IsNull())
        {
            LogUtil.Log("发射台坐标为NULL，无法初始化弹幕游戏");
            return;
        }
        //创建所有玩家
        miniGameBuilder.CreateAllCharacter(gameBarrageData.listUserGameData, gameBarrageData.userStartPosition);
        //创建发射器
        for (int i = 0; i < gameBarrageData.listEjectorPosition.Count; i++)
            miniGameBuilder.CreateEjector(gameBarrageData.listEjectorPosition[i]);
        //打开游戏准备倒计时UI
        OpenCountDownUI(gameBarrageData);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
        if (miniGameData == null)
        {
            LogUtil.Log("弹幕游戏数据为NULL，无法开始弹幕游戏");
            return;
        }
        //开始倒计时
        StartCoroutine(StartCountDown(miniGameData.winSurvivalTime));
        //开始射击
        StartCoroutine(StartLaunch());
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="isWinGame">是否赢得游戏</param>
    public override void EndGame(MiniGameResultEnum miniGameResult)
    {
        base.EndGame(miniGameResult);
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
            List<MiniGameBarrageEjectorCpt> listEjector = miniGameBuilder.GetEjector();
            if (!listEjector.IsNull())
            {
                foreach (MiniGameBarrageEjectorCpt itemEjector in listEjector)
                {
                    //获取所有的目标
                    List<NpcAIMiniGameBarrageCpt> listPlayer = miniGameBuilder.GetPlayerList();
                    //随机获取一个NPC
                    NpcAIMiniGameBarrageCpt npcCpt = RandomUtil.GetRandomDataByList(listPlayer);
                    Vector3 launchTarget = Vector3.zero;
                    if (npcCpt != null)
                        launchTarget = npcCpt.transform.position;
                    //获取发射类型
                    if (miniGameData.launchTypes.IsNull())
                    {
                        miniGameData.launchTypes = new MiniGameBarrageEjectorCpt.LaunchTypeEnum[]
                        {
                             MiniGameBarrageEjectorCpt.LaunchTypeEnum.Single
                        };
                    }
                    MiniGameBarrageEjectorCpt.LaunchTypeEnum launchType = RandomUtil.GetRandomDataByArray(miniGameData.launchTypes);

                    itemEjector.StartLaunch(launchType,miniGameData.bulletType,launchTarget, miniGameData.launchSpeed);
                }
            }
            //发射间隔时间
            float launchIntervalTime = miniGameData.launchInterval;
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
        UIMiniGameBarrage uiMiniGame = UIHandler.Instance.GetUI<UIMiniGameBarrage>();
        while (true)
        {
            //设置游戏UI时间
            uiMiniGame.SetTime(miniGameData.currentTime);
            yield return new WaitForSeconds(1);
            miniGameData.currentTime--;
            if (miniGameData.currentTime <= 0)
                break;
        }
        EndGame(MiniGameResultEnum.Win);
    }

    #region 倒计时回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
        GameControlHandler.Instance.StartControl<ControlForMiniGameBarrageCpt>(GameControlHandler.ControlEnum.MiniGameBarrage);
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //打开弹幕游戏UI
        UIMiniGameBarrage uiMiniGameBarrage = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameBarrage>();
        uiMiniGameBarrage.SetData(miniGameData);
        //开始游戏
        StartGame();
    }
    #endregion
}