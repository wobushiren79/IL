using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcEventBuilder : NpcNormalBuilder, IBaseObserver
{
    //捣乱者模型
    public GameObject objRascalModel;
    //好友模型
    public GameObject objFriendModel;
    //杂项模型
    public GameObject objSundryModel;
    //转换心情者模型
    public GameObject objConvertModel;

    //当天已经生成过的NPCID
    public List<long> listExistNpcId = new List<long>();
    //当天已经生成过的团队ID
    public List<long> listExistTeamId = new List<long>();

    public List<NpcTeamBean> listRascal = new List<NpcTeamBean>();
    public List<NpcTeamBean> listSundry = new List<NpcTeamBean>();
    public List<NpcTeamBean> listFriend = new List<NpcTeamBean>();
    public List<NpcTeamBean> listConvert = new List<NpcTeamBean>();

    private void Start()
    {
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 开始事件
    /// </summary>
    public void StartEvent()
    {
        if (GameCommonInfo.GameConfig.statusForEvent == 0)
        {
            return;
        }
        int eventType = UnityEngine.Random.Range(0, 10);
        //先检测是否超过当天生成事件上限
        if (!GameCommonInfo.DailyLimitData.CheckEventNumber(1))
        {
            return;
        }
        switch (eventType)
        {
            case 0:
                BuildSundry();
                break;
            case 1:
                BuildRascal();
                break;
            case 2:
                BuildConvert();
                break;
            case 3:
                //TODO 待定 需要处理单个朋友和团队朋友重复出现的问题
                //BuildTownFriendsForTeam();
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                BuildTownFriendsForOne();
                break;
        }
    }

    /// <summary>
    /// 创建转换者事件
    /// </summary>
    public void BuildConvert()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = RandomUtil.GetRandomDataByList(listConvert);
        BuildConvert(teamData, npcPosition);
    }

    public void BuildConvert(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetConvertTeam(teamId);
        BuildConvert(teamData, npcPosition);
    }

    public void BuildConvert(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (npcTeam == null || listExistTeamId.Contains(npcTeam.id))
            return;
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
            GameObject npcObj = BuildNpc(objConvertModel, characterData, npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            //设置意图
            NpcAIConvertCpt convertCpt = npcObj.GetComponent<NpcAIConvertCpt>();
            CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
            convertCpt.SetTeamData(teamCode, npcTeam, i);
            convertCpt.SetIntent(NpcAISundryCpt.SundryIntentEnum.GoToInn);
        }
        listExistTeamId.Add(npcTeam.id);
    }

    /// <summary>
    /// 创建捣乱事件
    /// </summary>
    public void BuildRascal()
    {

        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = RandomUtil.GetRandomDataByList(listRascal);
        BuildRascal(teamData, npcPosition);

    }

    public void BuildRascal(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetRascalTeam(teamId);
        BuildRascal(teamData, npcPosition);
    }

    public void BuildRascal(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (npcTeam == null)
            return;
        //if (listExistTeamId.Contains(npcTeam.id))
        //改成了每日一次
        if ( listExistTeamId.Count > 0 )
             return;
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
            GameObject npcObj = BuildNpc(objRascalModel, characterData, npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            //设置意图
            NpcAIRascalCpt rascalCpt = npcObj.GetComponent<NpcAIRascalCpt>();
            CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
            rascalCpt.SetTeamData(teamCode, npcTeam, i);
            rascalCpt.SetFavorabilityData(characterFavorability);
            rascalCpt.AddStatusIconForRascal();
            rascalCpt.SetIntent(NpcAIRascalCpt.RascalIntentEnum.GoToInn);

            //设置捣乱者血量
            characterData.GetAttributes( out CharacterAttributesBean characterAttributes);
            rascalCpt.characterMaxLife = characterAttributes.life;
            rascalCpt.characterLife = characterAttributes.life;
        }
        listExistTeamId.Add(npcTeam.id);
    }


    /// <summary>
    /// 创建杂项事件
    /// </summary>
    public void BuildSundry()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = RandomUtil.GetRandomDataByList(listSundry);
        BuildSundry(teamData, npcPosition);
    }

    public void BuildSundry(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetSundryTeam(teamId);
        BuildSundry(teamData, npcPosition);
    }

    public void BuildSundry(NpcTeamBean npcTeam, Vector3 npcPosition)
    {
        if (listExistTeamId.Contains(npcTeam.id))
            return;
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
            GameObject npcObj = BuildNpc(objSundryModel, characterData, npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            //设置意图
            NpcAISundryCpt sundryCpt = npcObj.GetComponent<NpcAISundryCpt>();
            CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
            sundryCpt.SetTeamData(teamCode, npcTeam, i);
            sundryCpt.SetIntent(NpcAISundryCpt.SundryIntentEnum.GoToInn);
        }
        listExistTeamId.Add(npcTeam.id);
    }


    /// <summary>
    /// 创建小镇好友
    /// </summary>
    public void BuildTownFriendsForOne()
    {
        List<CharacterFavorabilityBean> listFavorabilityData = gameDataManager.gameData.listCharacterFavorability;
        //获取小镇角色
        List<CharacterFavorabilityBean> listTownFavorabilityData = new List<CharacterFavorabilityBean>();
        foreach (CharacterFavorabilityBean itemFavorability in listFavorabilityData)
        {
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemFavorability.characterId);
            if (characterData == null)
                continue;

            if (characterData.npcInfoData.npc_type == (int)NpcTypeEnum.Town
                //生成的NPC中不包含这个角色
                && !listExistNpcId.Contains(characterData.npcInfoData.id)
                //好感度大于1
                && itemFavorability.GetFavorabilityLevel() >= 1)
            {
                listTownFavorabilityData.Add(itemFavorability);
            }
        }
        //符合条件的NPC中随机取一人
        CharacterFavorabilityBean randomFavorabilityData = RandomUtil.GetRandomDataByList(listTownFavorabilityData);
        if (randomFavorabilityData == null)
            return;
        CharacterBean randomCharacterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(randomFavorabilityData.characterId);
        Vector3 npcPosition = GetRandomStartPosition();
        BuildTownFriendsForOne(randomCharacterData, randomFavorabilityData, npcPosition);
    }

    public void BuildTownFriendsForTeam()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = RandomUtil.GetRandomDataByList(listFriend);
        BuildTownFriendsForTeam(teamData, npcPosition);
    }

    public void BuildTownFriendsForTeam(long teamId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        NpcTeamBean teamData = npcTeamManager.GetFriendTeam(teamId);
        BuildTownFriendsForTeam(teamData, npcPosition);
    }

    public void BuildTownFriendsForOne(long npcId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(npcId);
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(characterData.npcInfoData.id);
        BuildTownFriendsForOne(characterData, characterFavorability, npcPosition);
    }

    public void BuildTownFriendsForOne(CharacterBean characterData, CharacterFavorabilityBean characterFavorability, Vector3 npcPosition)
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
        if (listExistTeamId.Contains(teamData.id))
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

            GameObject npcObj = Instantiate(objContainer, objFriendModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));

            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);
            baseNpcAI.AddStatusIconForFriend();
            baseNpcAI.AddStatusIconForGuestTeam(teamColor);
            baseNpcAI.SetFavorabilityData(characterFavorability);

            NpcAICostomerForFriendCpt customerAI = baseNpcAI.GetComponent<NpcAICostomerForFriendCpt>();
            customerAI.SetTeamData(teamCode, teamData, i, teamColor);
            customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);

        }
        listExistTeamId.Add(teamData.id);
    }

    /// <summary>
    /// 通过团队Code获取捣乱成员
    /// </summary>
    /// <param name="teamCode"></param>
    /// <returns></returns>
    public List<NpcAIRascalCpt> GetRascalTeamByTeamCode(string teamCode)
    {
        List<NpcAIRascalCpt> listTeamMember = new List<NpcAIRascalCpt>();
        if (CheckUtil.StringIsNull(teamCode))
            return listTeamMember;
        NpcAIRascalCpt[] arrayTeam = objContainer.GetComponentsInChildren<NpcAIRascalCpt>();
        foreach (NpcAIRascalCpt itemData in arrayTeam)
        {
            if (itemData.teamCode.EndsWith(teamCode))
            {
                listTeamMember.Add(itemData);
            }
        }
        return listTeamMember;
    }

    /// <summary>
    /// 通过团队Code获取杂项成员
    /// </summary>
    /// <param name="teamCode"></param>
    /// <returns></returns>
    public List<NpcAISundryCpt> GetSundryTeamByTeamCode(string teamCode)
    {
        List<NpcAISundryCpt> listTeamMember = new List<NpcAISundryCpt>();
        if (CheckUtil.StringIsNull(teamCode))
            return listTeamMember;
        NpcAISundryCpt[] arrayTeam = objContainer.GetComponentsInChildren<NpcAISundryCpt>();
        foreach (NpcAISundryCpt itemData in arrayTeam)
        {
            if (itemData.teamCode.EndsWith(teamCode))
            {
                listTeamMember.Add(itemData);
            }
        }
        return listTeamMember;
    }

    public override void ClearNpc()
    {
        base.ClearNpc();
    }

    #region 时间回调通知
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
            listExistNpcId.Clear();
            listExistTeamId.Clear();
            //更新数据
            listRascal = npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Rascal, gameDataManager.gameData);
            listSundry = npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Sundry, gameDataManager.gameData);
            listFriend = npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Friend, gameDataManager.gameData);
            listConvert.Clear();
            listConvert.AddRange(npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Entertain, gameDataManager.gameData));
            listConvert.AddRange(npcTeamManager.GetRandomTeamMeetConditionByType(NpcTeamTypeEnum.Disappointed, gameDataManager.gameData));
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = Convert.ToInt32(obj[0]);
            if (hour >= 9 && hour <= 20)
            {
                StartEvent();
            }
        }
    }
    #endregion
}