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

    protected GameItemsManager gameItemsManager;
    protected IconDataManager iconDataManager;
    protected GameDataManager gameDataManager;
    protected ToastManager toastManager;
    protected UIGameManager uiGameManager;
    protected InnBuildManager innBuildManager;
    protected DialogManager dialogManager;
    protected NpcInfoManager npcInfoManager;
    protected CharacterDressManager characterDressManager;
    protected InnFoodManager innFoodManager;
    private void Awake()
    {
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

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
        tvTitle.text = GameCommonInfo.GetUITextById(71) + ":" + name;
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
        foreach (var itemData in listPreData)
        {
            GameObject objPre = Instantiate(objPreContainer, objPreModel);
            PreTypeEnumTools.GetPreDetails(
                itemData,
                gameDataManager.gameData,
                iconDataManager,
                innFoodManager,
                npcInfoManager);
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
            RewardTypeEnumTools.GetRewardDetails(itemData, iconDataManager, innBuildManager, npcInfoManager);
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
        if (isAllPre)
        {
            //前置如果有需要临时支付的条件
            PreTypeEnumTools.CompletePre(storeInfo.pre_data, gameDataManager.gameData);
            //获取所有奖励
            RewardTypeEnumTools.CompleteReward(
                toastManager,
                npcInfoManager,
                iconDataManager,
                innBuildManager,
                gameDataManager,
                null,
                storeInfo.reward_data);
            //客栈升级
            gameDataManager.gameData.innAttributes.SetInnLevelUp();

            toastManager.ToastHint(ivTitleIcon.sprite, GameCommonInfo.GetUITextById(1062));
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));

            DialogBean dialogData = new DialogBean();
            AchievementDialogView achievementDialog = dialogManager.CreateDialog<AchievementDialogView>(DialogEnum.Achievement, this, dialogData);
            achievementDialog.SetData(storeInfo);
        }
        else
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1061));
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