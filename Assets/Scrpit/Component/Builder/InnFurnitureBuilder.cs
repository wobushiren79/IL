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

    /// <summary>
    /// 开始建造
    /// </summary>
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
        return buildItemObj;
    }


    /// <summary>
    /// 删除指定坐标的建筑
    /// </summary>
    /// <param name="position"></param>
    public void DestroyFurnitureByPosition(Vector3 position)
    {
        BaseBuildItemCpt buildCpt = GetFurnitureByPosition(position);
        if(buildCpt != null)
            Destroy(buildCpt.gameObject);
    }

    /// <summary>
    /// 通过坐标获取建筑物
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseBuildItemCpt GetFurnitureByPosition(Vector3 position)
    {
        BaseBuildItemCpt[] buildList = buildContainer.GetComponentsInChildren<BaseBuildItemCpt>();
        BaseBuildItemCpt target = null;
        foreach (BaseBuildItemCpt itemData in buildList)
        {
            if (itemData.buildItemData.id == -1)
                continue;
            List<Vector3> listPosition = itemData.GetBuildWorldPosition();
            foreach (Vector3 itemPosition in listPosition)
            {
                if (itemPosition.x == position.x-0.5f && itemPosition.y == position.y+0.5f)
                {
                    target = itemData;
                    break;
                }
            }
        }
        return target;
    }
}