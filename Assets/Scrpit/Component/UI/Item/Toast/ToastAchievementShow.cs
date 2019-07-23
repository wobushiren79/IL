using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ToastAchievementShow : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Button btBack;

    public GameObject objToastContent;
    public GameObject objToast;

    public GameObject objRewardContent;
    public GameObject objRewardModel;

    public AchievementInfoBean achievementInfo;
    public GameItemsManager gameItemsManager;
    public InnBuildManager innBuildManager;
    public InnFoodManager innFoodManager;

    public void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(Close);
    }

    public void Toast(AchievementInfoBean achievementInfo)
    {
        objToastContent.SetActive(true);
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.type, achievementInfo.icon_key);
        SetName(achievementInfo.name);
        SetReward(achievementInfo);
        objToast.transform.DOKill();
        objToast.transform.DOScale(new Vector3(0, 0, 0), 1.5f).From().SetEase(Ease.OutElastic);
    }

    public void Close()
    {
        objToastContent.SetActive(false);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(int type,string iconKey)
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
                spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
            }
            
            if (spIcon != null)
                ivIcon.sprite = spIcon;
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
        //添加奖励
        if (achievementInfo.reward_guildcoin != 0)
        {
            Sprite spIcon = gameItemsManager.GetItemsSpriteByName("guild_coin_2");
            CreateRewardItem(spIcon, animTimeDelay);
            animTimeDelay += 0.5f;
        }
        //添加装备
        if (!CheckUtil.StringIsNull(achievementInfo.reward_items_ids))
        {
            foreach (long id in achievementInfo.GetRewardItems())
            {
                Sprite spIcon = null;
                ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(id);
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
                CreateRewardItem(spIcon, animTimeDelay);
                animTimeDelay += 0.5f;
            }
        }
        //添加建筑材料
        if (!CheckUtil.StringIsNull(achievementInfo.reward_build_ids))
        {
            foreach (long id in achievementInfo.GetRewardBuild())
            {
                BuildItemBean buildItem= innBuildManager.GetBuildDataById(id);
                Sprite spIcon = innBuildManager.GetFurnitureSpriteByName(buildItem.icon_key);
                CreateRewardItem(spIcon, animTimeDelay);
                animTimeDelay += 0.5f;
            }
        }
    }

    private void CreateRewardItem(Sprite spIcon,float delay)
    {
        GameObject objReward = Instantiate(objRewardModel, objRewardContent.transform);
        objReward.SetActive(true);
        Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
        objReward.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).SetDelay(delay);
    }
}