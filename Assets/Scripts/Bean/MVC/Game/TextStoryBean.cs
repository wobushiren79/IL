using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class TextStoryBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///文本编号
	/// </summary>
	public long mark_id;
	/// <summary>
	/////类型 0默认文本 1选择对话    4书本详情  5黑幕标题  99默认
	/// </summary>
	public int type;
	/// <summary>
	///文本顺序
	/// </summary>
	public int text_order;
	/// <summary>
	///指定下一顺序
	/// </summary>
	public int next_order;
	/// <summary>
	///对话类型 0普通对话，1送礼回复对话  2招募对话  3后续对话  4第一次对话
	/// </summary>
	public int talk_type;
	/// <summary>
	///文本发起对象ID
	/// </summary>
	public long user_id;
	/// <summary>
	///触发条件 好感区间
	/// </summary>
	public int condition_min_favorability;
	/// <summary>
	///触发条件 好感区间
	/// </summary>
	public int condition_max_favorability;
	/// <summary>
	///选择对话 的类型 0提示文本  1选项
	/// </summary>
	public int select_type;
	/// <summary>
	///增加的好感
	/// </summary>
	public int add_favorability;
	/// <summary>
	///小游戏的前置数据
	/// </summary>
	public string pre_data_minigame;
	/// <summary>
	///奖励物品
	/// </summary>
	public string reward_data;
	/// <summary>
	///停留时间
	/// </summary>
	public int wait_time;
	/// <summary>
	///是否停止时间
	/// </summary>
	public int is_stoptime;
	/// <summary>
	///场景人物表情
	/// </summary>
	public string scene_expression;
	/// <summary>
	///支付条件
	/// </summary>
	public string pre_data;
	/// <summary>
	///文本发起人名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(TextStoryCfg.fileName, name, 1)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///文本内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(TextStoryCfg.fileName, content)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class TextStoryCfg : BaseCfg<long, TextStoryBean>
{
	public static string fileName = "TextStory";
	protected static Dictionary<long, TextStoryBean> dicData = null;
	public static Dictionary<long, TextStoryBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static TextStoryBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static TextStoryBean GetItemData(long key)
	{
		if (dicData == null)
		{
			TextStoryBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(TextStoryBean[] arrayData)
	{
		dicData = new Dictionary<long, TextStoryBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			TextStoryBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
