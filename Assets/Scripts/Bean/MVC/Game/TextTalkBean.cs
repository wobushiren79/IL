using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class TextTalkBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///mark_id
	/// </summary>
	public long mark_id;
	/// <summary>
	///type
	/// </summary>
	public int type;
	/// <summary>
	///text_order
	/// </summary>
	public int text_order;
	/// <summary>
	///next_order
	/// </summary>
	public int next_order;
	/// <summary>
	///talk_type
	/// </summary>
	public int talk_type;
	/// <summary>
	///user_id
	/// </summary>
	public long user_id;
	/// <summary>
	///condition_min_favorability
	/// </summary>
	public int condition_min_favorability;
	/// <summary>
	///condition_max_favorability
	/// </summary>
	public int condition_max_favorability;
	/// <summary>
	///select_type
	/// </summary>
	public int select_type;
	/// <summary>
	///add_favorability
	/// </summary>
	public int add_favorability;
	/// <summary>
	///pre_data_minigame
	/// </summary>
	public string pre_data_minigame;
	/// <summary>
	///reward_data
	/// </summary>
	public string reward_data;
	/// <summary>
	///wait_time
	/// </summary>
	public int wait_time;
	/// <summary>
	///is_stoptime
	/// </summary>
	public int is_stoptime;
	/// <summary>
	///scene_expression
	/// </summary>
	public string scene_expression;
	/// <summary>
	///pre_data
	/// </summary>
	public string pre_data;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(TextTalkCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(TextTalkCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class TextTalkCfg : BaseCfg<long, TextTalkBean>
{
	public static string fileName = "TextTalk";
	protected static Dictionary<long, TextTalkBean> dicData = null;
	public static Dictionary<long, TextTalkBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static TextTalkBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static TextTalkBean GetItemData(long key)
	{
		if (dicData == null)
		{
			TextTalkBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(TextTalkBean[] arrayData)
	{
		dicData = new Dictionary<long, TextTalkBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			TextTalkBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
