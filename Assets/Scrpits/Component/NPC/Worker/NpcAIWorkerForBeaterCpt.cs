﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class NpcAIWorkerForBeaterCpt : NpcAIWokerFoBaseCpt
{
    public enum BeaterIntentEnum
    {
        Idle,//空闲
        GoToRascal,//前往闹事者
        Fighting,//开始战斗
        Rest,//休息
    }

    public BeaterIntentEnum beaterIntent = BeaterIntentEnum.Idle;
    //工作进度
    public GameObject beaterPro;

    public Vector3 movePosition;
    public NpcAIRascalCpt npcAIRascal;//闹事者
  
    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        switch (beaterIntent)
        {
            case BeaterIntentEnum.GoToRascal:
                //如果闹事者已经准备离开
                if (npcAIRascal.rascalIntent == NpcAIRascalCpt.RascalIntentEnum.Leave)
                {
                    SetIntent(BeaterIntentEnum.Idle);
                    return;
                }
                //计算是否到达
                if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(BeaterIntentEnum.Fighting);
                    npcAIRascal.SetIntent(NpcAIRascalCpt.RascalIntentEnum.Fighting);
                }
                break;
        }
    }

    public void StartFight(NpcAIRascalCpt npcAIRascal)
    {
        //捣乱者不动
        this.npcAIRascal = npcAIRascal;
        SetIntent(BeaterIntentEnum.GoToRascal);
    }

    public void SetIntent(BeaterIntentEnum intent)
    {
        StopAllCoroutines();
        this.beaterIntent = intent;
        switch (intent)
        {
            case BeaterIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case BeaterIntentEnum.GoToRascal:
                SetIntentForGoToRascal();
                break;
            case BeaterIntentEnum.Fighting:
                SetIntentForFighting();
                break;
            case BeaterIntentEnum.Rest:
                SetIntentForRest();
                break;
        }
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        beaterPro.SetActive(false);
        npcAIRascal = null;
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 意图-前往闹事者
    /// </summary>
    public void SetIntentForGoToRascal()
    {
        beaterPro.SetActive(true);
        npcAIRascal.SetMoveStatus(false);
        movePosition = npcAIRascal.transform.position;
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-打斗
    /// </summary>
    public void SetIntentForFighting()
    {
        beaterPro.SetActive(false);
        StartCoroutine(StartFighting());
    }

    /// <summary>
    /// 意图-休息
    /// </summary>
    public void SetIntentForRest()
    {
        float restTime = npcAIWorker.characterData.CalculationBeaterRestTime();
        npcAIRascal = null;
        //设置心情
        npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Dead, restTime);
        //设置角色死亡
        npcAIWorker.SetCharacterDead();
        //弹窗提示
        string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1015), npcAIWorker.characterData.baseInfo.name, restTime + "");
        UIHandler.Instance.ToastHint<ToastView>(toastStr, 5);
        //设置不工作
        StartCoroutine(StartRest(restTime));
    }


    /// <summary>
    /// 开始休息
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRest(float restTime)
    {
        yield return new WaitForSeconds(restTime);
        npcAIWorker.SetCharacterData(npcAIWorker.characterData);
        npcAIWorker.SetCharacterLive();
        SetIntent(BeaterIntentEnum.Idle);      
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFighting()
    {
        float fightTime = npcAIWorker.characterData.CalculationBeaterFightTime();
        yield return new WaitForSeconds(fightTime);
        int damage = npcAIWorker.characterData.CalculationBeaterDamage();
        int life = npcAIRascal.AddLife(-damage);

        if (life <= 0)
        {
            //添加经验
            npcAIWorker.characterData.baseInfo.beaterInfo.AddExp(10, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Beater);
            }
            //记录
            npcAIWorker.characterData.baseInfo.beaterInfo.AddFightWinNumber(1);
            SetIntent(BeaterIntentEnum.Idle);
        }
        else
        {
            //添加经验
            npcAIWorker.characterData.baseInfo.beaterInfo.AddExp(5, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Beater);
            }
            //记录
            npcAIWorker.characterData.baseInfo.beaterInfo.AddFightLoseNumber(1);
            //设置继续闹事
            npcAIRascal.SetIntent(NpcAIRascalCpt.RascalIntentEnum.ContinueMakeTrouble);
            SetIntent(BeaterIntentEnum.Rest);
        }
    }
}