using System;
using System.Collections.Generic;
public partial class ItemsInfoBean
{
    public ItemsInfoBean()
    {

    }

    public ItemsInfoBean(long id)
    {
        this.id = id;
    }

    /// <summary>
    /// 获取类型
    /// </summary>
    /// <returns></returns>
    public GeneralEnum GetItemsType()
    {
        return (GeneralEnum)items_type;
    }

    /// <summary>
    /// 获取物品稀有度
    /// </summary>
    /// <returns></returns>
    public RarityEnum GetItemRarity()
    {
        return (RarityEnum)rarity;
    }
}
public partial class ItemsInfoCfg
{
}
