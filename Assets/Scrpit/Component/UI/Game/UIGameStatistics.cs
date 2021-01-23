using UnityEngine;
using UnityEditor;

public class UIGameStatistics : UIBaseOne,IRadioGroupCallBack
{
    public UIGameStatisticsForInn innUI;
    public UIGameStatisticsForRevenue revenueUI;
    public UIGameStatisticsForAch achUI;
    public UIGameStatisticsForCustomer customerUI;
    public UIGameStatisticsForMenu menuUI;
    public RadioGroupView rgType;

    public RadioButtonView rbTypeInn;
    public RadioButtonView rbTypeRevenue;
    public RadioButtonView rbTypeAch;
    public RadioButtonView rbTypeCustomer;
    public RadioButtonView rbTypeMenu;
    public override void Start()
    {
        base.Start();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameTimeHandler.Instance.SetTimeStatus(false);

        rgType.SetCallBack(this);
        rgType.SetPosition(0, true);
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

        innUI.Close();
        revenueUI.Close();
        achUI.Close();
        customerUI.Close();
        menuUI.Close();
        if (rbview== rbTypeInn)
        {
            innUI.Open();
        }
        else if (rbview == rbTypeRevenue)
        {
            revenueUI.Open();
        }
        else if (rbview == rbTypeAch)
        {
            achUI.Open();
        }
        else if (rbview == rbTypeCustomer)
        {
            customerUI.Open();
        }
        else if (rbview == rbTypeMenu)
        {
            menuUI.Open();
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}