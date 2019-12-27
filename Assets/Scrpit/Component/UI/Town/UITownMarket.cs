using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UITownMarket : UIBaseOne, StoreInfoManager.ICallBack
{
    public GameObject objGoodsContent;
    public GameObject objGoodsModel;

    public List<IconBean> listGoodsIcon;

    public override void OpenUI()
    {
        base.OpenUI();
        StoreInfoManager storeInfoManager = GetUIMananger<UIGameManager>().storeInfoManager;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForMarket();
    }

    public void CreateGoods(List<StoreInfoBean> listData)
    {
        if (objGoodsContent == null || objGoodsModel == null || listData == null)
            return;
        CptUtil.RemoveChildsByActive(objGoodsContent);
        GameCommonInfo.InitRandomSeed();
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            GameObject objGoods = Instantiate(objGoodsContent, objGoodsModel);
            ItemTownGoodsMarketCpt itemCpt = objGoods.GetComponent<ItemTownGoodsMarketCpt>();
            if (itemCpt != null)
            {
                IconBean iconData = BeanUtil.GetIconBeanByName(itemData.icon_key, listGoodsIcon);
                itemCpt.SetData(itemData, iconData.value);
            }
            objGoods.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.01f).From();
        };
    }


    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        if (type==StoreTypeEnum.Market)
        {
            CreateGoods(listData);
        }
    }
    #endregion
}