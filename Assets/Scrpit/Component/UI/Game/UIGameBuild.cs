using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIGameBuild : BaseUIComponent
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
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
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
        if (tvAesthetics != null)
            tvAesthetics.text = gameDataManager.gameData.GetInnAttributesData().GetAesthetics() + "";
        CreateBuildList(BuildItemTypeEnum.Table);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //停止时间
        GetUIManager<UIGameManager>().gameTimeHandler.SetTimeStatus(true);
        GetUIManager<UIGameManager>().controlHandler.StartControl(ControlHandler.ControlEnum.Build);
        GetUIManager<UIGameManager>().innHandler.CloseInn();
    }
    public override void CloseUI()
    {
        base.CloseUI();
        //时间添加1小时
        GetUIManager<UIGameManager>().gameTimeHandler.AddHour(1);
        //继续时间
        GetUIManager<UIGameManager>().gameTimeHandler.SetTimeStatus(false);
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshUI()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        //刷新列表数据
        CreateBuildList(buildType);
        //刷新美观值
        gameDataManager.gameData.GetInnAttributesData().SetAesthetics(innBuildManager, gameDataManager.gameData.GetInnBuildData());
        if (tvAesthetics != null)
            tvAesthetics.text = gameDataManager.gameData.GetInnAttributesData().GetAesthetics() + "";
    }

    /// <summary>
    /// 创建建筑列表
    /// </summary>
    /// <param name="type"></param>
    public void CreateBuildList(BuildItemTypeEnum  type)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        buildType = type;
        //删除当前选中
        ((ControlForBuildCpt)(controlHandler.GetControl(ControlHandler.ControlEnum.Build))).DestoryBuildItem();
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (gameDataManager == null)
            return;
        if (gameDataManager.gameData == null)
            return;
        if (gameDataManager.gameData.listBuild == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);

        for (int i = 0; i < gameDataManager.gameData.listBuild.Count; i++)
        {
            ItemBean itemData = gameDataManager.gameData.listBuild[i];
            BuildItemBean buildData = innBuildManager.GetBuildDataById(itemData.itemId);
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
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        ((ControlForBuildCpt)(controlHandler.GetControl(ControlHandler.ControlEnum.Build))).SetDismantleMode();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        InnHandler innHandler = GetUIManager<UIGameManager>().innHandler;
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        NavMeshSurface navMesh = GetUIManager<UIGameManager>().navMesh;

        //删除当前选中
        ((ControlForBuildCpt)(controlHandler.GetControl(ControlHandler.ControlEnum.Build))).DestoryBuildItem();
        //重新构建地形
        navMesh.BuildNavMesh();
        //打开主UI
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));

        if (gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Work)
        {
            //如果是工作日 开店继续营业
            innHandler.OpenInn();
            //恢复工作日控制器
            controlHandler.StartControl(ControlHandler.ControlEnum.Work);
        }
        else
        {
            //恢复休息日控制器
            controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
        }
    }
}