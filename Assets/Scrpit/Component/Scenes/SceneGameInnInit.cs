using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
public class SceneGameInnInit : BaseManager
{
    public CharacterDressManager characterDressManager;
    public GameDataManager gameDataManager;
    public NpcInfoManager npcInfoManager;
    public InnBuildManager innBuildManager;

    public InnFloorBuilder innFloorBuilder;
    public InnWallBuilder innWallBuilder;
    public InnFurnitureBuilder innFurnitureBuilder;

    public InnHandler innHandler;

    public NavMeshSurface2d navMesh;

    private void Start()
    {
        if (characterDressManager != null)
            characterDressManager.equipInfoController.GetAllEquipInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (innBuildManager != null)
            innBuildManager.buildDataController.GetAllBuildItemsData();

        if (innFloorBuilder != null)
            innFloorBuilder.StartBuild();
        if (innWallBuilder != null)
            innWallBuilder.StartBuild();
        if (innFurnitureBuilder != null)
            innFurnitureBuilder.StartBuild();
        //初始化客栈处理
        if (innHandler != null)
            innHandler.InitInn();
        navMesh.BuildNavMesh();
    }

}