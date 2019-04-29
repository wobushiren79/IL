using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterBean
{
    //角色基础信息
    public CharacterBaseBean baseInfo = new CharacterBaseBean();
    //角色属性
    public CharacterAttributesBean attributes = new CharacterAttributesBean();
    //角色身体属性
    public CharacterBodyBean body = new CharacterBodyBean();
    //角色装备属性
    public CharacterEquipBean equips = new CharacterEquipBean();
}