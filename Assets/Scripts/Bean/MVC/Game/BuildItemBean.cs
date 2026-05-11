using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class BuildItemBean : BaseBean
{
	/// <summary>
	///建筑类型
	/// </summary>
	public int build_type;
	/// <summary>
	///模型名字
	/// </summary>
	public string model_name;
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///图标列表
	/// </summary>
	public string icon_list;
	/// <summary>
	///tile名字
	/// </summary>
	public string tile_name;
	/// <summary>
	///是否有效
	/// </summary>
	public int valid;
	/// <summary>
	///美观值
	/// </summary>
	public float aesthetics;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(BuildItemCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///描述
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(BuildItemCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class BuildItemCfg : BaseCfg<long, BuildItemBean>
{
	public static string fileName = "BuildItem";
	protected static Dictionary<long, BuildItemBean> dicData = null;
	public static Dictionary<long, BuildItemBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static BuildItemBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static BuildItemBean GetItemData(long key)
	{
		if (dicData == null)
		{
			BuildItemBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(BuildItemBean[] arrayData)
	{
		dicData = new Dictionary<long, BuildItemBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			BuildItemBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
