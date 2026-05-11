using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class TextLookBean : BaseBean
{
	/// <summary>
	///type
	/// </summary>
	public int type;
	/// <summary>
	///mark_id
	/// </summary>
	public long mark_id;
	/// <summary>
	///text_order
	/// </summary>
	public int text_order;
	/// <summary>
	///user_id
	/// </summary>
	public long user_id;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(TextLookCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(TextLookCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class TextLookCfg : BaseCfg<long, TextLookBean>
{
	public static string fileName = "TextLook";
	protected static Dictionary<long, TextLookBean> dicData = null;
	public static Dictionary<long, TextLookBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static TextLookBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static TextLookBean GetItemData(long key)
	{
		if (dicData == null)
		{
			TextLookBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(TextLookBean[] arrayData)
	{
		dicData = new Dictionary<long, TextLookBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			TextLookBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
