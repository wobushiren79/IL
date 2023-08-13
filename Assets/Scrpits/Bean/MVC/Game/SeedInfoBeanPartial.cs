using System;
using System.Collections.Generic;
public partial class SeedInfoBean
{
    public Dictionary<IngredientsEnum, int> dicIngNum;

    public Dictionary<ItemBean, float> dicItemsNum;
    /// <summary>
    /// 获取种子的Tile
    /// </summary>
    /// <param name="growTime"></param>
    /// <returns></returns>
    public string GetSeedTile(int growTime)
    {
        if (growTime > growup_totleloop - 1)
        {
            growTime = growup_totleloop - 1;
        }
        return $"{tile}{growTime + 1}";
    }

    /// <summary>
    /// 获取收获的食材
    /// </summary>
    public void GetIng(out Dictionary<IngredientsEnum, int> dicIng)
    {
        if (dicIngNum == null)
        {
            dicIngNum = new Dictionary<IngredientsEnum, int>();
            if (get_ingredient.IsNull())
            {
                dicIng = dicIngNum;
                return;
            }
            string[] ingDataStr = get_ingredient.Split('&');
            for (int i = 0; i < ingDataStr.Length; i++)
            {
                string itemIngDataStr = ingDataStr[i];
                int[] dataIng = itemIngDataStr.SplitForArrayInt('_');
                dicIngNum.Add((IngredientsEnum)dataIng[0], dataIng[1]);
            }
        }
        dicIng = dicIngNum;
    }

    /// <summary>
    /// 获取收获的道具
    /// </summary>
    public void GetItems(out List<ItemBean> listAddItems)
    {
        listAddItems = new List<ItemBean>();
        if (dicItemsNum == null)
        {
            dicItemsNum = new Dictionary<ItemBean, float>();
            if (get_items.IsNull())
            {
                return;
            }
            string[] itemsDataStr = get_items.Split('&');
            for (int i = 0; i < itemsDataStr.Length; i++)
            {
                string itemDataStr = itemsDataStr[i];
                string[] dataIng = itemDataStr.SplitForArrayStr('_');
                ItemBean itemData = new ItemBean();
                itemData.itemId = long.Parse(dataIng[0]);
                itemData.itemNumber = long.Parse(dataIng[1]);
                dicItemsNum.Add(itemData,float.Parse(dataIng[2]));
            }
        }
        foreach (var itemData in dicItemsNum)
        {
            float randomRate = UnityEngine.Random.Range(0f, 1f);
            if (randomRate <= itemData.Value)
            {
                listAddItems.Add(itemData.Key);
            }
        }
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
