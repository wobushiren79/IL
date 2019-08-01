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
    public InnStatusEnum innStatus = InnStatusEnum.Close;

    //数据管理
    public GameDataManager gameDataManager;
    public InnFoodManager innFoodManager;
    //NPC创建
    public NpcWorkerBuilder workerBuilder;

    //客栈桌子处理
    public InnTableHandler innTableHandler;
    //烹饪处理
    public InnCookHandler innCookHandler;
    //服务处理
    public InnWaiterHandler innWaiterHandler;
    // 支付处理
    public InnPayHandler innPayHandler;
    // 入口处理
    public InnEntranceHandler innEntranceHandler;

    //排队的人
    public List<NpcAICustomerCpt> cusomerQueue = new List<NpcAICustomerCpt>();
    //排队等待烹饪的食物
    public List<MenuForCustomer> foodQueue = new List<MenuForCustomer>();
    //排队送餐的食物
    public List<FoodForCustomerCpt> sendQueue = new List<FoodForCustomerCpt>();
    //排队清理的食物
    public List<FoodForCustomerCpt> clearQueue = new List<FoodForCustomerCpt>();
    //顾客列表
    public List<NpcAICustomerCpt> cusomerList = new List<NpcAICustomerCpt>();
    //当天记录流水
    public InnRecordBean innRecord = new InnRecordBean();

    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        innEntranceHandler.InitDoorList();
        innTableHandler.InitTableList();
        innCookHandler.InitStoveList();
        innPayHandler.InitCounterList();
        InitWorker();
    }

    /// <summary>
    /// 初始化员工
    /// </summary>
    public void InitWorker()
    {
        workerBuilder.RefreshWorkStatus();
        innPayHandler.InitAccountingCpt();
        innCookHandler.InitChefCpt();
        innWaiterHandler.InitWaiterCpt();
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
                    NpcAICustomerCpt npc = cusomerQueue[0];
                    cusomerQueue.RemoveAt(0);
                    npc.SetTable(tableCpt);
                }
            }
            //排队支付处理
            if (!CheckUtil.ListIsNull(innPayHandler.listCounterCpt))
            {
                for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
                {
                    BuildCounterCpt counterCpt = innPayHandler.listCounterCpt[i];
                    if (!CheckUtil.ListIsNull(counterCpt.payQueue))
                    {
                        bool isSuccess = innPayHandler.SetPay(counterCpt.payQueue[0]);
                        if (isSuccess)
                        {
                            counterCpt.payQueue.RemoveAt(0);
                        }
                    }
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
            //排队做菜处理
            if (!CheckUtil.ListIsNull(foodQueue))
            {
                bool isSuccess = innCookHandler.SetChefForCook(foodQueue[0]);
                if (isSuccess)
                {
                    foodQueue.RemoveAt(0);
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

    public void CloseInn()
    {
        innStatus = InnStatusEnum.Close;
        //驱除所有顾客
        for (int i = 0; i < cusomerList.Count; i++)
        {
            NpcAICustomerCpt npcAI = cusomerList[i];
            if (npcAI.tableForEating != null)
                npcAI.tableForEating.tableState = BuildTableCpt.TableStateEnum.Idle;
            if (npcAI.foodCpt != null)
                Destroy(npcAI.foodCpt.gameObject);
            Destroy(npcAI.gameObject);
        }
        for (int i = 0; i < sendQueue.Count; i++)
        {
            FoodForCustomerCpt food = sendQueue[i];
            Destroy(food.gameObject);
        }
        for (int i = 0; i < clearQueue.Count; i++)
        {
            FoodForCustomerCpt food = clearQueue[i];
            Destroy(food.gameObject);
        }
        for (int i = 0; i < innTableHandler.listTableCpt.Count; i++)
        {
            BuildTableCpt buildTableCpt = innTableHandler.listTableCpt[i];
            buildTableCpt.ClearTable();
        };

        cusomerQueue.Clear();
        foodQueue.Clear();
        sendQueue.Clear();
        clearQueue.Clear();
        cusomerList.Clear();
        workerBuilder.ClearAllWork();
    }

    public void OpenInn()
    {
        workerBuilder.BuildAllWorker();
        InitInn();
        innStatus = InnStatusEnum.Open;
    }

    /// <summary>
    /// 点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(NpcAICustomerCpt customerCpt, BuildTableCpt table)
    {
        List<MenuOwnBean> listOwnMenu = gameDataManager.gameData.GetSellMenuList();
        if (listOwnMenu.Count == 0)
            return null;
        MenuOwnBean menuOwnItem = RandomUtil.GetRandomDataByList(listOwnMenu);
        if (menuOwnItem == null)
            return null;
        if (innFoodManager.listMenuData.TryGetValue(menuOwnItem.menuId, out MenuInfoBean menuInfo))
        {
            MenuForCustomer menuForCustomer = new MenuForCustomer();
            menuForCustomer.food = menuInfo;
            menuForCustomer.customer = customerCpt;
            menuForCustomer.table = table;
            foodQueue.Add(menuForCustomer);
            return menuInfo;
        }

        return null;
    }

    /// <summary>
    /// 获取柜台
    /// </summary>
    /// <returns></returns>
    public BuildCounterCpt GetCounter()
    {
        BuildCounterCpt counterCpt = RandomUtil.GetRandomDataByList(innPayHandler.listCounterCpt);
        return counterCpt;
    }

    /// <summary>
    /// 取消食物 用于顾客不满意要离开
    /// </summary>
    /// <param name="customerCpt"></param>
    public void CanelOrder(NpcAICustomerCpt customerCpt)
    {

    }

    /// <summary>
    /// 获取所有入口
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetEntrancePositionList()
    {
        return innEntranceHandler.GetEntrancePositionList();
    }

    /// <summary>
    /// 付钱
    /// </summary>
    /// <param name="food"></param>
    public void PayMoney(FoodForCustomerCpt foodCpt, float multiple)
    {
        //账本记录
        if (innRecord.sellNumber.ContainsKey(foodCpt.foodData.food.id))
            innRecord.sellNumber[foodCpt.foodData.food.id] += 1;
        else
            innRecord.sellNumber.Add(foodCpt.foodData.food.id, 1);
        innRecord.incomeS += foodCpt.foodData.food.price_s;
        innRecord.incomeM += foodCpt.foodData.food.price_m;
        innRecord.incomeL += foodCpt.foodData.food.price_l;
        //记录+1
        gameDataManager.gameData.ChangeMenuSellNumber(1, foodCpt.foodData.food.id);
        //金钱增加
        gameDataManager.gameData.moneyS += (long)(foodCpt.foodData.food.price_s * multiple);
        gameDataManager.gameData.moneyM += (long)(foodCpt.foodData.food.price_m * multiple);
        gameDataManager.gameData.moneyL += (long)(foodCpt.foodData.food.price_l * multiple);
        innPayHandler.ShowPayEffects
            (foodCpt.foodData.customer.transform.position,
            foodCpt.foodData.food.price_l,
            foodCpt.foodData.food.price_m,
            foodCpt.foodData.food.price_s);
    }

    /// <summary>
    /// 材料消耗记录
    /// </summary>
    public void ConsumeIngRecord(MenuInfoBean foodData)
    {
        innRecord.consumeIngOilsalt += foodData.ing_oilsalt;
        innRecord.consumeIngMeat += foodData.ing_meat;
        innRecord.consumeIngRiverfresh += foodData.ing_riverfresh;
        innRecord.consumeIngSeafood += foodData.ing_seafood;
        innRecord.consumeIngVegetablest += foodData.ing_vegetables;
        innRecord.consumeIngMelonfruit += foodData.ing_melonfruit;
        innRecord.consumeIngWaterwine += foodData.ing_waterwine;
        innRecord.consumeIngFlour += foodData.ing_flour;
    }

}