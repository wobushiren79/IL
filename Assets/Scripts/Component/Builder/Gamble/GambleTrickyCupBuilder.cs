using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GambleTrickyCupBuilder : BaseGambleBuilder
{
    public List<GambleTrickyCupItem> listCup;

    /// <summary>
    /// 初始化所有杯子
    /// 随机设置一个杯子有骰子
    /// 设置所有杯子状态
    /// </summary>
    public void InitAllCup()
    {
        if (listCup.IsNull())
        {
            return;
        }
        for (int i = 0; i < listCup.Count; i++)
        {
            GambleTrickyCupItem itemCup = listCup[i];
            itemCup.SetStatus(GambleTrickyCupItem.CupStatusEnum.Idle);
        }
    }

    /// <summary>
    /// 获取所有杯子
    /// </summary>
    /// <returns></returns>
    public List<GambleTrickyCupItem> GetAllCup()
    {
        if (listCup == null)
            listCup = new List<GambleTrickyCupItem>();
        return listCup;
    }

    /// <summary>
    /// 增加杯子
    /// </summary>
    /// <param name="itemCup"></param>
    public void AddCup(GambleTrickyCupItem itemCup)
    {
        if (listCup == null)
            listCup = new List<GambleTrickyCupItem>();
        listCup.Add(itemCup);
    }

    /// <summary>
    /// 清理所有杯子
    /// </summary>
    public void CleanAllCup()
    {
        if (listCup.IsNull())
            return;
        foreach (GambleTrickyCupItem itemCupt in listCup)
        {
            Destroy(itemCupt.gameObject);
        }
        listCup.Clear();
    }
}