using System;
using System.Collections.Generic;
[Serializable]
public partial class SeedInfoBean : BaseBean
{
	/// <summary>
	///道具ID
	/// </summary>
	public int item_id;
	/// <summary>
	///tilemap数据
	/// </summary>
	public string tile;
	/// <summary>
	///每个周期的天数
	/// </summary>
	public int growup_oneloopday;
	/// <summary>
	///总共的成长周期
	/// </summary>
	public int growup_totleloop;
	/// <summary>
	///收获的道具
	/// </summary>
	public string get_items;
	/// <summary>
	///收获的食材
	/// </summary>
	public string get_ingredient;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class SeedInfoCfg : BaseCfg<long, SeedInfoBean>
{
	public static string fileName = "SeedInfo";
	protected static Dictionary<long, SeedInfoBean> dicData = null;
	public static Dictionary<long, SeedInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			SeedInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static SeedInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			SeedInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(SeedInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, SeedInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			SeedInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
