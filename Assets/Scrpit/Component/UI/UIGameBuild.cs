using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIGameBuild : BaseUIComponent
{
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

    //数据管理
    public GameDataManager gameDataManager;
    public InnBuildManager innBuildManager;

    //控制
    public ControlForBuildCpt controlForBuild;
    public ControlForMoveCpt controlForMove;

    //游戏进程处理
    public InnHandler innHandler;
    public NavMeshSurface2d navMesh;

    public BuildItemBean.BuildType buildType = BuildItemBean.BuildType.Table;
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
        CreateBuildList(BuildItemBean.BuildType.Table);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        controlForBuild.StartControl();
        controlForMove.EndControl();
        innHandler.CloseInn();
    }

    /// <summary>
    /// 创建建筑列表
    /// </summary>
    /// <param name="type"></param>
    public void CreateBuildList(BuildItemBean.BuildType type)
    {
        buildType = type;
        //删除当前选中
        controlForBuild.DestoryBuild();
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (gameDataManager == null)
            return;
        if (gameDataManager.gameData == null)
            return;
        if (gameDataManager.gameData.buildItemList == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);

        for (int i = 0; i < gameDataManager.gameData.buildItemList.Count; i++)
        {
            ItemBean itemData = gameDataManager.gameData.buildItemList[i];
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
        ItemGameBuildCpt itemCpt= itemBuildObj.GetComponent<ItemGameBuildCpt>();
        itemCpt.SetData(itemData, buildData);
    }

    public void CreateTableList()
    {
        CreateBuildList(BuildItemBean.BuildType.Table);
    }
    
    public void CreateStoveList()
    {
        CreateBuildList(BuildItemBean.BuildType.Stove);
    }

    public void CreateCounterList()
    {
        CreateBuildList(BuildItemBean.BuildType.Counter);
    }
    public void CreateDoorList()
    {
        CreateBuildList(BuildItemBean.BuildType.Door);
    }

    public void DismantleMode()
    {
        controlForBuild.DismantleMode();
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public void RefreshData()
    {
        CreateBuildList(buildType);
    }
    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        controlForMove.StartControl();
        controlForBuild.EndControl();
        controlForBuild.DestoryBuild();
        navMesh.BuildNavMesh();
        innHandler.OpenInn();
        uiManager.OpenUIAndCloseOtherByName("Main");
    }
}