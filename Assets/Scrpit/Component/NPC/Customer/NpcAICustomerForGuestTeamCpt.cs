using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcAICustomerForGuestTeamCpt : NpcAICustomerCpt
{
    //团队代码
    public string teamCode;
    //团队数据
    public NpcTeamBean teamData;
    //团队地位
    public int teamRank;
    //团队颜色
    public Color teamColor;

    //集合点
    public Vector3 togetherPosition;

    protected NpcCustomerBuilder npcCustomerBuilder;

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
        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    /// <summary>
    /// 设置队伍ID
    /// </summary>
    /// <param name="teamId"></param>
    public void SetTeamData(string teamCode,NpcTeamBean teamData,int teamRank,Color teamColor)
    {
        this.teamCode = teamCode;
        this.teamData = teamData;
        this.teamRank = teamRank;
        this.teamColor = teamColor;
    }

    /// <summary>
    /// 获取所有小队成员
    /// </summary>
    /// <returns></returns>
    public List<NpcAICustomerForGuestTeamCpt> GetGuestTeam()
    {
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcCustomerBuilder.GetGuestTeamByTeamCode(teamCode);
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
        //如果没有自己专有喜欢的菜，没有则随机点一个在卖的
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
            ChangeMood(-9999);
            characterShoutCpt.Shout(GameCommonInfo.GetUITextById(13002));
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
        movePosition = innHandler.GetRandomEntrancePosition();
        //移动到门口附近
        if (movePosition == null || movePosition == Vector3.zero)
        {
            //如果找不到门则离开 散散步
            SetIntent(CustomerIntentEnum.Walk);
        }
        else
        {
            //前往门
            characterMoveCpt.SetDestination(movePosition);
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
                    teamMember.ChangeMood(-99999, false);
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
        //加入排队队伍
        //添加一个订单
        OrderForCustomer orderForCustomer = innHandler.CreateOrder(this);
        innHandler.cusomerQueue.Add(orderForCustomer);
        if (teamRank == 0)
        {
            StartCoroutine(StartWaitSeat());
        }
    }

    /// <summary>
    /// 开始等待就餐计时
    /// </summary>
    /// <returns></returns>
    public override IEnumerator StartWaitSeat()
    {
        yield return new WaitForSeconds(timeWaitSeat);
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
        bool allWait = true;
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember.customerIntent != CustomerIntentEnum.WaitSeat)
            {
                allWait = false;
            }
        }
        if (allWait)
        {
            foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
            {
                innHandler.EndOrderForForce(teamMember.orderForCustomer, false);
                teamMember.SetIntent(CustomerIntentEnum.Leave);
            }
        }
    }
}