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
    public string  mouth_id;

    public string name;//npc名字

    public float position_x;
    public float position_y;
}