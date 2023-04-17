using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System;

public class NpcCustomerBuilder : NpcNormalBuilder
{
    //团队顾客
    public GameObject objGuestTeamModel;
    //住宿顾客
    public GameObject objCustomerForHotelModel;

    protected List<NpcTeamBean> listTeamCustomer = new List<NpcTeamBean>();

    protected float buildTeamGustomerRate = 0;
    protected float buildCustomerForHotelRate = 0;


    public Queue<GameObject> listGuestTeamHide = new Queue<GameObject>();
    public Queue<GameObject> listCustomerForHotelHide = new Queue<GameObject>();

    //团队成员
    public Dictionary<string, List<NpcAICustomerForGuestTeamCpt>> dicGuestTeam = new Dictionary<string, List<NpcAICustomerForGuestTeamCpt>>();

    private void Start()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        InnBuildBean innBuild = gameData.GetInnBuildData();
        buildCustomerForHotelRate = innAttributes.CalculationCustomerForHotelRate(innBuild);
        buildTeamGustomerRate = innAttributes.CalculationTeamCustomerBuildRate();
        buildMaxNumber = 1000;
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
        StartBuildCustomer();
    }
    float timeUpdateBuild = 0;
    
    private void Update()
    {
        timeUpdateBuild += Time.deltaTime;
        if (timeUpdateBuild > buildInterval)
        {
            timeUpdateBuild = 0;
            if (!isBuildNpc)
                return;
            try
            {
                BuildCustomer();
                //有一定概率创建团队
                float buildTeamRate = UnityEngine.Random.Range(0, 1f);
                if (buildTeamRate < buildTeamGustomerRate)
                {
                    BuildGuestTeam();
                }
                //有一定概率创建住宿
                float buildCustomerHotelRateRandom = UnityEngine.Random.Range(0, 1f);
                if (buildCustomerHotelRateRandom < buildCustomerForHotelRate)
                {
                    BuildCustomerForHotel();
                }
            }
            catch (Exception e)
            {
                LogUtil.LogError(e.ToString());
            }
        }    
    }

    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }

    /// <summary>
    /// 初始化生成NPC
    /// </summary>
    /// <param name="npcNumber"></param>
    public void BuilderCustomerForInit(int npcNumber)
    {
        CptUtil.RemoveChildsByActive(objContainer);
        for (int i = 0; i < npcNumber; i++)
        {
            //获取随机坐标
            Vector3 npcPosition = GetRandomInitStartPosition();
            BuildCustomer(npcPosition);
        }
    }

    /// <summary>
    /// 开始建造NPC
    /// </summary>
    public void StartBuildCustomer()
    {
        isBuildNpc = true;
    }

    /// <summary>
    /// 停止建造NPC
    /// </summary>
    public void StopBuildCustomer()
    {
        LogUtil.Log("停止建造NPC");
        isBuildNpc = false;
    }


    /// <summary>
    /// 创建普通顾客
    /// </summary>
    public void BuildCustomer()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        BuildCustomer(npcPosition);
    }

    public void BuildCustomer(Vector3 npcPosition)
    {
        //生成NPC
        GameObject npcObj = BuildNpc(npcPosition);
        //设置意图
        NpcAICustomerCpt customerAI = npcObj.GetComponent<NpcAICustomerCpt>();
        //想要吃饭概率
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work && IsWantEat(CustomerTypeEnum.Normal))
        {
            customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
        }
        else
        {
            customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
        }
    }

    /// <summary>
    /// 创建团队顾客
    /// </summary>
    public void BuildGuestTeam()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = RandomUtil.GetRandomDataByList(listTeamCustomer);
        BuildGuestTeam(teamData, npcPosition);
    }

    public void BuildGuestTeam(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = NpcTeamHandler.Instance.manager.GetCustomerTeam(teamId);
        BuildGuestTeam(teamData, npcPosition);
    }

    public void BuildGuestTeam(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (npcTeam == null)
            return;
        //设置小队相关
        string teamCode = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Color teamColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        //设置是否想吃
        bool isWant = IsWantEat(CustomerTypeEnum.Team);
        //获取小队成员数据
        npcTeam.GetTeamCharacterData(out List<CharacterBean> listLeader, out List<CharacterBean> listMembers);
        //设置小队人数(团队领袖全生成，小队成员随机生成)
        int npcNumber = UnityEngine.Random.Range(listLeader.Count + 1, listLeader.Count + 1 + npcTeam.team_number);

        List<NpcAICustomerForGuestTeamCpt> listGuestTeam = new List<NpcAICustomerForGuestTeamCpt>();
        for (int i = 0; i < npcNumber; i++)
        {
            CharacterBean characterData = null;
            if (i < listLeader.Count)
            {
                characterData = listLeader[i];
            }
            else
            {
                characterData = RandomUtil.GetRandomDataByList(listMembers);
            }
            //如果发现成员数据为空 默认取领导的
            if (characterData == null)
            {
                characterData = listLeader[0];
            }
            //随机生成身体数据
            characterData.body.CreateRandomEye();

            GameObject npcObj = BuildNpc(listGuestTeamHide, objGuestTeamModel, characterData, npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);
            NpcAICustomerForGuestTeamCpt customerAI = baseNpcAI.GetComponent<NpcAICustomerForGuestTeamCpt>();
            customerAI.SetTeamData(teamCode, npcTeam, i, teamColor);
            if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work && isWant)
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
            }
            else
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
            }
            listGuestTeam.Add(customerAI);
        }
        dicGuestTeam.Add(teamCode, listGuestTeam);
    }

    /// <summary>
    /// 创建普通住宿顾客
    /// </summary>
    public void BuildCustomerForHotel()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        BuildCustomerForHotel(npcPosition);
    }
    public void BuildCustomerForHotel(Vector3 npcPosition)
    {
        //生成NPC
        GameObject npcObj = BuildNpc(listCustomerForHotelHide,objCustomerForHotelModel, npcPosition);
        //设置意图
        NpcAICustomerForHotelCpt customerAI = npcObj.GetComponent<NpcAICustomerForHotelCpt>();
        //想要吃饭概率
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work && InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Open)
        {
            customerAI.SetIntent(NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToInn);
        }
        else
        {
            customerAI.SetIntent(NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.Walk);
        }
    }

    /// <summary>
    /// 通过团队Code获取团队成员
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public List<NpcAICustomerForGuestTeamCpt> GetGuestTeamByTeamCode(string teamCode)
    {
        if (teamCode.IsNull())
            return new List<NpcAICustomerForGuestTeamCpt>();
        if (dicGuestTeam.TryGetValue(teamCode,out List<NpcAICustomerForGuestTeamCpt> data))
        {
            return data;
        }
        return new List<NpcAICustomerForGuestTeamCpt>();
    }

    /// <summary>
    ///  计算是否想要吃饭
    /// </summary>
    /// <returns></returns>
    private bool IsWantEat(CustomerTypeEnum customerType)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //想要吃饭概率
        float eatProbability = UnityEngine.Random.Range(0f, 1f);
        float rateWant  = gameData.GetInnAttributesData().CalculationCustomerWantRate(customerType);
        //设定是否吃饭
        if (eatProbability <= rateWant)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 处理NPC刷新时间
    /// </summary>
    /// <param name="hour"></param>
    /// <returns></returns>
    private void HandleNpcBuildTime(int hour)
    {
        if (hour > 6 && hour <= 9)
        {
            buildInterval = 3.5f;
        }
        else if (hour > 9 && hour <= 12)
        {
            buildInterval = 2.5f;
        }
        else if (hour > 12 && hour <= 18)
        {
            buildInterval = 1.5f;
        }
        else if (hour > 18 && hour <= 21)
        {
            buildInterval = 2.5f;
        }
        else if (hour > 21 && hour <= 24)
        {
            buildInterval = 3.5f;
        }
        else
        {
            buildInterval = 5;
        }
        //天气加成
        buildInterval -= GameWeatherHandler.Instance.manager.weatherData.weatherAddition;

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        gameData.GetInnAttributesData().GetInnLevel(out int levelTitle, out int levelStar);
        if (levelTitle == 1)
        {
            buildInterval = buildInterval * 0.9f;
        }
        else if (levelTitle == 2)
        {
            buildInterval = buildInterval * 0.65f;
        }
        else if (levelTitle == 3)
        {
            buildInterval = buildInterval * 0.4f;
        }
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        InnBuildBean innBuild = gameData.GetInnBuildData();

        buildCustomerForHotelRate = innAttributes.CalculationCustomerForHotelRate(innBuild);
        buildTeamGustomerRate = innAttributes.CalculationTeamCustomerBuildRate();
        float buildGustomerRate = innAttributes.CalculationCustomerBuildRate();
        buildInterval = buildInterval / buildGustomerRate;
    }

    /// <summary>
    /// 时间回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="type"></param>
    /// <param name="obj"></param>
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
            //重新获取顾客信息
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            listTeamCustomer = NpcTeamHandler.Instance.manager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Customer, gameData);
            //开始建造顾客
            StartBuildCustomer();
            HandleNpcBuildTime(6);
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            StopBuildCustomer();
            //ClearNpc();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            HandleNpcBuildTime((int)timeHour);
        }
    }

    public override void ClearNpc()
    {
        base.ClearNpc();
        dicGuestTeam.Clear();
    }

    /// <summary>
    /// 清除小队
    /// </summary>
    /// <param name="teamCode"></param>
    public void ClearGuestTeam(string teamCode)
    {
        if (NpcHandler.Instance.builderForCustomer.dicGuestTeam.ContainsKey(teamCode))
        {
            NpcHandler.Instance.builderForCustomer.dicGuestTeam.Remove(teamCode);
        }
    }
}