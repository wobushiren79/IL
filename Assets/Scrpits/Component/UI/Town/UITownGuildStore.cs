﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class UITownGuildStore : UIBaseOne, IRadioGroupCallBack
{
    public GameObject objGuidStoreContent;
    public GameObject objGuidStoreForItemsModel;
    public GameObject objGuidStoreForBuildModel;

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

        Action<List<StoreInfoBean>> callBack = SetStoreData;
        StoreInfoHandler.Instance.manager.GetStoreInfoForGuildGoods(callBack);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="type"></param>
    public void InitDataByType(StoreForGuildGoodsTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        listData = GetListDataByType(type);
        CreateGroceryData(listData);
    }

    /// <summary>
    /// 通过类型获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected List<StoreInfoBean> GetListDataByType(StoreForGuildGoodsTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mGuidStoreListData == null)
            return listData;
        for (int i = 0; i < mGuidStoreListData.Count; i++)
        {
            StoreInfoBean itemData = mGuidStoreListData[i];
            if (itemData.store_goods_type== (int)type)
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
        if (listData == null || objGuidStoreContent == null)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //检测是否满足前置成就
            if (!itemData.CheckPreAchIds(gameData))
            {
                continue;
            }
            GameObject itemObj = null;
            if (itemData.mark_type == 1)
            {
                 itemObj = Instantiate(objGuidStoreContent, objGuidStoreForItemsModel);
                ItemTownStoreForGoodsCpt groceryCpt = itemObj.GetComponent<ItemTownStoreForGoodsCpt>();
                groceryCpt.SetData(itemData);
            }
            else if (itemData.mark_type == 2)
            {
                itemObj = Instantiate(objGuidStoreContent, objGuidStoreForBuildModel);
                ItemTownCerpenterCpt cerpenterCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
                cerpenterCpt.SetData(itemData);
            }
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    /// <summary>
    /// 设置商店数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetStoreData(List<StoreInfoBean> listData)
    {
        mGuidStoreListData = listData;
        InitDataByType(StoreForGuildGoodsTypeEnum.Menu);
    }


    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        StoreForGuildGoodsTypeEnum type = view.name.GetEnum<StoreForGuildGoodsTypeEnum>();
        InitDataByType(type);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion
}