using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class PriceShowView : BaseMonoBehaviour
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


    public void SetPrice(long number, long priceL, long priceM, long priceS)
    {
        SetPrice(number, priceL, priceM, priceS, 0, 0, 0, 0, 0);
    }
    /// <summary>
    /// 设置价格
    /// </summary>
    public void SetPrice(long number,
    long priceL, long priceM, long priceS,
    long coin,
    long trophyElementary, long trophyIntermediate, long trophyAdvanced, long trophyLegendary)
    {
        if (priceL == 0 && objPriceL != null)
            objPriceL.SetActive(false);
        else
            objPriceL.SetActive(true);


        if (priceM == 0 && objPriceM != null)
            objPriceM.SetActive(false);
        else
            objPriceM.SetActive(true);

        if (priceS == 0 && objPriceS != null)
            objPriceS.SetActive(false);
        else
            objPriceS.SetActive(true);

        if (coin == 0 && objGuildCoin != null)
            objGuildCoin.SetActive(false);
        else
            objGuildCoin.SetActive(true);

        if (trophyElementary == 0 && objTrophyElementary != null)
            objTrophyElementary.SetActive(false);
        else
            objTrophyElementary.SetActive(true);

        if (trophyIntermediate == 0 && objTrophyIntermediate != null)
            objTrophyIntermediate.SetActive(false);
        else
            objTrophyIntermediate.SetActive(true);

        if (trophyAdvanced == 0 && objTrophyAdvanced != null)
            objTrophyAdvanced.SetActive(false);
        else
            objTrophyAdvanced.SetActive(true);

        if (trophyLegendary == 0 && objTrophyLegendary != null)
            objTrophyLegendary.SetActive(false);
        else
            objTrophyLegendary.SetActive(true);

        if (tvPriceL != null)
            tvPriceL.text = priceL * number + "";
        if (tvPriceM != null)
            tvPriceM.text = priceM * number + "";
        if (tvPriceS != null)
            tvPriceS.text = priceS * number + "";

        if (tvGuildCoin != null)
            tvGuildCoin.text = coin * number + "";

        if (tvTrophyElementary != null)
            tvTrophyElementary.text = trophyElementary * number + "";
        if (tvTrophyIntermediate != null)
            tvTrophyIntermediate.text = trophyIntermediate * number + "";
        if (tvTrophyAdvanced != null)
            tvTrophyAdvanced.text = trophyAdvanced * number + "";
        if (tvTrophyLegendary != null)
            tvTrophyLegendary.text = trophyLegendary * number + "";
    }
}