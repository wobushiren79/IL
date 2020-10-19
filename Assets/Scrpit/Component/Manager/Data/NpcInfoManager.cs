using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using JetBrains.Annotations;

public class NpcInfoManager : BaseManager, INpcInfoView
{
    public NpcInfoController npcInfoController;

    public Dictionary<long, NpcInfoBean> listNpcInfo;//NPC数据

    public void Awake()
    {
        npcInfoController = new NpcInfoController(this, this);
    }

    /// <summary>
    /// 获取一个随机NPC的数据
    /// </summary>
    public CharacterBean GetRandomCharacterData(long idStart, long idEnd)
    {
        long randomId = Random.Range((int)idStart, (int)idEnd + 1);
        CharacterBean characterData = null;
        if (listNpcInfo.TryGetValue(randomId, out NpcInfoBean npcInfo))
        {
            characterData = NpcInfoBean.NpcInfoToCharacterData(npcInfo);
        }
        return characterData;
    }

    /// <summary>
    /// 获取一个随机NPC的数据 
    /// </summary>
    public CharacterBean GetRandomCharacterData()
    {
        return GetRandomCharacterData(1, 38);
    }

    /// <summary>
    /// 通过ID获取NPC数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterBean GetCharacterDataById(long id)
    {
        if (listNpcInfo == null)
            return null;
        if (listNpcInfo.TryGetValue(id, out NpcInfoBean npcInfo))
        {
            CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(npcInfo);
            return characterData;
        }
        return null;
    }

    /// <summary>
    /// 通过ID获取NPC数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<CharacterBean> GetCharacterDataByIds(long[] ids)
    {
        List<CharacterBean> listData = new List<CharacterBean>();
        if (listNpcInfo == null || ids == null)
            return listData;
        foreach (long id in ids)
        {
            if (listNpcInfo.TryGetValue(id, out NpcInfoBean npcInfo))
            {
                CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(npcInfo);
                listData.Add(characterData);
            }
        }
        return listData;
    }

    /// <summary>
    ///  获取指定类型NPC数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<CharacterBean> GetCharacterDataByType(int type)
    {
        return GetCharacterDataByType(new int[] { type });
    }
    public List<CharacterBean> GetCharacterDataByType(NpcTypeEnum type)
    {
        return GetCharacterDataByType(new int[] { (int)type });
    }
    public List<CharacterBean> GetCharacterDataByType(int[] types)
    {
        List<CharacterBean> listData = new List<CharacterBean>();
        if (listNpcInfo == null)
            return listData;
        foreach (int type in types)
        {
            foreach (long key in listNpcInfo.Keys)
            {
                NpcInfoBean itemData = listNpcInfo[key];
                if (itemData.npc_type == type)
                {
                    CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(itemData);
                    listData.Add(characterData);
                }
            }
        }
        return listData;
    }

    public List<CharacterBean> GetCharacterDataByInfiniteTowersLayer(CharacterBodyManager characterBodyManager, long layer)
    {
        List<CharacterBean> listData = new List<CharacterBean>();

        if (layer % 10 == 0)
        {
            //Boss层数

        }
        else
        {
            //普通层数
            List<CharacterBean> listCharacter = GetCharacterDataByType(NpcTypeEnum.GuestTeam);
            List<CharacterBean> listTempCharacter = new List<CharacterBean>();
            if(!CheckUtil.ListIsNull(listCharacter))
            for (int i = 0; i < listCharacter.Count; i++)
            {
                CharacterBean itemCharacter = listCharacter[i];
     
                //判断层数
                int level = int.Parse(itemCharacter.npcInfoData.remark);
                int layerLevel = Mathf.FloorToInt(layer / 10f);
                if (layerLevel <= 15 && level == layerLevel)
                {
                    listTempCharacter.Add(itemCharacter);
                }
            }
            CharacterBean baseCharacterData = null;
            if (listTempCharacter.Count == 0)
            {
                baseCharacterData =  RandomUtil.GetRandomDataByList(listCharacter);
            }
            else
            {
                baseCharacterData = RandomUtil.GetRandomDataByList(listTempCharacter);
            }

            //随机生成身体数据
            CharacterBean characterOne = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterOne.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterOne);
            CharacterBean characterTwo = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterTwo.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterTwo);
            CharacterBean characterThree = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterThree.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterThree);

        }

        return listData;
    }

    /// <summary>
    /// 获取所有角色数据
    /// </summary>
    public List<CharacterBean> GetAllCharacterData()
    {
        List<CharacterBean> listData = new List<CharacterBean>();
        foreach (long key in listNpcInfo.Keys)
        {
            NpcInfoBean itemData = listNpcInfo[key];
            CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(itemData);
            listData.Add(characterData);
        }
        return listData;
    }

    /// <summary>
    /// 根据类型获取NPC信息
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public List<NpcInfoBean> GetNpcInfoByType(int[] types)
    {
        List<NpcInfoBean> listData = new List<NpcInfoBean>();
        if (listNpcInfo == null)
            return listData;
        foreach (int type in types)
        {
            foreach (long key in listNpcInfo.Keys)
            {
                NpcInfoBean itemData = listNpcInfo[key];
                if (itemData.npc_type == type)
                {
                    listData.Add(itemData);
                }
            }
        }
        return listData;
    }

    #region 数据回调
    public void GetNpcInfoFail(int type)
    {

    }

    public void GetNpcInfoSuccess(int type, List<NpcInfoBean> listData)
    {
        this.listNpcInfo = new Dictionary<long, NpcInfoBean>();
        foreach (NpcInfoBean itemData in listData)
        {
            listNpcInfo.Add(itemData.id, itemData);
        };
    }
    #endregion
}