using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIGameBuild : UIGameComponent
{
    public Text tvAesthetics;

    //返回按钮
    public Button btBack;
    public Button btDismantle;
    //类型按钮
    public Button btTypeTable;
    public Button btTypeStove;
    public Button btTypeCounter;
    public Button btTypeDoor;

    public GameObject listBuildContent;
    public GameObject itemBuildModel;

    public BuildItemTypeEnum buildType = BuildItemTypeEnum.Table;

    public void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btDismantle != null)
            btDismantle.onClick.AddListener(DismantleMode);
        if (btTypeTable != null)
            btTypeTable.onClick.AddListener(CreateTableList);
        if (btTypeStove != null)
            btTypeStove.onClick.AddListener(CreateStoveList);
        if (btTypeCounter != null)
            btTypeCounter.onClick.AddListener(CreateCounterList);
        if (btTypeDoor != null)
            btTypeDoor.onClick.AddListener(CreateDoorList);

        SetInnAesthetics();
        CreateBuildList(BuildItemTypeEnum.Table);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //停止时间
        uiGameManager.gameTimeHandler.SetTimeStatus(true);
        uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Build);
        uiGameManager.innHandler.CloseInn();
    }
    public override void CloseUI()
    {
        base.CloseUI();
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
    public void CreateBuildList(BuildItemTypeEnum  type)
    {
        buildType = type;
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).DestoryBuildItem();
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (uiGameManager.gameDataManager.gameData.listBuild == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);

        for (int i = 0; i < uiGameManager.gameDataManager.gameData.listBuild.Count; i++)
        {
            ItemBean itemData = uiGameManager.gameDataManager.gameData.listBuild[i];
            BuildItemBean buildData = uiGameManager.innBuildManager.GetBuildDataById(itemData.itemId);
            if (buildData == null)
                continue;
            if ((int)type == buildData.build_type)
            {
                CreateBuildItem(itemData, buildData);
            }
        }
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

    public void CreateTableList()
    {
        CreateBuildList(BuildItemTypeEnum.Table);
    }

    public void CreateStoveList()
    {
        CreateBuildList(BuildItemTypeEnum.Stove);
    }

    public void CreateCounterList()
    {
        CreateBuildList(BuildItemTypeEnum.Counter);
    }

    public void CreateDoorList()
    {
        CreateBuildList(BuildItemTypeEnum.Door);
    }

    public void DismantleMode()
    {
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).SetDismantleMode();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        //删除当前选中
        ((ControlForBuildCpt)(uiGameManager.controlHandler.GetControl(ControlHandler.ControlEnum.Build))).DestoryBuildItem();
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
}