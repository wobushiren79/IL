﻿using UnityEngine;

public enum WorkerEnum
{
    Chef = 1, //厨师
    Waiter = 2,//跑堂
    Accountant = 3, //账房
    Accost = 4,// 吆喝
    Beater = 5// 打手
}

public enum WorkerDetilsEnum
{
    ChefForCook = 11,

    WaiterForSend = 21,
    WaiterForCleanTable = 22,
    WaiterForCleanBed = 23,

    AccountantForPay=31,

    AccostForSolicit = 41,
    AccostForGuide = 42,

    BeaterForDrive = 51,
}


public static class WorkerEnumTools
{
    public static Sprite GetWorkerSprite(WorkerEnum worker)
    {
        string spriteKey = "";
        switch (worker)
        {
            case WorkerEnum.Chef:
                spriteKey = "worker_chef_1";
                break;
            case WorkerEnum.Waiter:
                spriteKey = "worker_waiter_1";
                break;
            case WorkerEnum.Accountant:
                spriteKey = "worker_accountant_1";
                break;
            case WorkerEnum.Accost:
                spriteKey = "worker_accost_1";
                break;
            case WorkerEnum.Beater:
                spriteKey = "worker_beater_1";
                break;
        }
        return IconHandler.Instance.GetIconSpriteByName(spriteKey);
    }

    public static string GetWorkerName(WorkerEnum worker)
    {
        string workName = "";
        switch (worker)
        {
            case WorkerEnum.Chef:
                workName = TextHandler.Instance.manager.GetTextById(11);
                break;
            case WorkerEnum.Waiter:
                workName = TextHandler.Instance.manager.GetTextById(12);
                break;
            case WorkerEnum.Accountant:
                workName = TextHandler.Instance.manager.GetTextById(13);
                break;
            case WorkerEnum.Accost:
                workName = TextHandler.Instance.manager.GetTextById(14);
                break;
            case WorkerEnum.Beater:
                workName = TextHandler.Instance.manager.GetTextById(15);
                break;
        }
        return workName;
    }
}