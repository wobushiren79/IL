﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseOne : BaseUIComponent, DialogView.IDialogCallBack
{
    //返回按钮
    public Button btBack;
    public Button btSell;
    //金钱
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;
    //公会硬币
    public Text tvGuildCoin;
    //斗技场奖杯
    public Text tvTrophy1;
    public Text tvTrophy2;
    public Text tvTrophy3;
    public Text tvTrophy4;

    public virtual void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OnClickForBack);
        if (btSell != null)
            btSell.onClick.AddListener(OnClickForSell);
    }

    public virtual void Update()
    {
        SetMoney();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameControlHandler.Instance.StopControl();
        GameTimeHandler.Instance.SetTimeStatus(true);
    }

    public override void CloseUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        if (gameObject.activeSelf)
        {
            GameControlHandler.Instance.RestoreControl();
        }
        GameTimeHandler.Instance.SetTimeRestore();
        base.CloseUI();
    }

    public void SetMoney()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (tvMoneyL != null)
        {
            tvMoneyL.text = gameData.moneyL + "";
        }
        if (tvMoneyM != null)
        {
            tvMoneyM.text = gameData.moneyM + "";
        }
        if (tvMoneyS != null)
        {
            tvMoneyS.text = gameData.moneyS + "";
        }
        if (tvGuildCoin != null)
        {
            tvGuildCoin.text = gameData.guildCoin + "";
        }
        if (tvTrophy1 != null)
        {
            tvTrophy1.text = gameData.trophyElementary + "";
        }
        if (tvTrophy2 != null)
        {
            tvTrophy2.text = gameData.trophyIntermediate + "";
        }
        if (tvTrophy3 != null)
        {
            tvTrophy3.text = gameData.trophyAdvanced + "";
        }
        if (tvTrophy4 != null)
        {
            tvTrophy4.text = gameData.trophyLegendary + "";
        }
    }

    /// <summary>
    /// 售卖按钮
    /// </summary>
    public virtual void OnClickForSell()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        CreatePickForSellDialogView(out PickForSellDialogView pickForSellDialog);
    }

    protected virtual void CreatePickForSellDialogView(out PickForSellDialogView pickForSellDialog)
    {
        DialogBean dialogData = new DialogBean();
        dialogData.title = TextHandler.Instance.manager.GetTextById(3101);
        dialogData.dialogType = DialogEnum.PickForSell;
        dialogData.callBack = this;
        pickForSellDialog = UIHandler.Instance.ShowDialog<PickForSellDialogView>(dialogData);
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public virtual void OnClickForBack()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }

    #region 选择出售回调
    public virtual void Submit(DialogView dialogView, DialogBean dialogBean)
    {

    }

    public virtual void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}