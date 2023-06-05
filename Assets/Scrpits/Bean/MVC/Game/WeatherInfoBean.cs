using System;
using System.Collections.Generic;
[Serializable]
public partial class WeatherInfoBean : BaseBean
{
	/// <summary>
	///场景类型
	/// </summary>
	public int scene_type;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
	/// <summary>
	///风的位置
	/// </summary>
	public string wind_position;
	/// <summary>
	///风的缩放
	/// </summary>
	public string wind_scale;
	/// <summary>
	///风的缩放范围
	/// </summary>
	public string wind_scaleRange;
	/// <summary>
	///雾的缩放范围
	/// </summary>
	public string fog_scaleRange;
	/// <summary>
	///雨的位置
	/// </summary>
	public string rain_position;
	/// <summary>
	///雨的缩放
	/// </summary>
	public string rain_scale;
	/// <summary>
	///雪的位置
	/// </summary>
	public string snow_position;
	/// <summary>
	///雪的缩放
	/// </summary>
	public string snow_scale;
	/// <summary>
	///多云的位置
	/// </summary>
	public string sunny_position;
	/// <summary>
	///多云的缩放
	/// </summary>
	public string sunny_scale;
}
public partial class WeatherInfoCfg : BaseCfg<long, WeatherInfoBean>
{
	public static string fileName = "WeatherInfo";
	protected static Dictionary<long, WeatherInfoBean> dicData = null;
	public static Dictionary<long, WeatherInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			WeatherInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static WeatherInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			WeatherInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(WeatherInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, WeatherInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			WeatherInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
