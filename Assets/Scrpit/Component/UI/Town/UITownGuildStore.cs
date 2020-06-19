using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownGuildStore : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public GameObject objGuidStoreContent;
    public GameObject objGuidStoreForItemsModel;
    public GameObject objGuidStoreForBuildModel;

    public RadioGroupView rgGuildStoreType;
    
    private List<StoreInfoBean> mGuidStoreListData;

    public new void Start()
    {
        base.Start();
        if (rgGuildStoreType != null)
            rgGuildStoreType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgGuildStoreType.SetPosition(0, false);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForGuildGoods();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="type"></param>
    public void InitDataByType(StoreForGuildGoodsTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        listData = GetListDataByType(type);
        CreateGroceryData(listData);
    }

    /// <summary>
    /// 通过类型获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected List<StoreInfoBean> GetListDataByType(StoreForGuildGoodsTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mGuidStoreListData == null)
            return listData;
        for (int i = 0; i < mGuidStoreListData.Count; i++)
        {
            StoreInfoBean itemData = mGuidStoreListData[i];
            if (itemData.store_goods_type== (int)type)
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
    public void CreateGroceryData(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGuidStoreContent.transform);
        if (listData == null || objGuidStoreContent == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //检测是否满足前置成就
            if (!itemData.CheckPreAchIds(uiGameManager.gameDataManager.gameData))
            {
                continue;
            }
            GameObject itemObj = null;
            if (itemData.mark_type == 1)
            {
                 itemObj = Instantiate(objGuidStoreContent, objGuidStoreForItemsModel);
                ItemTownStoreForGoodsCpt groceryCpt = itemObj.GetComponent<ItemTownStoreForGoodsCpt>();
                groceryCpt.SetData(itemData);
            }
            else if (itemData.mark_type == 2)
            {
                itemObj = Instantiate(objGuidStoreContent, objGuidStoreForBuildModel);
                ItemTownCerpenterCpt cerpenterCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
                cerpenterCpt.SetData(itemData);
            }
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }


    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mGuidStoreListData = listData;
        InitDataByType(StoreForGuildGoodsTypeEnum.Menu);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForGuildGoodsTypeEnum type = EnumUtil.GetEnum<StoreForGuildGoodsTypeEnum>(view.name);
        InitDataByType(type);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion
}