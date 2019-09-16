using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildStoveCpt : BaseBuildItemCpt
{
    public enum StoveStatusEnum {
        Idle = 0,//空闲
        Ready = 1,//准备中
        Cooking = 2,//料理中
    }
    public GameObject objCookPosition;
    public GameObject objTakePosition;
    public GameObject objFoodContainer;

    //食物模型
    public GameObject itemFoodModel;


    public StoveStatusEnum stoveStatus = StoveStatusEnum.Idle;

    /// <summary>
    /// 获取获取食物坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTakeFoodPosition()
    {
        return objTakePosition.transform.position;
    }

    /// <summary>
    /// 设置食物
    /// </summary>
    /// <param name="foodData"></param>
    public void CreateFood(InnFoodManager innFoodManager, OrderForCustomer orderForCustomer)
    {
        //创建食物
        GameObject foodObj = Instantiate(itemFoodModel, objFoodContainer.transform);
        foodObj.SetActive(true);
        foodObj.transform.position = GameUtil.GetTransformInsidePosition2D(foodObj.transform);
        FoodForCustomerCpt foodCpt = foodObj.GetComponent<FoodForCustomerCpt>();
        foodCpt.innFoodManager = innFoodManager;
        foodCpt.SetData(orderForCustomer.foodData);
        orderForCustomer.foodCpt= foodCpt;
    }

    /// <summary>
    /// 获取烹饪点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookPosition()
    {
        return objCookPosition.transform.position;
    }

    /// <summary>
    /// 设置灶台状态
    /// </summary>
    /// <param name="stoveStatus"></param>
    public void SetStoveStatus(StoveStatusEnum stoveStatus)
    {
        this.stoveStatus = stoveStatus;
    }

    /// <summary>
    /// 清理灶台
    /// </summary>
    public void ClearStove()
    {
        CptUtil.RemoveChild(objFoodContainer.transform);
        SetStoveStatus(StoveStatusEnum.Idle);
    }
}