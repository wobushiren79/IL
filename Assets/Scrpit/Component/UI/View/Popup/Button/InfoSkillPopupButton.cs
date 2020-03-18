using UnityEngine;
using UnityEditor;

public class InfoSkillPopupButton : PopupButtonView
{
    public SkillInfoBean skillInfo;
    public int usedNumber;

    private void Awake()
    {
        UIGameManager uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        SetPopupShowView(uiGameManager.infoSkillPopup);
    }

    public void SetData(SkillInfoBean skillInfo, int usedNumber)
    {
        this.skillInfo = skillInfo;
        this.usedNumber = usedNumber;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        ((InfoSkillPopupShow)popupShow).SetData(skillInfo, usedNumber);
    }
}