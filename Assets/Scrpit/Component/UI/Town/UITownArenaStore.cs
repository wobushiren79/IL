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

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForArenaGoods();
    }

    public void InitDataByType(StoreForArenaGoodsTypeEnum type)
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
    public List<StoreInfoBean> GetListArenaGoodsByType(StoreForArenaGoodsTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (arenaStoreListData == null)
            return listData;

        foreach (StoreInfoBean itemStoreinfo in arenaStoreListData)
        {
            if ((StoreForArenaGoodsTypeEnum)itemStoreinfo.store_goods_type == type)
            {
                listData.Add(itemStoreinfo);
            }
        }
        return listData;
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
        InitDataByType(StoreForArenaGoodsTypeEnum.Dress);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        StoreForArenaGoodsTypeEnum goodsType= EnumUtil.GetEnum<StoreForArenaGoodsTypeEnum>(rbview.name);
        InitDataByType(goodsType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
    }
    #endregion
}