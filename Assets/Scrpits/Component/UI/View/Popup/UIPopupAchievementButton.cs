using UnityEngine;
using UnityEditor;

public class UIPopupAchievementButton : PopupButtonView<UIPopupAchievementShow>
{
    public AchievementInfoBean achievementInfo;
    public AchievementStatusEnum status;

    public void SetData(AchievementStatusEnum status, AchievementInfoBean achievementInfo)
    {
        this.achievementInfo = achievementInfo;
        this.status = status;
    }

    public override void PopupShow()
    {
        if (status == 0)
        {
            popupShow.gameObject.SetActive(false);
        }
        else
        {
            popupShow.SetData(status, achievementInfo);
        }
    }

    public override void PopupHide()
    {

    }
}