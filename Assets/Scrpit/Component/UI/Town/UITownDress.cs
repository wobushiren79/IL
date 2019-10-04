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

    private int mStyleType = 0;
    private int mPartType = 0;
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
        rgStyleType.SetPosition(0, false);
        rgPartType.SetPosition(0, false);
        InitDataByType(0, 0);
    }

    public void InitDataByType(int styleType, int partType)
    {
        List<StoreInfoBean> createListData = new List<StoreInfoBean>();
        switch (partType)
        {
            case 0:
                createListData = mClothesListData;
                break;
            case 1:
            case 2:
            case 3:
                createListData = GetClothesListDataByMark(partType + "");
                break;
        }
        CreateClothesData(GetClothesListDataByMarkType(styleType, createListData));
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetClothesListDataByMarkType(int styleType, List<StoreInfoBean> listStoreData)
    {
        if (styleType == 0)
        {
            return listStoreData;
        }
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (listStoreData == null)
            return listData;

        for (int i = 0; i < listStoreData.Count; i++)
        {
            StoreInfoBean itemData = listStoreData[i];
            if (itemData.mark_type == styleType)
            {
                listData.Add(itemData);
            }
        }
        return listData;
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
            GameObject itemObj = Instantiate(objGroceryContent, objGroceryModel);
            ItemTownDressStoreCpt clothesCpt = itemObj.GetComponent<ItemTownDressStoreCpt>();
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

    public void GetStoreInfoByTypeSuccess(int type,List<StoreInfoBean> listData)
    {
        mClothesListData = listData;
    }

    public void GetStoreInfoByTypeFail(int type)
    {
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        if (rgView == rgStyleType)
        {
            mStyleType = position;
        }
        else if (rgView == rgPartType)
        {
            mPartType = position;
        }
        InitDataByType(mStyleType, mPartType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion
}