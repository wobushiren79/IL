using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class StoryInfoBean : BaseBean
{
	/// <summary>
	///valid
	/// </summary>
	public int valid;
	/// <summary>
	///事件发生场景 1客栈 2城镇 3竞技场
	/// </summary>
	public int story_scene;
	/// <summary>
	///地点类型
	/// </summary>
	public int location_type;
	/// <summary>
	///发生地外还是里
	/// </summary>
	public int out_in;
	/// <summary>
	///事件发生位置
	/// </summary>
	public float position_x;
	/// <summary>
	///事件发生位置
	/// </summary>
	public float position_y;
	/// <summary>
	///该事件是否可以反复触发
	/// </summary>
	public int trigger_loop;
	/// <summary>
	///
	/// </summary>
	public string note;
	/// <summary>
	///触发条件
	/// </summary>
	public string trigger_condition;
}
public partial class StoryInfoCfg : BaseCfg<long, StoryInfoBean>
{
	public static string fileName = "StoryInfo";
	protected static Dictionary<long, StoryInfoBean> dicData = null;
	public static Dictionary<long, StoryInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static StoryInfoBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static StoryInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			StoryInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(StoryInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, StoryInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			StoryInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
