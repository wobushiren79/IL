using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ArenaGameBarrageHandler : BaseHandler
{
    //弹幕游戏生成器
    public ArenaGameBarrageBuilder arenaGameBarrageBuilder;
    //弹幕游戏数据
    public ArenaPrepareBean arenaPrepareData;


    public Transform tfTarget;
    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="arenaPrepareData"></param>
    public void InitGame(ArenaPrepareBean arenaPrepareData)
    {
        if (arenaPrepareData == null)
        {
            LogUtil.Log("竞技场数据为NULL，无法初始化弹幕游戏");
            return;
        }
        this.arenaPrepareData = arenaPrepareData;
        //创建发射器
        BarrageEjectorCpt barrageEjector = arenaGameBarrageBuilder.CreateEjector();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (arenaPrepareData==null)
        {
            LogUtil.Log("竞技场数据为NULL，无法开始弹幕游戏");
            return;
        }
        StartCoroutine(StartShoot());
    }


    public IEnumerator StartShoot()
    {
        while (true)
        {
            List<BarrageEjectorCpt> listEjector= arenaGameBarrageBuilder.GetEjector();
            if (!CheckUtil.ListIsNull(listEjector))
            {
                foreach (BarrageEjectorCpt itemEjector in listEjector)
                {
                    itemEjector.StartLaunch(BarrageEjectorCpt.LaunchTypeEnum.Single, tfTarget.transform.position, 5);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}