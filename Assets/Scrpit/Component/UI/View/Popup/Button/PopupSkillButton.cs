using UnityEngine;
using UnityEditor;

public class PopupSkillButton : PopupButtonView<PopupSkillShow>
{
    public SkillInfoBean skillInfo;

    public void SetData(SkillInfoBean skillInfo)
    {
        this.skillInfo = skillInfo;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        popupShow.SetData(skillInfo);
    }
}