using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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