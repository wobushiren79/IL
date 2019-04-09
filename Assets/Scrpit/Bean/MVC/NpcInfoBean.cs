using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class NpcInfoBean : BaseBean
{
    public long npc_id;//npcId
    public int npc_type;//0默认NPC，
    public int sex;//性别
    public long hat_id;
    public long clothes_id;
    public long shoes_id;
    public string hair_id;
    public string eye_id;
    public string  mouth_id;

    public string name;//npc名字
}