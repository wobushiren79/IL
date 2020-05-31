using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcCustomerBuilder : NpcNormalBuilder, IBaseObserver
{
    //团队顾客
    public GameObject objGuestTeamModel;

    protected List<NpcTeamBean> listTeamCustomer=new List<NpcTeamBean>();
    protected float buildTeamGustomerRate = 0;

    private void Start()
    {
        InnAttributesBean innAttributes = gameDataManager.gameData.GetInnAttributesData();
        buildTeamGustomerRate =  innAttributes.CalculationTeamCustomerBuildRate();
        buildMaxNumber = 500;
        gameTimeHandler.AddObserver(this);
        StartBuildCustomer();
    }

    /// <summary>
    /// 初始化生成NPC
    /// </summary>
    /// <param name="npcNumber"></param>
    public void BuilderCustomerForInit(int npcNumber)
    {
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
        StopAllCoroutines();
        isBuildNpc = true;
        StartCoroutine(StartBuild());
    }

    /// <summary>
    /// 停止建造NPC
    /// </summary>
    public void StopBuildCustomer()
    {
        isBuildNpc = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// 开始创建NPC
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartBuild()
    {
        while (isBuildNpc)
        {
            yield return new WaitForSeconds(buildInterval);
           // BuildCustomer();
            //有一定概率创建团队
            float buildTeamRate = Random.Range(0, 1f);
            if (buildTeamRate <= buildTeamGustomerRate)
            {
                BuildGuestTeam();
            }
        }
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
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(npcPosition);
        //设置意图
        NpcAICustomerCpt customerAI = npcObj.GetComponent<NpcAICustomerCpt>();
        //想要吃饭概率
        if (IsWantEat())
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
        NpcTeamBean teamData = npcTeamManager.GetCustomerTeam(teamId);
        BuildGuestTeam(teamData, npcPosition);
    }

    public void BuildGuestTeam(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (npcTeam == null)
            return;
        //设置小队相关
        string teamCode = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Color teamColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //设置是否想吃
        bool isWant = IsWantEat();
        //获取小队成员数据
        npcTeam.GetTeamCharacterData(npcInfoManager, out List<CharacterBean> listLeader, out List<CharacterBean> listMembers);
        //设置小队人数(团队领袖全生成，小队成员随机生成)
        int npcNumber = Random.Range(listLeader.Count + 1, listLeader.Count + 1 + npcTeam.team_number);
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
            //随机生成身体数据
            characterData.body.CreateRandomEye(characterBodyManager);

            GameObject npcObj = Instantiate(objContainer, objGuestTeamModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);
            NpcAICustomerForGuestTeamCpt customerAI = baseNpcAI.GetComponent<NpcAICustomerForGuestTeamCpt>();
            customerAI.SetTeamData(teamCode, npcTeam, i, teamColor);
            if (isWant)
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
            }
            else
            {
                customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
            }
        }
    }

    /// <summary>
    /// 通过团队Code获取团队成员
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public List<NpcAICustomerForGuestTeamCpt> GetGuestTeamByTeamCode(string teamCode)
    {
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = new List<NpcAICustomerForGuestTeamCpt>();
        if (CheckUtil.StringIsNull(teamCode))
            return listTeamMember;
        NpcAICustomerForGuestTeamCpt[] arrayTeam = objContainer.GetComponentsInChildren<NpcAICustomerForGuestTeamCpt>();
        foreach (NpcAICustomerForGuestTeamCpt itemData in arrayTeam)
        {
            if (itemData.teamCode.EndsWith(teamCode))
            {
                listTeamMember.Add(itemData);
            }
        }
        return listTeamMember;
    }

    /// <summary>
    ///  计算是否想要吃饭
    /// </summary>
    /// <returns></returns>
    private bool IsWantEat()
    {
        //想要吃饭概率
        float eatProbability = UnityEngine.Random.Range(0f, 1f);
        float rateWant = gameDataManager.gameData.GetInnAttributesData().CalculationCustomerWantRate();
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
    private float HandleNpcBuildTime(int hour)
    {
        if (hour > 6 && hour <= 9)
        {
            buildInterval = 4;
        }
        else if (hour > 9 && hour <= 12)
        {
            buildInterval = 3;
        }
        else if (hour > 12 && hour <= 18)
        {
            buildInterval = 2;
        }
        else if (hour > 18 && hour <= 21)
        {
            buildInterval = 3;
        }
        else if (hour > 21 && hour <= 24)
        {
            buildInterval = 5;
        }
        else
        {
            buildInterval = 5;
        }
        //天气加成
        if (weatherHandler != null)
        {
            buildInterval -= weatherHandler.weatherData.weatherAddition;
        }
        return buildInterval;
    }

    /// <summary>
    /// 时间回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="type"></param>
    /// <param name="obj"></param>
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            //重新获取顾客信息
            listTeamCustomer = npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Customer, gameDataManager.gameData);
            //开始建造顾客
            StartBuildCustomer();
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            StopBuildCustomer();
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = System.Convert.ToInt32(obj[0]);
            HandleNpcBuildTime(hour);
        }
    }
}