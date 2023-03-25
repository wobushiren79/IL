using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System;
using UnityEngine.UI;
public class UITownBank : UIBaseOne, IRadioGroupCallBack
{
    public UITownBankChangeMoney uITownBankChange;
    public UITownBankLoans uITownBankLoans;

    public RadioGroupView rgType;
    public Text tvLoansNumber;

    protected string loansStr;
    public override void Start()
    {
        base.Start();
        rgType.SetCallBack(this);
        loansStr = TextHandler.Instance.manager.GetTextById(191);
    }

    public override void Update()
    {
        base.Update();
        if (tvLoansNumber != null)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            tvLoansNumber.text = loansStr + "\n" + "(" + gameData.listLoans.Count + "/" + gameData.loansNumberLimit + ")";
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);
        InitDataByType(StoreForBankTypeEnum.Change);
    }

    private void InitDataByType(StoreForBankTypeEnum type)
    {
        uITownBankLoans.CloseUI();
        uITownBankChange.CloseUI();
        switch (type)
        {
            case StoreForBankTypeEnum.Change:
                uITownBankChange.OpenUI();
                break;
            case StoreForBankTypeEnum.Loans:
                uITownBankLoans.OpenUI();
                break;
        }
    }

    #region 回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForBankTypeEnum type = rbview.name.GetEnum<StoreForBankTypeEnum>();
        InitDataByType(type);
    }



    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion 
}