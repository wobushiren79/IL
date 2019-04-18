using UnityEngine;
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
    //排队的人
    public List<NpcAICustomerCpt> cusomerQueue = new List<NpcAICustomerCpt>();
    //排队等待烹饪的食物
    public List<MenuInfoBean> foodQueue = new List<MenuInfoBean>();

    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        innTableHandler.InitTableList();
        innCookHandler.InitStoveList();
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
        }
    }

    /// <summary>
    /// 加入排队
    /// </summary>
    public void AddWaitQueue(NpcAICustomerCpt customerCpt)
    {
        cusomerQueue.Add(customerCpt);
    }

    /// <summary>
    /// 移除排队
    /// </summary>
    /// <param name="customerCpt"></param>
    public void RemoveWaitQueue(NpcAICustomerCpt customerCpt)
    {
        cusomerQueue.Remove(customerCpt);
    }

    /// <summary>
    /// 加入食物烹饪列队
    /// </summary>
    /// <param name="menuInfo"></param>
    public void AddFoodQueue(MenuInfoBean menuInfo)
    {
        foodQueue.Add(menuInfo);
    }

    /// <summary>
    /// 移除食物烹饪列队
    /// </summary>
    /// <param name="menuInfo"></param>
    public void RemoveFoodQueue(MenuInfoBean menuInfo)
    {
        foodQueue.Remove(menuInfo);
    }

    /// <summary>
    /// 点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood()
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
                AddFoodQueue(menuInfo);
                return menuInfo;
            }
        }
        return null;
    }
}