using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupAchievementShow : PopupShowView
{
    public Image ivIcon;
    public Image ivRemark;
    public Text tvName;
    public Text tvStatus;
    public Text tvContent;

    public GameObject objAchieveContent;
    public GameObject objAchieveModel;

    public GameObject objRewardTitle;
    public GameObject objRewardContent;
    public GameObject objRewardModel;

    public Material materialGray;

    protected GameDataManager gameDataManager;
    protected IconDataManager iconDataManager;
    protected InnBuildManager innBuildManager;
    protected InnFoodManager innFoodManager;
    protected NpcInfoManager npcInfoManager;

    public AchievementInfoBean achievementInfo;
    public AchievementStatusEnum status;

    public Color colorStatusCompleted;
    public Color colorStatusProcessing;
    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
    }

    public void SetData(AchievementStatusEnum status, AchievementInfoBean achievementInfo)
    {
        this.status = status;
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.GetAchievementType(), achievementInfo.icon_key, achievementInfo.icon_key_remark);
        SetName(achievementInfo.name);
        SetContent(achievementInfo.content);
        SetAchieve(status, achievementInfo);
        SetStatus(status);
        SetReward(achievementInfo);
    }

    public void SetIcon(AchievementTypeEnum type, string iconKey, string iconKeyRemark)
    {
        Sprite spIcon;
        spIcon = iconDataManager.GetIconSpriteByName(iconKey);

        if (spIcon != null && ivIcon != null && ivRemark != null)
        {
            ivIcon.sprite = spIcon;
            switch (status)
            {
                case AchievementStatusEnum.Completed:
                    ivIcon.material = null;
                    ivRemark.material = null;
                    break;
                case AchievementStatusEnum.Processing:
                case AchievementStatusEnum.ToBeConfirmed:
                    ivIcon.material = materialGray;
                    ivRemark.material = materialGray;
                    break;
            }
        }

        //设置备用图标
        if (ivRemark != null && !CheckUtil.StringIsNull(iconKeyRemark))
        {
            ivRemark.gameObject.SetActive(true);
            Sprite spIconRemark = iconDataManager.GetIconSpriteByName(iconKey);
            if (spIconRemark != null)
                ivRemark.sprite = spIconRemark;
        }
        else
        {
            ivRemark.gameObject.SetActive(false);
        }
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(AchievementStatusEnum status)
    {
        if (tvStatus != null)
        {
            switch (status)
            {
                case AchievementStatusEnum.Completed:
                    tvStatus.text = GameCommonInfo.GetUITextById(12001);
                    tvStatus.color = colorStatusCompleted;
                    break;
                case AchievementStatusEnum.Processing:
                    tvStatus.text = GameCommonInfo.GetUITextById(12002);
                    tvStatus.color = new Color();
                    tvStatus.color = colorStatusProcessing;
                    break;
                case AchievementStatusEnum.ToBeConfirmed:
                    tvStatus.text = GameCommonInfo.GetUITextById(12003);
                    tvStatus.color = new Color();
                    tvStatus.color = new Color(1, 0.2f, 0, 1);
                    break;
            }

        }
    }

    /// <summary>
    /// 设置成就达成条件
    /// </summary>
    /// <param name="data"></param>
    public void SetAchieve(AchievementStatusEnum status, AchievementInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objAchieveContent.transform);
        if (data == null)
            return;
        List<PreTypeBean> listPreData = PreTypeEnumTools.GetListPreData(data.pre_data);
        foreach (var itemPreData in listPreData)
        {
            if (status == AchievementStatusEnum.Completed)
            {
                PreTypeEnumTools.GetPreDetails(itemPreData, gameDataManager.gameData, 
                    iconDataManager,
                    innFoodManager,
                    npcInfoManager,
                    true);
            }
            else
            {
                PreTypeEnumTools.GetPreDetails(itemPreData, gameDataManager.gameData, 
                    iconDataManager,
                    innFoodManager,
                    npcInfoManager,
                    false);
            }
            string preDes = itemPreData.preDescribe;
            float progress = itemPreData.progress;
            CreateAchieveItem(preDes, progress);
        }
    }

    /// <summary>
    /// 设置奖励
    /// </summary>
    /// <param name="data"></param>
    public void SetReward(AchievementInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objRewardContent.transform);
        if (data == null)
            return;

        List<RewardTypeBean> listRewardData = RewardTypeEnumTools.GetListRewardData(data.reward_data);
        GameObject objTitle = Instantiate(objRewardContent, objRewardTitle);
        foreach (var itemRewardData in listRewardData)
        {
            RewardTypeEnumTools.GetRewardDetails(itemRewardData, iconDataManager, innBuildManager,npcInfoManager);
            string rewardDes = itemRewardData.rewardDescribe;
            Sprite spReward = itemRewardData.spRewardIcon;
            CreateRewardItem(rewardDes, spReward);
        }

        //}
        ////奖励-道具
        //if (!CheckUtil.StringIsNull(data.reward_items_ids))
        //{
        //    List<long> listItems = data.GetRewardItems();
        //    foreach (long itemId in listItems)
        //    {
        //        ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemId);
        //        if (itemsInfo == null)
        //            continue;
        //        Sprite spIcon;
        //        if (itemsInfo.items_type == (int)GeneralEnum.Hat)
        //        {
        //            spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName("unknown_hat_1");
        //        }
        //        else if (itemsInfo.items_type == (int)GeneralEnum.Clothes)
        //        {
        //            spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName("unknown_clothes_1");
        //        }
        //        else if (itemsInfo.items_type == (int)GeneralEnum.Shoes)
        //        {
        //            spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName("unknown_shoes_1");
        //        }
        //        else
        //        {
        //            spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName(itemsInfo.icon_key);
        //        }
        //        CreateRewardItem(itemsInfo.name, 0, spIcon);
        //    }
        //}
        ////奖励-建筑材料
        //if (!CheckUtil.StringIsNull(data.reward_build_ids))
        //{
        //    List<long> listBuild = data.GetRewardBuild();
        //    foreach (long buildId in listBuild)
        //    {
        //        BuildItemBean buildItem = innBuildManager.GetBuildDataById(buildId);
        //        if (buildItem == null)
        //            continue;
        //        Sprite spIcon = innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
        //        CreateRewardItem(buildItem.name, 0, spIcon);
        //    }
        //}
    }

    /// <summary>
    /// 创建成就前提条件Item
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pro"></param>
    private void CreateAchieveItem(string name, float pro)
    {
        GameObject objAchieve = Instantiate(objAchieveModel, objAchieveContent.transform);
        objAchieve.SetActive(true);
        ItemGamePopupAchCpt itemAchieve = objAchieve.GetComponent<ItemGamePopupAchCpt>();
        itemAchieve.SetData(name, pro);
    }

    private void CreateRewardItem(string name, Sprite spIcon)
    {
        GameObject objReward = Instantiate(objRewardModel, objRewardContent.transform);
        objReward.SetActive(true);
        ItemGamePopupAchRewardCpt itemAchieve = objReward.GetComponent<ItemGamePopupAchRewardCpt>();
        itemAchieve.SetData(name, spIcon);
    }

}