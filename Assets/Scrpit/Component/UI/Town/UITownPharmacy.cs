using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class UITownPharmacy : UIBaseOne, IRadioGroupCallBack
{
    public GameObject objItemsContainer;
    public GameObject objItemsModel;

    public RadioGroupView rgType;

    protected List<StoreInfoBean> listData;
    public override void Awake()
    {
        base.Awake();
        rgType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);

        Action<List<StoreInfoBean>> callBack = SetStoreData;
        StoreInfoHandler.Instance.manager.GetStoreInfoForPharmacy(callBack);
    }

    /// <summary>
    /// 创建列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateItemsList(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objItemsContainer);
        if (listData == null)
            return;
        foreach (StoreInfoBean storeInfo in listData)
        {
            GameObject objItem=  Instantiate(objItemsContainer, objItemsModel);
            ItemTownStoreForGoodsCpt itemCpt= objItem.GetComponent<ItemTownStoreForGoodsCpt>();
            itemCpt.SetData(storeInfo);
        }
    }
    
    /// <summary>
    /// 设置商店数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetStoreData(List<StoreInfoBean> listData)
    {
        this.listData = listData;
        CreateItemsList(listData);
    }
    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        switch (rbview.name)
        {
            case "Medicine":
                CreateItemsList(listData);
                break;
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

}