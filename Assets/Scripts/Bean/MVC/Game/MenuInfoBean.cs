using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class MenuInfoBean : BaseBean
{
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///动画名称
	/// </summary>
	public string anim_key;
	/// <summary>
	///烹饪时间
	/// </summary>
	public float cook_time;
	/// <summary>
	///油盐类
	/// </summary>
	public int ing_oilsalt;
	/// <summary>
	///肉类
	/// </summary>
	public int ing_meat;
	/// <summary>
	///河鲜类
	/// </summary>
	public int ing_riverfresh;
	/// <summary>
	///海鲜类
	/// </summary>
	public int ing_seafood;
	/// <summary>
	///蔬菜类
	/// </summary>
	public int ing_vegetables;
	/// <summary>
	///瓜果类
	/// </summary>
	public int ing_melonfruit;
	/// <summary>
	///水酒类
	/// </summary>
	public int ing_waterwine;
	/// <summary>
	///面粉类
	/// </summary>
	public int ing_flour;
	/// <summary>
	///是否有效
	/// </summary>
	public int valid;
	/// <summary>
	///价格s
	/// </summary>
	public int price_s;
	/// <summary>
	///价格m
	/// </summary>
	public int price_m;
	/// <summary>
	///价格l
	/// </summary>
	public int price_l;
	/// <summary>
	///稀有度
	/// </summary>
	public int rarity;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(MenuInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(MenuInfoCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class MenuInfoCfg : BaseCfg<long, MenuInfoBean>
{
	public static string fileName = "MenuInfo";
	protected static Dictionary<long, MenuInfoBean> dicData = null;
	public static Dictionary<long, MenuInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static MenuInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static MenuInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			MenuInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(MenuInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, MenuInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			MenuInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
