﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameItemsManager : BaseManager, IItemsInfoView
{
    //装备图标
    public IconBeanDictionary listItemsIcon;
    //装备控制
    public ItemsInfoController itemsInfoController;
    //装备数据
    public Dictionary<long, ItemsInfoBean> listDataItems;
    //动画列表
    public AnimBeanDictionary listItemsAnim;



    public void Awake()
    {
        itemsInfoController = new ItemsInfoController(this, this);
    }

    /// <summary>
    /// 获取所有的帽子数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetHatList()
    {
        return GetItemsListByType(GeneralEnum.Hat);
    }

    /// <summary>
    /// 获取所有服装数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetClothesList()
    {
        return GetItemsListByType(GeneralEnum.Clothes);
    }

    /// <summary>
    /// 获取所有药数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetMedicineList()
    {
        return GetItemsListByType(GeneralEnum.Medicine);
    }
    /// <summary>
    /// 获取所有鞋子信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetShoesList()
    {
        return GetItemsListByType(GeneralEnum.Shoes);
    }

    /// <summary>
    /// 获取所有物品
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetAllItems()
    {
        List<ItemsInfoBean> tempList = new List<ItemsInfoBean>();
        if (listDataItems == null)
            return tempList;

        foreach (long key in this.listDataItems.Keys)
        {
            ItemsInfoBean itemData = this.listDataItems[key];
            tempList.Add(itemData);
        };
        return tempList;
    }

    /// <summary>
    /// 根据装备类型获取装备信息
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsListByType(GeneralEnum type)
    {
        List<ItemsInfoBean> tempList = new List<ItemsInfoBean>();
        if (listDataItems == null)
            return tempList;

        foreach (long key in this.listDataItems.Keys)
        {
            ItemsInfoBean itemData = this.listDataItems[key];
            if (itemData.items_type == (int)type)
            {
                tempList.Add(itemData);
            }
        };
        return tempList;
    }

    /// <summary>
    /// 根据装备ID获取装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsById(long id)
    {
        if (listDataItems == null || id == 0)
            return null;
        if (this.listDataItems.TryGetValue(id, out ItemsInfoBean itemData))
            return itemData;
        return
            null;
    }
    /// <summary>
    /// 根据装备ID获取装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsByIds(List<long> ids)
    {
        List<ItemsInfoBean> listData = new List<ItemsInfoBean>();
        if (listDataItems == null || ids == null)
            return listData;
        foreach (long id in ids)
        {
            if (this.listDataItems.TryGetValue(id, out ItemsInfoBean itemData))
                listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 根据装备ID获取装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsByIds(long[] ids)
    {
        List<ItemsInfoBean> listData = new List<ItemsInfoBean>();
        if (listDataItems == null || ids == null)
            return listData;
        foreach (long id in ids)
        {
            if (this.listDataItems.TryGetValue(id, out ItemsInfoBean itemData))
                listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 根据装备IDs获取装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsById(long[] ids)
    {
        List<ItemsInfoBean> listData = new List<ItemsInfoBean>();
        if (listDataItems == null || ids == null)
            return listData;
        foreach (long id in ids)
        {
            if (this.listDataItems.TryGetValue(id, out ItemsInfoBean itemData))
                listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 根据装备信息获取装备
    /// </summary>
    /// <param name="itemBeans"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsByItemBean(List<ItemBean> itemBeans)
    {
        List<ItemsInfoBean> listData = new List<ItemsInfoBean>();
        if (listDataItems == null || itemBeans == null)
            return listData;
        foreach (ItemBean itemBean in itemBeans)
        {
            if (this.listDataItems.TryGetValue(itemBean.itemId, out ItemsInfoBean itemData))
                listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 通过名字获取Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemsSpriteByName(string name)
    {
        return GetSpriteByName(name, listItemsIcon);
    }

    /// <summary>
    /// 根据名字获取物品动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetItemsAnimClipByName(string name)
    {
        return GetAnimClipByName(name, listItemsAnim);
    }

    #region   装备获取回调
    public void GetItemsInfoSuccess(List<ItemsInfoBean> listData)
    {
        this.listDataItems = new Dictionary<long, ItemsInfoBean>();
        foreach (ItemsInfoBean itemData in listData)
        {
            this.listDataItems.Add(itemData.id, itemData);
        }
    }

    public void GetItemsInfoFail()
    {
    }
    #endregion
}