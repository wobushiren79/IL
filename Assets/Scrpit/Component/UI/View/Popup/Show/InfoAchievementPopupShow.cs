﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoAchievementPopupShow : PopupShowView
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
    protected GameItemsManager gameItemsManager;
    protected IconDataManager iconDataManager;
    protected CharacterDressManager characterDressManager;
    protected InnBuildManager innBuildManager;
    protected InnFoodManager innFoodManager;

    public AchievementInfoBean achievementInfo;
    public ItemTownGuildAchievementCpt.AchievementStatusEnum status;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.InnBuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

    public void SetData(ItemTownGuildAchievementCpt.AchievementStatusEnum status, AchievementInfoBean achievementInfo)
    {
        this.status = status;
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.type, achievementInfo.icon_key, achievementInfo.icon_key_remark);
        SetName(achievementInfo.name);
        SetContent(achievementInfo.content);
        SetAchieve(achievementInfo);
        SetStatus(status);
        SetReward(achievementInfo);
    }

    public void SetIcon(int type, string iconKey, string iconKeyRemark)
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

        if (spIcon != null && ivIcon != null && ivRemark != null)
        {
            ivIcon.sprite = spIcon;
            switch (status)
            {
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.Completed:
                    ivIcon.material = null;
                    ivRemark.material = null;
                    break;
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.Processing:
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.ToBeConfirmed:
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
    public void SetStatus(ItemTownGuildAchievementCpt.AchievementStatusEnum status)
    {
        if (tvStatus != null)
        {
            switch (status)
            {
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.Completed:
                    tvStatus.text = GameCommonInfo.GetUITextById(12001);
                    tvStatus.color = new Color(0, 1, 0, 1);
                    break;
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.Processing:
                    tvStatus.text = GameCommonInfo.GetUITextById(12002);
                    tvStatus.color = new Color();
                    tvStatus.color = new Color(0, 0, 0, 1);
                    break;
                case ItemTownGuildAchievementCpt.AchievementStatusEnum.ToBeConfirmed:
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
    public void SetAchieve(AchievementInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objAchieveContent.transform);
        if (data == null)
            return;
        Dictionary<PreTypeEnum,string> listPreData= PreTypeEnumTools.GetPreData(data.pre_data);
        foreach (var itemPreData in listPreData)
        {
            PreTypeEnum preType = itemPreData.Key;
            string preDes=  PreTypeEnumTools.GetPreDescribe(preType, itemPreData.Value, gameDataManager.gameData,out bool isPre,out float progress);
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
        if (data == null || gameItemsManager == null)
            return;

        Dictionary<RewardTypeEnum, string> listRewardData = RewardTypeEnumTools.GetRewardData(data.reward_data);
        GameObject objTitle = Instantiate(objRewardContent, objRewardTitle);
        foreach (var itemRewardData in listRewardData)
        {
            RewardTypeEnum rewardType = itemRewardData.Key;
            string rewardDes=  RewardTypeEnumTools.GetRewardDescribe(rewardType, itemRewardData.Value);
            Sprite spReward = RewardTypeEnumTools.GetRewardSprite(rewardType, iconDataManager);
            CreateRewardItem(rewardDes, spReward);
        }
   
        //}
        ////奖励-道具
        //if (!CheckUtil.StringIsNull(data.reward_items_ids))
        //{
        //    List<long> listItems = data.GetRewardItems();
        //    foreach (long itemId in listItems)
        //    {
        //        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemId);
        //        if (itemsInfo == null)
        //            continue;
        //        Sprite spIcon;
        //        if (itemsInfo.items_type == (int)GeneralEnum.Hat)
        //        {
        //            spIcon = gameItemsManager.GetItemsSpriteByName("unknown_hat_1");
        //        }
        //        else if (itemsInfo.items_type == (int)GeneralEnum.Clothes)
        //        {
        //            spIcon = gameItemsManager.GetItemsSpriteByName("unknown_clothes_1");
        //        }
        //        else if (itemsInfo.items_type == (int)GeneralEnum.Shoes)
        //        {
        //            spIcon = gameItemsManager.GetItemsSpriteByName("unknown_shoes_1");
        //        }
        //        else
        //        {
        //            spIcon = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
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

    private void CreateRewardItem(string name,Sprite spIcon)
    {
        GameObject objReward = Instantiate(objRewardModel, objRewardContent.transform);
        objReward.SetActive(true);
        ItemGamePopupAchRewardCpt itemAchieve = objReward.GetComponent<ItemGamePopupAchRewardCpt>();
        itemAchieve.SetData(name , spIcon);
    }

}