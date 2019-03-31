using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterDressManager : BaseManager,IEquipInfoView
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
        equipInfoController = new EquipInfoController(this,this);
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
    /// 根据名字获取衣服
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconClothes);
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