using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class UITownCarpenter : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public Button btCustomBed;
    public RadioGroupView rgCerpenterType;

    public GameObject objCarpenterContent;
    public GameObject objCarpenterModel;

    private List<StoreInfoBean> mCarpenterListData;
    protected StoreForCarpenterTypeEnum selectType;
    public override void Awake()
    {
        base.Awake();
        if (rgCerpenterType != null)
            rgCerpenterType.SetCallBack(this);
        if (btCustomBed != null)
            btCustomBed.onClick.AddListener(OnClickForCustomBed);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgCerpenterType.SetPosition(0, false);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForCarpenter();
        SetCustomBed();
    }




    public override void RefreshUI()
    {
        base.RefreshUI();
        InitDataByType(selectType);
        SetCustomBed();
    }


    public void InitDataByType(StoreForCarpenterTypeEnum type)
    {
        List<StoreInfoBean> listData = GetCerpenterListDataByType(type);
        CreateCarpenterData(listData, type);
    }

    /// <summary>
    /// 打开自定义床单UI
    /// </summary>
    public void OnClickForCustomBed()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiGameManager.OpenUIAndCloseOther(UIEnum.GameCustomBed);
    }

    public void SetCustomBed()
    {
        //设置是否展示定制床位功能
        InnBuildBean innBuild = uiGameManager.gameDataManager.gameData.GetInnBuildData();
        //innBuild.GetInnSize(2, out int innWidth, out int innHeight, out int offsetHeight);
        if (innBuild.buildSecondLevel != 0)
        {
            btCustomBed.gameObject.SetActive(true);
        }
        else
        {
            btCustomBed.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///  根据备注类型获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetCerpenterListDataByType(StoreForCarpenterTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mCarpenterListData == null)
            return listData;
        for (int i = 0; i < mCarpenterListData.Count; i++)
        {
            StoreInfoBean itemData = mCarpenterListData[i];
            if (itemData.store_goods_type == (int)type)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建商品列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateCarpenterData(List<StoreInfoBean> listData, StoreForCarpenterTypeEnum type)
    {
        CptUtil.RemoveChildsByActive(objCarpenterContent.transform);
        if (listData == null || objCarpenterContent == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //如果扩建 
            InnBuildBean innBuild = uiGameManager.gameDataManager.gameData.GetInnBuildData();
            if (itemData.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                if (itemData.mark_type == 1)
                {
                    if (innBuild.buildLevel + 1 != int.Parse(itemData.mark))
                    {
                        continue;
                    }
                }
                else if (itemData.mark_type == 2)
                {
                    if (innBuild.buildSecondLevel + 1 != int.Parse(itemData.mark) || innBuild.buildSecondLevel > innBuild.buildLevel)
                    {
                        continue;
                    }
                }
            }

            GameObject itemObj = Instantiate(objCarpenterContent, objCarpenterModel);
            ItemTownCerpenterCpt cerpenterCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
            cerpenterCpt.SetData(itemData);
            //itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    /// <summary>
    /// 创建出售数据
    /// </summary>
    /// <param name="pickForSellDialog"></param>
    protected override void CreatePickForSellDialogView(out PickForSellDialogView pickForSellDialog)
    {
        base.CreatePickForSellDialogView(out pickForSellDialog);
        pickForSellDialog.SetData(mCarpenterListData);
    }

    #region 获取商店物品回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mCarpenterListData = listData;
        selectType = StoreForCarpenterTypeEnum.Expansion;
        InitDataByType(selectType);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);

        selectType = EnumUtil.GetEnum<StoreForCarpenterTypeEnum>(rbview.name);
        InitDataByType(selectType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}