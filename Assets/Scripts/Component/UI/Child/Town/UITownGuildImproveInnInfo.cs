using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UITownGuildImproveInnInfo : BaseUIView
{
    public Text tvNull;

    public GameObject objInnLevelContainer;
    public GameObject objInnLevelModel;

    public GameDataBean gameData;

    public void InitData(GameDataBean gameData)
    {
        CptUtil.RemoveChildsByActive(objInnLevelContainer);
        this.gameData = gameData;
        Action<List<StoreInfoBean>> callBack = SetStoreData;
        StoreInfoHandler.Instance.manager.GetStoreInfoForGuildInnLevel(callBack);
    }

    /// <summary>
    /// 创建客栈升级ITEM
    /// </summary>
    /// <param name="innLevelStr"></param>
    /// <param name="spInnLevel"></param>
    private void CreateInnLevelItem(string innLevelStr, Sprite spInnLevel, StoreInfoBean storeInfo)
    {
        GameObject objItem = Instantiate(objInnLevelContainer, objInnLevelModel);
        ItemTownGuildImproveInnLevelCpt itemCpt = objItem.GetComponent<ItemTownGuildImproveInnLevelCpt>();
        itemCpt.SetData(innLevelStr, spInnLevel, storeInfo);
    }

    /// <summary>
    /// 根据等级获取数据
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="levelTitle"></param>
    /// <param name="levelStar"></param>
    /// <returns></returns>
    private StoreInfoBean GetStoreInfoByLevel(List<StoreInfoBean> listData, int levelTitle, int levelStar)
    {
        foreach (StoreInfoBean itemData in listData)
        {
            int innLevel = levelTitle * 10 + levelStar;
            if (innLevel == itemData.mark_type)
            {
                return itemData;
            }
        }
        return null;
    }

   /// <summary>
   /// 设置商店数据
   /// </summary>
   /// <param name="listData"></param>
    public void SetStoreData(List<StoreInfoBean> listData)
    {
        tvNull.gameObject.SetActive(false);
        string innLevelStr = gameData.innAttributes.GetNextInnLevel(out int levelTitle, out int levelStar);
        if (levelTitle > 3)
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        Sprite spInnLevel = IconHandler.Instance.GetIconSpriteByName("inn_level_" + levelTitle + "_" + (levelStar - 1));
        StoreInfoBean storeInfoData = GetStoreInfoByLevel(listData, levelTitle, levelStar);
        if (storeInfoData != null)
        {
            CreateInnLevelItem(innLevelStr, spInnLevel, storeInfoData);
        }
    }

}