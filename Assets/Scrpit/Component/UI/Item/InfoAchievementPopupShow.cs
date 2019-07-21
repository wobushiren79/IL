using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class InfoAchievementPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvStatus;
    public Text tvContent;
    public RectTransform rtContent;

    public GameObject objAchieveContent;
    public GameObject objAchieveModel;

    public Material materialGray;

    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public AchievementInfoBean achievementInfo;
    public ItemGameGuildAchievementCpt.AchievementStatusEnum status;

    public void SetData(ItemGameGuildAchievementCpt.AchievementStatusEnum status,AchievementInfoBean achievementInfo)
    {
        this.status = status;
        this.achievementInfo = achievementInfo;
        SetIcon(achievementInfo.icon_key);
        SetName(achievementInfo.name);
        SetContent(achievementInfo.content);
        SetAchieve(achievementInfo);
        SetStatus(status);
    }

    public void SetIcon(string iconKey)
    {
        Sprite spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
        if (spIcon != null && ivIcon != null)
        {
            ivIcon.sprite = spIcon;
            switch (status)
            {
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.Completed:
                    ivIcon.material = null;
                    break;
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.Processing:
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.ToBeConfirmed:
                    ivIcon.material = materialGray;
                    break;
            }
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

    public void SetStatus(ItemGameGuildAchievementCpt.AchievementStatusEnum status)
    {
        if(tvStatus!=null)
        {
            switch (status)
            {
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.Completed:
                    tvStatus.text = GameCommonInfo.GetUITextById(12001);
                    tvStatus.color = new Color(0,1,0,1);
                    break;
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.Processing:
                    tvStatus.text = GameCommonInfo.GetUITextById(12002);
                    tvStatus.color = new Color();
                    tvStatus.color = new Color(0, 0, 0, 1);
                    break;
                case ItemGameGuildAchievementCpt.AchievementStatusEnum.ToBeConfirmed:
                    tvStatus.text = GameCommonInfo.GetUITextById(12003);
                    tvStatus.color = new Color();
                    tvStatus.color = new Color(1, 0.2f, 0, 1);
                    break;
            }
         
        }
    }

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
    }


    private void CreateAchieveItem(string name, float pro)
    {
        GameObject objAchieve = Instantiate(objAchieveModel, objAchieveContent.transform);
        objAchieve.SetActive(true);
        ItemGamePopupAchCpt itemAchieve = objAchieve.GetComponent<ItemGamePopupAchCpt>();
        itemAchieve.SetData(name, pro);
    }

    private void CreateProStr(long own, long achieve, out string proStr, out float pro)
    {
        if (status == ItemGameGuildAchievementCpt.AchievementStatusEnum.Completed)
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