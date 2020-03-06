using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITownGrocery : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgGroceryType;

    private List<StoreInfoBean> mGroceryListData;

    public new void Start()
    {
        base.Start();
        if (rgGroceryType != null)
            rgGroceryType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgGroceryType.SetPosition(0, false);
        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForGrocery();
    }

    public void InitDataByType(StoreForGroceryTypeEnum type)
    {
        List<StoreInfoBean> listData = GetGroceryListDataByType(type);
        CreateGroceryData(listData);
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetGroceryListDataByType(StoreForGroceryTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mGroceryListData == null)
            return listData;
        for (int i = 0; i < mGroceryListData.Count; i++)
        {
            StoreInfoBean itemData = mGroceryListData[i];
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
    public void CreateGroceryData(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGroceryContent.transform);
        if (listData == null || objGroceryContent == null || objGroceryModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            GameObject itemObj = Instantiate(objGroceryContent, objGroceryModel);
            ItemTownStoreForGoodsCpt goodsCpt = itemObj.GetComponent<ItemTownStoreForGoodsCpt>();
            goodsCpt.SetData(itemData);
        }
    }

    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mGroceryListData = listData;
        InitDataByType(StoreForGroceryTypeEnum.Menu);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForGroceryTypeEnum type = EnumUtil.GetEnum<StoreForGroceryTypeEnum>(view.name);
        InitDataByType(type);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
    }
    #endregion
}