using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIGameCustomBed : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public BedShowView bedShow;
    public PriceShowView priceShow;

    public Text tvBedBase;
    public Text tvBedBar;
    public Text tvBedSheets;
    public Text tvBedPillow;
    public InputField etBedName;

    public Button btSubmit;

    public RadioGroupView rgBedType;

    public List<StoreInfoBean> listBedData;

    public GameObject objItemContainer;
    public GameObject objItemModel;

    protected BuildBedBean customBedData;

    protected long customPriceL;
    protected long customPriceM;
    protected long customPriceS;

    public override void Awake()
    {
        base.Awake();
        if (rgBedType != null)
            rgBedType.SetCallBack(this);
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickForSumbit);
        if (etBedName != null)
            etBedName.onEndEdit.AddListener(OnEndEidtForName);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        CptUtil.RemoveChildsByActive(objItemContainer);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForCarpenterBed();

        if (customBedData == null)
            customBedData = new BuildBedBean();

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        bedShow.SetData(customBedData);
        SetBedText(customBedData);
        SetBedPrice(customBedData, listBedData);
    }

    /// <summary>
    /// 通过类型设置床数据
    /// </summary>
    /// <param name="buildItemType"></param>
    /// <param name="buildId"></param>
    public void SetBedDataByType(BuildItemTypeEnum buildItemType, long buildId)
    {
        switch (buildItemType)
        {
            case BuildItemTypeEnum.BedBase:
                customBedData.bedBase = buildId;
                break;
            case BuildItemTypeEnum.BedBar:
                customBedData.bedBar = buildId;
                break;
            case BuildItemTypeEnum.BedSheets:
                customBedData.bedSheets = buildId;
                break;
            case BuildItemTypeEnum.BedPillow:
                customBedData.bedPillow = buildId;
                break;
        }
        RefreshUI();
    }

    /// <summary>
    /// 设置床的文本
    /// </summary>
    /// <param name="customBedData"></param>
    public void SetBedText(BuildBedBean buildBedData)
    {
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        BuildItemBean bedBaseData = innBuildManager.GetBuildDataById(buildBedData.bedBase);
        BuildItemBean bedBarData = innBuildManager.GetBuildDataById(buildBedData.bedBar);
        BuildItemBean bedSheetsData = innBuildManager.GetBuildDataById(buildBedData.bedSheets);
        BuildItemBean bedPillowData = innBuildManager.GetBuildDataById(buildBedData.bedPillow);

        if (tvBedBase != null && bedBaseData != null)
            tvBedBase.text = bedBaseData.name;
        if (tvBedBar != null && bedBarData != null)
            tvBedBar.text = bedBarData.name;
        if (tvBedSheets != null && bedSheetsData != null)
            tvBedSheets.text = bedSheetsData.name;
        if (tvBedPillow != null && bedPillowData != null)
            tvBedPillow.text = bedPillowData.name;
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void SetBedPrice(BuildBedBean customBedData, List<StoreInfoBean> listBedData)
    {
        if (priceShow == null)
            return;
        customPriceL = 0;
        customPriceM = 0;
        customPriceS = 0;

        for (int i = 0; i < listBedData.Count; i++)
        {
            StoreInfoBean storeInfo = listBedData[i];
            if (customBedData.bedBase == storeInfo.mark_id)
            {
                customPriceL += storeInfo.price_l;
                customPriceM += storeInfo.price_m;
                customPriceS += storeInfo.price_s;
            }
            if (customBedData.bedBar == storeInfo.mark_id)
            {
                customPriceL += storeInfo.price_l;
                customPriceM += storeInfo.price_m;
                customPriceS += storeInfo.price_s;
            }
            if (customBedData.bedSheets == storeInfo.mark_id)
            {
                customPriceL += storeInfo.price_l;
                customPriceM += storeInfo.price_m;
                customPriceS += storeInfo.price_s;
            }
            if (customBedData.bedPillow == storeInfo.mark_id)
            {
                customPriceL += storeInfo.price_l;
                customPriceM += storeInfo.price_m;
                customPriceS += storeInfo.price_s;
            }
        }
        priceShow.SetPrice(1, customPriceL, customPriceM, customPriceS, 0, 0, 0, 0, 0);
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listData"></param>
    public void CreateBedDataList(List<StoreInfoBean> listData)
    {
        if (listData == null)
        {
            return;
        }
        CptUtil.RemoveChildsByActive(objItemContainer);
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean storeInfo = listData[i];
            GameObject objItem = Instantiate(objItemContainer, objItemModel);
            ItemTownCerpenterCpt itemCpt = objItem.GetComponent<ItemTownCerpenterCpt>();
            itemCpt.SetData(storeInfo);
        }
    }

    /// <summary>
    /// 根据类型获取数据
    /// </summary>
    /// <param name="buildItemType"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetListBedDataByType(BuildItemTypeEnum buildItemType)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (listBedData == null)
            return listData;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        for (int i = 0; i < listBedData.Count; i++)
        {
            StoreInfoBean storeInfo = listBedData[i];
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildItem.GetBuildType() == buildItemType)
            {
                listData.Add(storeInfo);
            }

        }
        return listData;
    }


    /// <summary>
    /// 编辑名字
    /// </summary>
    /// <param name="name"></param>
    public void OnEndEidtForName(string name)
    {
        customBedData.bedName = name;
    }

    /// <summary>
    /// 返回按钮
    /// </summary>
    public override void OnClickForBack()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.TownCarpenter));
    }

    /// <summary>
    /// 确认提交
    /// </summary>
    public void OnClickForSumbit()
    {
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (!gameData.HasEnoughMoney(customPriceL, customPriceM, customPriceS))
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        if (etBedName.text.Length <= 0)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1312));
            return;
        }

        DialogBean dialogData = new DialogBean();
        string moneyStr = "";
        if (customPriceL != 0)
        {
            moneyStr += customPriceL + GameCommonInfo.GetUITextById(16);
        }
        if (customPriceM != 0)
        {
            moneyStr += customPriceM + GameCommonInfo.GetUITextById(17);
        }
        if (customPriceS != 0)
        {
            moneyStr += customPriceS + GameCommonInfo.GetUITextById(18);
        }
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3103), moneyStr);
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
    }


    #region 获取数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listBedData = listData;
        rgBedType.SetPosition(0, true);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        BuildItemTypeEnum buildItemType = EnumUtil.GetEnum<BuildItemTypeEnum>(rbview.name);
        List<StoreInfoBean> listData = GetListBedDataByType(buildItemType);
        CreateBedDataList(listData);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 弹窗回调
    public override void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        base.Submit(dialogView, dialogBean);
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (dialogView as FindBedDialogView)
        {
            FindBedDialogView findBedDialog = dialogView as FindBedDialogView;
            gameData.AddBed(findBedDialog.buildBedData);
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1311));
        }
        else
        {
            if (!gameData.HasEnoughMoney(customPriceL, customPriceM, customPriceS))
            {
                uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
                return;
            }
            if (etBedName.text.Length <= 0)
            {
                uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1312));
                return;
            }
            //支付金钱
            gameData.PayMoney(customPriceL, customPriceM, customPriceS);
            //播放音效
            uiGameManager.audioHandler.PlaySound(AudioSoundEnum.Reward);

            DialogBean dialogData = new DialogBean();
            FindBedDialogView findBedDialog=(FindBedDialogView)uiGameManager.dialogManager.CreateDialog(DialogEnum.FindBed, this, dialogData);
            //如果幸运值生成数据
            gameData.userCharacter.GetAttributes(uiGameManager.gameItemsManager,out CharacterAttributesBean characterAttributes);
            BuildBedBean buildBedData = customBedData.RandomDataByLucky(characterAttributes.lucky);
            findBedDialog.SetData(buildBedData);

        }
    }

    public override void CloseUI()
    {
        base.CloseUI();

    }
    #endregion
}