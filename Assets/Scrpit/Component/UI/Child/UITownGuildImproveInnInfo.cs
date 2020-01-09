using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownGuildImproveInnInfo : BaseMonoBehaviour, StoreInfoManager.ICallBack
{
    public GameObject objInnLevelContainer;
    public GameObject objInnLevelModel;

    protected StoreInfoManager storeInfoManager;
    protected GameItemsManager gameItemsManager;

    public GameDataBean gameData;
    private void Awake()
    {
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    public void InitData(GameDataBean gameData)
    {
        this.gameData = gameData;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForGuildInnLevel();
    }

    /// <summary>
    /// 创建客栈升级ITEM
    /// </summary>
    /// <param name="innLevelStr"></param>
    /// <param name="spInnLevel"></param>
    private void CreateInnLevelItem(string innLevelStr, Sprite spInnLevel, StoreInfoBean storeInfo)
    {
        CptUtil.RemoveChildsByActive(objInnLevelContainer);
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

    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        string innLevelStr = gameData.innAttributes.GetNextInnLevel(out int levelTitle, out int levelStar);
        Sprite spInnLevel = gameItemsManager.GetItemsSpriteByName("inn_level_" + levelTitle + "_" + (levelStar - 1));
        StoreInfoBean storeInfoData = GetStoreInfoByLevel(listData, levelTitle, levelStar);
        if (storeInfoData != null)
        {
            CreateInnLevelItem(innLevelStr, spInnLevel, storeInfoData);
        }
    }
    #endregion
}