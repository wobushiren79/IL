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

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ChangeUIType(typePosition);
    }

    /// <summary>
    /// 改变UI类型
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUIType(int type)
    {
        uiInnInfo.CloseUI() ;
        uiCharacterInfo.CloseUI();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        switch (type)
        {
            case 0:
                uiInnInfo.OpenUI();
                uiInnInfo.InitData(gameData);
                break;
            case 1:
                uiCharacterInfo.OpenUI();
                uiCharacterInfo.InitData(gameData);
                break;
        }
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.typePosition = position;
        ChangeUIType(typePosition);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}