using UnityEngine;
using UnityEditor;

public class NpcAICustomerForGuestTeamCpt : NpcAICustomerCpt
{
    public MenuOwnBean menuOwn;

    public void SetMenu(MenuOwnBean menuOwn)
    {
        this.menuOwn = menuOwn;
    }

    /// <summary>
    /// 点餐
    /// </summary>
    public override void OrderForFood()
    {
        //首先调整修改朝向
        SetCharacterFace(orderForCustomer.table.GetUserFace());
        //点餐
        innHandler.OrderForFood(orderForCustomer, menuOwn);
        if (orderForCustomer.foodData != null)
        {
            //喊出需要的菜品
            characterShoutCpt.Shout(orderForCustomer.foodData.name);
        }
        //判断是否出售
        if (!menuOwn.isSell)
        {
            //如果菜品没有出售 心情直接降100 
            ChangeMood(-100);
            //离开
            SetIntent(CustomerIntentEnum.Leave);
            return;
        }
        //如果没有这菜
        if (orderForCustomer.foodData == null)
        {
            //如果没有菜品出售 心情直接降100 
            ChangeMood(-100);
            //离开
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            //设置等待食物
            SetIntent(CustomerIntentEnum.WaitFood);
        }
    }
}