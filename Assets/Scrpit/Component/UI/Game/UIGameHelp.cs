using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameHelp : BaseUIComponent,IRadioGroupCallBack
{
    public RadioGroupView rgHelpType;
    public Button btExit;

    public GameObject objBaseHelp;
    public GameObject objBuildHelp;
    public GameObject objManageHelp;
    public GameObject objLevelUpHelp;
    public GameObject objFavorabilityHelp;
    public GameObject objHotel;

    public override  void Awake()
    {
        base.Awake();
        if (btExit != null) btExit.onClick.AddListener(OnClickExit);
        if (rgHelpType != null) rgHelpType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //记录数据
        UserAchievementBean userAchievement =uiGameManager.gameDataManager.gameData.GetAchievementData();
        userAchievement.isOpenedHelp = true;
        rgHelpType.SetPosition(0,true);
    }

    public void OnClickExit()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        objBaseHelp.SetActive(false);
        objBuildHelp.SetActive(false);
        objManageHelp.SetActive(false);
        objLevelUpHelp.SetActive(false);
        objFavorabilityHelp.SetActive(false);
        objHotel.SetActive(false);
        switch (rbview.name)
        {
            case "Base":
                objBaseHelp.SetActive(true);
                break;
            case "Build":
                objBuildHelp.SetActive(true);
                break;
            case "Manage":
                objManageHelp.SetActive(true);
                break;
            case "LevelUp":
                objLevelUpHelp.SetActive(true);
                break;
            case "Favorability":
                objFavorabilityHelp.SetActive(true);
                break;
            case "Hotel":
                objHotel.SetActive(true);
                break;
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}