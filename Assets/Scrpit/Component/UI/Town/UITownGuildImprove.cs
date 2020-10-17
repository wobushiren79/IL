using UnityEngine;
using UnityEditor;

public class UITownGuildImprove : UIBaseOne, IRadioGroupCallBack
{
    public RadioGroupView rgType;
    public UITownGuildImproveInnInfo uiInnInfo;
    public UITownGuildImproveCharacterInfo uiCharacterInfo;

    protected int typePosition=0;
    public override void Awake()
    {
        base.Awake();
        rgType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        typePosition = 0;
        rgType.SetPosition(typePosition, true);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        ChangeUIType(typePosition);
    }

    /// <summary>
    /// 改变UI类型
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUIType(int type)
    {

        uiInnInfo.Close() ;
        uiCharacterInfo.Close();
        switch (type)
        {
            case 0:
                uiInnInfo.Open();
                uiInnInfo.InitData(uiGameManager.gameDataManager.gameData);
                break;
            case 1:
                uiCharacterInfo.Open();
                uiCharacterInfo.InitData(uiGameManager.gameDataManager.gameData);
                break;
        }
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.typePosition = position;
        ChangeUIType(typePosition);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}