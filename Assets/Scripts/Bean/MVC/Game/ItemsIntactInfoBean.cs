using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class ItemsIntactInfoBean : BaseBean
{
	/// <summary>
	///套装数量
	/// </summary>
	public int intact_number;
}
public partial class ItemsIntactInfoCfg : BaseCfg<long, ItemsIntactInfoBean>
{
	public static string fileName = "ItemsIntactInfo";
	protected static Dictionary<long, ItemsIntactInfoBean> dicData = null;
	public static Dictionary<long, ItemsIntactInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static ItemsIntactInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static ItemsIntactInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			ItemsIntactInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(ItemsIntactInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, ItemsIntactInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			ItemsIntactInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
