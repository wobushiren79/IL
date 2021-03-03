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
    //伴侣数据
    public CharacterForFamilyBean mateCharacter;
    //孩子数据
    public List<CharacterForFamilyBean> listChildCharacter = new List<CharacterForFamilyBean>();

    /// <summary>
    /// 获取所有家庭数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterForFamilyBean> GetAllFamilyData()
    {
        List<CharacterForFamilyBean> listData = new List<CharacterForFamilyBean>();
        if (mateCharacter != null) { listData.Add(mateCharacter); }
        if (!CheckUtil.ListIsNull(listChildCharacter))
        {
            listData.AddRange(listChildCharacter);
        }
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
        if (currentDay >= marryDay)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检测是否是结婚日
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool CheckIsMarryDay(TimeBean time)
    {
        if (time.year == timeForMarry.year && time.month == timeForMarry.month && time.day == timeForMarry.day)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否是妻子
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public bool CheckIsMate(long characterId)
    {
        if (mateCharacter == null || mateCharacter.npcInfoData.id != characterId)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public CharacterForFamilyBean CreateChild(string name, CharacterBean userData, CharacterBean mateData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        CharacterForFamilyBean childData = new CharacterForFamilyBean(gameData.gameTime);
        childData.baseInfo.name = name;

        childData.body.eye = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.eye : mateData.body.eye;
        childData.body.eyeColor = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.eyeColor : mateData.body.eyeColor;
        childData.body.hair = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.hair : mateData.body.hair;
        childData.body.hairColor = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.hairColor : mateData.body.hairColor;
        childData.body.mouth = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.mouth : mateData.body.mouth;
        childData.body.mouthColor = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.mouthColor : mateData.body.mouthColor;
        childData.body.skin = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.skin : mateData.body.skin;
        childData.body.skinColor = UnityEngine.Random.Range(0, 2) == 0 ? userData.body.skinColor : mateData.body.skinColor;

        childData.attributes.cook = 1;
        childData.attributes.speed = 1;
        childData.attributes.account = 1;
        childData.attributes.charm = 1;
        childData.attributes.force = 1;
        childData.attributes.lucky = 1;
        childData.attributes.life = 50;

        int sex = UnityEngine.Random.Range(0, 2);
        if (sex == 0)
        {
            childData.SetFamilyType(FamilyTypeEnum.Son);
            childData.body.SetSex(SexEnum.Man);
        }
        else
        {
            childData.SetFamilyType(FamilyTypeEnum.Daughter);
            childData.body.SetSex(SexEnum.Woman);
        }

        listChildCharacter.Add(childData);
        return childData;
    }
}