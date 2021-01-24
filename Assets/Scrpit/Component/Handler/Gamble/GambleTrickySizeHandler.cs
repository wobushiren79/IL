using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GambleTrickySizeHandler : BaseGambleHandler<GambleTrickySizeBean, GambleTrickySizeBuilder>, GambleTrickySizeItem.ICallBack
{
    protected UIGambleTrickySize gambleUI;

    public override void InitGame(GambleTrickySizeBean gambleData)
    {
        base.InitGame(gambleData);
        gambleUI = UIHandler.Instance.manager.GetUI<UIGambleTrickySize>((UIEnum.GambleTrickySize));
        //初始化
        gambleBuilder.InitCup();
    }

    /// <summary>
    /// 开始改变
    /// </summary>
    public override void StartChange()
    {
        base.StartChange();
        StartCoroutine(CoroutineForStartChangeSize());
    }

    /// <summary>
    /// 开始竞猜
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
        GambleTrickySizeItem cup = gambleBuilder.GetCup();
        cup.SetCallBack(this);
        cup.SetStatus(GambleTrickySizeItem.CupStatusEnum.Choosing);
    }

    /// <summary>
    /// 开始结算
    /// </summary>
    public override void StartSettlement()
    {
        base.StartSettlement();
        //展示结果
        GambleTrickySizeItem cup = gambleBuilder.GetCup();
        cup.SetStatus(GambleTrickySizeItem.CupStatusEnum.Result);
        StartCoroutine(CoroutineForSettlement());
    }

    /// <summary>
    /// 协程-开始改变杯子
    /// </summary>
    public IEnumerator CoroutineForStartChangeSize()
    {
        GambleTrickySizeItem cup = gambleBuilder.GetCup();
        cup.SetStatus(GambleTrickySizeItem.CupStatusEnum.Changing);
        yield return new WaitForSeconds(2.6f);
        StartGame();
    }

    /// <summary>
    /// 协程-结算
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForSettlement()
    {
        //延迟半秒结算 结果展示有动画所以需要延迟
        yield return new WaitForSeconds(0.6f);

        if (gambleData.isWin)
        {
            //展示胜利动画
            gambleUI.AnimForWinMoney();
        }
        else
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Passive);
            EndGame();
        }
    }

    #region 竞猜回调
    public void SizeChoose(int size)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GambleTrickySizeItem cup = gambleBuilder.GetCup();
        float winRate =  Random.Range(0f, 1f); 
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //幸运加成
        gameData.userCharacter.GetAttributes( out CharacterAttributesBean characterAttributes);
        float addWinRate = 0.0025f * characterAttributes.lucky;

        if (winRate <= (gambleData.winRate + addWinRate))
        {
            gambleData.SetIsWin(true);
            if (size == 0)
            {
                int number = Random.Range(1, 4);
                cup.SetResult(number);
            }
            else
            {
                int number = Random.Range(4, 7);
                cup.SetResult(number);
            }
        }
        else
        {
            gambleData.SetIsWin(false);
            if (size == 0)
            {
                int number = Random.Range(4, 7);
                cup.SetResult(number);
            }
            else
            {
                int number = Random.Range(1, 4);
                cup.SetResult(number);
            }
        }
        StartSettlement();
    }
    #endregion
}