using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class CookingThemeBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///油盐
	/// </summary>
	public int ing_oilsalt;
	/// <summary>
	///肉
	/// </summary>
	public int ing_meat;
	/// <summary>
	///河鱼
	/// </summary>
	public int ing_riverfresh;
	/// <summary>
	///海鲜
	/// </summary>
	public int ing_seafood;
	/// <summary>
	///蔬菜
	/// </summary>
	public int ing_vegetables;
	/// <summary>
	///水果
	/// </summary>
	public int ing_melonfruit;
	/// <summary>
	///酒水
	/// </summary>
	public int ing_waterwine;
	/// <summary>
	///粉面
	/// </summary>
	public int ing_flour;
	/// <summary>
	///等级
	/// </summary>
	public int theme_level;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(CookingThemeCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
}
public partial class CookingThemeCfg : BaseCfg<long, CookingThemeBean>
{
	public static string fileName = "CookingTheme";
	protected static Dictionary<long, CookingThemeBean> dicData = null;
	public static Dictionary<long, CookingThemeBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static CookingThemeBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static CookingThemeBean GetItemData(long key)
	{
		if (dicData == null)
		{
			CookingThemeBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(CookingThemeBean[] arrayData)
	{
		dicData = new Dictionary<long, CookingThemeBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			CookingThemeBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
