using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MiniGameBirthHandler : BaseMiniGameHandler<MiniGameBirthBuilder, MiniGameBirthBean>
{

    public List<MiniGameBirthSpermBean> listSperm = new List<MiniGameBirthSpermBean>();

    protected override void Awake()
    {
        builderName = "MiniGameBirthBuilder";
        base.Awake();
    }

    public override void InitGame(MiniGameBirthBean miniGameData)
    {
        listSperm.Clear();
        base.InitGame(miniGameData);
        //打开倒计时UI
        OpenCountDownUI(miniGameData, false);
    }

    public override void StartGame()
    {
        base.StartGame();
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void EndGame(MiniGameResultEnum gameResult,bool isSlow)
    {
        //每日限制减少
        GameCommonInfo.DailyLimitData.numberForBirth--;
        listSperm.Clear();
        base.EndGame(gameResult, isSlow);
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        StartGame();
    }

    /// <summary>
    /// 到达卵子
    /// </summary>
    public void ArriveEgg(MiniGameBirthSpermBean spermData)
    {
        //获取家族数据
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
        //增加怀孕进度
        familyData.addBirthPro(miniGameData.addBirthPro);
        DestroySperm(spermData);
        //刷新UI
        BaseUIComponent ui = UIHandler.Instance.manager.GetOpenUI();
        ui.RefreshUI();
    }

    /// <summary>
    /// 删除精子
    /// </summary>
    /// <param name="spermData"></param>
    public void DestroySperm(MiniGameBirthSpermBean spermData)
    {
        if (listSperm.Contains(spermData))
        {
            listSperm.Remove(spermData);
        }
        CheckGameOver();
    }

    /// <summary>
    /// 发射精子
    /// </summary>
    public bool FireSperm(out MiniGameBirthSpermBean spermData)
    {
        CheckGameOver();
        spermData = null;
        //没有次数了
        if (miniGameData.fireNumber <= 0)
            return false;
        spermData = new MiniGameBirthSpermBean
        {
            timeForSpeed = miniGameData.playSpeed,
        };
        listSperm.Add(spermData);
        //减少次数
        miniGameData.fireNumber--;
        return true;
    }

    /// <summary>
    /// 检测游戏是否结束
    /// </summary>
    /// <returns></returns>
    public bool CheckGameOver()
    {
        if (miniGameData.fireNumber <= 0 && listSperm.Count <= 0)
        {
            EndGame(MiniGameResultEnum.Win,false);
            return true;
        }
        return false;
    }
}