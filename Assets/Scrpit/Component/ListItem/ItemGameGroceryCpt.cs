using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameGroceryCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvOwn;

    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public CharacterDressManager characterDressManager;
    public InnFoodManager innFoodManager;


    public void SetData(StoreInfoBean storeInfo)
    {
        SetIcon(storeInfo.icon_key, storeInfo.mark, storeInfo.mark_id);
        SetName(storeInfo.name);
        SetContent(storeInfo.content);
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(string iconKey,string mark,long markId)
    {
        Sprite spIcon = null;
        if (int.Parse(mark) == 1)
        {
            //食物
            spIcon = innFoodManager.GetFoodSpriteByName(iconKey);
        }
        if (ivIcon != null&& spIcon!=null)
            ivIcon.sprite=spIcon;
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
    /// 设置价格
    /// </summary>
    public void SetPrice(long priceL,long priceM,long priceS)
    {
        if (priceL == 0)
            objPriceL.SetActive(false);
        if (priceM == 0)
            objPriceM.SetActive(false);
        if (priceS == 0)
            objPriceS.SetActive(false);
        tvPriceL.text = priceL+"";
        tvPriceM.text = priceM + "";
        tvPriceS.text = priceS + "";
    }
}