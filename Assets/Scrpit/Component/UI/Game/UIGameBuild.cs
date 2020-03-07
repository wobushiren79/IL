using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

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
        //继续时间
        uiGameManager.gameTimeHandler.SetTimeStatus(false);
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshUI()
    {
        //刷新列表数据
        CreateBuildList(buildType);
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
            long aesthetics = uiGameManager.gameDataManager.gameData.GetInnAttributesData().GetAesthetics(out string aestheticsLevel);
            tvAesthetics.text = aesthetics + " " + aestheticsLevel;
        }
    }

    /// <summary>
    /// 创建建筑列表
    /// </summary>
    /// <param name="type"></param>
    public void CreateBuildList(BuildItemTypeEnum type)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        buildType = type;
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (uiGameManager.gameDataManager.gameData.listBuild == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);

        bool hasData = false;
        for (int i = 0; i < uiGameManager.gameDataManager.gameData.listBuild.Count; i++)
        {
            ItemBean itemData = uiGameManager.gameDataManager.gameData.listBuild[i];
            BuildItemBean buildData = uiGameManager.innBuildManager.GetBuildDataById(itemData.itemId);
            if (buildData == null)
                continue;
            if ((int)type == buildData.build_type)
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
        uiGameManager.navMesh.BuildNavMesh();
        //打开主UI
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));

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
    }

    /// <summary>
    /// 设置建造相关是否展示
    /// </summary>
    /// <param name="wall"></param>
    /// <param name="furniture"></param>
    private void SetInnBuildActive(bool wall, bool furniture)
    {
        if (uiGameManager.innWallBuilder != null)
            uiGameManager.innWallBuilder.GetTilemapContainer().SetActive(wall);
        if (uiGameManager.innFurnitureBuilder != null)
            uiGameManager.innFurnitureBuilder.buildContainer.SetActive(furniture);
    }

    #region  类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
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