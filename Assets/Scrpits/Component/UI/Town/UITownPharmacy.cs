using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public partial class UITownPharmacy : UIBaseOne, IRadioGroupCallBack
{
    public GameObject objItemsContainer;
    public GameObject objItemsModel;

    public RadioGroupView rgType;

    protected List<StoreInfoBean> listMedicineData;
    protected List<StoreInfoBean> listSeedData;

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
            GameObject objItem = Instantiate(objItemsContainer, objItemsModel);
            ItemTownStoreForGoodsCpt itemCpt = objItem.GetComponent<ItemTownStoreForGoodsCpt>();
            itemCpt.SetData(storeInfo);
        }
    }

    /// <summary>
    /// 设置商店数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetStoreData(List<StoreInfoBean> listData)
    {
        listMedicineData = new List<StoreInfoBean>();
        listSeedData = new List<StoreInfoBean>();
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            if (itemData.store_goods_type == 1)
            {
                listMedicineData.Add(itemData);
            }
            else if (itemData.store_goods_type == 2)
            {
                listSeedData.Add(itemData);
            }
        }
        CreateItemsList(listMedicineData);
    }
    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        if (rbview == ui_Medicine)
        {
            CreateItemsList(listMedicineData);
        }
        else if (rbview == ui_Seed)
        {
            CreateItemsList(listSeedData);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

}