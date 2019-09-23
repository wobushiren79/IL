using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForBeaterBean : CharacterWorkerBaseBean
{
    public long fightTotalNumber ;
    public long fightWinNumber;
    public long fightLoseNumber;
    public float fightTotalTime;
    public float restTotalTime;


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

    public void AddFightTime(float fightTime)
    {
        fightTotalTime += fightTime;
    }

    public void AddRestTime(float restTime)
    {
        restTotalTime += restTime;
    }
}