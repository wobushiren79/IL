using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoManager : BaseManager, INpcInfoView
{
    public NpcInfoController npcInfoController;

    public Dictionary<long, NpcInfoBean> listNpcInfo;//重要NPC数据
    public Dictionary<long, NpcInfoBean> listNormalNpcInfo;//普通路人NPC数据

    private void Awake()
    {
        npcInfoController = new NpcInfoController(this, this);
    }

    /// <summary>
    /// 获取一个随机NPC的数据
    /// </summary>
    public CharacterBean GetRandomCharacterData(long idStart, long idEnd)
    {
        long randomId = (long)Random.Range(idStart, idEnd);
        CharacterBean characterData = null;
        if (listNormalNpcInfo.TryGetValue(randomId, out NpcInfoBean npcInfo))
        {
            characterData = NpcInfoBean.NpcInfoToCharacterData(npcInfo);
        }
        return characterData;
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
    ///  获取指定类型NPC数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<CharacterBean> GetCharacterDataByType(int type)
    {
        return GetCharacterDataByType(new int[] { type });
    }

    public List<CharacterBean> GetCharacterDataByType(int[] types)
    {
        List<CharacterBean> listData = new List<CharacterBean>();
        if (listNpcInfo == null)
            return listData;
        foreach (int type in types)
        {
            if (type == 0)
            {
                foreach (long key in listNormalNpcInfo.Keys)
                {
                    NpcInfoBean itemData = listNpcInfo[key];
                    CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(itemData);
                    listData.Add(characterData);
                }
            }
            else
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
            if (type == 0)
            {
                foreach (long key in listNpcInfo.Keys)
                {
                    NpcInfoBean itemData = listNormalNpcInfo[key];
                    listData.Add(itemData);
                }

            }
            else
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
        this.listNormalNpcInfo = new Dictionary<long, NpcInfoBean>();
        foreach (NpcInfoBean itemData in listData)
        {
            if (itemData.npc_type == 0)
            {
                listNormalNpcInfo.Add(itemData.id, itemData);
            }
            else
            {
                listNpcInfo.Add(itemData.id, itemData);
            }
        };
    }
    #endregion
}