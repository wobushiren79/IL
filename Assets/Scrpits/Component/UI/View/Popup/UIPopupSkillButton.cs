using UnityEngine;
using UnityEditor;

public class UIPopupSkillButton : PopupButtonView<UIPopupSkillShow>
{
    public SkillInfoBean skillInfo;

    public void SetData(SkillInfoBean skillInfo)
    {
        this.skillInfo = skillInfo;
    }

    public override void PopupShow()
    {
        popupShow.SetData(skillInfo);
    }

    public override void PopupHide()
    {

    }
}