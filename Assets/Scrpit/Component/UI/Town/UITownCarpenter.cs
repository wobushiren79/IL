using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownCarpenter : UIBaseOne, IStoreInfoView, IRadioGroupCallBack
{
    public RadioGroupView rgCerpenterType;

    public GameObject objCarpenterContent;
    public GameObject objCarpenterModelForExpansion;
    public GameObject objCarpenterModelForGoods;

    private StoreInfoController mStoreInfoController;
    private List<StoreInfoBean> mCarpenterListData;

    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
        mStoreInfoController.GetCarpenterInfo();
    }

    public new void Start()
    {
        base.Start();
        if (rgCerpenterType != null)
            rgCerpenterType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgCerpenterType.SetPosition(0, false);
        InitDataByType(0);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        InitDataByType(0);
    }

    public void InitDataByType(int type)
    {
        List<StoreInfoBean> listData = GetCerpenterListDataByMark(type);
        CreateCarpenterData(listData, type);
    }

    /// <summary>
    ///  根据备注类型获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetCerpenterListDataByMark(int type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mCarpenterListData == null)
            return listData;
        for (int i = 0; i < mCarpenterListData.Count; i++)
        {
            StoreInfoBean itemData = mCarpenterListData[i];
            if (itemData.mark_type.Equals(type))
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
    public void CreateCarpenterData(List<StoreInfoBean> listData, int type)
    {
        CptUtil.RemoveChildsByActive(objCarpenterContent.transform);
        if (listData == null || objCarpenterContent == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            GameObject itemObj;
            if (type == 0)
            {
                if (itemData.id > 300000 && itemData.id <= 300010)
                {
                    if (int.Parse(itemData.mark) - 1 == GetUIMananger<UIGameManager>().gameDataManager.gameData.GetInnBuildData().buildLevel)
                    {
                        itemObj = Instantiate(objCarpenterContent, objCarpenterModelForExpansion);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    itemObj = Instantiate(objCarpenterContent, objCarpenterModelForExpansion);
                }
            }
            else
            {
                itemObj = Instantiate(objCarpenterContent, objCarpenterModelForGoods);
            }
            ItemTownCerpenterCpt groceryCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
            groceryCpt.SetData(itemData);
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    #region 获取商店物品回调
    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData)
    {

    }
    public void GetAllStoreInfoFail()
    {

    }

    public void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mCarpenterListData = listData;
    }

    public void GetStoreInfoByTypeFail(StoreTypeEnum type)
    {
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        InitDataByType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
    }

    #endregion
}