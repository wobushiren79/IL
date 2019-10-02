using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UITownMarket : UIBaseOne, IStoreInfoView
{
    public GameObject objGoodsContent;
    public GameObject objGoodsModel;

    public List<IconBean> listGoodsIcon;
    //商店数据
    private StoreInfoController mStoreInfoController;


    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        mStoreInfoController.GetMarketStoreInfo();
    }


    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData)
    {
    }

    public void GetAllStoreInfoFail()
    {
    }

    public void GetStoreInfoByTypeSuccess(List<StoreInfoBean> listData)
    {
        CreateGoods(listData);
    }

    public void GetStoreInfoByTypeFail()
    {

    }

    public void CreateGoods(List<StoreInfoBean> listData)
    {
        if (objGoodsContent == null || objGoodsModel == null || listData == null)
            return;
        CptUtil.RemoveChildsByActive(objGoodsContent.transform);

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
            objGoods.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        };
    }
}