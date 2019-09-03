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
}