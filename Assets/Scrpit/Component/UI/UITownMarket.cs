using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UITownMarket : BaseUIComponent, IStoreInfoView
{
    //返回按钮
    public Button btBack;
    //金钱
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;
    //控制处理
    public ControlHandler controlHandler;

    public GameObject objGoodsContent;
    public GameObject objGoodsModel;

    public List<IconBean> listGoodsIcon;
    public GameDataManager gameDataManager;
    //商店数据
    private StoreInfoController mStoreInfoController;


    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
    }

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    private void Update()
    {
        if (gameDataManager != null)
        {
            if (tvMoneyL!=null)
            {
                tvMoneyL.text = gameDataManager.gameData.moneyL+"";
            }
            if (tvMoneyM != null)
            {
                tvMoneyM.text = gameDataManager.gameData.moneyM + "";
            }
            if (tvMoneyS != null)
            {
                tvMoneyS.text = gameDataManager.gameData.moneyS + "";
            }
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (controlHandler != null)
            controlHandler.StopControl();
        mStoreInfoController.GetMarketStoreInfo();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        if (controlHandler != null)
            controlHandler.RestoreControl();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
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
            GameObject objGoods = Instantiate(objGoodsModel, objGoodsContent.transform);
            objGoods.SetActive(true);
            ItemGameGoodsMarketCpt itemCpt = objGoods.GetComponent<ItemGameGoodsMarketCpt>();
            if (itemCpt != null)
            {
                IconBean iconData = BeanUtil.GetIconBeanByName(itemData.icon_key, listGoodsIcon);
                itemCpt.SetData(itemData, iconData.value);
            }
            objGoods.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        };
    }
}