﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UIGameBuild : UIGameComponent, IRadioGroupCallBack
{
    public Text tvAesthetics;
    public Text tvNull;
    //返回按钮
    public Button btBack;
    public Button btDismantle;

    public RadioGroupView rgType;
    //类型按钮
    public RadioButtonView rbTypeTable;
    public RadioButtonView rbTypeStove;
    public RadioButtonView rbTypeCounter;
    public RadioButtonView rbTypeDoor;
    public RadioButtonView rbTypeDecoration;
    public RadioButtonView rbTypeFloor;
    public RadioButtonView rbTypeWall;

    public GameObject listBuildContent;
    public GameObject itemBuildModel;

    public BuildItemTypeEnum buildType = BuildItemTypeEnum.Table;

    public List<ItemGameBuildCpt> listBuildItem = new List<ItemGameBuildCpt>();
    public void Start()
    {
        if (rgType != null)
            rgType.SetCallBack(this);
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btDismantle != null)
            btDismantle.onClick.AddListener(DismantleMode);

        SetInnAesthetics();

    }

    public override void OpenUI()
    {
        base.OpenUI();
        btDismantle.gameObject.SetActive(true);
        rgType.SetPosition(0, false);
        CreateBuildList(BuildItemTypeEnum.Table);
        //停止时间
        uiGameManager.gameTimeHandler.SetTimeStatus(true);
        uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Build);
        uiGameManager.innHandler.CloseInn();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).ClearBuildItem();
        SetInnBuildActive(true, true);
        //时间添加1小时
        uiGameManager.gameTimeHandler.AddHour(1);
        //添加研究经验
        uiGameManager.gameDataHandler.AddMenuResearch(1);
        //继续时间
        uiGameManager.gameTimeHandler.SetTimeStatus(false);
        //设置角色到门口
        Vector3 startPosition = uiGameManager.innHandler.GetRandomEntrancePosition();
        uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Normal).SetFollowPosition(startPosition + new Vector3(0, -2, 0));
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshUI()
    {
        //刷新列表数据
        List<ItemBean> listBuildData = uiGameManager.gameDataManager.gameData.GetBuildDataByType(uiGameManager.innBuildManager,buildType);
        for (int i = 0; i < listBuildItem.Count; i++)
        {
            ItemGameBuildCpt itemBuild = listBuildItem[i];
            //如果数据已经没有了则删除该项
            if (itemBuild.itemData == null || itemBuild.itemData.itemNumber == 0)
            {
                listBuildItem.Remove(itemBuild);
                Destroy(itemBuild.gameObject);
                i--;
            }
        }
        //如果有新增的数据
        foreach (ItemBean itemData in listBuildData)
        {
            bool hasData = false;
            for (int i = 0; i < listBuildItem.Count; i++)
            {
                ItemGameBuildCpt itemBuild = listBuildItem[i];
                itemBuild.RefreshUI();
                if (itemData.itemId == itemBuild.itemData.itemId)
                {
                    hasData = true;
                }
            }
            if (!hasData)
            {
                BuildItemBean buildData = uiGameManager.innBuildManager.GetBuildDataById(itemData.itemId);
                CreateBuildItem(itemData, buildData);
                tvNull.gameObject.SetActive(false);
            }
        }
        //刷新美观值
        uiGameManager.gameDataManager.gameData.GetInnAttributesData().SetAesthetics
            (uiGameManager.innBuildManager, uiGameManager.gameDataManager.gameData.GetInnBuildData());
        SetInnAesthetics();
    }

    /// <summary>
    /// 设置客栈美观
    /// </summary>
    public void SetInnAesthetics()
    {
        if (tvAesthetics != null)
        {
            uiGameManager.gameDataManager.gameData.GetInnAttributesData().GetAesthetics(out float maxAesthetics, out float aesthetics);
            tvAesthetics.text = aesthetics + "/" + maxAesthetics;
        }
    }

    /// <summary>
    /// 创建建筑列表
    /// </summary>
    /// <param name="type"></param>
    public void CreateBuildList(BuildItemTypeEnum type)
    {
        buildType = type;
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (uiGameManager.gameDataManager.gameData.listBuild == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);
        listBuildItem.Clear();
        bool hasData = false;
        for (int i = 0; i < uiGameManager.gameDataManager.gameData.listBuild.Count; i++)
        {
            ItemBean itemData = uiGameManager.gameDataManager.gameData.listBuild[i];
            BuildItemBean buildData = uiGameManager.innBuildManager.GetBuildDataById(itemData.itemId);
            if (buildData == null)
                continue;
            if (type == buildData.GetBuildType())
            {
                CreateBuildItem(itemData, buildData);
                hasData = true;
            }
        }
        if (hasData)
            tvNull.gameObject.SetActive(false);
        else
            tvNull.gameObject.SetActive(true);
    }

    /// <summary>
    /// 创建单个数据
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="buildData"></param>
    public void CreateBuildItem(ItemBean itemData, BuildItemBean buildData)
    {
        GameObject itemBuildObj = Instantiate(itemBuildModel, itemBuildModel.transform);
        itemBuildObj.SetActive(true);
        itemBuildObj.transform.SetParent(listBuildContent.transform);
        ItemGameBuildCpt itemCpt = itemBuildObj.GetComponent<ItemGameBuildCpt>();
        itemCpt.SetData(itemData, buildData);
        listBuildItem.Add(itemCpt);
    }

    /// <summary>
    /// 拆除模式
    /// </summary>
    public void DismantleMode()
    {
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).SetDismantleMode();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).ClearBuildItem();
        //重新构建地形
        AstarPath.active.Scan();
        //重新构建客栈
        uiGameManager.innHandler.InitInn();
        if (uiGameManager.gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作日 开店继续营业
            uiGameManager.innHandler.OpenInn();
            //恢复工作日控制器
            uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Work);
        }
        else
        {
            //恢复休息日控制器
            uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
        }
        //打开主UI
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

    /// <summary>
    /// 设置建造相关是否展示
    /// </summary>
    /// <param name="wall"></param>
    /// <param name="furniture"></param>
    private void SetInnBuildActive(bool wall, bool furniture)
    {
        //if (uiGameManager.innWallBuilder != null)
        //    uiGameManager.innWallBuilder.GetTilemapContainer().SetActive(wall);
        //if (uiGameManager.innFurnitureBuilder != null)
        //    uiGameManager.innFurnitureBuilder.buildContainer.SetActive(furniture);

        //设置半透明状态
        if (uiGameManager.innWallBuilder != null)
        {
            Tilemap tilemap= uiGameManager.innWallBuilder.GetTilemapContainer().GetComponent<Tilemap>();
            if (wall)
            {
                tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 1f);
            }
            else
            {
                tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 0.3f);
            }
        }
        if (uiGameManager.innFurnitureBuilder != null)
        {
            SpriteRenderer[] listRenderer = uiGameManager.innFurnitureBuilder.buildContainer.GetComponentsInChildren<SpriteRenderer>();
            if (listRenderer!=null)
            {
                foreach (SpriteRenderer itemRenderer in listRenderer)
                {
                    if (furniture)
                    {
                        itemRenderer.color = new Color(itemRenderer.color.r, itemRenderer.color.g, itemRenderer.color.b, 1f);
                    }
                    else
                    {
                        itemRenderer.color = new Color(itemRenderer.color.r, itemRenderer.color.g, itemRenderer.color.b, 0.3f);
                    }
                }
            }
        }
    }

    #region  类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).ClearBuildItem();
        btDismantle.gameObject.SetActive(true);
        if (rbview == rbTypeTable)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Table);
        }
        else if (rbview == rbTypeStove)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Stove);

        }
        else if (rbview == rbTypeCounter)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Counter);
        }
        else if (rbview == rbTypeDoor)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Door);
        }
        else if (rbview == rbTypeDecoration)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Decoration);
        }
        else if (rbview == rbTypeFloor)
        {
            SetInnBuildActive(false, false);
            btDismantle.gameObject.SetActive(false);
            CreateBuildList(BuildItemTypeEnum.Floor);
        }
        else if (rbview == rbTypeWall)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Wall);
        }

    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion 
}