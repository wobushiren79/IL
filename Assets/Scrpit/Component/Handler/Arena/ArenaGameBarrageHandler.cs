using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ArenaGameBarrageHandler : BaseHandler,UIGameCountDown.ICallBack
{
    //UI管理
    public UIGameManager uiGameManager;
    //弹幕游戏生成器
    public MiniGameBarrageBuilder miniGameBarrageBuilder;
    //弹幕游戏数据
    public ArenaPrepareBean arenaPrepareData;
    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="arenaPrepareData"></param>
    public void InitGame(ArenaPrepareBean arenaPrepareData)
    {
        if (arenaPrepareData == null)
        {
            LogUtil.Log("竞技场数据为NULL，无法初始化弹幕游戏");
            return;
        }
        this.arenaPrepareData = arenaPrepareData;
        //创建所有玩家
        miniGameBarrageBuilder.CreateAllPlayer(arenaPrepareData.gameBarrageData.userGameData,arenaPrepareData.gameBarrageData.listEnemyGameData);
        //创建发射器
        miniGameBarrageBuilder.CreateEjector();
        //打开倒计时UI
        UIGameCountDown uiCountDown =  (UIGameCountDown)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameCountDown));
        uiCountDown.SetCallBack(this);

        //设置标题
        string targetTitleStr = GameCommonInfo.GetUITextById(202);
        //设置胜利条件
        List<string> listWinConditions = arenaPrepareData.gameBarrageData.GetListWinConditions();
        uiCountDown.SetData(targetTitleStr, listWinConditions);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (arenaPrepareData==null)
        {
            LogUtil.Log("竞技场数据为NULL，无法开始弹幕游戏");
            return;
        }
        StartCoroutine(StartShoot());
    }


    public IEnumerator StartShoot()
    {
        while (true)
        {
            //获取所有发射器
            List<BarrageEjectorCpt> listEjector= miniGameBarrageBuilder.GetEjector();
            if (!CheckUtil.ListIsNull(listEjector))
            {
                foreach (BarrageEjectorCpt itemEjector in listEjector)
                {
                    //获取所有的目标
                    List<NpcAIMiniGameBarrageCpt> listPlayer = miniGameBarrageBuilder.GetPlayerList();
                    //随机获取一个NPC
                    NpcAIMiniGameBarrageCpt npcCpt= RandomUtil.GetRandomDataByList(listPlayer);
                    itemEjector.StartLaunch(BarrageEjectorCpt.LaunchTypeEnum.Single, npcCpt.transform.position, 5);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    #region 倒计时回调
    public void CountDownEnd()
    {
        //打开弹幕游戏UI
        UIMiniGameBarrage uiMiniGameBarrage = (UIMiniGameBarrage)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameBarrage));
        uiMiniGameBarrage.SetData(arenaPrepareData.gameBarrageData);
        //开始游戏
        StartGame();
    }
    #endregion
}