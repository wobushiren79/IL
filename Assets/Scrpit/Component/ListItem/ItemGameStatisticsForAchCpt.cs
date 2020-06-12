using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameStatisticsForAchCpt : ItemGameBaseCpt
{
    public InfoAchievementPopupButton popupButton;
    public Image ivIcon;
    public Image ivBackground;

    public Sprite spUnLockBackground;
    public Sprite spLockBackground;
    private void Start()
    {
        InfoAchievementPopupShow popupShow = FindInChildren<InfoAchievementPopupShow>(ImportantTypeEnum.Popup);
        popupButton.SetPopupShowView(popupShow);
    }

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
            UIGameManager uiGameManager = uiComponent.GetUIManager<UIGameManager>();
            Sprite spIcon = uiGameManager.iconDataManager.GetIconSpriteByName(achievementInfo.icon_key);
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