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
        loansStr = GameCommonInfo.GetUITextById(191);
    }

    public override void Update()
    {
        base.Update();
        if (tvLoansNumber != null)
        {
            GameDataBean gameData = uiGameManager.gameDataManager.gameData;
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