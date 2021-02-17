using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FamilyDataBean : BaseBean
{
    //怀孕进度
    public float birthPro = 0;
    //结婚日
    public TimeBean timeForMarry = new TimeBean();

    //怀孕天数
    public int birthDay = 0;
    //妻子数据
    public CharacterForFamilyBean wifeCharacter;
    //孩子数据
    public List<CharacterForFamilyBean> listChildCharacter = new List<CharacterForFamilyBean>();

    /// <summary>
    /// 获取所有家庭数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterForFamilyBean> GetAllFamilyData()
    {
        List<CharacterForFamilyBean> listData = new List<CharacterForFamilyBean>();
        if (wifeCharacter != null) { listData.Add(wifeCharacter); }
        if (!CheckUtil.ListIsNull(listChildCharacter)) { listData.AddRange(listChildCharacter); }
        return listData;
    }

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

    /// <summary>
    /// 检测是否已经结婚
    /// </summary>
    /// <param name="time"></param>
    public bool CheckMarry(TimeBean time)
    {
        if (timeForMarry == null)
            return false;
        if (timeForMarry.year == 0 || timeForMarry.month == 0 || timeForMarry.day == 0)
            return false;
        int marryDay = timeForMarry.year * 4 * 42 + timeForMarry.month * 42 + timeForMarry.day;
        int currentDay = time.year * 4 * 42 + time.month * 42 + time.day;
        if (currentDay > marryDay)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}