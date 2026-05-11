using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class UITownArenaStore : UIBaseOne, IRadioGroupCallBack
{
    public RadioGroupView rgType;
    public GameObject objGoodsContainer;
    public GameObject objGoodsForItemsModel;
    public GameObject objGoodsForBuildModel;
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

        Action<List<StoreInfoBean>> callBack = SetStoreData;
        StoreInfoHandler.Instance.manager.GetStoreInfoForArenaGoods(callBack);
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
        GameObject objItem = null;
        if (storeInfo.mark_type == 1)
        {
            objItem = Instantiate(objGoodsContainer, objGoodsForItemsModel);
            ItemTownStoreForGoodsCpt goodsItem = objItem.GetComponent<ItemTownStoreForGoodsCpt>();
            goodsItem.SetData(storeInfo);
        }
        else if (storeInfo.mark_type == 2)
        {
            objItem = Instantiate(objGoodsContainer, objGoodsForBuildModel);
            ItemTownCerpenterCpt cerpenterCpt = objItem.GetComponent<ItemTownCerpenterCpt>();
            cerpenterCpt.SetData(storeInfo);
        }
        objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
    }


    /// <summary>
    /// 设置商店数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetStoreData(List<StoreInfoBean> listData)
    {
        arenaStoreListData = listData;
        InitDataByType(StoreForArenaGoodsTypeEnum.Dress);
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForArenaGoodsTypeEnum goodsType = rbview.name.GetEnum<StoreForArenaGoodsTypeEnum>();
        InitDataByType(goodsType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
    }
    #endregion
}