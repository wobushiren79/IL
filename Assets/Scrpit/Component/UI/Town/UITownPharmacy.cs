using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownPharmacy : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
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
        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForPharmacy();
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

    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        this.listData = listData;
        CreateItemsList(listData);
    }
    #endregion
}