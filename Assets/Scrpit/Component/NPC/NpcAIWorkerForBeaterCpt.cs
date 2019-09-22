using UnityEngine;
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
                else
                {
                    movePosition = npcAIRascal.transform.position;
                    npcAIWorker.characterMoveCpt.SetDestination(movePosition);
                }
                break;
        }
    }

    public void StartFight(NpcAIRascalCpt npcAIRascal)
    {
        this.npcAIRascal = npcAIRascal;
        SetIntent(BeaterIntentEnum.GoToRascal);
    }

    public void SetIntent(BeaterIntentEnum intent)
    {
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
    public void SetIntentForRest() {
        npcAIRascal = null;
        //设置心情
        npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Dead,60);
        //设置角色死亡
        npcAIWorker.SetCharacterDead();
        //弹窗提示
        string toastStr= string.Format(GameCommonInfo.GetUITextById(1015),npcAIWorker.characterData.baseInfo.name,60+"");
        EventHandler.Instance.EventTriggerForToast(toastStr,6);
        //设置不工作
        StartCoroutine(StartRest(60));
    }

    /// <summary>
    /// 开始休息
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRest(float restTime)
    {
        yield return new WaitForSeconds(restTime);
        npcAIWorker.SetCharacterData(npcAIWorker.characterData);
        SetIntent(BeaterIntentEnum.Idle);
    }
    
    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFighting()
    {
        yield return new WaitForSeconds(5);
        int life= npcAIRascal.AddLife(-5);
        if (life <= 0)
        {
            SetIntent(BeaterIntentEnum.Idle);
        }
        else
        {
            //设置继续闹事
            npcAIRascal.SetIntent(NpcAIRascalCpt.RascalIntentEnum.ContinueMakeTrouble);
            SetIntent(BeaterIntentEnum.Rest);
        }
    }
}