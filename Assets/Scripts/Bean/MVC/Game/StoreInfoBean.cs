using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class StoreInfoBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///类型 9市场
	/// </summary>
	public int type;
	/// <summary>
	///商店商品类型（不同商店有不同类型）
	/// </summary>
	public int store_goods_type;
	/// <summary>
	///获的 购买一次获得的数量
	/// </summary>
	public int get_number;
	/// <summary>
	///价格
	/// </summary>
	public long price_l;
	/// <summary>
	///价格
	/// </summary>
	public long price_m;
	/// <summary>
	///价格
	/// </summary>
	public long price_s;
	/// <summary>
	///金币
	/// </summary>
	public long guild_coin;
	/// <summary>
	///奖杯
	/// </summary>
	public long trophy_elementary;
	/// <summary>
	///奖杯
	/// </summary>
	public long trophy_intermediate;
	/// <summary>
	///奖杯
	/// </summary>
	public long trophy_advanced;
	/// <summary>
	///奖杯
	/// </summary>
	public long trophy_legendary;
	/// <summary>
	///key
	/// </summary>
	public string icon_key;
	/// <summary>
	///其他
	/// </summary>
	public string mark;
	/// <summary>
	///其他
	/// </summary>
	public long mark_id;
	/// <summary>
	///其他
	/// </summary>
	public int mark_type;
	/// <summary>
	///其他
	/// </summary>
	public int mark_x;
	/// <summary>
	///其他
	/// </summary>
	public int mark_y;
	/// <summary>
	///前置成就ID
	/// </summary>
	public string pre_ach_ids;
	/// <summary>
	///前置数据
	/// </summary>
	public string pre_data;
	/// <summary>
	///小游戏前置条件
	/// </summary>
	public string pre_data_minigame;
	/// <summary>
	///奖励
	/// </summary>
	public string reward_data;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(StoreInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(StoreInfoCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class StoreInfoCfg : BaseCfg<long, StoreInfoBean>
{
	public static string fileName = "StoreInfo";
	protected static Dictionary<long, StoreInfoBean> dicData = null;
	public static Dictionary<long, StoreInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static StoreInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static StoreInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			StoreInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(StoreInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, StoreInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			StoreInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
