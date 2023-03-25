using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class UserCustomerBean
{
    //ID
    public string id;
    //接客数量
    public long number;
    //解锁的喜爱菜单
    public List<long> unlockLoveMenu=new List<long>();

    public void AddNumber(int number)
    {
        this.number += number;
    }

    public void AddMenu(long menuId)
    {
        if (!unlockLoveMenu.Contains(menuId))
        {
            unlockLoveMenu.Add(menuId);
        }
    }

    public bool CheckHasMenu(long menuId)
    {
        if (unlockLoveMenu.Contains(menuId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}