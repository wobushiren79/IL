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
    public string hair_color;
    public string eye_id;
    public string eye_color;
    public string mouth_id;
    public string mouth_color;

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
        characterData.baseInfo.name = npcInfo.name;

        characterData.body = new CharacterBodyBean();
        characterData.body.hair = npcInfo.hair_id;
        //设置头发颜色
        ColorBean hairColor = ColorStrToColorBean(npcInfo.hair_color);
        if (hairColor != null)
            characterData.body.hairColor = hairColor;
        characterData.body.eye = npcInfo.eye_id;
        //设置眼睛颜色
        ColorBean eyeColor = ColorStrToColorBean(npcInfo.eye_color);
        if (eyeColor != null)
            characterData.body.eyeColor = eyeColor;
        characterData.body.mouth = npcInfo.mouth_id;
        //设置嘴巴颜色
        ColorBean mouthColor = ColorStrToColorBean(npcInfo.mouth_color);
        if (mouthColor != null)
            characterData.body.mouthColor = mouthColor;
        characterData.body.sex = npcInfo.sex;
        characterData.body.face = npcInfo.face;

        characterData.equips = new CharacterEquipBean();
        characterData.equips.hatId = npcInfo.hat_id;
        characterData.equips.clothesId = npcInfo.clothes_id;
        characterData.equips.shoesId = npcInfo.shoes_id;

        return characterData;
    }

    public static ColorBean ColorStrToColorBean(string color)
    {
        if (CheckUtil.StringIsNull(color))
        {
            return null;
        }
        else
        {
            float[] listData = StringUtil.SplitBySubstringForArrayFloat(color, ',');
            if (listData == null)
                return null;
            if (listData.Length == 3)
            {
                return new ColorBean(listData[0], listData[1], listData[2], 1);
            }
            else if (listData.Length == 4)
            {
                return new ColorBean(listData[0], listData[1], listData[2], listData[3]);
            }
            else
            {
                return null;
            }
        }
    }
}