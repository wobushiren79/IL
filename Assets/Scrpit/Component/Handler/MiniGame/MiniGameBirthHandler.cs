using UnityEditor;
using UnityEngine;

public class MiniGameBirthHandler : BaseMiniGameHandler<MiniGameBirthBuilder, MiniGameBirthBean>
{
    public override void InitGame(MiniGameBirthBean miniGameData)
    {
        base.InitGame(miniGameData);
        //打开倒计时UI
        // OpenCountDownUI(miniGameData);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void StartGame()
    {
        base.StartGame();
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void EndGame(MiniGameResultEnum gameResult)
    {
        base.EndGame(gameResult);
    }

    /// <summary>
    /// 到达卵子
    /// </summary>
    public void ArriveEgg()
    {

    }

    /// <summary>
    /// 发射精子
    /// </summary>
    public bool FireSperm(out MiniGameBirthSpermBean spermData)
    {
        spermData = new MiniGameBirthSpermBean();
        spermData.timeForSpeed = 10;
        return true;
    }
}