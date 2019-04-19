using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildStoveCpt : BaseBuildItemCpt
{
    //当前操作厨师
    public NpcAIWorkerCpt chefCpt;
    //客栈管理
    public InnHandler innHandler;
    //食物资源管理
    public InnFoodManager innFoodManager;

    public GameObject leftTakeFoodObj;
    public GameObject RightTakeFoodObj;
    public GameObject upTakeFoodObj;
    public GameObject downTakeFoodObj;

    //食物模型
    public GameObject itemFoodModel;

    /// <summary>
    /// 获取获取食物坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTakeFoodPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftTakeFoodObj.transform.position;
            case Direction2DEnum.Right:
                return RightTakeFoodObj.transform.position;
            case Direction2DEnum.UP:
                return upTakeFoodObj.transform.position;
            case Direction2DEnum.Down:
                return downTakeFoodObj.transform.position;
        }
        return transform.position;
    }

    /// <summary>
    /// 设置初始
    /// </summary>
    /// <param name="chef"></param>
    public void SetChef(NpcAIWorkerCpt chef)
    {
        this.chefCpt = chef;
    }

    /// <summary>
    /// 清楚厨师
    /// </summary>
    public void ClearChef()
    {
        chefCpt = null;
    }

    /// <summary>
    /// 设置食物
    /// </summary>
    /// <param name="foodData"></param>
    public void SetFood(MenuForCustomer foodData)
    {
        GameObject buildObj = GetBuilObj();
        Transform tfFoodFather = CptUtil.GetCptInChildrenByName<Transform>(buildObj, "Food");
        GameObject foodObj = Instantiate(itemFoodModel, tfFoodFather);
        foodObj.transform.position = new Vector3
            (Random.Range(foodObj.transform.position.x - 0.2f, foodObj.transform.position.x + 0.2f),
            Random.Range(foodObj.transform.position.y - 0.2f, foodObj.transform.position.y + 0.2f),
            0);
        FoodForCustomerCpt foodCpt = foodObj.GetComponent<FoodForCustomerCpt>();
        foodCpt.innFoodManager = innFoodManager;
        foodCpt.SetData(foodData);

        //送餐
        innHandler.sendQueue.Add(foodCpt);
    }

    /// <summary>
    /// 获取烹饪点
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetCookPosition()
    {
        GameObject buildObj = GetBuilObj();
        Transform[] allTF = buildObj.GetComponentsInChildren<Transform>();
        List<Vector3> listPosition = new List<Vector3>();
        foreach (Transform itemTF in allTF)
        {
            if (itemTF.name.Contains("CookPosition"))
            {
                listPosition.Add(itemTF.position);
            }
        }
        return listPosition;
    }
}