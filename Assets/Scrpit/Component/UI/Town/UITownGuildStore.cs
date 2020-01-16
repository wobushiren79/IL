using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownGuildStore : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public GameObject objGuidStoreContent;
    public GameObject objGuidStoreModel;

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

        StoreInfoManager storeInfoManager = GetUIManager<UIGameManager>().storeInfoManager;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForGuildGoods();
    }

    public void InitDataByType(int type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        switch (type)
        {
            case 0:
                listData = mGuidStoreListData;
                break;
        }
        CreateGroceryData(listData);
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    //public List<StoreInfoBean> GetGroceryListDataByMark(string mark)
    //{
    //    List<StoreInfoBean> listData = new List<StoreInfoBean>();
    //    if (mGuidStoreListData == null)
    //        return listData;
    //    for (int i = 0; i < mGuidStoreListData.Count; i++)
    //    {
    //        StoreInfoBean itemData = mGuidStoreListData[i];
    //        if (itemData.mark.Equals(mark))
    //        {
    //            listData.Add(itemData);
    //        }
    //    }
    //    return listData;
    //}

    /// <summary>
    /// 创建商品列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateGroceryData(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGuidStoreContent.transform);
        if (listData == null || objGuidStoreContent == null || objGuidStoreModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //检测是否满足前置成就
            if (!itemData.CheckPreAchIds(GetUIManager<UIGameManager>().gameDataManager.gameData))
            {
                continue;
            }
            GameObject itemObj = Instantiate(objGuidStoreContent, objGuidStoreModel);
            ItemTownStoreForGoodsCpt groceryCpt = itemObj.GetComponent<ItemTownStoreForGoodsCpt>();
            groceryCpt.SetData(itemData);
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }


    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mGuidStoreListData = listData;
        InitDataByType(0);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        InitDataByType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion
}