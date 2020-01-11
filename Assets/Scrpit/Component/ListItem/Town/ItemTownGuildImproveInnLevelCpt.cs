using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownGuildImproveInnLevelCpt : BaseMonoBehaviour
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

    public StoreInfoBean storeInfo;
    public bool isAllPre = true;

    protected GameItemsManager gameItemsManager;
    protected IconDataManager iconDataManager;
    protected GameDataManager gameDataManager;
    protected ToastManager toastManager;
    protected UIGameManager uiGameManager;
    protected InnBuildManager innBuildManager;

    private void Awake()
    {
        gameItemsManager= Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
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
            PreTypeEnumTools.GetPreDetails(itemData, gameDataManager.gameData, iconDataManager);
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
                tvContent.color = Color.green;
            }
            else
            {
                isAllPre = false;
                ivStatus.sprite = spUnPre;
                tvContent.color = Color.red;
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
            RewardTypeEnumTools.GetRewardDetails(itemData,iconDataManager,gameItemsManager,innBuildManager);
            //设置图标
            Sprite spIcon = itemData.spRewardIcon;
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            ivIcon.sprite = spIcon;
            //设置描述
            string rewardDes = itemData.rewardDescribe;
            Text tvContent = CptUtil.GetCptInChildrenByName<Text>(objReward, "Text");
            tvContent.text = rewardDes;
            tvContent.color = Color.green;
        }
        GameUtil.RefreshRectViewHight((RectTransform)objRewardContainer.transform, true);
        GameUtil.RefreshRectViewHight((RectTransform)transform, true);
    }

    /// <summary>
    /// 提交晋升
    /// </summary>
    public void OnClickSubmit()
    {
        if (isAllPre)
        {
            //前置如果有需要临时支付的条件
            PreTypeEnumTools.CompletePre(storeInfo.pre_data, gameDataManager.gameData);
            //获取所有奖励
            RewardTypeEnumTools.CompleteReward(storeInfo.reward_data, gameDataManager.gameData);
            //客栈升级
            gameDataManager.gameData.innAttributes.SetInnLevelUp();

            toastManager.ToastHint(ivTitleIcon.sprite, GameCommonInfo.GetUITextById(1062));
            uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
        }
        else
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1061));
        }
    }
}