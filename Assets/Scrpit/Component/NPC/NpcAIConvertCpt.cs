using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NpcAIConvertCpt : NpcAISundryCpt,TextInfoHandler.ICallBack
{
    //检测范围展示
    public GameObject objConvertSpaceShow;
    public SpriteRenderer srConvertSpaceShow;

    public Sprite spEntertainSpaceShow;
    public Sprite spDisappointedSpaceShow;

    public ConvertIntentEnum convertIntent = ConvertIntentEnum.Idle;

    public GameObject objEntertainPS;

    protected EffectHandler effectHandler;
    //想要说的对话
    public List<TextInfoBean> listShoutTextInfo = new List<TextInfoBean>();
    //文本
    protected TextInfoHandler textInfoHandler;

    public float timeForConvert = 60;

    public override void Awake()
    {
        base.Awake();
        effectHandler = Find<EffectHandler>(ImportantTypeEnum.EffectHandler);
        textInfoHandler = Find<TextInfoHandler>(ImportantTypeEnum.TextManager);
    }

    public enum ConvertIntentEnum
    {
        Idle = 0,
        Entertain = 1,
        Disappointed = 2,
    }

    public override void Update()
    {
        base.Update();
        if (sundryIntent == SundryIntentEnum.WaitingForReply)
        {
            HandleForEntertain();
            HandleForDisappointed();
        }
    }

    public void HandleForEntertain()
    {
        if (convertIntent == ConvertIntentEnum.Entertain)
        {
            if (CheckCharacterIsArrive())
            {
                movePosition = innHandler.GetRandomInnPositon();
                if (CheckUtil.CheckPath(transform.position, movePosition))
                {
                    SetCharacterMove(movePosition);
                } 
            }
        }
    }

    public void HandleForDisappointed()
    {
        if (convertIntent == ConvertIntentEnum.Disappointed)
        {
            if (CheckCharacterIsArrive())
            {
                movePosition = innHandler.GetRandomInnPositon();
                if (CheckUtil.CheckPath(transform.position, movePosition))
                {
                    SetCharacterMove(movePosition);
                }
            }
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="convertIntent"></param>
    public void SetIntent(ConvertIntentEnum convertIntent)
    {
        if (!gameObject)
            return;
        objConvertSpaceShow.SetActive(false);
        this.convertIntent = convertIntent;
        switch (convertIntent)
        {
            case ConvertIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case ConvertIntentEnum.Entertain:
                SetIntentForEntertain();
                break;
            case ConvertIntentEnum.Disappointed:
                SetIntentForDisappointed();
                break;
        }
    }

    /// <summary>
    /// 意图闲置
    /// </summary>
    protected void SetIntentForIdle()
    {
        objEntertainPS.SetActive(false);
        objConvertSpaceShow.SetActive(false);
  
    }

    /// <summary>
    /// 意图 助兴
    /// </summary>
    protected void SetIntentForEntertain()
    {
        long[] shoutIds = teamData.GetShoutIds();
        textInfoHandler.SetCallBack(this);
        textInfoHandler.GetTextInfoFotTalkByMarkId(shoutIds[0]);

        objEntertainPS.SetActive(true);
        objConvertSpaceShow.SetActive(true);
        srConvertSpaceShow.sprite = spEntertainSpaceShow;

        //设置持续时间
        if (teamData.effect_time == 0)
        {
            timeForConvert = 60;
        }
        else
        {
            timeForConvert = teamData.effect_time;
        }
        StartCoroutine(CoroutineForEntertain(timeForConvert));

    }

    /// <summary>
    /// 意图 扫兴
    /// </summary>
    protected void SetIntentForDisappointed()
    {
        long[] shoutIds = teamData.GetShoutIds();
        textInfoHandler.SetCallBack(this);
        textInfoHandler.GetTextInfoFotTalkByMarkId(shoutIds[0]);

        objEntertainPS.SetActive(false);
        objConvertSpaceShow.SetActive(true);
        srConvertSpaceShow.sprite = spDisappointedSpaceShow;

        //设置持续时间
        if (teamData.effect_time == 0)
        {
            timeForConvert = 60;
        }
        else
        {
            timeForConvert = teamData.effect_time;
        }
        StartCoroutine(CoroutineForDisappointed(timeForConvert));
    }

    /// <summary>
    /// 对话结束
    /// </summary>
    protected override void EventEnd()
    {
        //根据对话的好感加成 不同的反应
        if (addFavorability > 0)
        {
            if (teamData.GetTeamType() == NpcTeamTypeEnum.Entertain)
            {
                SetTeamIntent(ConvertIntentEnum.Entertain);
            }
            else if (teamData.GetTeamType() == NpcTeamTypeEnum.Disappointed)
            {
                SetTeamIntent(ConvertIntentEnum.Disappointed);
            }
            else
            {
                SetTeamIntent(SundryIntentEnum.Leave);
            }
        }
        else if (addFavorability <= 0)
        {
            SetTeamIntent(SundryIntentEnum.Leave);
        }
    }

    /// <summary>
    /// 设置全体意图
    /// </summary>
    public void SetTeamIntent(ConvertIntentEnum convertIntent)
    {
        List<NpcAISundryCpt> listNpc = npcEventBuilder.GetSundryTeamByTeamCode(teamCode);
        foreach (NpcAISundryCpt itemNpc in listNpc)
        {
            NpcAIConvertCpt itemConvert = (NpcAIConvertCpt)itemNpc;
            itemConvert.SetIntent(convertIntent);
        }
    }

    /// <summary>
    /// 检测
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (objConvertSpaceShow.activeSelf)
        {
            NpcAICustomerCpt customerCpt = collision.GetComponent<NpcAICustomerCpt>();
            if (customerCpt)
            {
                if (customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Leave
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Walk
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Want
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.WaitAccost
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.TalkWithAccost)
                {
                    if (teamData.GetTeamType() == NpcTeamTypeEnum.Entertain)
                    {
                        effectHandler.PlayEffectPS(customerCpt.objEffectContainer, "Effect_Happy_1", customerCpt.transform.position + new Vector3(0, 0.5f));
                        customerCpt.ChangeMood(20f);
                    }
                    else if (teamData.GetTeamType() == NpcTeamTypeEnum.Disappointed)
                    {
                        effectHandler.PlayEffectPS(customerCpt.objEffectContainer, "Effect_Sulkiness_1", customerCpt.transform.position + new Vector3(0, 0.5f));
                        customerCpt.ChangeMood(-20f);
                    }
                    else
                    {

                    }
                }
            }
        }
    }

    /// <summary>
    /// 协程 助兴
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator CoroutineForEntertain(float time)
    {
        while (convertIntent == ConvertIntentEnum.Entertain)
        {
            if (!CheckUtil.ListIsNull(listShoutTextInfo))
            {
                TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listShoutTextInfo);
                characterShoutCpt.Shout(textInfo.content);
            }
            float intervalTime = Random.Range(3f,5f);
            yield return new WaitForSeconds(intervalTime);
            time -= intervalTime;
            if (time <= 0)
            {
                SetIntent(ConvertIntentEnum.Idle);
                SetIntent(SundryIntentEnum.Leave);
            }
        }
    }

    /// <summary>
    /// 协程 扫兴
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator CoroutineForDisappointed(float time)
    {
        while (convertIntent == ConvertIntentEnum.Disappointed)
        {
            if (!CheckUtil.ListIsNull(listShoutTextInfo))
            {
                TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listShoutTextInfo);
                characterShoutCpt.Shout(textInfo.content);
            }
            float intervalTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(intervalTime);
            time -= intervalTime;
            if (time <= 0)
            {
                SetIntent(ConvertIntentEnum.Idle);
                SetIntent(SundryIntentEnum.Leave);
            }
        }

    }

    #region 文本回调
    public void GetTextInfoSuccess(List<TextInfoBean> listData)
    {
        listShoutTextInfo = listData;
    }

    public void GetTextInfoFail()
    {

    }
    #endregion
}