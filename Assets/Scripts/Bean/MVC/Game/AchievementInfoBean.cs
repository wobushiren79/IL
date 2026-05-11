using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class AchievementInfoBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///类型
	/// </summary>
	public int type;
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///图标备用
	/// </summary>
	public string icon_key_remark;
	/// <summary>
	///前置成就
	/// </summary>
	public string pre_ach_ids;
	/// <summary>
	///备用ID
	/// </summary>
	public int remark_id;
	/// <summary>
	///前置条件
	/// </summary>
	public string pre_data;
	/// <summary>
	///奖励数据
	/// </summary>
	public string reward_data;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(AchievementInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(AchievementInfoCfg.fileName, content, 1)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class AchievementInfoCfg : BaseCfg<long, AchievementInfoBean>
{
	public static string fileName = "AchievementInfo";
	protected static Dictionary<long, AchievementInfoBean> dicData = null;
	public static Dictionary<long, AchievementInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static AchievementInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static AchievementInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			AchievementInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(AchievementInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, AchievementInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			AchievementInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
