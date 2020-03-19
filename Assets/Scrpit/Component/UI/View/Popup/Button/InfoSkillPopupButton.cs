using UnityEngine;
using UnityEditor;

public class InfoSkillPopupButton : PopupButtonView
{
    public SkillInfoBean skillInfo;

    private void Awake()
    {
        UIGameManager uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        SetPopupShowView(uiGameManager.infoSkillPopup);
    }

    public void SetData(SkillInfoBean skillInfo)
    {
        this.skillInfo = skillInfo;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        ((InfoSkillPopupShow)popupShow).SetData(skillInfo);
    }
}