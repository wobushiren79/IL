using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownDress : UIBaseOne, IStoreInfoView, IRadioGroupCallBack
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgStyleType;
    public RadioGroupView rgPartType;

    private StoreInfoController mStoreInfoController;
    private List<StoreInfoBean> mClothesListData;

    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
        mStoreInfoController.GetClothesStoreInfo();
    }

    public new void Start()
    {
        base.Start();
        if (rgStyleType != null)
            rgStyleType.SetCallBack(this);
        if (rgPartType != null)
            rgPartType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgStyleType.SetPosition(0,false);
        rgPartType.SetPosition(0, false);
        InitDataByType(0,0);
    }

    public void InitDataByType(int styleType,int partType)
    {
        switch (partType)
        {
            case 0:
                CreateClothesData(mClothesListData);
                break;
            case 1:
            case 2:
            case 3:
                CreateClothesData(GetClothesListDataByMark(partType+""));
                break;
        }
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetClothesListDataByMark(string mark)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mClothesListData == null)
            return listData;
        for (int i = 0; i < mClothesListData.Count; i++)
        {
            StoreInfoBean itemData = mClothesListData[i];
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
    public void CreateClothesData(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGroceryContent.transform);
        if (listData == null || objGroceryContent == null || objGroceryModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            GameObject itemObj = Instantiate(objGroceryModel, objGroceryContent.transform);
            itemObj.SetActive(true);
            ItemGameDressStoreCpt clothesCpt = itemObj.GetComponent<ItemGameDressStoreCpt>();
            clothesCpt.SetData(itemData);
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
        mClothesListData = listData;
    }

    public void GetStoreInfoByTypeFail()
    {
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(int position, RadioButtonView view)
    {
        InitDataByType(0,position);
    }

    public void RadioButtonUnSelected(int position, RadioButtonView view)
    {
    }
    #endregion
}