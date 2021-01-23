using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
public class GambleTrickyCupHandler : BaseGambleHandler<GambleTrickyCupBean , GambleTrickyCupBuilder>,GambleTrickyCupItem.ICallBack
{
    protected UIGambleTrickyCup gambleUI;

    public override void InitGame(GambleTrickyCupBean gambleData)
    {
        base.InitGame(gambleData);
        gambleUI  =  UIHandler.Instance.manager.GetUI<UIGambleTrickyCup>(UIEnum.GambleTrickyCup);
        //初始化所有杯子
        gambleBuilder.InitAllCup();
    }

    public override void StartChange()
    {
        base.StartChange();
        StartCoroutine(CoroutineForStartChangeCup());
    }

    /// <summary>
    /// 开始竞猜
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
        List<GambleTrickyCupItem> listCup = gambleBuilder.GetAllCup();
        foreach (GambleTrickyCupItem itemCup in listCup)
        {
            itemCup.SetCallBack(this);
            itemCup.SetStatus(GambleTrickyCupItem.CupStatusEnum.Choosing);
        }
    }

    /// <summary>
    /// 开始结算
    /// </summary>
    public override void StartSettlement()
    {
        base.StartSettlement();
        //展示结果
        List<GambleTrickyCupItem> listCup = gambleBuilder.GetAllCup();
        foreach (GambleTrickyCupItem itemCup in listCup)
        {
            itemCup.SetStatus(GambleTrickyCupItem.CupStatusEnum.Result);
        }
        StartCoroutine(CoroutineForSettlement());
    }

    /// <summary>
    /// 协程-开始改变杯子
    /// </summary>
    public IEnumerator CoroutineForStartChangeCup()
    {
        List<GambleTrickyCupItem> listCup =  gambleBuilder.GetAllCup();
        foreach ( GambleTrickyCupItem itemCup in listCup)
        {
            itemCup.SetStatus(GambleTrickyCupItem.CupStatusEnum.Changing);
        }
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < gambleData.changeNumber; i++)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.GetCard);
            List<GambleTrickyCupItem> listChangeCup = RandomUtil.GetRandomDataByListForNumberNR(listCup, 2);
            RectTransform trf1 = ((RectTransform)listChangeCup[0].transform);
            RectTransform trf2 = ((RectTransform)listChangeCup[1].transform);
            Vector2 position1 = trf1.anchoredPosition;
            Vector2 position2 = trf2.anchoredPosition;
            trf1.DOAnchorPos(position2, gambleData.changeIntervalTime);
            trf2.DOAnchorPos(position1, gambleData.changeIntervalTime);
            yield return new WaitForSeconds(gambleData.changeIntervalTime);
        }
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

    #region 杯子选择回调
    public void CupChoose(GambleTrickyCupItem chooseCup)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //设置输赢
        if (chooseCup.CheckHasDice())
        {
            gambleData.SetIsWin(true);
        }
        else
        {
            gambleData.SetIsWin(false);
        }
        StartSettlement();
    }
    #endregion
}