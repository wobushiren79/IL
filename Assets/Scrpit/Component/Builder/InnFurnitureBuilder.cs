using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFurnitureBuilder : BaseMonoBehaviour
{
    public InnBuildManager innBuildManager;
    public GameDataManager gameDataManager;
    //装饰容器
    public GameObject buildContainer;
    //客栈处理
    public InnHandler innHandler;
    //食物资源管理
    public InnFoodManager innFoodManager;

    public void StartBuild()
    {
        List<InnResBean> listData = gameDataManager.gameData.GetInnBuildData().GetFurnitureList();
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            BuildFurniture(itemData);
        }
    }

    /// <summary>
    /// 修建建筑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject BuildFurniture(long id)
    {
        InnResBean innResBean = new InnResBean(id, Vector3.zero, new List<Vector3>(), Direction2DEnum.Left);
        return BuildFurniture(innResBean);
    }

    /// <summary>
    /// 修建建筑
    /// </summary>
    /// <param name="furnitureData"></param>
    /// <returns></returns>
    public GameObject BuildFurniture(InnResBean furnitureData)
    {
        if (furnitureData == null)
            return null;
        GameObject buildItemObj = innBuildManager.GetFurnitureObjById(furnitureData.id, buildContainer.transform);
        buildItemObj.transform.position = TypeConversionUtil.Vector3BeanToVector3(furnitureData.startPosition);
        BaseBuildItemCpt buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();
        buildItemCpt.SetDirection(furnitureData.direction);

        if (buildItemCpt.buildId >= 40000 && buildItemCpt.buildId < 50000)
        {
            //判断是灶台
            BuildStoveCpt stoveCpt = (BuildStoveCpt)buildItemCpt;
            stoveCpt.innFoodManager = innFoodManager;
            stoveCpt.innHandler = innHandler;
        }
        return buildItemObj;
    }

    /// <summary>
    /// 删除指定坐标的建筑
    /// </summary>
    /// <param name="position"></param>
    public void DestroyFurnitureByPosition(Vector3 position)
    {
        BaseBuildItemCpt[] buildList = buildContainer.GetComponentsInChildren<BaseBuildItemCpt>();
        BaseBuildItemCpt destroyCpt = null;
        foreach (BaseBuildItemCpt itemData in buildList)
        {
            List<Vector3> listPosition = itemData.GetBuildWorldPosition();
            foreach (Vector3 itemPosition in listPosition)
            {
                if (itemPosition.x == position.x && itemPosition.y == position.y)
                {
                    destroyCpt = itemData;
                    break;
                }
            }
            if (destroyCpt != null)
                break;
        };
        if (destroyCpt != null)
            Destroy(destroyCpt.gameObject);
    }
}