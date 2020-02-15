using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcEventBuilder : NpcNormalBuilder, IBaseObserver
{
    //捣乱者模型
    public GameObject objRascalModel;
    //团队模型
    public GameObject objGuestTeamModel;
    //好友模型
    public GameObject objFriendModel;
    //当天已经生成过的NPCID
    public List<long> listExistNpcId = new List<long>();

    private void Start()
    {
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 开始事件
    /// </summary>
    public void StartEvent()
    {
        //TODO 各种事件的完善
        //RascalEvent();
    }

    /// <summary>
    /// 团队事件
    /// </summary>
    public void TeamEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetRandomCustomerTeam(gameDataManager.gameData);
        BuildGuestTeam(teamData, npcPosition);
    }

    public void TeamEvent(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetCustomerTeam(teamId);
        BuildGuestTeam(teamData, npcPosition);
    }

    /// <summary>
    /// 好友事件
    /// </summary>
    public void FriendsEventForOne()
    {
        List<CharacterFavorabilityBean> listFavorabilityData = gameDataManager.gameData.listCharacterFavorability;
        //获取小镇角色
        List<CharacterFavorabilityBean> listTownFavorabilityData = new List<CharacterFavorabilityBean>();
        foreach (CharacterFavorabilityBean itemFavorability in listFavorabilityData)
        {
            CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemFavorability.characterId);
            if (characterData == null)
                continue;

            if (characterData.npcInfoData.npc_type == (int)NpcTypeEnum.Town
                //生成的NPC中不包含这个角色
                && !listExistNpcId.Contains(characterData.npcInfoData.id))
            {
                listTownFavorabilityData.Add(itemFavorability);
            }
        }
        //符合条件的NPC中随机取一人
        CharacterFavorabilityBean randomFavorabilityData = RandomUtil.GetRandomDataByList(listTownFavorabilityData);
        if (randomFavorabilityData == null)
            return;
        CharacterBean randomCharacterData = npcInfoManager.GetCharacterDataById(randomFavorabilityData.characterId);
        Vector3 npcPosition = GetRandomStartPosition();
        BuildTownFriends(randomCharacterData, randomFavorabilityData, npcPosition);
    }

    public void FriendsEventForTeam(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetFriendTeam(teamId);
        BuildTownFriendsForTeam(teamData, npcPosition);
    }

    public void FriendsEventForOne(long npcId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(npcId);
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(characterData.npcInfoData.id);
        BuildTownFriends(characterData, characterFavorability, npcPosition);
    }

    /// <summary>
    /// 恶棍事件
    /// </summary>
    public void RascalEvent()
    {

    }

    public void RascalEvent(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetRascalTeam(teamId);
        BuildRascal(teamData, npcPosition);
    }

    /// <summary>
    /// 创建捣乱事件
    /// </summary>
    /// <param name="npcTeam"></param>
    /// <param name="npcPosition"></param>
    public void BuildRascal(NpcTeamBean npcTeam, Vector3 npcPosition)
    {        
        //获取小队成员数据
        npcTeam.GetTeamCharacterData(npcInfoManager, out List<CharacterBean> listLeader, out List<CharacterBean> listMembers);
        //设置小队相关
        string teamCode = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        int npcNumber = listLeader.Count + listMembers.Count;
        for (int i = 0; i < npcNumber; i++)
        {
            CharacterBean characterData = null;
            if (i < listLeader.Count)
            {
                characterData = listLeader[i];
            }
            else
            {
                characterData = listMembers[i - listLeader.Count];
            }
            //生成NPC
            GameObject npcObj = BuildNpc(objRascalModel, characterData, npcPosition);
            //设置意图
            NpcAIRascalCpt rascalCpt = npcObj.GetComponent<NpcAIRascalCpt>();
            CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
            rascalCpt.SetTeamData(teamCode,npcTeam, i);
            rascalCpt.SetFavorabilityData(characterFavorability);
            rascalCpt.SetIntent(NpcAIRascalCpt.RascalIntentEnum.GoToInn);
        }
    }

    /// <summary>
    /// 创建团队顾客
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildGuestTeam(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (npcTeam == null)
            return;
        //设置小队相关
        string teamCode = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Color teamColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        //设置是否想吃
        bool isWant = IsWantEat();
        //获取小队成员数据
        npcTeam.GetTeamCharacterData(npcInfoManager, out List<CharacterBean> listLeader, out List<CharacterBean> listMembers);
        //设置小队人数(团队领袖全生成，小队成员随机生成)
        int npcNumber = UnityEngine.Random.Range(listLeader.Count + 1, listLeader.Count + 1 + npcTeam.team_number);
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
            // CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            GameObject npcObj = Instantiate(objContainer, objGuestTeamModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));

            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);

            NpcAICustomerForGuestTeamCpt customerAI = baseNpcAI.GetComponent<NpcAICustomerForGuestTeamCpt>();
            customerAI.SetTeamData(teamCode,npcTeam, i);
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
    /// 创建小镇好友
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildTownFriends(CharacterBean characterData, CharacterFavorabilityBean characterFavorability, Vector3 npcPosition)
    {
        //随机生成身体数据
        GameObject npcObj = Instantiate(objContainer, objFriendModel);

        npcObj.transform.localScale = new Vector3(1, 1);
        npcObj.transform.position = npcPosition;

        BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
        baseNpcAI.SetCharacterData(characterData);
        baseNpcAI.SetFavorabilityData(characterFavorability);
        baseNpcAI.AddStatusIconForFriend();

        NpcAICostomerForFriendCpt customerAI = baseNpcAI.GetComponent<NpcAICostomerForFriendCpt>();
        customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);

        listExistNpcId.Add(characterData.npcInfoData.id);
    }

    public void BuildTownFriendsForTeam(NpcTeamBean teamData, Vector3 npcPosition)
    {
        if (teamData == null)
            return;
        //设置小队相关
        string teamCode = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Color teamColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        //获取小队成员数据
        teamData.GetTeamCharacterData(npcInfoManager, out List<CharacterBean> listLeader, out List<CharacterBean> listMembers);
        //设置小队人数(团队领袖全生成，小队成员随机生成)
        int npcNumber = listLeader.Count + listMembers.Count;
        for (int i = 0; i < npcNumber; i++)
        {
            CharacterBean characterData = null;
            if (i < listLeader.Count)
            {
                characterData = listLeader[i];
            }
            else
            {
                characterData = listMembers[i - listLeader.Count];
            }

            //获取好感
            CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(characterData.npcInfoData.id);

            GameObject npcObj = Instantiate(objContainer, objGuestTeamModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));

            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForFriend();
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);
            baseNpcAI.SetFavorabilityData(characterFavorability);

            NpcAICustomerForGuestTeamCpt customerAI = baseNpcAI.GetComponent<NpcAICustomerForGuestTeamCpt>();
            customerAI.SetTeamData(teamCode, teamData, i);
            customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);

            listExistNpcId.Add(characterData.npcInfoData.id);
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

    #region 时间回调通知
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
            listExistNpcId.Clear();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = Convert.ToInt32(obj[0]);
            if (hour > 9 && hour <= 20)
            {
                StartEvent();
            }
        }
    }
    #endregion
}