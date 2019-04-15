using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFurnitureBuilder : BaseMonoBehaviour
{
    public InnBuildManager innBuildManager;
    public GameDataManager gameDataManager;
    //装饰容器
    public GameObject buildContainer;

    private void Start()
    {
        List<InnResBean>  listData=  gameDataManager.gameData.GetInnBuildData().GetFurnitureList() ;
        for(int i=0;i< listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildFurniture(itemData);
        }
    }

    public void BuildFurniture(InnResBean furnitureData)
    {
        if (furnitureData == null)
            return;
        GameObject buildItemObj = innBuildManager.GetFurnitureObjById(furnitureData.id);
        buildItemObj.transform.SetParent(buildContainer.transform);
        buildItemObj.transform.position = TypeConversionUtil.Vector3BeanToVector3(furnitureData.startPosition);
        BaseBuildItemCpt buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();
        buildItemCpt.SetDirection(furnitureData.direction);
    }


}