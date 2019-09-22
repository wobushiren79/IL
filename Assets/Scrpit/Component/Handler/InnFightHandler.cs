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
        for (int i = 0; i < listRascal.Count; i++)
        {
            NpcAIRascalCpt itemRascal = listRascal[i];
            if (itemRascal.npcFight == null)
            {
                itemRascal.npcFight = workNpc;
                workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Beater, itemRascal);
                return itemRascal;
            }
        }
        return null;
    }
}