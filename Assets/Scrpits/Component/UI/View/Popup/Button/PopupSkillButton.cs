using UnityEngine;
using UnityEditor;

public class PopupSkillButton : PopupButtonView<PopupSkillShow>
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