﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnHandler : BaseMonoBehaviour
{
    public enum InnStatusEnum
    {
        Open,
        Close,
    }

    //客栈状态
    public InnStatusEnum innStatus = InnStatusEnum.Open;
    //大门位置
    public Vector3 doorPosition;

    //数据管理
    public GameDataManager gameDataManager;
    public InnFoodManager innFoodManager;

    //客栈桌子处理
    public InnTableHandler innTableHandler;
    //烹饪处理
    public InnCookHandler innCookHandler;
    //服务处理
    public InnWaiterHandler innWaiterHandler;
    // 支付处理
    public InnPayHandler innPayHandler;

    //排队的人
    public List<NpcAICustomerCpt> cusomerQueue = new List<NpcAICustomerCpt>();
    //排队等待烹饪的食物
    public List<MenuForCustomer> foodQueue = new List<MenuForCustomer>();
    //排队送餐的食物
    public List<FoodForCustomerCpt> sendQueue = new List<FoodForCustomerCpt>();
    //排队清理的食物
    public List<FoodForCustomerCpt> clearQueue = new List<FoodForCustomerCpt>();
    //排队算账的人
    public List<NpcAICustomerCpt> payQueue = new List<NpcAICustomerCpt>();
    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        innTableHandler.InitTableList();
        innCookHandler.InitStoveList();
        innPayHandler.InitCounterList();
    }

    private void FixedUpdate()
    {
        if (innStatus == InnStatusEnum.Open)
        {
            //排队等待处理
            if (!CheckUtil.ListIsNull(cusomerQueue))
            {
                BuildTableCpt tableCpt = innTableHandler.GetIdleTable();
                if (tableCpt != null)
                {
                    cusomerQueue[0].SetTable(tableCpt);
                    cusomerQueue.RemoveAt(0);
                }
            }
            //排队做菜处理
            if (!CheckUtil.ListIsNull(foodQueue))
            {
                bool isSuccess= innCookHandler.SetChefForCook(foodQueue[0]);
                if (isSuccess)
                {
                    foodQueue.RemoveAt(0);
                }
            }
            //排队送菜处理
            if (!CheckUtil.ListIsNull(sendQueue))
            {
                bool isSuccess = innWaiterHandler.SetSendFood(sendQueue[0]);
                if (isSuccess)
                {
                    sendQueue.RemoveAt(0);
                }
            }
            //排队清理处理
            if (!CheckUtil.ListIsNull(clearQueue))
            {
                bool isSuccess = innWaiterHandler.SetClearFood(clearQueue[0]);
                if (isSuccess)
                {
                    clearQueue.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// 点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(NpcAICustomerCpt customerCpt, BuildTableCpt table)
    {
        List<MenuOwnBean> listOwnMenu = gameDataManager.gameData.menuList;
        MenuOwnBean menuOwnItem = RandomUtil.GetRandomDataByList(listOwnMenu);
        if (menuOwnItem == null)
            return null;
        for (int i = 0; i < innFoodManager.listMenuData.Count; i++)
        {
            MenuInfoBean menuInfo = innFoodManager.listMenuData[i];
            if (menuInfo.menu_id == menuOwnItem.menuId)
            {
                MenuForCustomer menuForCustomer = new MenuForCustomer();
                menuForCustomer.food = menuInfo;
                menuForCustomer.customer = customerCpt;
                menuForCustomer.table = table;
                foodQueue.Add(menuForCustomer);
                return menuInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取柜台
    /// </summary>
    /// <returns></returns>
    public BuildCounterCpt GetCounter()
    {
        BuildCounterCpt counterCpt=RandomUtil.GetRandomDataByList(innPayHandler.listCounterCpt);
        return counterCpt;
    }

}