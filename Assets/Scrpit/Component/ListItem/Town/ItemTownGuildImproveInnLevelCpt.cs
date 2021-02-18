using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownGuildImproveInnLevelCpt : BaseMonoBehaviour, DialogView.IDialogCallBack
{
    public Text tvTitle;
    public Image ivTitleIcon;

    public GameObject objPreContainer;
    public GameObject objPreModel;
    public GameObject objRewardContainer;
    public GameObject objRewardModel;

    public Button btSubmit;

    public Sprite spRePre;
    public Sprite spUnPre;

    public Color colorPre;
    public Color colorUnPre;

    public StoreInfoBean storeInfo;
    public bool isAllPre = true;

    private void Start()
    {
        if (btSubmit != null)
        {
            btSubmit.onClick.AddListener(OnClickSubmit);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="innLevelStr"></param>
    /// <param name="spInnLevel"></param>
    /// <param name="storeInfo"></param>
    public void SetData(string innLevelStr, Sprite spInnLevel, StoreInfoBean storeInfo)
    {
        this.storeInfo = storeInfo;
        SetTitleName(innLevelStr);
        SetTitleIcon(spInnLevel);
        CreatePreDataItem(storeInfo.pre_data);
        CreateRewardDataItem(storeInfo.reward_data);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="name"></param>
    public void SetTitleName(string name)
    {
        if (tvTitle == null)
            return;
        tvTitle.text = TextHandler.Instance.manager.GetTextById(71) + ":" + name;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spInnLevel"></param>
    public void SetTitleIcon(Sprite spInnLevel)
    {
        if (ivTitleIcon == null || spInnLevel == null)
            return;
        ivTitleIcon.sprite = spInnLevel;
    }

    /// <summary>
    /// 创建前置数据
    /// </summary>
    /// <param name="preData"></param>
    public void CreatePreDataItem(string preData)
    {
        List<PreTypeBean> listPreData = PreTypeEnumTools.GetListPreData(preData);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        foreach (var itemData in listPreData)
        {
            GameObject objPre = Instantiate(objPreContainer, objPreModel);
            PreTypeEnumTools.GetPreDetails(itemData, gameData);
            //设置图标
            Sprite spIcon = itemData.spPreIcon;
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objPre, "Icon");
            ivIcon.sprite = spIcon;
            //设置描述
            string preDes = itemData.preDescribe;
            Text tvContent = CptUtil.GetCptInChildrenByName<Text>(objPre, "Text");
            tvContent.text = preDes;
            //设置是否满足条件
            Image ivStatus = CptUtil.GetCptInChildrenByName<Image>(objPre, "Status");
            if (itemData.isPre)
            {
                ivStatus.sprite = spRePre;
                tvContent.color = colorPre;
            }
            else
            {
                isAllPre = false;
                ivStatus.sprite = spUnPre;
                tvContent.color = colorUnPre;
            }

        }
        GameUtil.RefreshRectViewHight((RectTransform)objPreContainer.transform, true);
        GameUtil.RefreshRectViewHight((RectTransform)transform, true);
    }

    /// <summary>
    /// 创建奖励数据
    /// </summary>
    /// <param name="rewardData"></param>
    public void CreateRewardDataItem(string rewardData)
    {
        List<RewardTypeBean> listRewardData = RewardTypeEnumTools.GetListRewardData(rewardData);
        foreach (var itemData in listRewardData)
        {
            GameObject objReward = Instantiate(objRewardContainer, objRewardModel);
            RewardTypeEnumTools.GetRewardDetails(itemData);
            //设置图标
            Sprite spIcon = itemData.spRewardIcon;
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            ivIcon.sprite = spIcon;
            //设置描述
            string rewardDes = itemData.rewardDescribe;
            Text tvContent = CptUtil.GetCptInChildrenByName<Text>(objReward, "Text");
            tvContent.text = rewardDes;
            tvContent.color = colorPre;
        }
        GameUtil.RefreshRectViewHight((RectTransform)objRewardContainer.transform, true);
        GameUtil.RefreshRectViewHight((RectTransform)transform, true);
    }

    /// <summary>
    /// 提交晋升
    /// </summary>
    public void OnClickSubmit()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (isAllPre)
        {
            //前置如果有需要临时支付的条件
            PreTypeEnumTools.CompletePre(storeInfo.pre_data, gameData);
            //获取所有奖励
            RewardTypeEnumTools.CompleteReward(null, storeInfo.reward_data);
            //客栈升级
            gameData.innAttributes.SetInnLevelUp();

            ToastHandler.Instance.ToastHint(ivTitleIcon.sprite, TextHandler.Instance.manager.GetTextById(1062));
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);

            DialogBean dialogData = new DialogBean();
            AchievementDialogView achievementDialog = DialogHandler.Instance.CreateDialog<AchievementDialogView>(DialogEnum.Achievement, this, dialogData);
            achievementDialog.SetData(storeInfo);
        }
        else
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1061));
        }
    }

    #region
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}