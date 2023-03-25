using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
public class GameItemsManager : BaseManager, IItemsInfoView
{
    //物品图标
    public SpriteAtlas itemsAtlas;
    //物品图标
    public IconBeanDictionary dicItemsIcon = new IconBeanDictionary();
    //物品动画列表
    public AnimBeanDictionary dicItemsAnim = new AnimBeanDictionary();

    //物品控制
    public ItemsInfoController itemsInfoController;
    //物品数据
    public Dictionary<long, ItemsInfoBean> listDataItems;

    public Dictionary<string, Texture2DArray> listItemsTex = new Dictionary<string, Texture2DArray>();
    public void Awake()
    {
        itemsInfoController = new ItemsInfoController(this, this);
        itemsInfoController.GetAllItemsInfo();
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
    /// 获取所有其他信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetOtherList()
    {
        return GetItemsListByType(GeneralEnum.Other);
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
        return GetSpriteDataByName(name);
    }

    public Texture2DArray GetItemsTextureByName(string name, int animLength)
    {
        if (listItemsTex.TryGetValue(name, out Texture2DArray texture2DArray))
        {
            return texture2DArray;
        }
        for (int i = 0; i < animLength; i++)
        {
            Sprite itemsp;
            if (animLength == 1)
            {
                itemsp = GetItemsSpriteByName(name);
            }
            else
            {
                itemsp = GetItemsSpriteByName($"{name}_{i}");
            }

            if (itemsp == null)
                return null;
            Texture2D itemTex = TextureUtil.SpriteToTexture2D(itemsp);

            if (texture2DArray == null)
            {
                texture2DArray = new Texture2DArray(itemTex.width, itemTex.height, animLength, itemTex.format, true, false);
                texture2DArray.filterMode = FilterMode.Point;
                texture2DArray.wrapMode = TextureWrapMode.Clamp;
            }
            texture2DArray.SetPixels(itemTex.GetPixels(), i);
        }
        texture2DArray.Apply();
        listItemsTex.Add(name, texture2DArray);
        return texture2DArray;
    }

    /// <summary>
    /// 根据名字获取物品动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetItemsAnimClipByName(string name)
    {
        return GetAnimClipByName(name);
    }

    protected Sprite GetSpriteDataByName(string name)
    {
        return GetSpriteByName(dicItemsIcon, ref itemsAtlas, "AtlasForItems", ProjectConfigInfo.ASSETBUNDLE_SPRITEATLAS, name, "Assets/Texture/SpriteAtlas/AtlasForItems.spriteatlas");
    }

    protected AnimationClip GetAnimClipByName(string name)
    {
        return GetModel(dicItemsAnim, "anim/items", name);
    }

    #region   装备获取回调
    public void GetItemsInfoSuccess(List<ItemsInfoBean> listData)
    {
        this.listDataItems = new Dictionary<long, ItemsInfoBean>();
        for (int i = 0; i < listData.Count; i++)
        {
            ItemsInfoBean itemData = listData[i];
            this.listDataItems.Add(itemData.id, itemData);
        }
    }

    public void GetItemsInfoFail()
    {
    }
    #endregion
}