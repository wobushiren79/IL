using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FamilyDataBean : BaseBean
{
    //怀孕进度
    public float birthPro = 0;
    //怀孕天数
    public int birthDay = 0;
    //妻子数据
    public CharacterBean wifeCharacter;
    //孩子数据
    public List<CharacterBean> listChildCharacter;
    //结婚日
    public TimeBean timeForMarry;

    /// <summary>
    /// 增加怀孕进度
    /// </summary>
    /// <param name="add"></param>
    public void addBirthPro(float add)
    {
        birthPro += add;
        if (birthPro < 0)
            birthPro = 0;
        if (birthPro > 1)
            birthPro = 1;
    }
}