﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoManager : BaseManager,INpcInfoView
{
    public NpcInfoController npcInfoController;

    public Dictionary<long,NpcInfoBean> listNpcInfo;

    private void Awake()
    {
        npcInfoController = new NpcInfoController(this,this);
    }

    /// <summary>
    /// 获取一个随机NPC的数据
    /// </summary>
    public CharacterBean GetRandomCharacterData()
    {
        List<CharacterBean> listData = GetCharacterDataByType(0);
        CharacterBean characterData = RandomUtil.GetRandomDataByList(listData);
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
        if(listNpcInfo.TryGetValue(id,out NpcInfoBean npcInfo))
        {
            CharacterBean characterData = NpcInfoToCharacterData(npcInfo);
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
        foreach (long key in listNpcInfo.Keys)
        {
            foreach (int type in types) {
                NpcInfoBean itemData = listNpcInfo[key];
                if (itemData.npc_type == type)
                {
                    CharacterBean characterData = NpcInfoToCharacterData(itemData);
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
        foreach (long key in listNpcInfo.Keys)
        {
            foreach (int type in types)
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


    /// <summary>
    /// NPC信息转为角色信息
    /// </summary>
    /// <param name="npcInfo"></param>
    /// <returns></returns>
    public static CharacterBean NpcInfoToCharacterData(NpcInfoBean npcInfo)
    {
        CharacterBean characterData = new CharacterBean();

        characterData.body = new CharacterBodyBean();
        characterData.body.hair = npcInfo.hair_id;
        characterData.body.eye = npcInfo.eye_id;
        characterData.body.mouth = npcInfo.mouth_id;
        characterData.body.sex = npcInfo.sex;
        characterData.body.face = npcInfo.face;

        characterData.equips = new CharacterEquipBean();
        characterData.equips.hatId = npcInfo.hat_id;
        characterData.equips.clothesId = npcInfo.clothes_id;
        characterData.equips.shoesId = npcInfo.shoes_id;

        return characterData;
    }

    #region 数据回调
    public void GetNpcInfoFail(int type)
    {
       
    }

    public void GetNpcInfoSuccess(int type, List<NpcInfoBean> listData)
    {
        this.listNpcInfo = new Dictionary<long, NpcInfoBean>();
        foreach (NpcInfoBean itemData in listData) {
            listNpcInfo.Add(itemData.id,itemData);
        };
    }
    #endregion
}