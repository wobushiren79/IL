using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoManager : BaseManager,INpcInfoView
{
    public NpcInfoController npcInfoController;

    public List<NpcInfoBean> listNpcInfo;

    private void Awake()
    {
        npcInfoController = new NpcInfoController(this,this);
    }

    /// <summary>
    /// 获取一个随机NPC的数据
    /// </summary>
    public CharacterBean GetRandomCharacterData()
    {
        NpcInfoBean itemData= RandomUtil.GetRandomDataByList(listNpcInfo);
        CharacterBean characterData = NpcInfoToCharacterData(itemData);
        return characterData;
    }

    /// <summary>
    ///  获取指定类型NPC数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public CharacterBean GetCharacterDataByType(int type)
    {  
        for (int i=0;i< listNpcInfo.Count; i++)
        {
            NpcInfoBean itemData = listNpcInfo[i];
            if (itemData.npc_type == type)
            {
                CharacterBean characterData = NpcInfoToCharacterData(itemData);
                return characterData;
            }
        }
        return null;
    }

    /// <summary>
    /// NPC信息转为角色信息
    /// </summary>
    /// <param name="npcInfo"></param>
    /// <returns></returns>
    public CharacterBean NpcInfoToCharacterData(NpcInfoBean npcInfo)
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

    public void GetNpcInfoSuccess(int type, List<NpcInfoBean> listNpcInfo)
    {
        this.listNpcInfo = listNpcInfo;
    }
    #endregion
}