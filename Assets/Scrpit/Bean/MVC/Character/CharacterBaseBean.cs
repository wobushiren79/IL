using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterBaseBean 
{
    public string characterId;
    //名字
    public string name;
    //工作天数
    public long workDay;
    //出生
    //public TimeBean born;
    //每天工资    
    public long priceS;
    public long priceM;
    public long priceL;

    public bool isChef;//是否开启厨师
    public bool isWaiter;//是否开启服务生
    public bool isAccounting;//是否开启记帐
    public bool isBeater;//是否开启打手
    public bool isAccost;//是否开启招呼

    public bool isAttendance;//是否出勤

    /// <summary>
    /// 根据能力生成工资
    /// </summary>
    /// <param name="attributesBean"></param>
    public void CreatePriceByAttributes(CharacterAttributesBean attributesBean)
    {
        int totalAttribute = attributesBean.cook + attributesBean.speed + attributesBean.charm + attributesBean.force + attributesBean.lucky + attributesBean.account;
        priceS = 100;
        if (totalAttribute > 6)
        {
            priceS += (totalAttribute - 6)*50;
        }  
    }
}