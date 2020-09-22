using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SelectForBedDialogView : DialogView
{
    public BuildBedCpt buildBedCpt;
    public Text tvName;
    public Text tvPrice;
    public BedShowView bedShowView;
    public Text tvAesthetics;

    public void SetData(BuildBedCpt buildBedCpt)
    {
        this.buildBedCpt = buildBedCpt;
        BuildBedBean buildBedData = buildBedCpt.buildBedData;
        SetName(buildBedData.bedName);

        //设置价格
        buildBedCpt.GetPrice(out long basePriceS, out long addPriceS);
        SetPrice(basePriceS, addPriceS);

        SetBed(buildBedData);
        buildBedCpt.GetAesthetics(out float addAesthetics, out float subAesthetics, out float totalthetics);
        SetAesthetics(addAesthetics, subAesthetics);
        buildBedCpt.ShowRange(true);
    }

    private void OnDisable()
    {
        buildBedCpt.ShowRange(false);
    }

    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    public void SetPrice(long basePrice, long addPrice)
    {
        if (tvPrice != null)
        {
            tvPrice.text = basePrice + "+" + addPrice + "/h";
        }
    }

    public void SetBed(BuildBedBean buildBedData)
    {
        if (bedShowView != null)
        {
            bedShowView.SetData(buildBedData);
        }
    }

    public void SetAesthetics(float addAesthetics,float subAesthetics)
    {
        if (tvAesthetics != null)
        {
            if (subAesthetics!=0)
            {
                tvAesthetics.text = GameCommonInfo.GetUITextById(10) + ":" + addAesthetics + "" + subAesthetics;
            }
            else
            {
                tvAesthetics.text = GameCommonInfo.GetUITextById(10) + ":" + addAesthetics;
            }
      
        }
    }
}