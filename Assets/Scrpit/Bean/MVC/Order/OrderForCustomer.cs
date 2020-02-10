using UnityEngine;
using UnityEditor;

public class OrderForCustomer 
{
    //需要的顾客
    public NpcAICustomerCpt customer;
    //需求的食物
    public MenuInfoBean foodData;
    //顾客所在的座位
    public BuildTableCpt table;
    //烹饪的灶台  
    public BuildStoveCpt stove;
    //支付的地方
    public BuildCounterCpt counter;
    //做好的食物 
    public FoodForCustomerCpt foodCpt;
    //做食物的厨师
    public NpcAIWorkerCpt chef;
    //送餐的人
    public NpcAIWorkerCpt waiterForSend;
    //做出的食物等级 -1 0 1 2
    public int foodLevel;
    //评价数据
    public InnEvaluationBean innEvaluation = new InnEvaluationBean();

    public OrderForCustomer(NpcAICustomerCpt customer)
    {
        this.customer = customer;
    }

    /// <summary>
    /// 检测订单是否有效
    /// </summary>
    /// <returns></returns>
    public bool CheckOrder()
    {
        if (foodData == null || customer == null || customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}