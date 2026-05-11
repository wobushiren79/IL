using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class ItemsInfoBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///装备类型 1帽子 2衣服  3鞋子
	/// </summary>
	public int items_type;
	/// <summary>
	///装备图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///动画名字
	/// </summary>
	public string anim_key;
	/// <summary>
	///动画长度
	/// </summary>
	public int anim_length;
	/// <summary>
	///添加生命值
	/// </summary>
	public int add_life;
	/// <summary>
	///增加 做菜
	/// </summary>
	public int add_cook;
	/// <summary>
	///增加 跑堂
	/// </summary>
	public int add_speed;
	/// <summary>
	///增加 算账
	/// </summary>
	public int add_account;
	/// <summary>
	///增加 吆喝
	/// </summary>
	public int add_charm;
	/// <summary>
	///增加 武力
	/// </summary>
	public int add_force;
	/// <summary>
	///增加 切菜
	/// </summary>
	public int add_lucky;
	/// <summary>
	///增加信任
	/// </summary>
	public int add_loyal;
	/// <summary>
	///增加的内容ID
	/// </summary>
	public long add_id;
	/// <summary>
	///套装ID
	/// </summary>
	public long intact_id;
	/// <summary>
	///选择角度
	/// </summary>
	public int rotation_angle;
	/// <summary>
	///效果 增加效果
	/// </summary>
	public string effect;
	/// <summary>
	///效果详情
	/// </summary>
	public string effect_details;
	/// <summary>
	///稀有度
	/// </summary>
	public int rarity;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(ItemsInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long content;
	[JsonIgnore]
	public string content_language { get => _content_language.Get(() => TextHandler.Instance.GetTextById(ItemsInfoCfg.fileName, content)); set => _content_language.Set(value); }
	private LanguageCache _content_language;
}
public partial class ItemsInfoCfg : BaseCfg<long, ItemsInfoBean>
{
	public static string fileName = "ItemsInfo";
	protected static Dictionary<long, ItemsInfoBean> dicData = null;
	public static Dictionary<long, ItemsInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static ItemsInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static ItemsInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			ItemsInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(ItemsInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, ItemsInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			ItemsInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
