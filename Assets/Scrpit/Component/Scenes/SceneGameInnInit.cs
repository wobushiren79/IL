using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;

public class SceneGameInnInit : BaseManager
{
    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;
    public NpcInfoManager npcInfoManager;
    public InnBuildManager innBuildManager;

    public InnFloorBuilder innFloorBuilder;
    public InnWallBuilder innWallBuilder;
    public InnFurnitureBuilder innFurnitureBuilder;

    public InnHandler innHandler;

    public NavMeshSurface navMesh;

    public NpcCustomerBuilder leftCustomerBuilder;
    public NpcCustomerBuilder rightCustomerBuilder;

    private void Start()
    {
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();

        //构建地板
        if (innFloorBuilder != null)
            innFloorBuilder.StartBuild();
        //构建墙壁
        if (innWallBuilder != null)
            innWallBuilder.StartBuild();
        //构建建筑
        if (innFurnitureBuilder != null)
            innFurnitureBuilder.StartBuild();
        //初始化客栈处理
        if (innHandler != null)
            innHandler.InitInn();


        //客栈边界生成
        if (leftCustomerBuilder != null)
        {
            leftCustomerBuilder.transform.position = new Vector3(-30, -2.5f, 0);
        }
        if (rightCustomerBuilder != null)
        {
            rightCustomerBuilder.transform.position = new Vector3(30 + gameDataManager.gameData.GetInnBuildData().innWidth, -2.5f, 0);
        }

        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        navMesh.BuildNavMesh();
    }  
}