using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class NpcInfoBean : BaseBean
{
    public long npc_id;//npcId
    public int npc_type;//0默认NPC，
    public int sex;//性别
    public int face;//面向 1.左边 2右边
    public long hat_id;
    public long clothes_id;
    public long shoes_id;
    public string hair_id;
    public string eye_id;
    public string mouth_id;

    public string name;//npc名字

    public float position_x;
    public float position_y;

    public string talk_ids;//人物对话ID


    /// <summary>
    /// NPC信息转为角色信息
    /// </summary>
    /// <param name="npcInfo"></param>
    /// <returns></returns>
    public static CharacterBean NpcInfoToCharacterData(NpcInfoBean npcInfo)
    {
        CharacterBean characterData = new CharacterBean();

        characterData.baseInfo.characterId = npcInfo.id + "";

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
}