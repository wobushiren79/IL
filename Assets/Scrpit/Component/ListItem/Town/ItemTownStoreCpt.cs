using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemTownStoreCpt : ItemGameBaseCpt
{
    public PriceShowView priceShowView;

    public Text tvGetNumber;

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
        priceShowView.SetPrice(1, priceL, priceM, priceS, coin,  trophyElementary,  trophyIntermediate,  trophyAdvanced,  trophyLegendary);
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
    public virtual void SetOwn()
    {
        if (tvOwn == null)
            return;
        tvOwn.text = (GameCommonInfo.GetUITextById(4001) + "\n" + GetUIManager<UIGameManager>().gameDataManager.gameData.GetItemsNumber(storeInfo.mark_id));
    }

    /// <summary>
    /// 设置获取数量
    /// </summary>
    /// <param name="number"></param>
    public virtual void SetGetNumber(int number)
    {
        if (tvGetNumber != null && number !=0 )
        {
            tvGetNumber.gameObject.SetActive(true);
            tvGetNumber.text = "x" + number;
        }
        else
        {
            tvGetNumber.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public virtual void OnClickSubmitBuy()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
    }
}