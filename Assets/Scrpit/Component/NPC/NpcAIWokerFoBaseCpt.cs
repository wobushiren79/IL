using UnityEngine;
using UnityEditor;

public class NpcAIWokerFoBaseCpt : BaseMonoBehaviour
{
    //主控AI
    public NpcAIWorkerCpt npcAIWorker;

    private void Start()
    {
        npcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

}