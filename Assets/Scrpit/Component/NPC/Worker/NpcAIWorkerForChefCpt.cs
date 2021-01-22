using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public class NpcAIWorkerForChefCpt : NpcAIWokerFoBaseCpt
{
    public enum ChefIntentEnum
    {
        Idle,//空闲
        GoToCook,//做菜之前的路上
        Cooking,//做菜中
    }

    //食物管理
    public InnFoodManager innFoodManager;
    //做菜的进度图标
    public GameObject cookPro;
    //顾客的订单
    public OrderForCustomer orderForCustomer;
    //厨师状态
    public ChefIntentEnum chefIntent = ChefIntentEnum.Idle;
    //移动的目标店
    public Vector3 movePosition;

    private void Update()
    {
        switch (chefIntent)
        {
            case ChefIntentEnum.Idle:
                break;
            case ChefIntentEnum.GoToCook:
                //先检测订单是否有效
                if (orderForCustomer.CheckOrder())
                {
                    //检测是否到达烹饪点
                    if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                    {
                        //设置朝向
                        npcAIWorker.SetCharacterFace(orderForCustomer.stove.GetUserFace());
                        //开始做菜
                        SetIntent(ChefIntentEnum.Cooking);
                    }
                }
                else
                {
                    //设置闲置
                    SetIntent(ChefIntentEnum.Idle);
                }
                break;
            case ChefIntentEnum.Cooking:
                if (!orderForCustomer.CheckOrder())
                {
                    //设置闲置
                    SetIntent(ChefIntentEnum.Idle);
                }
                break;
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="chefIntent"></param>
    /// <param name="orderForCustomer"></param>
    public void SetIntent(ChefIntentEnum chefIntent, OrderForCustomer orderForCustomer)
    {
        this.chefIntent = chefIntent;
        this.orderForCustomer = orderForCustomer;
        switch (chefIntent)
        {
            case ChefIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case ChefIntentEnum.GoToCook:
                SetIntentForGoToCook(orderForCustomer);
                break;
            case ChefIntentEnum.Cooking:
                SetIntentForCooking(orderForCustomer);
                break;
        }
    }
    public void SetIntent(ChefIntentEnum chefIntent)
    {
        SetIntent(chefIntent, orderForCustomer);
    }

    /// <summary>
    /// 设置做菜
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void StartCook(OrderForCustomer orderForCustomer)
    {
        if (orderForCustomer == null)
            return;
        SetIntent(ChefIntentEnum.GoToCook, orderForCustomer);
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        cookPro.SetActive(false);
        chefIntent = ChefIntentEnum.Idle;
        //设置灶台为空闲
        if (orderForCustomer != null && orderForCustomer.stove != null)
            orderForCustomer.stove.SetStoveStatus(BuildStoveCpt.StoveStatusEnum.Idle);
        orderForCustomer = null;
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 意图-前往做菜
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForGoToCook(OrderForCustomer orderForCustomer)
    {
        //如果订单是否有效
        if (!orderForCustomer.CheckOrder())
        {
            SetIntent(ChefIntentEnum.Idle);
            return;
        }
        movePosition = orderForCustomer.stove.GetCookPosition();
        if (movePosition == null)
        {
            LogUtil.Log("厨师寻路失败-没有灶台烹饪点");
            return;
        }
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
        cookPro.SetActive(true);
    }

    /// <summary>
    /// 意图-做菜中
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForCooking(OrderForCustomer orderForCustomer)
    {
        //检测是否能烹饪
        bool canCook = gameDataManager.gameData.CheckCookFood(orderForCustomer.foodData);
        if (canCook)
        {
            //扣除食材
            gameDataManager.gameData.DeductIng(orderForCustomer.foodData);
            //记录食材消耗
            InnHandler.Instance.ConsumeIngRecord(orderForCustomer.foodData);
            //设置灶台状态
            orderForCustomer.stove.SetStoveStatus(BuildStoveCpt.StoveStatusEnum.Cooking);
            StartCoroutine(StartCook());
        }
        else
        {
            npcAIWorker.characterShoutCpt.Shout(GameCommonInfo.GetUITextById(13001));
            orderForCustomer.customer.SendForCanNotCook();
            SetIntent(ChefIntentEnum.Idle);
        }
    }

    /// <summary>
    ///  开始做菜
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCook()
    {
        float foodTime = npcAIWorker.characterData.CalculationChefMakeFoodTime(orderForCustomer.foodData.cook_time);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Cook);
        yield return new WaitForSeconds(foodTime);
        //如果顾客已经没人
        //记录数据
        if (gameObject != null)
        {
            npcAIWorker.characterData.baseInfo.chefInfo.AddCookNumber(1, orderForCustomer.foodData.id);
            //添加经验
            npcAIWorker.characterData.baseInfo.chefInfo.AddExp(1, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Chef);
            }
            //计算食物生成等级
            // orderForCustomer.foodLevel = npcAIWorker.characterData.CalculationChefFoodLevel(gameItemsManager);
            orderForCustomer.foodLevel = 0;
            //在灶台创建一个食物
            orderForCustomer.stove.CreateFood(innFoodManager, orderForCustomer);
            //通知送餐
            InnHandler.Instance.sendQueue.Add(orderForCustomer);
            //设置状态为闲置
            SetIntent(ChefIntentEnum.Idle);
        }
    }


}