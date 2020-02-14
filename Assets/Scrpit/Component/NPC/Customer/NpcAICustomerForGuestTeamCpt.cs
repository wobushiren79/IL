using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcAICustomerForGuestTeamCpt : NpcAICustomerCpt
{
    //团队ID
    public string teamId;
    //集合点
    public Vector3 togetherPosition;

    protected NpcEventBuilder npcEventBuilder;

    public enum CustomerIntentForGuestTeamEnum
    {
        Together,
        WaitTeam,
        GoToTeam,
    }

    public CustomerIntentForGuestTeamEnum guestTeamIntent = CustomerIntentForGuestTeamEnum.Together;

    public override void Awake()
    {
        base.Awake();
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    /// <summary>
    /// 设置队伍ID
    /// </summary>
    /// <param name="teamId"></param>
    public void SetTeamId(string teamId)
    {
        this.teamId = teamId;
    }

    /// <summary>
    /// 获取所有小队成员
    /// </summary>
    /// <returns></returns>
    public List<NpcAICustomerForGuestTeamCpt> GetGuestTeam()
    {
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
        if (listTeamMember.Count == 0)
            listTeamMember.Add(this);
        return listTeamMember;
    }

    /// <summary>
    /// 离开处理
    /// </summary>
    public override void HandleForLeave()
    {
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        switch (guestTeamIntent)
        {
            case CustomerIntentForGuestTeamEnum.GoToTeam:
                //一起离开处理
                guestTeamIntent = CustomerIntentForGuestTeamEnum.WaitTeam;
                List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
                bool allReady = true;
                foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
                {
                    if (teamMember.guestTeamIntent != CustomerIntentForGuestTeamEnum.WaitTeam)
                    {
                        allReady = false;
                    }
                }
                if (allReady)
                {
                    Vector3 leavePostion = sceneInnManager.GetRandomSceneExportPosition();
                    foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
                    {
                        teamMember.guestTeamIntent = CustomerIntentForGuestTeamEnum.Together;
                        teamMember.characterMoveCpt.SetDestination(leavePostion);
                    }
                }

                break;
            case CustomerIntentForGuestTeamEnum.Together:
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// 点餐
    /// </summary>
    public override void HandleForOrderFood()
    {
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        //首先调整修改朝向
        SetCharacterFace(orderForCustomer.table.GetUserFace());
        //点餐
        //判断是否有自己喜欢的菜
        List<long> loveMenus = characterData.npcInfoData.GetLoveMenus();
        //如果没有自己专有喜欢的菜，则随机点一个在卖的
        if (loveMenus.Count == 0)
        {
            innHandler.OrderForFood(orderForCustomer);
        }
        else
        {
            //检测是否拥有自己喜欢的菜品
            if (gameDataManager.gameData.CheckHasLoveMenus(loveMenus, out List<MenuOwnBean> ownLoveMenus))
            {
                //随机获取一个喜欢的菜
                MenuOwnBean loveMenu = RandomUtil.GetRandomDataByList(ownLoveMenus);
                innHandler.OrderForFood(orderForCustomer, loveMenu);
            }
            else
            {
                //如果没有自己喜欢的菜品则点一杯茶
                innHandler.OrderForFood(orderForCustomer, 1);
            }
        }
        if (orderForCustomer.foodData != null)
        {
            //喊出需要的菜品
            characterShoutCpt.Shout(orderForCustomer.foodData.name);
        }
        //如果没有这菜
        if (orderForCustomer.foodData == null)
        {
            //如果没有菜品出售 心情直接降100 
            ChangeMood(-100);
            //离开
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            //设置等待食物
            SetIntent(CustomerIntentEnum.WaitFood);
        }
    }

    /// <summary>
    /// 意图-想要就餐
    /// </summary>
    public override void IntentForWant()
    {
        base.IntentForWant();
        //通知其他团队成员
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this && teamMember.customerIntent != CustomerIntentEnum.Want)
            {
                teamMember.SetIntent(CustomerIntentEnum.Want);
                //统一入口
                teamMember.movePosition = movePosition;
                teamMember.characterMoveCpt.SetDestination(movePosition);
            }
        }
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public override void IntentForLeave()
    {
        //如果还没有生成订单 （路上被招待）
        if (orderForCustomer == null)
        {
            characterMoveCpt.SetDestination(movePosition);
            //通知其他团队成员
            List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
            foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
            {
                if (teamMember != this && teamMember.customerIntent != CustomerIntentEnum.Leave)
                {
                    teamMember.SetIntent(CustomerIntentEnum.Leave);
                }
            }
            return;
        }
        //如果有订单则强制结束订单
        innHandler.EndOrderForForce(orderForCustomer);
        //随机获取一个退出点
        togetherPosition = innHandler.GetRandomEntrancePosition();
        guestTeamIntent = CustomerIntentForGuestTeamEnum.GoToTeam;
        togetherPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        characterMoveCpt.SetDestination(togetherPosition);
    }

    /// <summary>
    /// 意图-等待招待过来
    /// </summary>
    public override void IntentForWaitAccost()
    {
        base.IntentForWaitAccost();
        //通知其他团队成员
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this && teamMember.customerIntent != CustomerIntentEnum.WaitAccost)
            {
                teamMember.SetIntent(CustomerIntentEnum.WaitAccost);
            }
        }
    }

    /// <summary>
    ///  改变心情
    /// </summary>
    /// <param name="mood"></param>
    /// <param name="isNotice"> 是否通知其他其他成员</param>
    public void ChangeMood(float mood, bool isNotice)
    {
        base.ChangeMood(mood);
        //通知其他团队成员
        if (orderForCustomer.innEvaluation.mood <= 0 && isNotice)
        {
            List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
            foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
            {
                if (teamMember != this)
                {
                    teamMember.ChangeMood(-9999, false);
                }
            }
        }
    }

    public override void ChangeMood(float mood)
    {
        ChangeMood(mood, true);
    }

    /// <summary>
    /// 意图-排队就餐
    /// </summary>
    public override void IntentForWaitSeat()
    {
        OrderForCustomer orderForCustomer = innHandler.CreateOrder(this);
        innHandler.cusomerQueue.Add(orderForCustomer);
    }
}