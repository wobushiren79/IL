using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownArenaStore : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public RadioGroupView rgType;
    public GameObject objGoodsContainer;
    public GameObject objGoodsModel;

    public List<StoreInfoBean> arenaStoreListData;

    public new void Start()
    {
        base.Start();
        if (rgType != null)
            rgType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);

        StoreInfoManager storeInfoManager = GetUIManager<UIGameManager>().storeInfoManager;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForArenaGoods();
    }

    public void InitDataByType(int type)
    {
        CptUtil.RemoveChildsByActive(objGoodsContainer);
        List<StoreInfoBean> listData = GetListArenaGoodsByType(type);
        foreach (StoreInfoBean itemData in listData)
        {
            CreateArenaGoodsItem(itemData);
        }
    }

    /// <summary>
    /// 按照类型获取商品
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetListArenaGoodsByType(int type)
    {
        //TODO 首先按照是否是建筑材料区分
        switch (type)
        {
            case 0:
                return arenaStoreListData;
        }
        return arenaStoreListData;
    }
    
    /// <summary>
    /// 创建商品ITEM
    /// </summary>
    /// <param name="storeInfo"></param>
    public void CreateArenaGoodsItem(StoreInfoBean storeInfo)
    {
        GameObject objItem = Instantiate(objGoodsContainer, objGoodsModel);
        ItemTownStoreForGoodsCpt goodsItem= objItem.GetComponent<ItemTownStoreForGoodsCpt>();
        goodsItem.SetData(storeInfo);
    }

    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        arenaStoreListData = listData;
        InitDataByType(0);
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