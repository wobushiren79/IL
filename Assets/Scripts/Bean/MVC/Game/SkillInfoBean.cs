using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class SkillInfoBean : BaseBean
{
	/// <summary>
	///是否有效
	/// </summary>
	public int valid;
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///附加特性
	/// </summary>
	public string effect;
	/// <summary>
	///附加特性
	/// </summary>
	public string effect_details;
	/// <summary>
	///使用次数
	/// </summary>
	public int use_number;
	/// <summary>
	///前置解锁条件
	/// </summary>
	public string pre_data;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(SkillInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///介绍
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(SkillInfoCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class SkillInfoCfg : BaseCfg<long, SkillInfoBean>
{
	public static string fileName = "SkillInfo";
	protected static Dictionary<long, SkillInfoBean> dicData = null;
	public static Dictionary<long, SkillInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static SkillInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static SkillInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			SkillInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(SkillInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, SkillInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			SkillInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
