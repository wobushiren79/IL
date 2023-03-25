using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class UIGameHotel : BaseUIComponent
{
    public Button btBack;
    public ScrollGridVertical gridVertical;

    [Header("排序")]
    public Button btSortDef;
    public Button btSortLevel;
    public Button btSortLevelUp;
    public Button btSortPrice;

    public List<BuildBedBean> listBedData = new List<BuildBedBean>();

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OnClickForBack);
        if (btSortDef != null)
            btSortDef.onClick.AddListener(OnClickForSortDef);
        if (btSortLevel != null)
            btSortLevel.onClick.AddListener(OnClickForSortLevel);
        if (btSortLevelUp != null)
            btSortLevelUp.onClick.AddListener(OnClickForSortLevelUp);
        if (btSortPrice != null)
            btSortPrice.onClick.AddListener(OnClickForSortPrice);

        gridVertical.AddCellListener(OnCellForItems);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<BuildBedBean> listBed = gameData.listBed;
        this.listBedData.Clear();
        this.listBedData.AddRange(listBed);
        gridVertical.SetCellCount(listBedData.Count);
        gridVertical.RefreshAllCells();
    }

    public void OnCellForItems(ScrollGridCell itemCell)
    {
        BuildBedBean itemData = listBedData[itemCell.index];
        ItemGameHotelBedCpt itemCpt = itemCell.GetComponent<ItemGameHotelBedCpt>();
        itemCpt.SetData(itemData);
    }


    /// <summary>
    /// 默认排序点击
    /// </summary>
    public void OnClickForSortDef()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<BuildBedBean> listBed = gameData.listBed;
        this.listBedData.Clear();
        this.listBedData.AddRange(listBed);
        gridVertical.SetCellCount(listBedData.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLevelUp()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.bedStatus;
            }).ToList();
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortPrice()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.priceS;
            }).ToList();
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 等级排序点击
    /// </summary>
    public void OnClickForSortLevel()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.bedLevel;
            }).ToList();
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OnClickForBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }
}