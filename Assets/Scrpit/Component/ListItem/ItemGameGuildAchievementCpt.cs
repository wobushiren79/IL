using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameGuildAchievementCpt : BaseMonoBehaviour
{
    public enum AchievementStatusEnum
    {
        UnKnown = 0,
        Completed = 1,
        Processing = 2,
        ToBeConfirmed = 3
    }

    public Image ivIcon;
    public Image ivIconRemark;
    public Image ivBackground;
    public Button btSubmit;
    public InfoAchievementPopupButton popupButton;
    public ToastAchievementShow toastAchievement;

    public UITownGuildAchievement uiTownGuildAchievement;
    public InnFoodManager innFoodManager;
    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;

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
    }

    public void SetData(AchievementInfoBean data)
    {
        this.achievementInfo = data;
        SetIcon(data.id, data.pre_ach_id, data.icon_key);
    }

    public void SetIcon(long achId, long preId, string iconKey)
    {
        if (gameItemsManager == null || ivIcon == null || gameDataManager == null || ivBackground == null)
            return;
        //检测是否拥有该成就
        bool hasAch = gameDataManager.gameData.GetAchievementData().CheckAchievementList(achId);
        if (hasAch)
        {
            SetAchStatus(AchievementStatusEnum.Completed);
            return;
        }
        //检测前置成就
        if (preId == 0)
        {
            //检测是否符合条件
            if (CheckAchieve())
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
            bool hasPre = gameDataManager.gameData.GetAchievementData().CheckAchievementList(preId);
            if (hasPre)
            {
                //检测是否符合条件
                if (CheckAchieve())
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
                SetIcon(achievementInfo.type,achievementInfo.icon_key, achievementInfo.icon_key_remark, null);
                ivBackground.sprite = spBackUnLock;
                break;
            case AchievementStatusEnum.Processing:
                //未解锁 不满足条件
                SetIcon(achievementInfo.type, achievementInfo.icon_key, achievementInfo.icon_key_remark, materialGray);
                ivBackground.sprite = spBackLock;
                break;
            case AchievementStatusEnum.ToBeConfirmed:
                //未解锁 满足条件  
                SetIcon(achievementInfo.type, achievementInfo.icon_key, achievementInfo.icon_key_remark, materialGray);
                ivBackground.sprite = spBackPass;
                break;
        }
        //弹出框刷新数据
        if (popupButton != null)
            popupButton.SetData(status, achievementInfo);
    }

    public void SetIcon(int type, string iconKey,string iconKeyRemark, Material material)
    {
        if (gameItemsManager == null || ivIcon == null)
            return;
        Sprite spIcon;
        if (type == 1)
        {
            spIcon = innFoodManager.GetFoodSpriteByName(iconKey);
        }
        else
        {
            spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
        }
        if (spIcon != null)
            ivIcon.sprite = spIcon;
        else
            ivIcon.sprite = spIconUnknow;
        ivIcon.material = material;
        //设置备用图标
        if (!CheckUtil.StringIsNull(iconKeyRemark))
        {
            Sprite spIconRemark = gameItemsManager.GetItemsSpriteByName(iconKeyRemark);
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
    /// 检测成就
    /// </summary>
    /// <returns></returns>
    public bool CheckAchieve()
    {
        if (achievementInfo == null || gameDataManager == null)
            return false;
        return achievementInfo.CheckAchievement(gameDataManager.gameData);
    }

    /// <summary>
    /// 完成成就
    /// </summary>
    public void SubmitAchievement()
    {
        if (gameDataManager == null || achievementInfo == null)
            return;
        if (status == AchievementStatusEnum.ToBeConfirmed)
        {
            //添加该成就和奖励
            gameDataManager.gameData.AddAchievement(achievementInfo);
            //设置状态
            SetAchStatus(AchievementStatusEnum.Completed);
            //刷新UI
            if (uiTownGuildAchievement != null)
                uiTownGuildAchievement.InitDataByType(achievementInfo.type);
            //弹出特效提示
            if (toastAchievement != null)
                toastAchievement.Toast(achievementInfo);
        }
    }
}