using UnityEngine;
using UnityEditor;
using System.Collections;

public class BaseGambleHandler<T,B> : BaseHandler 
    where T: GambleBaseBean 
    where B: BaseGambleBuilder
{
    public T gambleData;

    protected B gambleBuilder;
    private void Awake()
    {
        tag = TagInfo.Tag_Gamble;
        gambleBuilder = gameObject.AddComponent<B>();
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public virtual void InitGame(T gambleData)
    {
        this.gambleData = gambleData;
        this.gambleData.SetGambleStatus(GambleStatusType.Prepare);
    }

    /// <summary>
    /// 开始改变
    /// </summary>
    public virtual void StartChange()
    {
        this.gambleData.SetGambleStatus(GambleStatusType.Changing);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public virtual void StartGame()
    {
        this.gambleData.SetGambleStatus(GambleStatusType.Gambling);
    }

    /// <summary>
    /// 开始结算
    /// </summary>
    public virtual void StartSettlement()
    {
        this.gambleData.SetGambleStatus(GambleStatusType.Settlementing);
    }

    /// <summary>
    /// 结束游戏游戏
    /// </summary>
    public virtual void EndGame()
    {
        this.gambleData.SetGambleStatus(GambleStatusType.End);
        ResetGame();
    }


    /// <summary>
    /// 重置游戏
    /// </summary>
    public virtual void ResetGame()
    {
        gambleData.ResetData();
        InitGame(gambleData);
    }


    /// <summary>
    /// 下注
    /// </summary>
    public virtual void BetMoney(int moneyL, int moneyM, int moneyS)
    {
        //检测是否有足够金钱
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (!gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        //检测是否超过最大下注金额
        if (gambleData.betForMoneyS + moneyS > gambleData.betMaxForMoneyS)
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1302));
            return;
        }
        gameData.PayMoney(moneyL, moneyM, moneyS);

        gambleData.betForMoneyL += moneyL;
        gambleData.betForMoneyM += moneyM;
        gambleData.betForMoneyS += moneyS;
    }


}