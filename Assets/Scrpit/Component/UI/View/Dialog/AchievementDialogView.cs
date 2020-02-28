using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class AchievementDialogView : DialogView
{
    public Image ivIcon;
    public Image ivRemark;
    public Text tvName;

    public GameObject objAchContainer;
    public GameObject objRewardContent;
    public GameObject objRewardModel;

    public AchievementInfoBean achievementInfo;

    protected GameItemsManager gameItemsManager;
    protected IconDataManager iconDataManager;
    protected InnBuildManager innBuildManager;
    protected InnFoodManager innFoodManager;

    public override void Awake()
    {
        base.Awake();
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

    public void SetData(AchievementInfoBean achievementInfo)
    {
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.type, achievementInfo.icon_key);
        SetName(achievementInfo.name);
        SetReward(achievementInfo);
        objAchContainer.transform.DOKill();
        objAchContainer.transform.DOScale(new Vector3(0, 0, 0), 1.5f).From().SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(int type, string iconKey)
    {
        if (ivIcon != null && gameItemsManager != null && achievementInfo != null)
        {
            Sprite spIcon;
            if (type == 1)
            {
                spIcon = innFoodManager.GetFoodSpriteByName(iconKey);
            }
            else
            {
                spIcon = iconDataManager.GetIconSpriteByName(iconKey);
            }

            if (spIcon != null)
                ivIcon.sprite = spIcon;
        }
        //设置备用图标
        if (ivRemark != null && achievementInfo != null && !CheckUtil.StringIsNull(achievementInfo.icon_key_remark))
        {
            ivRemark.gameObject.SetActive(true);
            Sprite spIconRemark = gameItemsManager.GetItemsSpriteByName(achievementInfo.icon_key_remark);
            if (spIconRemark != null)
                ivRemark.sprite = spIconRemark;
        }
        else
        {
            ivRemark.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null && achievementInfo != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置奖励
    /// </summary>
    public void SetReward(AchievementInfoBean achievementInfo)
    {
        float animTimeDelay = 1f;
        CptUtil.RemoveChildsByActive(objRewardContent.transform);
        List<RewardTypeBean> listRewardData = RewardTypeEnumTools.GetListRewardData(achievementInfo.reward_data);
        foreach (var itemRewardData in listRewardData)
        {
            RewardTypeEnumTools.GetRewardDetails(itemRewardData, iconDataManager,gameItemsManager,innBuildManager);
            Sprite spReward = itemRewardData.spRewardIcon;
            CreateRewardItem(spReward, animTimeDelay);
            animTimeDelay += 0.1f;
        }
    }

    private void CreateRewardItem(Sprite spIcon, float delay)
    {
        GameObject objReward = Instantiate(objRewardContent, objRewardModel);
        Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
        objReward.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(delay);
    }
}