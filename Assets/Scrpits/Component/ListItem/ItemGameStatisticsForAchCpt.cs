using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameStatisticsForAchCpt : ItemGameBaseCpt
{
    public UIPopupAchievementButton popupButton;
    public Image ivIcon;
    public Image ivBackground;

    public Sprite spUnLockBackground;
    public Sprite spLockBackground;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="achievementInfo"></param>
    /// <param name="spIcon"></param>
    /// <param name="isUnLock"></param>
    public void SetData(AchievementInfoBean achievementInfo, bool isUnLock)
    {
        if (isUnLock)
        {
            Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(achievementInfo.icon_key);
            SetIcon(spIcon);
            popupButton.SetData(AchievementStatusEnum.Completed, achievementInfo);
            ivBackground.sprite = spUnLockBackground;
        }
        else
        {
            SetIcon(null);
            ivBackground.sprite = spLockBackground;
            popupButton.SetData(AchievementStatusEnum.UnKnown, achievementInfo);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon)
        {
            if (spIcon == null)
            {
                ivIcon.color = Color.clear;
            }
            else
            {
                ivIcon.color = Color.white;
                ivIcon.sprite = spIcon;
            }

        }
    }
}