using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class NpcInfoBean : BaseBean
{
	/// <summary>
	///是否有效
	/// </summary>
	public int valid;
	/// <summary>
	///类型
	/// </summary>
	public int npc_type;
	/// <summary>
	///人物对话ID
	/// </summary>
	public string talk_types;
	/// <summary>
	///面具ID
	/// </summary>
	public long mask_id;
	/// <summary>
	///手ID
	/// </summary>
	public long hand_id;
	/// <summary>
	///帽子ID
	/// </summary>
	public long hat_id;
	/// <summary>
	///衣服ID
	/// </summary>
	public long clothes_id;
	/// <summary>
	///鞋子ID
	/// </summary>
	public long shoes_id;
	/// <summary>
	///发型ID
	/// </summary>
	public string hair_id;
	/// <summary>
	///发型颜色
	/// </summary>
	public string hair_color;
	/// <summary>
	///眼睛ID
	/// </summary>
	public string eye_id;
	/// <summary>
	///眼睛颜色
	/// </summary>
	public string eye_color;
	/// <summary>
	///嘴巴ID
	/// </summary>
	public string mouth_id;
	/// <summary>
	///嘴巴颜色
	/// </summary>
	public string mouth_color;
	/// <summary>
	///皮肤颜色
	/// </summary>
	public string skin_color;
	/// <summary>
	///性别
	/// </summary>
	public int sex;
	/// <summary>
	///是否可以结婚
	/// </summary>
	public int marry_status;
	/// <summary>
	///面向 1.左边 2右边
	/// </summary>
	public int face;
	/// <summary>
	///位置X
	/// </summary>
	public float position_x;
	/// <summary>
	///位置Y
	/// </summary>
	public float position_y;
	/// <summary>
	///属性 忠诚
	/// </summary>
	public int attributes_loyal;
	/// <summary>
	///属性 生命
	/// </summary>
	public int attributes_life;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_cook;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_speed;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_account;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_charm;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_force;
	/// <summary>
	///属性 
	/// </summary>
	public int attributes_lucky;
	/// <summary>
	///喜爱的道具
	/// </summary>
	public string love_items;
	/// <summary>
	///喜爱的菜单
	/// </summary>
	public string love_menus;
	/// <summary>
	///出现条件
	/// </summary>
	public string condition;
	/// <summary>
	///技能ID
	/// </summary>
	public string skill_ids;
	/// <summary>
	///工资
	/// </summary>
	public int wage_s;
	/// <summary>
	///工资
	/// </summary>
	public int wage_m;
	/// <summary>
	///工资
	/// </summary>
	public int wage_l;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(NpcInfoCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
	/// <summary>
	///内容
	/// </summary>
	public long title_name;
	[JsonIgnore]
	public string title_name_language { get => _title_name_language.Get(() => TextHandler.Instance.GetTextById(NpcInfoCfg.fileName, title_name, 1)); set => _title_name_language.Set(value); }
	private LanguageCache _title_name_language;
}
public partial class NpcInfoCfg : BaseCfg<long, NpcInfoBean>
{
	public static string fileName = "NpcInfo";
	protected static Dictionary<long, NpcInfoBean> dicData = null;
	public static Dictionary<long, NpcInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static NpcInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static NpcInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			NpcInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(NpcInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, NpcInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			NpcInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
