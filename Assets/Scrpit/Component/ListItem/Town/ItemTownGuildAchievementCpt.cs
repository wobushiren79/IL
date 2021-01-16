using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownGuildAchievementCpt : ItemGameBaseCpt
{
    public Image ivIcon;
    public Image ivIconRemark;
    public Image ivBackground;
    public Button btSubmit;
    public InfoAchievementPopupButton popupButton;

    public Sprite spIconUnknow;
    public Sprite spBackPass;
    public Sprite spBackUnLock;
    public Sprite spBackLock;
    public Material materialGray;

    public AchievementInfoBean achievementInfo;
    public AchievementStatusEnum status = AchievementStatusEnum.UnKnown;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SubmitAchievement);
        if (popupButton != null)
            popupButton.SetPopupShowView(GetUIManager<UIGameManager>().infoAchievementPopup);
    }

    public void SetData(AchievementInfoBean data)
    {
        this.achievementInfo = data;
        SetIcon(data.id, data.GetPreAchIds(), data.icon_key, data.pre_data);
    }

    public void SetIcon(long achId, long[] preIds, string iconKey, string preData)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();

        bool isAllPre = PreTypeEnumTools.CheckIsAllPre(
            uiGameManager.gameItemsManager,
            uiGameManager.iconDataManager,
            uiGameManager.characterDressManager,
            uiGameManager.innFoodManager,
            uiGameManager.npcInfoManager,
            uiGameManager.gameDataManager.gameData, 
            uiGameManager.gameDataManager.gameData.userCharacter,
            preData, out string reason);

        if (ivIcon == null || uiGameManager.gameDataManager == null || ivBackground == null)
            return;
        //检测是否拥有该成就
        bool hasAch = uiGameManager.gameDataManager.gameData.GetAchievementData().CheckHasAchievement(achId);
        if (hasAch)
        {
            SetAchStatus(AchievementStatusEnum.Completed);
            return;
        }
        //检测前置成就
        if (CheckUtil.ArrayIsNull(preIds))
        {
            //检测是否符合条件
            if (isAllPre)
            {
                SetAchStatus(AchievementStatusEnum.ToBeConfirmed);
            }
            else
            {
                SetAchStatus(AchievementStatusEnum.Processing);
            }
        }
        else
        {
            bool hasPre = uiGameManager.gameDataManager.gameData.GetAchievementData().CheckHasAchievement(preIds);
            if (hasPre)
            {
                //检测是否符合条件
                if (isAllPre)
                {
                    SetAchStatus(AchievementStatusEnum.ToBeConfirmed);
                }
                else
                {
                    SetAchStatus(AchievementStatusEnum.Processing);
                }
            }
            else
            {
                SetAchStatus(AchievementStatusEnum.UnKnown);
            }
        }
    }

    public void SetAchStatus(AchievementStatusEnum status)
    {
        this.status = status;
        if (achievementInfo == null || ivIcon == null || ivBackground == null)
            return;
        switch (status)
        {
            case AchievementStatusEnum.UnKnown:
                //未知
                ivIcon.sprite = spIconUnknow;
                ivBackground.sprite = spBackLock;
                break;
            case AchievementStatusEnum.Completed:
                //已解锁
                SetIcon(achievementInfo.GetAchievementType(), achievementInfo.icon_key, achievementInfo.icon_key_remark, null);
                ivBackground.sprite = spBackUnLock;
                break;
            case AchievementStatusEnum.Processing:
                //未解锁 不满足条件
                SetIcon(achievementInfo.GetAchievementType(), achievementInfo.icon_key, achievementInfo.icon_key_remark, materialGray);
                ivBackground.sprite = spBackLock;
                break;
            case AchievementStatusEnum.ToBeConfirmed:
                //未解锁 满足条件  
                SetIcon(achievementInfo.GetAchievementType(), achievementInfo.icon_key, achievementInfo.icon_key_remark, materialGray);
                ivBackground.sprite = spBackPass;
                break;
        }
        //弹出框刷新数据
        if (popupButton != null)
            popupButton.SetData(status, achievementInfo);
    }

    public void SetIcon(AchievementTypeEnum type, string iconKey, string iconKeyRemark, Material material)
    {
        IconDataManager iconDataManager = GetUIManager<UIGameManager>().iconDataManager;
        InnFoodManager innFoodManager = GetUIManager<UIGameManager>().innFoodManager;

        if (iconDataManager == null || ivIcon == null)
            return;
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        if (spIcon != null)
            ivIcon.sprite = spIcon;
        else
            ivIcon.sprite = spIconUnknow;
        ivIcon.material = material;
        //设置备用图标
        if (!CheckUtil.StringIsNull(iconKeyRemark))
        {
            Sprite spIconRemark = iconDataManager.GetIconSpriteByName(iconKeyRemark);
            if (spIconRemark != null)
            {
                ivIconRemark.sprite = spIconRemark;
            }
            ivIconRemark.gameObject.SetActive(true);
            ivIconRemark.material = material;
        }
        else
        {
            ivIconRemark.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 完成成就
    /// </summary>
    public void SubmitAchievement()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();

        if (uiGameManager.gameDataManager == null || achievementInfo == null)
            return;
        if (status == AchievementStatusEnum.ToBeConfirmed)
        {
            //添加该成就和奖励
            uiGameManager.gameDataManager.gameData.GetAchievementData().AddAchievement(achievementInfo.id);
            RewardTypeEnumTools.CompleteReward(
                uiGameManager.toastManager,
                uiGameManager.npcInfoManager,
                uiGameManager.iconDataManager,
                uiGameManager.gameItemsManager,
                uiGameManager.innBuildManager,
                uiGameManager.gameDataManager,
                null,
                achievementInfo.reward_data);
            //设置状态
            SetAchStatus(AchievementStatusEnum.Completed);
            //刷新UI
            if (GetUIComponent<UITownGuildAchievement>() != null)
                GetUIComponent<UITownGuildAchievement>().InitDataByType(achievementInfo.type);
            //弹出特效提示
            DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
            AchievementDialogView achDialog = dialogManager.CreateDialog<AchievementDialogView>(DialogEnum.Achievement, null, null);
            achDialog.SetData(achievementInfo);
            //播放音效
            uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
            //解锁steam成就
            if (uiGameManager.steamHandler!=null)
            {
                uiGameManager.steamHandler.UnLockAchievement(achievementInfo.id);
            }
        }
    }
}