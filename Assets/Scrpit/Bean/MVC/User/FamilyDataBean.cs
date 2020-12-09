using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FamilyDataBean : BaseBean
{
    //妻子数据
    public CharacterBean wifeCharacter;
    //孩子数据
    public List<CharacterBean> listChildCharacter;
    //结婚日
    public TimeBean timeForMarry;

}