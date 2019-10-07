using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class CharacterWorkerForChefBean : CharacterWorkerBaseBean
{
    //烹饪次数
    public long cookNumber;
    //料理时间
    public float cookTime;
    //烹饪不同食物的次数
    public List<ItemBean> listFoodCookNumer = new List<ItemBean>();

    public CharacterWorkerForChefBean()
    {
        workerType = WorkerEnum.Chef;
    }


    /// <summary>
    /// 增加烹饪次数
    /// </summary>
    /// <param name="numerber"></param>
    /// <param name="foodId"></param>
    public void AddCookNumber(int numerber,long foodId)
    {
        cookNumber += numerber;
        bool isAdd = false;
        foreach (ItemBean itemData in listFoodCookNumer)
        {
            if(itemData.itemId== foodId)
            {    
                itemData.itemNumber += numerber;
                isAdd = true;
            }
        }
        if (!isAdd)
        {
            listFoodCookNumer.Add(new ItemBean(foodId,numerber));
        }
    }

    /// <summary>
    /// 增加料理时间
    /// </summary>
    /// <param name="cookTime"></param>
    public void AddCookTime(float cookTime)
    {
        this.cookTime += cookTime;
    }
}