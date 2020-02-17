using UnityEngine;
using UnityEditor;
using System;

public class NpcAISundry : BaseNpcAI, IBaseObserver
{
    public enum SundryIntentEnum
    {
        Idle = 0,
        GoToInn = 1,//前往客栈
        WaitingForReply = 2,//等待回复
        Leave = 10,//离开
    }

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

    protected InnHandler innHandler;
    protected EventHandler eventHandler;
    protected SceneInnManager sceneInnManager;

    public override void Awake()
    {
        base.Awake();
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
    }

    private void Update()
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

    private void OnDestroy()
    {
        eventHandler.RemoveObserver(this);
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
            if (innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
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
            bool isTrigger = eventHandler.EventTriggerForTalkBySundry(this, talk_ids);
            if (isTrigger)
            {
                eventHandler.AddObserver(this);
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
        movePosition = innHandler.GetRandomEntrancePosition();
        SetCharacterMove(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    protected void SetIntentForLeave()
    {
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        SetCharacterMove(movePosition);
    }

    #region 通知
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : UnityEngine.Object
    {
        if (observable == eventHandler)
        {
            if (type == (int)EventHandler.NotifyEventTypeEnum.TalkForAddFavorability)
            {
                addFavorability += (int)obj[1];
            }
            else if (type == (int)EventHandler.NotifyEventTypeEnum.EventEnd)
            {
                //根据对话的好感加成 不同的反应
                if (addFavorability > 0)
                {
                }
                else if (addFavorability < 0)
                {
                }
                SetIntent(SundryIntentEnum.Leave);
            }
        }
    }
    #endregion
}