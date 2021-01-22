using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFurnitureBuilder : BaseMonoBehaviour
{
    //装饰容器
    public GameObject buildContainer;

    //客栈处理
    protected InnHandler innHandler;
    protected GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
    }

    /// <summary>
    /// 开始建造
    /// </summary>
    public void StartBuild()
    {
        CptUtil.RemoveChildsByActive(buildContainer);
        List<InnResBean> listData = gameDataManager.gameData.GetInnBuildData().GetFurnitureList(1);
        BuildListFurniture(listData);
        List<InnResBean> listSecondData = gameDataManager.gameData.GetInnBuildData().GetFurnitureList(2);
        BuildListFurniture(listSecondData);
    }

    protected void BuildListFurniture(List<InnResBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            InnResBean itemData = listData[i];
            if (CheckUtil.StringIsNull(itemData.remarkId))
            {
                BuildFurniture(itemData, null);
            }
            else
            {
                //如果有备注ID说明是床或者其他建筑
                //是床
                BuildBedBean tempBuildBedData = null;
                //bool hasData = false;
                for (int f = 0; f < gameDataManager.gameData.listBed.Count; f++)
                {
                    BuildBedBean buildBedData = gameDataManager.gameData.listBed[f];
                    if (buildBedData != null && buildBedData.remarkId.Equals(itemData.remarkId))
                    {
                        tempBuildBedData = buildBedData;
                        //hasData = true;
                        break;
                    }
                }
                BuildFurniture(itemData, tempBuildBedData);
                //如果没有找到数据则删除这个建筑
                //if (!hasData)
                //{
                //    listData.Remove(itemData);
                //    i--;
                //}
            }
        }
    }

    /// <summary>
    /// 修建建筑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject BuildFurniture(long id, BuildBedBean buildBedData)
    {
        InnResBean innResBean = new InnResBean(id, Vector3.zero, new List<Vector3>(), Direction2DEnum.Left);
        return BuildFurniture(innResBean, buildBedData);
    }

    /// <summary>
    /// 修建建筑
    /// </summary>
    /// <param name="furnitureData"></param>
    /// <returns></returns>
    public GameObject BuildFurniture(InnResBean furnitureData, BuildBedBean buildBedData)
    {
        if (furnitureData == null)
            return null;
        GameObject buildItemObj = InnBuildHandler.Instance.manager.GetFurnitureObjById(furnitureData, buildContainer.transform, buildBedData);
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
        if (buildCpt != null)
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
                if (Mathf.RoundToInt((itemPosition.x + 0.5f)) == position.x && Mathf.RoundToInt((itemPosition.y - 0.5f)) == position.y)
                {
                    target = itemData;
                    break;
                }
            }
        }
        return target;
    }

    /// <summary>
    /// 获取所有的床
    /// </summary>
    /// <returns></returns>
    public BuildBedCpt[] GetAllBed()
    {
        BuildBedCpt[] buildList = buildContainer.GetComponentsInChildren<BuildBedCpt>();
        return buildList;
    }
}