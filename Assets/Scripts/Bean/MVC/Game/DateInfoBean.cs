using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class DateInfoBean : BaseBean
{
	/// <summary>
	///月
	/// </summary>
	public int month;
	/// <summary>
	///日
	/// </summary>
	public int day;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(DateInfoCfg.fileName, content)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class DateInfoCfg : BaseCfg<long, DateInfoBean>
{
	public static string fileName = "DateInfo";
	protected static Dictionary<long, DateInfoBean> dicData = null;
	public static Dictionary<long, DateInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static DateInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static DateInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			DateInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(DateInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, DateInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			DateInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
