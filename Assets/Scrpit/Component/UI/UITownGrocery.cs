using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITownGrocery : UIBaseOne, IStoreInfoView, IRadioGroupCallBack
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgGroceryType;

    private StoreInfoController mStoreInfoController;
    private List<StoreInfoBean> mGroceryListData;

    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
        mStoreInfoController.GetGroceryInfo();
    }

    public new void Start()
    {
        base.Start();
        if (rgGroceryType != null)
            rgGroceryType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgGroceryType.SetPosition(0,false);
        InitDataByType(0);
    }

    public void InitDataByType(int type)
    {
        switch (type)
        {
            case 0:
                CreateGroceryData(mGroceryListData);
                break;
            case 1:
                CreateGroceryData(GetGroceryListDataByMark("12"));
                break;
            case 2:
                CreateGroceryData(GetGroceryListDataByMark("11"));
                break;
        }
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetGroceryListDataByMark(string mark)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mGroceryListData == null)
            return listData;
        for(int i=0;i< mGroceryListData.Count; i++)
        {
            StoreInfoBean itemData= mGroceryListData[i];
            if (itemData.mark.Equals(mark))
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
            GameObject itemObj = Instantiate(objGroceryModel, objGroceryContent.transform);
            itemObj.SetActive(true);
            ItemGameGroceryCpt groceryCpt = itemObj.GetComponent<ItemGameGroceryCpt>();
            groceryCpt.SetData(itemData);
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }


    #region 商店数据回调
    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData)
    {
    }

    public void GetAllStoreInfoFail()
    {
    }

    public void GetStoreInfoByTypeSuccess(List<StoreInfoBean> listData)
    {
        mGroceryListData = listData;
    }

    public void GetStoreInfoByTypeFail()
    {
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