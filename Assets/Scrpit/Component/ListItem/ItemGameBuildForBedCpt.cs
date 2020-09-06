﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameBuildForBedCpt : ItemGameBaseCpt
{
    public BedShowView bedShow;
    public Text tvBedName;
    public Text tvBedPrice;
    public InfoBedPopupButton infoBedPopup;
    public Button btBuild;

    public BuildBedBean buildBedData;

    private void Awake()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(StartBuild);
    }

    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        infoBedPopup.SetData(buildBedData);
        SetName(buildBedData.bedName);
        buildBedData.GetPrice(out long outPriceL, out long outPriceM, out long outPriceS);
        SetPrice(outPriceS);
        SetBed(buildBedData);
    }


    public void SetBed(BuildBedBean buildBedData)
    {
        if (buildBedData != null && bedShow != null)
        {
            bedShow.SetData(buildBedData);
        }
    }

    public void SetName(string name)
    {
        if (tvBedName != null)
        {
            tvBedName.text = name;
        }
    }

    public void SetPrice(long priceS)
    {
        if (tvBedPrice != null)
        {
            tvBedPrice.text = priceS + "/" + GameCommonInfo.GetUITextById(37);
        }
    }

    /// <summary>
    /// 开始建造
    /// </summary>
    public void StartBuild()
    {
        ControlForBuildCpt controlForBuild = (ControlForBuildCpt)GetUIManager<UIGameManager>().controlHandler.GetControl(ControlHandler.ControlEnum.Build);
       // controlForBuild.ShowBuildItem(buildData.id);
    }
}