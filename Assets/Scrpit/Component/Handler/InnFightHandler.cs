using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class InnFightHandler : BaseMonoBehaviour
{

    public NpcAIRascalCpt SetFight(List<NpcAIRascalCpt> listRascal, NpcAIWorkerCpt workNpc)
    {
        if (CheckUtil.ListIsNull(listRascal))
        {
            return null;
        }
        NpcAIRascalCpt itemRascal = listRascal[0];
        workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Beater, itemRascal);
        return itemRascal;
    }
}