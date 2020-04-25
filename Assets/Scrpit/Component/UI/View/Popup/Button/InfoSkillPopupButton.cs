using UnityEngine;
using UnityEditor;

public class InfoSkillPopupButton : PopupButtonView<InfoSkillPopupShow>
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