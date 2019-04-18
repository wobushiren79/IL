using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildStoveCpt : BaseBuildItemCpt
{
    //当前操作厨师
    public NpcAIWorkerCpt chefCpt;
    //数据管理
    public InnFoodManager innFoodManager;
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
    public void SetFood(MenuInfoBean foodData)
    {
        GameObject buildObj = GetBuilObj();
        SpriteRenderer spFood= CptUtil.GetCptInChildrenByName<SpriteRenderer>(buildObj, "Take");
        spFood.sprite = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
    }

    public void TakeFood()
    {

    }

    /// <summary>
    /// 获取烹饪点
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetCookPosition()
    {
        GameObject buildObj= GetBuilObj();
        Transform[] allTF= buildObj.GetComponentsInChildren<Transform>();
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