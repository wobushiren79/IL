using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIBaseOne : BaseUIComponent ,DialogView.IDialogCallBack
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
        if (uiGameManager.gameDataManager != null)
        {
            if (tvMoneyL != null)
            {
                tvMoneyL.text = uiGameManager.gameData.moneyL + "";
            }
            if (tvMoneyM != null)
            {
                tvMoneyM.text = uiGameManager.gameData.moneyM + "";
            }
            if (tvMoneyS != null)
            {
                tvMoneyS.text = uiGameManager.gameData.moneyS + "";
            }
            if (tvGuildCoin != null)
            {
                tvGuildCoin.text = uiGameManager.gameData.guildCoin + "";
            }
            if (tvTrophy1 != null)
            {
                tvTrophy1.text = uiGameManager.gameData.trophyElementary+"";
            }
            if (tvTrophy2 != null)
            {
                tvTrophy2.text = uiGameManager.gameData.trophyIntermediate + "";
            }
            if (tvTrophy3 != null)
            {
                tvTrophy3.text = uiGameManager.gameData.trophyAdvanced + "";
            }
            if (tvTrophy4 != null)
            {
                tvTrophy4.text = uiGameManager.gameData.trophyLegendary + "";
            }
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
        dialogData.title = GameCommonInfo.GetUITextById(3101);
        pickForSellDialog = DialogHandler.Instance.CreateDialog<PickForSellDialogView>(DialogEnum.PickForSell, this, dialogData);
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public virtual void OnClickForBack()
    {
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
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