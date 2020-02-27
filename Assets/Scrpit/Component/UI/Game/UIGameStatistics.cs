using UnityEngine;
using UnityEditor;

public class UIGameStatistics : UIBaseOne,IRadioGroupCallBack
{
    public UIGameStatisticsForInn innUI;
    public UIGameStatisticsForRevenue revenueUI;
    public UIGameStatisticsForAch achUI;
    public RadioGroupView rgType;

    public override void Start()
    {
        base.Start();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetCallBack(this);
        rgType.SetPosition(0, true);
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);

        innUI.Close();
        revenueUI.Close();
        achUI.Close();

        switch (rbview.name)
        {
            case "Inn":
                innUI.Open();
                break;
            case "Revenue":
                revenueUI.Open();
                break;
            case "Ach":
                achUI.Open();
                break;
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}