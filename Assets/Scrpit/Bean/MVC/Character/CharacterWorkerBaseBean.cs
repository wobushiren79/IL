using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerBaseBean
{
    //职业类型
    public WorkerEnum workerType;
    //职业等级
    public int workerLevel;
    //当前职业经验值
    public int workerExp;

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="addExp"></param>
    public void AddExp(int addExp)
    {
        workerExp += addExp;
        //数据如果出现异常则修复
        int nextLevelExp = GetLevelUpExp(workerLevel + 1);
        if (workerExp > nextLevelExp)
        {
            workerExp = nextLevelExp;
        }
        else if (workerExp < 0)
        {
            workerExp = 0;
        }
    }

    /// <summary>
    /// 获取职业经验信息
    /// </summary>
    /// <param name="nextLevelExp">升级所需经验</param>
    /// <param name="currentExp">当前经验</param>
    /// <param name="levelProportion">经验百分比</param>
    public void GetWorkerExp(out int nextLevelExp, out int currentExp, out float levelProportion)
    {
        nextLevelExp = GetLevelUpExp(workerLevel + 1);
        currentExp = workerExp;
        levelProportion = (float)currentExp / nextLevelExp;
    }

    /// <summary>
    /// 升级
    /// </summary>
    public void LevelUp(CharacterAttributesBean characterAttributes)
    {
        workerLevel += 1;
        workerExp = 0;
        characterAttributes.life += 10;
        switch (workerType)
        {
            case WorkerEnum.Chef:
                characterAttributes.cook += 5;
                break;
            case WorkerEnum.Waiter:
                characterAttributes.speed += 5;
                break;
            case WorkerEnum.Accountant:
                characterAttributes.account += 5;
                break;
            case WorkerEnum.Accost:
                characterAttributes.charm += 5;
                break;
            case WorkerEnum.Beater:
                characterAttributes.force += 5;
                break;
        }
    }

    /// <summary>
    /// 获取指定等级的经验值
    /// </summary>
    /// <param name="leve">升级等级</param>
    /// <returns></returns>
    public static int GetLevelUpExp(int leve)
    {
        int nextLevelExp = int.MaxValue;
        switch (leve)
        {
            case 1:
                nextLevelExp = 100;
                break;
            case 2:
                nextLevelExp = 1000;
                break;
            case 3:
                nextLevelExp = 2000;
                break;
            case 4:
                nextLevelExp = 4000;
                break;
            case 5:
                nextLevelExp = 8000;
                break;
            case 6:
                nextLevelExp = 10000;
                break;
        }
        return nextLevelExp;
    }

    /// <summary>
    /// 获取职业名称
    /// </summary>
    /// <param name="workerType"></param>
    /// <returns></returns>
    public static string GetWorkerName(WorkerEnum workerType)
    {
        string workerName = "???";
        switch (workerType)
        {
            case WorkerEnum.Chef:
                workerName = GameCommonInfo.GetUITextById(11);
                break;
            case WorkerEnum.Waiter:
                workerName = GameCommonInfo.GetUITextById(12);
                break;
            case WorkerEnum.Accountant:
                workerName = GameCommonInfo.GetUITextById(13);
                break;
            case WorkerEnum.Accost:
                workerName = GameCommonInfo.GetUITextById(14);
                break;
            case WorkerEnum.Beater:
                workerName = GameCommonInfo.GetUITextById(15);
                break;
        }
        return workerName;
    }

    /// <summary>
    /// 获取职业等级名称
    /// </summary>
    /// <returns></returns>
    public static string GetWorkerLevelName(int level)
    {
        string workerLevelName = "???";
        switch (level)
        {
            case 0:
                workerLevelName = GameCommonInfo.GetUITextById(110);
                break;
            case 1:
                workerLevelName = GameCommonInfo.GetUITextById(111);
                break;
            case 2:
                workerLevelName = GameCommonInfo.GetUITextById(112);
                break;
            case 3:
                workerLevelName = GameCommonInfo.GetUITextById(113);
                break;
            case 4:
                workerLevelName = GameCommonInfo.GetUITextById(114);
                break;
            case 5:
                workerLevelName = GameCommonInfo.GetUITextById(115);
                break;
            case 6:
                workerLevelName = GameCommonInfo.GetUITextById(116);
                break;
        }
        return workerLevelName;
    }
}