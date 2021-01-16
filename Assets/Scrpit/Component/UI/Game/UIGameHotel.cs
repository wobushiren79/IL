using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class UIGameHotel : BaseUIComponent
{
    public Button btBack;
    public GameObject objBedContainer;
    public GameObject objBedModel;

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
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        List<BuildBedBean> listBed = uiGameManager.gameDataManager.gameData.listBed;
        this.listBedData.Clear();
        this.listBedData.AddRange(listBed);
        CreateBedList();
    }

    public void CreateBedList()
    {
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objBedContainer.transform);
        StartCoroutine(CoroutineForCreateBedList());
    }


    public IEnumerator CoroutineForCreateBedList()
    {
        for (int i = 0; i < listBedData.Count; i++)
        {
            BuildBedBean itemData = listBedData[i];
            GameObject objBed = Instantiate(objBedContainer, objBedModel);
            ItemGameHotelBedCpt itemCpt = objBed.GetComponent<ItemGameHotelBedCpt>();
            itemCpt.SetData(itemData);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 默认排序点击
    /// </summary>
    public void OnClickForSortDef()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        List<BuildBedBean> listBed = uiGameManager.gameDataManager.gameData.listBed;
        this.listBedData.Clear();
        this.listBedData.AddRange(listBed);
        CreateBedList();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLevelUp()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.bedStatus;
            }).ToList();
        CreateBedList();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortPrice()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.priceS;
            }).ToList();
        CreateBedList();
    }

    /// <summary>
    /// 等级排序点击
    /// </summary>
    public void OnClickForSortLevel()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listBedData = this.listBedData.OrderByDescending(
            (data) =>
            {
                return data.bedLevel;
            }).ToList();
        CreateBedList();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OnClickForBack()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}