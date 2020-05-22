using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameStatisticsForAchCpt : ItemGameBaseCpt
{
    public InfoAchievementPopupButton popupButton;
    public Image ivIcon;
    public Image ivBackground;

    public Sprite spLock;
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
    public void SetData(AchievementInfoBean achievementInfo,bool isUnLock)
    {
        if (isUnLock)
        {
            UIGameManager uiGameManager = uiComponent.GetUIManager<UIGameManager>();
            Sprite spIcon = uiGameManager.iconDataManager.GetIconSpriteByName(achievementInfo.icon_key);
            SetIcon(spIcon);
            popupButton.SetData(AchievementStatusEnum.Completed, achievementInfo);
        }
        else
        {
            SetIcon(spLock);
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
            ivIcon.sprite = spIcon;
        }
    }
}