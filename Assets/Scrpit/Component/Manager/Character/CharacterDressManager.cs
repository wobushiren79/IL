using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterDressManager : BaseManager, IEquipInfoView
{
    //面具列表
    public List<IconBean> listIconMask;
    //鞋子列表
    public List<IconBean> listIconShoes;
    //衣服列表
    public List<IconBean> listIconClothes;
    //帽子列表
    public List<IconBean> listIconHat;

    //装备数据
    public List<EquipInfoBean> listDataEquip;

    //装备控制
    public EquipInfoController equipInfoController;

    private void Awake()
    {
        equipInfoController = new EquipInfoController(this, this);
    }

    /// <summary>
    /// 获取所有的帽子数据
    /// </summary>
    /// <returns></returns>
    public List<EquipInfoBean> GetHatList()
    {
        return GetEquipListByType(1);
    }
    /// <summary>
    /// 获取所有服装数据
    /// </summary>
    /// <returns></returns>
    public List<EquipInfoBean> GetClothesList()
    {
        return GetEquipListByType(2);
    }
    /// <summary>
    /// 获取所有鞋子信息
    /// </summary>
    /// <returns></returns>
    public List<EquipInfoBean> GetShoesList()
    {
        return GetEquipListByType(3);
    }


    /// <summary>
    /// 根据装备类型获取装备信息
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<EquipInfoBean> GetEquipListByType(int type)
    {
        List<EquipInfoBean> tempList = new List<EquipInfoBean>();
        if (listDataEquip == null)
            return tempList;
        for (int i = 0; i < listDataEquip.Count; i++)
        {
            EquipInfoBean itemData = listDataEquip[i];
            if (itemData.equip_type == type)
            {
                tempList.Add(itemData);
            }
        }
        return tempList;
    }

    /// <summary>
    /// 根据装备ID获取装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipInfoBean GetEquipById(long id)
    {
        if (listDataEquip == null)
            return null;
        for (int i = 0; i < listDataEquip.Count; i++)
        {
            EquipInfoBean itemData = listDataEquip[i];
            if (itemData.id == id)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据名字获取面具
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMaskSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconMask);
    }

    /// <summary>
    /// 根据名字获取鞋子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconShoes);
    }

    /// <summary>
    /// 根据ID获取鞋子图标
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteById(long id)
    {
        EquipInfoBean equipInfoBean = GetEquipById(id);
        if (equipInfoBean == null)
            return null;
        return GetSpriteByName(equipInfoBean.icon_key, listIconShoes);
    }

    /// <summary>
    /// 根据名字获取衣服图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconClothes);
    }

    /// <summary>
    /// 根据ID获取衣服图标
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteById(long id)
    {
        EquipInfoBean equipInfoBean = GetEquipById(id);
        if (equipInfoBean == null)
            return null;
        return GetSpriteByName(equipInfoBean.icon_key, listIconClothes);
    }

    /// <summary>
    /// 根据名字获取帽子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconHat);
    }

    /// <summary>
    /// 根据ID获取帽子图标
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteById(long id)
    {
        EquipInfoBean equipInfoBean = GetEquipById(id);
        if (equipInfoBean == null)
            return null;
        return GetSpriteByName(equipInfoBean.icon_key, listIconHat);
    }

    #region   装备获取回调
    public void GetEquipInfoSuccess(List<EquipInfoBean> listData)
    {
        this.listDataEquip = listData;
    }

    public void GetEquipInfoFail()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}