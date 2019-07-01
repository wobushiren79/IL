using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameItemsManager : BaseManager, IItemsInfoView
{
    //装备图标
    public IconBeanDictionary listItemsIcon;
    //装备控制
    public ItemsInfoController itemsInfoController;
    //装备数据
    public Dictionary<long,ItemsInfoBean> listDataItems;

    private void Awake()
    {
        itemsInfoController = new ItemsInfoController(this, this);
    }

    /// <summary>
    /// 获取所有的帽子数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetHatList()
    {
        return GetItemsListByType(1);
    }
    /// <summary>
    /// 获取所有服装数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetClothesList()
    {
        return GetItemsListByType(2);
    }
    /// <summary>
    /// 获取所有鞋子信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetShoesList()
    {
        return GetItemsListByType(3);
    }

    /// <summary>
    /// 根据装备类型获取装备信息
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsListByType(int type)
    {
        List<ItemsInfoBean> tempList = new List<ItemsInfoBean>();
        if (listDataItems == null)
            return tempList;

        foreach (long key in this.listDataItems.Keys) {
            ItemsInfoBean itemData = this.listDataItems[key];
            if (itemData.items_type == type)
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
        if (listDataItems == null||id==0)
            return null;
        return this.listDataItems[id];
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