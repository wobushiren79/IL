using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class NpcTeamBean : BaseBean
{
	/// <summary>
	///是否有效
	/// </summary>
	public int valid;
	/// <summary>
	///队伍类型
	/// </summary>
	public int team_type;
	/// <summary>
	///领导者
	/// </summary>
	public string team_leader;
	/// <summary>
	///成员ID
	/// </summary>
	public string team_members;
	/// <summary>
	///成员数量
	/// </summary>
	public int team_number;
	/// <summary>
	///出现条件
	/// </summary>
	public string condition;
	/// <summary>
	///对话IDS
	/// </summary>
	public string talk_ids;
	/// <summary>
	///喜欢的菜单
	/// </summary>
	public string love_menus;
	/// <summary>
	///喊话的ID
	/// </summary>
	public string shout_ids;
	/// <summary>
	///效果时间
	/// </summary>
	public float effect_time;
	/// <summary>
	///名字
	/// </summary>
	public long name;
	[JsonIgnore]
	public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(NpcTeamCfg.fileName, name)); set => _name_language.Set(value); }
	private LanguageCache _name_language;
}
public partial class NpcTeamCfg : BaseCfg<long, NpcTeamBean>
{
	public static string fileName = "NpcTeam";
	protected static Dictionary<long, NpcTeamBean> dicData = null;
	public static Dictionary<long, NpcTeamBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static NpcTeamBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static NpcTeamBean GetItemData(long key)
	{
		if (dicData == null)
		{
			NpcTeamBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(NpcTeamBean[] arrayData)
	{
		dicData = new Dictionary<long, NpcTeamBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			NpcTeamBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
