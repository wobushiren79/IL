using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownGuildStore : UIBaseOne , IStoreInfoView,IRadioGroupCallBack
{
    public GameObject objGuidStoreContent;
    public GameObject objGuidStoreModel;

    public RadioGroupView rgGuildStoreType;

    private StoreInfoController mStoreInfoController;
    private List<StoreInfoBean> mGuidStoreListData;

    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
        mStoreInfoController.GetGuildStoreInfo();
    }

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
        InitDataByType(0);
    }

    public void InitDataByType(int type)
    {
        switch (type)
        {
            case 0:
                CreateGroceryData(mGuidStoreListData);
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
        if (mGuidStoreListData == null)
            return listData;
        for (int i = 0; i < mGuidStoreListData.Count; i++)
        {
            StoreInfoBean itemData = mGuidStoreListData[i];
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
        CptUtil.RemoveChildsByActive(objGuidStoreContent.transform);
        if (listData == null || objGuidStoreContent == null || objGuidStoreModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //检测是否满足前置成就
            if (!itemData.CheckPreAchIds(GetUIMananger<UIGameManager>().gameDataManager.gameData))
            {
                continue;
            }
            GameObject itemObj = Instantiate(objGuidStoreModel, objGuidStoreContent.transform);
            itemObj.SetActive(true);
            ItemGameGuildStoreCpt groceryCpt = itemObj.GetComponent<ItemGameGuildStoreCpt>();
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
        mGuidStoreListData = listData;
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