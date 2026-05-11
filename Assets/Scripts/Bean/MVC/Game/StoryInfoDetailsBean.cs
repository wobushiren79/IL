using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class StoryInfoDetailsBean : BaseBean
{
	/// <summary>
	///story_order
	/// </summary>
	public int story_order;
	/// <summary>
	///类型 1 NPC站位 11对话 12剧情自动跳转
	/// </summary>
	public int type;
	/// <summary>
	///npc_id
	/// </summary>
	public long npc_id;
	/// <summary>
	///key_name
	/// </summary>
	public string key_name;
	/// <summary>
	///坐标
	/// </summary>
	public float position_x;
	/// <summary>
	///坐标
	/// </summary>
	public float position_y;
	/// <summary>
	///编号
	/// </summary>
	public int num;
	/// <summary>
	///npc朝向
	/// </summary>
	public int face;
	/// <summary>
	///剧情自动跳转时间
	/// </summary>
	public float wait_time;
	/// <summary>
	///文本ID
	/// </summary>
	public long text_mark_id;
	/// <summary>
	///表情
	/// </summary>
	public int expression;
	/// <summary>
	///场景互动物体名称
	/// </summary>
	public string scene_intobj_name;
	/// <summary>
	///场景互动物体具体名称你
	/// </summary>
	public string scene_intcomponent_name;
	/// <summary>
	///场景互动物体方法
	/// </summary>
	public string scene_intcomponent_method;
	/// <summary>
	///场景互动物体方法参数
	/// </summary>
	public string scene_intcomponent_parameters;
	/// <summary>
	///摧毁的NPCID
	/// </summary>
	public string npc_destroy;
	/// <summary>
	///npc_hat
	/// </summary>
	public string npc_hat;
	/// <summary>
	///npc_clothes
	/// </summary>
	public string npc_clothes;
	/// <summary>
	///npc_shoes
	/// </summary>
	public string npc_shoes;
	/// <summary>
	///播放的音效
	/// </summary>
	public int audio_sound;
	/// <summary>
	///播放的音效
	/// </summary>
	public int audio_music;
	/// <summary>
	///偏移
	/// </summary>
	public float offset_x;
	/// <summary>
	///偏移
	/// </summary>
	public float offset_y;
	/// <summary>
	///横竖
	/// </summary>
	public int horizontal;
	/// <summary>
	///横竖
	/// </summary>
	public int vertical;
	/// <summary>
	///time_year
	/// </summary>
	public int time_year;
	/// <summary>
	///time_month
	/// </summary>
	public int time_month;
	/// <summary>
	///time_day
	/// </summary>
	public int time_day;
	/// <summary>
	///time_hour
	/// </summary>
	public int time_hour;
	/// <summary>
	///time_minute
	/// </summary>
	public int time_minute;
}
public partial class StoryInfoDetailsCfg : BaseCfg<long, StoryInfoDetailsBean>
{
	public static string fileName = "StoryInfoDetails";
	protected static Dictionary<long, StoryInfoDetailsBean> dicData = null;
	public static Dictionary<long, StoryInfoDetailsBean> GetAllData()
	{
		if (dicData == null)
		{
			var arrayData = GetAllArrayData();
			InitData(arrayData);
		}
		return dicData;
	}
	public static StoryInfoDetailsBean[] GetAllArrayData()
	{
		if (arrayData == null)
		{
			arrayData = GetInitData(fileName);
		}
		return arrayData;
	}
	public static StoryInfoDetailsBean GetItemData(long key)
	{
		if (dicData == null)
		{
			StoryInfoDetailsBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(StoryInfoDetailsBean[] arrayData)
	{
		dicData = new Dictionary<long, StoryInfoDetailsBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			StoryInfoDetailsBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
