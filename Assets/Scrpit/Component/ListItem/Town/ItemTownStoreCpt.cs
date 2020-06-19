using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemTownStoreCpt : ItemGameBaseCpt
{
    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;
    public GameObject objGuildCoin;
    public Text tvGuildCoin;
    public GameObject objTrophyElementary;
    public Text tvTrophyElementary;
    public GameObject objTrophyIntermediate;
    public Text tvTrophyIntermediate;
    public GameObject objTrophyAdvanced;
    public Text tvTrophyAdvanced;
    public GameObject objTrophyLegendary;
    public Text tvTrophyLegendary;

    public RectTransform rtIcon;
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvOwn;
    public Button btSubmit;

    public StoreInfoBean storeInfo;


    public virtual void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickSubmitBuy);
    }



    /// <summary>
    /// 刷新数据
    /// </summary>
    public virtual void RefreshUI()
    {
        SetOwn();
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    public void SetPrice(
        long priceL, long priceM, long priceS,
        long coin,
        long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        if (priceL == 0 && objPriceL != null)
            objPriceL.SetActive(false);
        if (priceM == 0 && objPriceM != null)
            objPriceM.SetActive(false);
        if (priceS == 0 && objPriceS != null)
            objPriceS.SetActive(false);

        if (coin == 0 && objGuildCoin != null)
            objGuildCoin.SetActive(false);

        if (trophyElementary == 0 && objTrophyElementary != null)
            objTrophyElementary.SetActive(false);
        if (trophyIntermediate == 0 && objTrophyIntermediate != null)
            objTrophyIntermediate.SetActive(false);
        if (trophyAdvanced == 0 && objTrophyAdvanced != null)
            objTrophyAdvanced.SetActive(false);
        if (trophyLegendary == 0 && objTrophyLegendary != null)
            objTrophyLegendary.SetActive(false);

        if (tvPriceL != null)
            tvPriceL.text = priceL + "";
        if (tvPriceM != null)
            tvPriceM.text = priceM + "";
        if (tvPriceS != null)
            tvPriceS.text = priceS + "";
        if (tvGuildCoin != null)
            tvGuildCoin.text = coin + "";

        if (tvTrophyElementary != null)
            tvTrophyElementary.text = trophyElementary + "";
        if (tvTrophyIntermediate != null)
            tvTrophyIntermediate.text = trophyIntermediate + "";
        if (tvTrophyAdvanced != null)
            tvTrophyAdvanced.text = trophyAdvanced + "";
        if (tvTrophyLegendary != null)
            tvTrophyLegendary.text = trophyLegendary + "";
    }
    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置描述
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn()
    {
        if (tvOwn == null)
            return;
        tvOwn.text = (GameCommonInfo.GetUITextById(4001) + "\n" + GetUIManager<UIGameManager>().gameDataManager.gameData.GetItemsNumber(storeInfo.mark_id));
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public virtual void OnClickSubmitBuy()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler audioHandler = uiGameManager.audioHandler;
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
    }
}