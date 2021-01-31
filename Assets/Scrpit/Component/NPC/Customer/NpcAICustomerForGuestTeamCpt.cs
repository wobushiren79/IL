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
        customerType = CustomerTypeEnum.Team;
    }

    /// <summary>
    /// 设置队伍ID
    /// </summary>
    /// <param name="teamId"></param>
    public void SetTeamData(string teamCode, NpcTeamBean teamData, int teamRank, Color teamColor)
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
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = NpcHandler.Instance.builderForCustomer.GetGuestTeamByTeamCode(teamCode);
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
        //判断是否有团队喜欢的菜
        List<long> loveMenus = new List<long>();
        if (teamData != null)
        {
            loveMenus = teamData.GetLoveMenus();
        }
        //如果没有团队专有喜欢的菜，没有则随机点
        if (loveMenus.Count == 0)
        {
            InnHandler.Instance.OrderForFood(orderForCustomer);
        }
        else
        {
            //检测是否拥有自己喜欢的菜品
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (gameData.CheckHasLoveMenus(loveMenus, out List<MenuOwnBean> ownLoveMenus))
            {
                //随机获取一个喜欢的菜
                MenuOwnBean loveMenu = RandomUtil.GetRandomDataByList(ownLoveMenus);
                InnHandler.Instance.OrderForFood(orderForCustomer, loveMenu);
            }
            else
            {
                //如果没有自己喜欢的菜品则点一杯茶
                //InnHandler.Instance.OrderForFood(orderForCustomer, 1);
            }
        }
        //如果有这菜
        if (orderForCustomer.foodData != null)
        {
            //喊出需要的菜品
            characterShoutCpt.Shout(orderForCustomer.foodData.name);
            //设置等待食物
            SetIntent(CustomerIntentEnum.WaitFood);
        }
        //如果没有这菜 甚至连茶都没有
        else
        {
            //如果没有菜品出售 心情直接降为不加好感
            SetMood(30,true);
            EndOrderAndLeave(true);

            characterShoutCpt.Shout(GameCommonInfo.GetUITextById(13002));
        }
    }

    /// <summary>
    /// 意图-想要就餐
    /// </summary>
    public override void IntentForWant()
    {
        Vector3 doorPosition = InnHandler.Instance.GetRandomEntrancePosition();
        //移动到门口附近
        if (doorPosition == null || doorPosition == Vector3.zero)
        {
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            movePosition = doorPosition;
            //前往门
            characterMoveCpt.SetDestination(movePosition);
            List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
            //通知其他团队成员
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
        togetherPosition = InnHandler.Instance.GetRandomEntrancePosition();
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
        if (orderForCustomer == null|| orderForCustomer.innEvaluation == null)
            return;
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

    public void SetMood(float mood, bool isNotice)
    {
        base.SetMood(mood);
        //通知其他团队成员
        if (!isNotice)
            return;
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this)
            {
                teamMember.SetMood(mood, false);
            }
        }
    }

    public void EndOrderAndLeave(bool isNotice)
    {
        base.EndOrderAndLeave();
        if (!isNotice)
            return;
        //通知其他团队成员
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = GetGuestTeam();
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this)
            {
                teamMember.EndOrderAndLeave(false);
            }
        }
    }

    /// <summary>
    /// 意图-排队就餐
    /// </summary>
    public override void IntentForWaitSeat()
    {
        //加入排队队伍
        //添加一个订单
        OrderForCustomer orderForCustomer = InnHandler.Instance.CreateOrder(this);
        InnHandler.Instance.cusomerQueue.Add(orderForCustomer);
        if (teamRank == 0)
        {
            StartCoroutine(CoroutineForStartWaitSeat());
        }
    }

    /// <summary>
    /// 开始等待就餐计时
    /// </summary>
    /// <returns></returns>
    public override IEnumerator CoroutineForStartWaitSeat()
    {
        AddWaitIcon();
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
                InnHandler.Instance.EndOrderForForce(teamMember.orderForCustomer, false);
                teamMember.SetIntent(CustomerIntentEnum.Leave);
            }
        }
    }
}