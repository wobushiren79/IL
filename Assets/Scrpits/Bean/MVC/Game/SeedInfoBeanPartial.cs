using System;
using System.Collections.Generic;
public partial class SeedInfoBean
{

	/// <summary>
	/// 获取种子的Tile
	/// </summary>
	/// <param name="growTime"></param>
	/// <returns></returns>
	public string GetSeedTile(int growTime)
    {
        if (growTime > growup_totleloop)
        {
			growTime = growup_totleloop;

		}
		return $"{tile}{growTime + 1}";
    }

}
public partial class SeedInfoCfg
{
	protected static Dictionary<long, SeedInfoBean> dicDataForItemId = null;

	public static SeedInfoBean GetItemDataByItemId(long key)
	{
		if (dicDataForItemId == null)
		{
			dicDataForItemId = new Dictionary<long, SeedInfoBean>();
			var allData = GetAllData();
            foreach (var item in allData)
            {
				dicDataForItemId.Add(item.Value.item_id, item.Value);
			}
		}
        if (dicDataForItemId.TryGetValue(key, out SeedInfoBean seedInfo))
        {
			return seedInfo;

		}
		return null;
	}
}
