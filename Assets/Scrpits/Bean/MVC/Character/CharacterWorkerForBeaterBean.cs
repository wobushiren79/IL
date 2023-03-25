using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForBeaterBean : CharacterWorkerBaseBean
{
    public long fightTotalNumber ;
    public long fightWinNumber;
    public long fightLoseNumber;

    public CharacterWorkerForBeaterBean()
    {
        workerType = WorkerEnum.Beater;
    }

    public void AddFightWinNumber(long number)
    {
        fightTotalNumber += number;
        fightWinNumber += number;
    }

    public void AddFightLoseNumber(long number)
    {
        fightTotalNumber += number;
        fightLoseNumber += number;
    }
}