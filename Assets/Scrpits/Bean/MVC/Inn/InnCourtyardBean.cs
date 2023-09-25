using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;

[Serializable]
public class InnCourtyardBean
{
    public int courtyardLevel = 0;

    public List<InnResBean> listSeedData = new List<InnResBean>();
    //是否自动播种
    public bool isAutoSeed = true;
    //管理人员Id
    public string managerId;
    public InnResBean GetSeedData(Vector3 buildPosition)
    {
        foreach (InnResBean itemData in listSeedData)
        {
            if (itemData.GetStartPosition() == buildPosition)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 增加天数据
    /// </summary>
    /// <param name="day"></param>
    public void AddDay(int day = 1)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        Dictionary<IngredientsEnum, int> dicIngDataAdd = new Dictionary<IngredientsEnum, int>();
        List<ItemBean> listItemsDataAdd = new List<ItemBean>();
        for (int i = 0; i < listSeedData.Count; i++)
        {
            InnResBean innRes = listSeedData[i];
            InnCourtyardSeedBean seedData = JsonUtil.FromJson<InnCourtyardSeedBean>(innRes.remark);
            seedData.growDay += day;
            SeedInfoBean seedInfoData = SeedInfoCfg.GetItemDataByItemId(innRes.id);
            //如果到达收获日子
            if (seedData.growDay >= seedInfoData.growup_oneloopday * seedInfoData.growup_totleloop)
            {
                //添加食材
                seedInfoData.GetIng(out Dictionary<IngredientsEnum, int> dicIngData);
                foreach (var itemIngData in dicIngData)
                {
                    float addRate = 0;
                    if (!managerId.IsNull())
                    {
                        //获取管理员数据
                        var managerCharaterData = gameData.GetCharacterDataById(managerId);
                        if (managerCharaterData != null)
                        {
                            managerCharaterData.GetAttributes(out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                            addRate = (totalAttributes.charm / 100f) + (totalAttributes.lucky / 100f);
                        }
                    }
                    if (dicIngDataAdd.TryGetValue(itemIngData.Key, out int addNum))
                    {
                        dicIngDataAdd[itemIngData.Key] = addNum + itemIngData.Value + (int)(addRate * itemIngData.Value);
                    }
                    else
                    {
                        dicIngDataAdd.Add(itemIngData.Key, itemIngData.Value + (int)(addRate * itemIngData.Value));
                    }
                }
                //添加道具
                seedInfoData.GetItems(out List<ItemBean> listItemsDataAddItem);
                for (int z = 0; z < listItemsDataAddItem.Count; z++)
                {
                    ItemBean itemDataZ = listItemsDataAddItem[z];
                    bool hasData = false;
                    for (int w = 0; w < listItemsDataAdd.Count; w++)
                    {
                        ItemBean itemDataW = listItemsDataAdd[w];
                        if (itemDataW.itemId == itemDataZ.itemId)
                        {
                            itemDataW.itemNumber += itemDataZ.itemNumber;
                            hasData = true;
                        }
                    }
                    if (hasData == false)
                    {
                        listItemsDataAdd.Add(new ItemBean(itemDataZ.itemId,itemDataZ.itemNumber));
                    }
                }

                //查看背包是否还有同类型的种子
                if (isAutoSeed && gameData.GetItemsNumber(innRes.id) > 0)
                {
                    //如果有则直接种植
                    seedData.growDay = 0;
                    innRes.remark = JsonUtil.ToJson(seedData);
                    //扣除背包的种子
                    gameData.AddItemsNumber(innRes.id, -1);
                }
                else
                {
                    //如果没有则移除
                    i--;
                    listSeedData.Remove(innRes);
                }
            }
            else
            {
                innRes.remark = JsonUtil.ToJson(seedData);
            }
        }

        //添加所有食材
        foreach (var itemIngData in dicIngDataAdd)
        {
            gameData.AddIng(itemIngData.Key, itemIngData.Value);
            Sprite spIng = IngredientsEnumTools.GetIngredientIcon(itemIngData.Key);
            string nameIng = IngredientsEnumTools.GetIngredientName(itemIngData.Key);
            UIHandler.Instance.ToastHint<ToastView>(spIng, string.Format(TextHandler.Instance.manager.GetTextById(6099), $"{nameIng}x{itemIngData.Value}"));
        }
        //添加道具
        for (int i = 0; i < listItemsDataAdd.Count; i++)
        {
            ItemBean itemData = listItemsDataAdd[i];
            gameData.AddItemsNumber(itemData.itemId, itemData.itemNumber);
            var itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);
            Sprite spItem = GeneralEnumTools.GetGeneralSprite(itemsInfo, false);
            UIHandler.Instance.ToastHint<ToastView>(spItem, string.Format(TextHandler.Instance.manager.GetTextById(6099), $"{itemsInfo.name}x{itemData.itemNumber}"));
        }
    }

}

[Serializable]
public class InnCourtyardSeedBean
{
    public int growDay = 0;
}