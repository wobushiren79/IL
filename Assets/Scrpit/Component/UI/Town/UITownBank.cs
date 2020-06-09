using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System;

public class UITownBank : UIBaseOne,IRadioGroupCallBack 
{
    public UITownBankChangeMoney uITownBankChange;
    public UITownBankLoans uITownBankLoans;

    public RadioGroupView rgType;

    public override void Start()
    {
        base.Start();
        rgType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);
        InitDataByType(StoreForBankTypeEnum.Change);
    }

    private void InitDataByType(StoreForBankTypeEnum type)
    {
        uITownBankLoans.Close();
        uITownBankChange.Close();
        switch (type)
        {
            case StoreForBankTypeEnum.Change:
                uITownBankChange.Open();
                break;
            case StoreForBankTypeEnum.Loans:
                uITownBankLoans.Open();
                break;
        }
    }

    #region 回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForBankTypeEnum type = EnumUtil.GetEnum<StoreForBankTypeEnum>(rbview.name);
        InitDataByType(type);
    }



    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion 
}