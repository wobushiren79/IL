using UnityEngine;
using UnityEditor;

public class InfoAchievementPopupButton : PopupButtonView<InfoAchievementPopupShow>
{
    public AchievementInfoBean achievementInfo;
    public AchievementStatusEnum status;

    public void SetData(AchievementStatusEnum status, AchievementInfoBean achievementInfo)
    {
        this.achievementInfo = achievementInfo;
        this.status = status;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
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

}