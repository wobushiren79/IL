using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public class NpcAIWorkerForChefCpt : BaseMonoBehaviour
{
    private NpcAIWorkerCpt mNpcAIWorker;
    //做菜的进度图标
    public GameObject cookPro;

    private void Start()
    {
        mNpcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }


    public enum ChefStatue
    {
        Idle,//空闲
        GoToCook,//做菜之前的路上
        Cooking,//做菜中
    }

    //灶台
    public BuildStoveCpt stoveCpt;
    //做的菜
    public MenuForCustomer foodData;
    //烹饪点
    public List<Vector3> cookPositionList;
    //厨师状态
    public ChefStatue chefStatue = ChefStatue.Idle;

    private Vector3 cookPosition;
    private float cookAnimTime;
    private void FixedUpdate()
    {
        if (CheckUtil.ListIsNull(cookPositionList))
            return;
        switch (chefStatue)
        {
            case ChefStatue.GoToCook:
                if (!CheckCustomerLeave() && Vector2.Distance(transform.position, cookPositionList[0]) < 0.1f)
                {
                    chefStatue = ChefStatue.Cooking;
                    StartCoroutine(StartCook());
                    ChangeCookPosition();
                    cookPro.SetActive(true);
                }
                break;
            case ChefStatue.Cooking:
                cookAnimTime -= Time.deltaTime;
                if (!CheckCustomerLeave() && Vector2.Distance(transform.position, cookPosition) < 0.1f && cookAnimTime < 0)
                {
                    ChangeCookPosition();
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
        if (foodData == null || foodData.customer == null || foodData.customer.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            StopAllCoroutines();
            cookPro.SetActive(false);
            SetStatusIdle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeCookPosition()
    {
        cookAnimTime = Random.Range(2, 3);
        cookPosition = RandomUtil.GetRandomDataByList(cookPositionList);
        mNpcAIWorker.characterMoveCpt.SetDestination(cookPosition);
    }

    public void SetCookData(BuildStoveCpt stoveCpt, MenuForCustomer foodData)
    {
        this.stoveCpt = stoveCpt;
        this.foodData = foodData;
        //如果顾客已经离开
        if (foodData.customer == null || foodData.customer.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            SetStatusIdle();
            return;
        }

        cookPositionList = stoveCpt.GetCookPosition();
        if (CheckUtil.ListIsNull(cookPositionList))
        {
            LogUtil.Log("厨师寻路失败-没有灶台烹饪点");
            return;
        }
        mNpcAIWorker.characterMoveCpt.SetDestination(cookPositionList[0]);
        chefStatue = ChefStatue.GoToCook;
    }

    public IEnumerator StartCook()
    {
        yield return new WaitForSeconds(foodData.food.cook_time);
        cookPro.SetActive(false);
        stoveCpt.SetFood(foodData);
        stoveCpt.ClearChef();
        chefStatue = ChefStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
    }

    /// <summary>
    /// 设置空闲状态
    /// </summary>
    public void SetStatusIdle()
    {
        cookPro.SetActive(false);
        stoveCpt.ClearChef();
        chefStatue = ChefStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
    }
}