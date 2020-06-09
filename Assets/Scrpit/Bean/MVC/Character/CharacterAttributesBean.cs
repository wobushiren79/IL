using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterAttributesBean
{
    public int loyal;//忠诚

    public int life = 0;//生命值
    public int cook = 0;// 厨力
    public int speed = 0;// 速度dw
    public int account = 0;// 计算
    public int charm = 0;// 魅力
    public int force = 0;// 武力
    public int lucky = 0;// 幸运

    //技能列表
    public List<long> listSkills = new List<long>();
    //学会的书籍
    public List<long> listLearnBook = new List<long>();


    /// <summary>
    /// 学习书籍
    /// </summary>
    /// <param name="bookId"></param>
    public void LearnBook(long bookId)
    {
        listLearnBook.Add(bookId);
    }

    /// <summary>
    /// 学习技能
    /// </summary>
    /// <param name="skillId"></param>
    public void LearnSkill(long skillId)
    {
        listSkills.Add(skillId);
    }
    /// <summary>
    /// 检测是否学习过该书籍
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public bool CheckLearnBook(long bookId)
    {
        return listLearnBook.Contains(bookId);
    }
    /// <summary>
    /// 检测是否学习过该技能
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public bool CheckLearnSkills(long skillId)
    {
        return listSkills.Contains(skillId);
    }

    /// <summary>
    /// 增加属性
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void AddAttributes(ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return;
        life += itemsInfo.add_life;
        loyal += itemsInfo.add_loyal;
        cook += itemsInfo.add_cook;
        speed += itemsInfo.add_speed;
        account += itemsInfo.add_account;
        charm += itemsInfo.add_charm;
        force += itemsInfo.add_force;
        lucky += itemsInfo.add_lucky;
    }
    /// <summary>
    /// 增加忠诚
    /// </summary>
    /// <param name="addLoyal"></param>
    public void AddLoyal(int addLoyal)
    {
        loyal += addLoyal;
        if (loyal > 100)
        {
            loyal = 100;
        }
        if (loyal < 0)
        {
            loyal = 0;
        }
    }



    /// <summary>
    /// 生成随机身体属性
    /// </summary>
    /// <param name="loyalMin"></param>
    /// <param name="loyalMax"></param>
    /// <param name="cookMin"></param>
    /// <param name="cookMax"></param>
    /// <param name="speedMin"></param>
    /// <param name="speedMax"></param>
    /// <param name="accountMin"></param>
    /// <param name="accountMax"></param>
    /// <param name="charmMin"></param>
    /// <param name="charmMax"></param>
    /// <param name="forceMin"></param>
    /// <param name="forceMax"></param>
    /// <param name="luckyMin"></param>
    /// <param name="luckyMax"></param>
    /// <returns></returns>
    public void CreateRandomData(
        int lifeMin, int lifeMax,
        int loyalMin, int loyalMax,
        int cookMin, int cookMax,
        int speedMin, int speedMax,
        int accountMin, int accountMax,
        int charmMin, int charmMax,
        int forceMin, int forceMax,
        int luckyMin, int luckyMax)
    {
        life = UnityEngine.Random.Range(lifeMin, lifeMax + 1);
        loyal = UnityEngine.Random.Range(loyalMin, loyalMax + 1);
        speed = UnityEngine.Random.Range(speedMin, speedMax + 1);
        account = UnityEngine.Random.Range(accountMin, accountMax + 1);
        charm = UnityEngine.Random.Range(charmMin, charmMax + 1);
        force = UnityEngine.Random.Range(forceMin, forceMax + 1);
        lucky = UnityEngine.Random.Range(luckyMin, luckyMax + 1);
        cook = UnityEngine.Random.Range(cookMin, cookMax + 1);
    }

    /// <summary>
    /// 根据能力生成工资
    /// </summary>
    /// <param name="attributesBean"></param>
    public void CreatePriceByAttributes(out long priceL, out long priceM, out long priceS)
    {
        priceL = 0;
        priceM = 0;
        priceS = 50;
        int totalAttribute = cook + speed + charm + force + lucky + account;
        if (totalAttribute > 6)
        {
            priceS += (totalAttribute - 6) * 5;
        }
    }
}