using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using static NpcAICustomerCpt;

public class NpcAIWorkerForWaiterCpt : BaseMonoBehaviour
{

    private NpcAIWorkerCpt mNpcAIWorker;

    public FoodForCustomerCpt foodCpt;
    //送菜的进度图标
    public GameObject sendPro;
    //清理进度
    public GameObject clearPro;

    public enum WaiterStatue
    {
        Idle,//空闲
        GoToGetFood,//获取食物的路上
        SendFood,//运送食物中
        GoToClear,//清理的路上
        Clear,//清理中
    }

    //服务员状态
    public WaiterStatue waiterStatue = WaiterStatue.Idle;

    private Transform mTableTF;
    private void Start()
    {
        mNpcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

    private void FixedUpdate()
    {
        switch (waiterStatue)
        {
            case WaiterStatue.GoToGetFood:

                if (!CheckCustomerLeave() && Vector2.Distance(transform.position, foodCpt.foodData.stove.GetTakeFoodPosition()) < 0.1f)
                {
                    Transform waitTake = CptUtil.GetCptInChildrenByName<Transform>(gameObject, "Take");
                    foodCpt.transform.SetParent(waitTake);
                    foodCpt.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    foodCpt.transform.localPosition = new Vector3(0, 0.1f, 0);
                    mTableTF = CptUtil.GetCptInChildrenByName<Transform>(foodCpt.foodData.table.gameObject, "Table");
                    mNpcAIWorker.characterMoveCpt.SetDestination(mTableTF.position);
                    waiterStatue = WaiterStatue.SendFood;
                    sendPro.SetActive(true);
                }
                break;
            case WaiterStatue.SendFood:
                if (!CheckCustomerLeave() && foodCpt.foodData.customer == null || foodCpt.foodData.customer.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
                {
                    waiterStatue = WaiterStatue.Idle;
                    mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
                    sendPro.SetActive(false);
                    Destroy(foodCpt.gameObject);
                    return;
                }
                if (Vector2.Distance(transform.position, mTableTF.position) < 1f)
                {
                    foodCpt.transform.SetParent(mTableTF);
                    foodCpt.transform.localPosition = new Vector3(0f, 0.1f, 0);
                    foodCpt.transform.localScale = new Vector3(1, 1, 1);
                    waiterStatue = WaiterStatue.Idle;
                    mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
                    sendPro.SetActive(false);
                    foodCpt.foodData.customer.SetDestinationByIntent(CustomerIntentEnum.Eatting, foodCpt);
                }
                break;
            case WaiterStatue.GoToClear:
                if (Vector2.Distance(transform.position, foodCpt.transform.position) < 1f)
                {
                    waiterStatue = WaiterStatue.Clear;
                    clearPro.SetActive(true);
                    this.foodCpt.foodData.table.tableState = BuildTableCpt.TableStateEnum.Cleaning;
                    StartCoroutine(StartClear());
                }
                break;
        }
    }

    /// <summary>
    /// 检测顾客是否离开
    /// </summary>
    /// <returns></returns>
    public bool CheckCustomerLeave()
    {
        if (foodCpt.foodData.customer == null || foodCpt.foodData.customer.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            if (foodCpt)
                Destroy(foodCpt.gameObject);
            SetStatusIdle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetFoodSend(FoodForCustomerCpt foodCpt)
    {
        if (CheckUtil.CheckPath(transform.position, foodCpt.foodData.stove.GetTakeFoodPosition()))
        {
            this.foodCpt = foodCpt;
            mNpcAIWorker.characterMoveCpt.SetDestination(foodCpt.foodData.stove.GetTakeFoodPosition());
            waiterStatue = WaiterStatue.GoToGetFood;
            sendPro.SetActive(true);
        }
        else
        {
            SetStatusIdle();
        }

    }

    public void SetFoodClear(FoodForCustomerCpt foodCpt)
    {
        this.foodCpt = foodCpt;
        mNpcAIWorker.characterMoveCpt.SetDestination(foodCpt.transform.position);
        waiterStatue = WaiterStatue.GoToClear;
        clearPro.SetActive(true);
    }

    public IEnumerator StartClear()
    {
        yield return new WaitForSeconds(5);
        SetStatusIdle();
        this.foodCpt.foodData.table.tableState = BuildTableCpt.TableStateEnum.Idle;
        Destroy(this.foodCpt.gameObject);
    }

    /// <summary>
    /// 设置当前员工无事可做
    /// </summary>
    public void SetStatusIdle()
    {
        waiterStatue = WaiterStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
        sendPro.SetActive(false);
        clearPro.SetActive(false);
    }
}