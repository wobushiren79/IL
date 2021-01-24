using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcAISundryCpt : BaseNpcAI
{
    public enum SundryIntentEnum
    {
        Idle = 0,
        GoToInn = 1,//前往客栈
        WaitingForReply = 2,//等待回复
        Leave = 10,//离开
    }

    public Vector3 leavePosition = Vector3.zero;
    public Vector3 movePosition = Vector3.zero;
    public SundryIntentEnum sundryIntent = SundryIntentEnum.Idle;
    //小队唯一代码
    public string teamCode;
    //小队数据
    public NpcTeamBean teamData;
    //小队中的位置
    public int teamRank;
    //增加的好感
    public int addFavorability = 0;

    protected SceneInnManager sceneInnManager;
    protected NpcEventBuilder npcEventBuilder;

    public override void Awake()
    {
        base.Awake();
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    public virtual void Update()
    {
        switch (sundryIntent)
        {
            case SundryIntentEnum.GoToInn:
                HandleGoToInn();
                break;
            case SundryIntentEnum.Leave:
                HandleLeave();
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEventHandler.Instance.UnRegisterNotifyForEvent(NotifyForEvent);
    }

    /// <summary>
    /// 设置小队数据
    /// </summary>
    /// <param name="teamCode"></param>
    /// <param name="npcTeam"></param>
    /// <param name="teamRank"></param>
    public void SetTeamData(string teamCode, NpcTeamBean teamData, int teamRank)
    {
        this.teamCode = teamCode;
        this.teamData = teamData;
        this.teamRank = teamRank;
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="sundryIntent"></param>
    public void SetIntent(SundryIntentEnum sundryIntent)
    {
        this.sundryIntent = sundryIntent;
        switch (sundryIntent)
        {
            case SundryIntentEnum.GoToInn:
                SetIntentForGoToInn();
                break;
            case SundryIntentEnum.WaitingForReply:
                SetIntentForWaitingForReply();
                break;
            case SundryIntentEnum.Leave:
                SetIntentForLeave();
                break;
        }
    }

    /// <summary>
    /// 处理-到达客栈
    /// </summary>
    private void HandleGoToInn()
    {
        if (CheckCharacterIsArrive())
        {
            if (InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                SetIntent(SundryIntentEnum.Leave);
            }
            else
            {
                SetIntent(SundryIntentEnum.WaitingForReply);
            }
        }
    }

    /// <summary>
    /// 处理-离开
    /// </summary>
    private void HandleLeave()
    {
        if (CheckCharacterIsArrive())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 意图-等待回复
    /// </summary>
    private void SetIntentForWaitingForReply()
    {
        if (teamRank == 0)
        {
            long talk_ids = RandomUtil.GetRandomDataByArray(teamData.GetTalkIds());
            if (talk_ids == 0)
            {
                SetIntent(SundryIntentEnum.Leave);
                return;
            }
            bool isTrigger = GameEventHandler.Instance.EventTriggerForTalkBySundry(this, talk_ids);

            if (isTrigger)
            {
                GameEventHandler.Instance.RegisterNotifyForEvent(NotifyForEvent);
            }
            else
            {
                //如果没有触发事件 则全体离开
                SetIntent(SundryIntentEnum.Leave);
            }
        }
    }

    /// <summary>
    /// 意图-移动到客栈
    /// </summary>
    private void SetIntentForGoToInn()
    {
        movePosition = InnHandler.Instance.GetRandomEntrancePosition();
        SetCharacterMove(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    protected void SetIntentForLeave()
    {
        if(leavePosition== Vector3.zero)
        {
            leavePosition = sceneInnManager.GetRandomSceneExportPosition();
        }
        movePosition = leavePosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        SetCharacterMove(movePosition);
    }

    /// <summary>
    /// 设置全体意图
    /// </summary>
    public void SetTeamIntent(SundryIntentEnum sundryIntent)
    {
        List<NpcAISundryCpt> listNpc = npcEventBuilder.GetSundryTeamByTeamCode(teamCode);
        if (sundryIntent == SundryIntentEnum.Leave)
        {
            leavePosition = sceneInnManager.GetRandomSceneExportPosition();
        }
        foreach (NpcAISundryCpt itemNpc in listNpc)
        {
            itemNpc.leavePosition = leavePosition;
            itemNpc.SetIntent(sundryIntent);
        }
    }

    /// <summary>
    /// 对话结束
    /// </summary>
    protected virtual void EventEnd()
    {
        //根据对话的好感加成 不同的反应
        if (addFavorability > 0)
        {

        }
        else if (addFavorability < 0)
        {

        }
        SetTeamIntent(SundryIntentEnum.Leave);
    }

    public void NotifyForEvent(GameEventHandler.NotifyEventTypeEnum notifyEventType,params object[] data)
    {
        if (notifyEventType == GameEventHandler.NotifyEventTypeEnum.TalkForAddFavorability)
        {
            addFavorability += (int)data[1];
        }
        else if (notifyEventType == GameEventHandler.NotifyEventTypeEnum.EventEnd)
        {
            EventEnd();
        }
    }
}