using UnityEngine;
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

    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;
    public InnBuildManager innBuildManager;
    public InnFoodManager innFoodManager;

    public AchievementInfoBean achievementInfo;
    public ItemTownGuildAchievementCpt.AchievementStatusEnum status;

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
            spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
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
            Sprite spIconRemark = gameItemsManager.GetItemsSpriteByName(iconKeyRemark);
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
        //携带金钱数
        if (data.achieve_money_s != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyS, data.achieve_money_s, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11101) + proStr, pro);
        }
        if (data.achieve_money_m != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyM, data.achieve_money_m, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11102) + proStr, pro);
        }
        if (data.achieve_money_l != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyL, data.achieve_money_l, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11103) + proStr, pro);
        }
        //支付金钱数
        if (data.achieve_pay_s != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyS, data.achieve_pay_s, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11104) + proStr, pro);
        }
        if (data.achieve_pay_m != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyM, data.achieve_pay_m, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11105) + proStr, pro);
        }
        if (data.achieve_pay_l != 0)
        {
            CreateProStr(gameDataManager.gameData.moneyL, data.achieve_pay_l, out string proStr, out float pro);
            CreateAchieveItem(GameCommonInfo.GetUITextById(11106) + proStr, pro);
        }
        //销售数量要求
        if (data.achieve_sell_number != 0)
        {
            MenuOwnBean menuOwn = gameDataManager.gameData.GetMenuById(data.remark_id);
            long sellNumber = 0;
            if (menuOwn != null)
            {
                sellNumber = menuOwn.sellNumber;
            }
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(data.remark_id);
            CreateProStr(sellNumber, data.achieve_sell_number, out string proStr, out float pro);
            CreateAchieveItem(string.Format(GameCommonInfo.GetUITextById(11107), menuInfo == null ? "???" : menuInfo.name, proStr), pro);
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
        GameObject objTitle = Instantiate(objRewardTitle, objRewardContent.transform);
        objTitle.SetActive(true);
        //奖励-公会硬币
        if (data.reward_guildcoin != 0)
        {
            Sprite spIcon = gameItemsManager.GetItemsSpriteByName("guild_coin_2");
            CreateRewardItem(GameCommonInfo.GetUITextById(11206), data.reward_guildcoin, spIcon);
        }
        //奖励-道具
        if (!CheckUtil.StringIsNull(data.reward_items_ids))
        {
            List<long> listItems = data.GetRewardItems();
            foreach (long itemId in listItems)
            {
                ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemId);
                if (itemsInfo == null)
                    continue;
                Sprite spIcon;
                if (itemsInfo.items_type == (int)GeneralEnum.Hat)
                {
                    spIcon = gameItemsManager.GetItemsSpriteByName("unknown_hat_1");
                }
                else if (itemsInfo.items_type == (int)GeneralEnum.Clothes)
                {
                    spIcon = gameItemsManager.GetItemsSpriteByName("unknown_clothes_1");
                }
                else if (itemsInfo.items_type == (int)GeneralEnum.Shoes)
                {
                    spIcon = gameItemsManager.GetItemsSpriteByName("unknown_shoes_1");
                }
                else
                {
                    spIcon = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
                }
                CreateRewardItem(itemsInfo.name, 0, spIcon);
            }
        }
        //奖励-建筑材料
        if (!CheckUtil.StringIsNull(data.reward_build_ids))
        {
            List<long> listBuild = data.GetRewardBuild();
            foreach (long buildId in listBuild)
            {
                BuildItemBean buildItem = innBuildManager.GetBuildDataById(buildId);
                if (buildItem == null)
                    continue;
                Sprite spIcon = innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
                CreateRewardItem(buildItem.name, 0, spIcon);
            }
        }
    }

    private void CreateAchieveItem(string name, float pro)
    {
        GameObject objAchieve = Instantiate(objAchieveModel, objAchieveContent.transform);
        objAchieve.SetActive(true);
        ItemGamePopupAchCpt itemAchieve = objAchieve.GetComponent<ItemGamePopupAchCpt>();
        itemAchieve.SetData(name, pro);
    }

    private void CreateRewardItem(string name, long number, Sprite spIcon)
    {
        GameObject objReward = Instantiate(objRewardModel, objRewardContent.transform);
        objReward.SetActive(true);
        ItemGamePopupAchRewardCpt itemAchieve = objReward.GetComponent<ItemGamePopupAchRewardCpt>();
        itemAchieve.SetData(name, number, spIcon);
    }

    private void CreateProStr(long own, long achieve, out string proStr, out float pro)
    {
        if (status == ItemTownGuildAchievementCpt.AchievementStatusEnum.Completed)
        {
            proStr = "(" + achieve + "/" + achieve + ")";
            pro = 1;
        }
        else
        {
            if (own >= achieve)
            {
                proStr = "(" + achieve + "/" + achieve + ")";
                pro = 1;
            }
            else
            {
                proStr = "(" + own + "/" + achieve + ")";
                pro = (float)own / (float)achieve;
            }
        }

    }
}