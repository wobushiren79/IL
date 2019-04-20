using UnityEngine;
using UnityEditor;
using System;

public class NpcAIWorkerForWaiterCpt : BaseMonoBehaviour
{

    private NpcAIWorkerCpt mNpcAIWorker;

    public FoodForCustomerCpt foodCpt;
    //送菜的进度图标
    public GameObject sendPro;

    public enum WaiterStatue
    {
        Idle,//空闲
        GoToGetFood,//获取食物的路上
        SendFood,//运送食物中
    }

    //服务员状态
    public WaiterStatue waiterStatue= WaiterStatue.Idle;

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
                if (Vector2.Distance(transform.position, foodCpt.foodData.stove.GetTakeFoodPosition()) < 1f)
                {
                    Transform waitTake = CptUtil.GetCptInChildrenByName<Transform>(gameObject, "Take");
                    foodCpt.transform.SetParent(waitTake);
                    foodCpt.transform.localScale = new Vector3(0.3f,0.3f,1);
                    foodCpt.transform.localPosition = new Vector3(0,0.1f,0);
                    mTableTF = CptUtil.GetCptInChildrenByName<Transform>(foodCpt.foodData.table.gameObject,"Table");   
                    mNpcAIWorker.characterMoveCpt.SetDestination(mTableTF.position);
                    waiterStatue = WaiterStatue.SendFood;
                    sendPro.SetActive(true);
                }
                break;
            case WaiterStatue.SendFood:
                if (Vector2.Distance(transform.position, mTableTF.position) < 1f)
                {
                    foodCpt.transform.SetParent(mTableTF);
                    foodCpt.transform.localPosition = new Vector3(0f,0.1f,0);
                    foodCpt.transform.localScale = new Vector3(1,1,1);
                    waiterStatue = WaiterStatue.Idle;
                    mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
                    sendPro.SetActive(false);
                }
                break;
        }
    }

    public void SetFoodSend(FoodForCustomerCpt foodCpt)
    {
        this.foodCpt = foodCpt;
        mNpcAIWorker.characterMoveCpt.SetDestination(foodCpt.foodData.stove.GetTakeFoodPosition());
        waiterStatue = WaiterStatue.GoToGetFood;
        sendPro.SetActive(true);
    }
}