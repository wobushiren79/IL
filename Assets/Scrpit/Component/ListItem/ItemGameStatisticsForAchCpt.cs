using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameStatisticsForAchCpt : ItemGameBaseCpt
{
    public InfoAchievementPopupButton popupButton;
    public Image ivIcon;
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
    public void SetData(AchievementInfoBean achievementInfo)
    {
        popupButton.SetData(AchievementStatusEnum.Completed, achievementInfo);

        UIGameManager uiGameManager = uiComponent.GetUIManager<UIGameManager>();
        Sprite spIcon = uiGameManager.iconDataManager.GetIconSpriteByName(achievementInfo.icon_key);
        SetIcon(spIcon);
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