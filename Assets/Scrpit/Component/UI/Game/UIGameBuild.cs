using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UIGameBuild : BaseUIComponent, IRadioGroupCallBack
{
    public Text tvAesthetics;
    public Text tvNull;
    //返回按钮
    public Button btBack;
    public Button btDismantle;

    public RadioGroupView rgType;
    //类型按钮
    public RadioButtonView rbTypeBed;
    public RadioButtonView rbTypeTable;
    public RadioButtonView rbTypeStove;
    public RadioButtonView rbTypeCounter;
    public RadioButtonView rbTypeDoor;
    public RadioButtonView rbTypeStairs;
    public RadioButtonView rbTypeDecoration;
    public RadioButtonView rbTypeFloor;
    public RadioButtonView rbTypeWall;

    public Button btLayerFirstLayer;
    public Button btLayerSecondLayer;
    public GameObject objLayerSelect;

    public GameObject itemBuildContainer;
    public GameObject itemBuildModel;
    public GameObject itemBuildForBedModel;
    public BuildItemTypeEnum buildType = BuildItemTypeEnum.Table;

    public List<ItemGameBuildCpt> listBuildItem = new List<ItemGameBuildCpt>();
    public List<ItemGameBuildForBedCpt> listBuildForBedItem = new List<ItemGameBuildForBedCpt>();
    //客栈楼层
    public int innLayer = 1;

    public override void Awake()
    {
        base.Awake();
        if (rgType != null)
            rgType.SetCallBack(this);
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btDismantle != null)
            btDismantle.onClick.AddListener(DismantleMode);
        if (btLayerFirstLayer != null)
            btLayerFirstLayer.onClick.AddListener(OnClickForFirstLayer);
        if (btLayerSecondLayer != null)
            btLayerSecondLayer.onClick.AddListener(OnClickForSecondLayer);
        SetInnAesthetics();
    }

    public override void OpenUI()
    {
        base.OpenUI();

        //停止时间
        uiGameManager.gameTimeHandler.SetTimeStatus(true);
        uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Build);
        uiGameManager.innHandler.CloseInn();

        SetInnLayer(1);

        SetBedRangeStatus(true);
    }


    /// <summary>
    /// 设置床的范围
    /// </summary>
    /// <param name="isShow"></param>
    protected void SetBedRangeStatus( bool isShow)
    {
        //打开所有床位的范围显示
        BuildBedCpt[] listBed = uiGameManager.innFurnitureBuilder.GetAllBed();
        if (!CheckUtil.ArrayIsNull(listBed) )
        {
            for (int i=0;i< listBed.Length;i++)
            {
                BuildBedCpt buildBed= listBed[i];
                buildBed.ShowRange(isShow);
            }
        }
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
        uiGameManager.gameDataHandler.AddTimeProcess(60);
        //继续时间
        uiGameManager.gameTimeHandler.SetTimeStatus(false);
        //设置角色到门口
        Vector3 startPosition = uiGameManager.innHandler.GetRandomEntrancePosition();
        uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Normal).SetFollowPosition(startPosition + new Vector3(0, -2, 0));
        //隐藏床的范围显示
        SetBedRangeStatus(false);
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshUI()
    {
        //刷新列表数据
        List<ItemBean> listBuildData = uiGameManager.gameDataManager.gameData.GetBuildDataByType(uiGameManager.innBuildManager, buildType);
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
        //刷新床列表
        for (int i = 0; i < listBuildForBedItem.Count; i++)
        {
            ItemGameBuildForBedCpt itemGameBuildForBed= listBuildForBedItem[i];
            if (itemGameBuildForBed.buildBedData.isSet)
            {
                itemGameBuildForBed.gameObject.SetActive(false);
            }
            else
            {
                itemGameBuildForBed.gameObject.SetActive(true);
            }
        }
        //刷新美观值
        uiGameManager.gameDataManager.gameData.GetInnAttributesData().SetAesthetics
            (uiGameManager.innBuildManager, uiGameManager.gameDataManager.gameData.GetInnBuildData());
        SetInnAesthetics();

        SetBedRangeStatus(true);
    }

    /// <summary>
    /// 设置客栈楼层
    /// </summary>
    /// <param name="layer"></param>
    public void SetInnLayer(int layer)
    {
        this.innLayer = layer;
        btDismantle.gameObject.SetActive(true);


        InnBuildBean innBuild = uiGameManager.gameDataManager.gameData.GetInnBuildData();
        if (innBuild.innSecondWidth == 0 || innBuild.innSecondHeight == 0)
        {
            objLayerSelect.SetActive(false);
        }
        else
        {
            objLayerSelect.SetActive(true);
        }

        //镜头初始化
        ControlForBuildCpt controlForBuild = ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build)));
        controlForBuild.SetLayer(innLayer);
        if (innLayer == 1)
        {
            rbTypeBed.gameObject.SetActive(false);
            rbTypeStairs.gameObject.SetActive(false);
            rbTypeTable.gameObject.SetActive(true);
            rbTypeCounter.gameObject.SetActive(true);
            rbTypeStove.gameObject.SetActive(true);
            rbTypeDoor.gameObject.SetActive(true);
            rgType.SetPosition(1, true);
        }
        else if (innLayer == 2)
        {
            rbTypeBed.gameObject.SetActive(true);
            rbTypeStairs.gameObject.SetActive(true);
            rbTypeTable.gameObject.SetActive(false);
            rbTypeCounter.gameObject.SetActive(false);
            rbTypeStove.gameObject.SetActive(false);
            rbTypeDoor.gameObject.SetActive(false);
            rgType.SetPosition(0, true);
        }
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
        if (itemBuildContainer == null)
            return;
        bool hasData = false;
        buildType = type;

        if (type == BuildItemTypeEnum.Bed)
        {
            if (itemBuildForBedModel == null)
                return;
            if (uiGameManager.gameDataManager.gameData.listBed == null)
                return;
            CptUtil.RemoveChildsByActive(itemBuildContainer);
            listBuildItem.Clear();
            listBuildForBedItem.Clear();
            for (int i = 0; i < uiGameManager.gameDataManager.gameData.listBed.Count; i++)
            {
                BuildBedBean itemData = uiGameManager.gameDataManager.gameData.listBed[i];
                CreateBuildForBedItem(itemData);
                hasData = true;
            }
        }
        else
        {
            if (itemBuildModel == null)
                return;
            if (uiGameManager.gameDataManager.gameData.listBuild == null)
                return;
            CptUtil.RemoveChildsByActive(itemBuildContainer);
            listBuildItem.Clear();
            listBuildForBedItem.Clear();

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
        GameObject itemBuildObj = Instantiate(itemBuildContainer, itemBuildModel);
        ItemGameBuildCpt itemCpt = itemBuildObj.GetComponent<ItemGameBuildCpt>();
        itemCpt.SetData(itemData, buildData);
        listBuildItem.Add(itemCpt);
    }

    /// <summary>
    /// 创建单个数据
    /// </summary>
    /// <param name="buildBedData"></param>
    public void CreateBuildForBedItem(BuildBedBean buildBedData)
    {
        GameObject itemBuildObj = Instantiate(itemBuildContainer, itemBuildForBedModel);
        ItemGameBuildForBedCpt itemCpt = itemBuildObj.GetComponent<ItemGameBuildForBedCpt>();
        itemCpt.SetData(buildBedData);
        listBuildForBedItem.Add(itemCpt);
        if (buildBedData.isSet)
        {
            itemBuildObj.SetActive(false);
        }
        else
        {
            itemBuildObj.SetActive(true);
        }
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).ClearBuildItem();
        //重新构建地形
        AstarPath.active.Scan();
        //重新构建客栈
        uiGameManager.innHandler.InitInn();
        if (uiGameManager.gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Work)
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
    /// 点击第一层
    /// </summary>
    public void OnClickForFirstLayer()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetInnLayer(1);
    }


    /// <summary>
    /// 点击第二层
    /// </summary>
    public void OnClickForSecondLayer()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetInnLayer(2);
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
            Tilemap tilemap = uiGameManager.innWallBuilder.GetTilemapContainer().GetComponent<Tilemap>();
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
            if (listRenderer != null)
            {
                foreach (SpriteRenderer itemRenderer in listRenderer)
                {
                    //排出半透明的范围显示
                    if (itemRenderer.name.Contains("AddRange")|| itemRenderer.name.Contains("Shadow"))
                    {
                        continue;
                    }
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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
        else if (rbview == rbTypeBed)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Bed);
        }
        else if (rbview == rbTypeStairs)
        {
            SetInnBuildActive(true, true);
            CreateBuildList(BuildItemTypeEnum.Stairs);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion 
}