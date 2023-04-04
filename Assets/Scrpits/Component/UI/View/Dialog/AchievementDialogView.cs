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


    public void SetData(int iconType, string iconKey)
    {
        SetIcon(iconType, iconKey, null);
        SetDataForCommon();
    }

    /// <summary>
    /// 成就展示
    /// </summary>
    /// <param name="achievementInfo"></param>
    public void SetData(AchievementInfoBean achievementInfo)
    {
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.type, achievementInfo.icon_key, achievementInfo.icon_key_remark);
        SetName(achievementInfo.name);
        SetTitle(TextHandler.Instance.manager.GetTextById(76));
        SetReward(achievementInfo.reward_data);
        SetDataForCommon();
    }

    /// <summary>
    /// 设置相关购买成功展示
    /// </summary>
    /// <param name="storeInfo"></param>
    public void SetData(StoreInfoBean storeInfo)
    {
        SetTitle(TextHandler.Instance.manager.GetTextById(43));
        SetIcon(2, "keyboard_button_up_1", "");
        SetName(storeInfo.name);
        SetReward(storeInfo.reward_data);
        SetDataForCommon();
    }

    public void SetDataForCommon()
    {
        objAchContainer.transform.DOKill();
        objAchContainer.transform.DOScale(new Vector3(0, 0, 0), 1.5f).From().SetEase(Ease.OutElastic);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(int type, string iconKey, string remarkIconKey)
    {
        if (ivIcon != null)
        {
            Sprite spIcon;
            if (type == 1)
            {
                spIcon = InnFoodHandler.Instance.manager.GetFoodSpriteByName(iconKey);
            }
            else
            {
                spIcon = IconHandler.Instance.GetIconSpriteByName(iconKey);
            }

            if (spIcon != null)
                ivIcon.sprite = spIcon;
        }
        //设置备用图标
        if (ivRemark != null && !remarkIconKey.IsNull())
        {
            ivRemark.gameObject.SetActive(true);
            Sprite spIconRemark = GameItemsHandler.Instance.manager.GetItemsSpriteByName(remarkIconKey);
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
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置奖励
    /// </summary>
    public void SetReward(string rewardData)
    {
        float animTimeDelay = 1f;
        CptUtil.RemoveChildsByActive(objRewardContent.transform);
        List<RewardTypeBean> listRewardData = RewardTypeEnumTools.GetListRewardData(rewardData);
        foreach (var itemRewardData in listRewardData)
        {
            RewardTypeEnumTools.GetRewardDetails(itemRewardData);
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