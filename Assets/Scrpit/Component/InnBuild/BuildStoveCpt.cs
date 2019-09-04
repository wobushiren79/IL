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

    public GameObject leftTakeFoodObj;
    public GameObject RightTakeFoodObj;
    public GameObject upTakeFoodObj;
    public GameObject downTakeFoodObj;

    //食物模型
    public GameObject itemFoodModel;

    public StoveStatusEnum stoveStatus = StoveStatusEnum.Idle;

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
    /// 设置食物
    /// </summary>
    /// <param name="foodData"></param>
    public void CreateFood(InnFoodManager innFoodManager, OrderForCustomer orderForCustomer)
    {
        GameObject buildObj = GetBuilObj();
        Transform tfFoodFather = CptUtil.GetCptInChildrenByName<Transform>(buildObj, "Food");
        //创建食物
        GameObject foodObj = Instantiate(itemFoodModel, tfFoodFather);
        foodObj.SetActive(true);
        foodObj.transform.position = GameUtil.GetTransformInsidePosition2D(tfFoodFather);
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
        GameObject buildObj = GetBuilObj();
        Transform tfCook = CptUtil.GetCptInChildrenByName<Transform>(buildObj,"CookPosition");
        return tfCook.position;
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
        GameObject buildObj = GetBuilObj();
        Transform tfFoodFather = CptUtil.GetCptInChildrenByName<Transform>(buildObj, "Food");
        CptUtil.RemoveChild(tfFoodFather);
        SetStoveStatus(StoveStatusEnum.Idle);
    }
}