using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class WorkerNumberView : BaseMonoBehaviour
{
    public Text tvNumberForIdle;
    public Text tvNumberForChef;
    public Text tvNumberForWaiter;
    public Text tvNumberForAccount;
    public Text tvNumberForAccost;
    public Text tvNumberForBeater;

    protected NpcWorkerBuilder workerBuilder;

    private void Awake()
    {
        workerBuilder = Find<NpcWorkerBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    private void Update()
    {
        HandleForUI();
    }

    public void HandleForUI()
    {
        if (workerBuilder != null && workerBuilder.listNpcWorker != null)
        {
            int numberForIdle = 0;
            int numberForChef = 0;
            int numberForWaiter = 0;
            int numberForAccount = 0;
            int numberForAccost = 0;
            int numberForBeater = 0;
            for (int i = 0; i < workerBuilder.listNpcWorker.Count; i++)
            {
                NpcAIWorkerCpt npcWorker = workerBuilder.listNpcWorker[i];
                NpcAIWorkerCpt.WorkerIntentEnum workerIntent = npcWorker.GetWorkerStatus();
                if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle || workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Daze)
                {
                    numberForIdle++;
                }
                else if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Cook)
                {
                    numberForChef++;
                }
                else if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.WaiterClean || workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.WaiterSend)
                {
                    numberForWaiter++;
                }
                else if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Accounting)
                {
                    numberForAccount++;
                }
                else if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.AccostSolicit|| workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.AccostGuide)
                {
                    numberForAccost++;
                }
                else if (workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Beater)
                {
                    numberForBeater++;
                }
            }
            tvNumberForIdle.text = numberForIdle + "";
            tvNumberForChef.text = numberForChef + "";
            tvNumberForWaiter.text = numberForWaiter + "";
            tvNumberForAccount.text = numberForAccount + "";
            tvNumberForAccost.text = numberForAccost + "";
            tvNumberForBeater.text = numberForBeater + "";
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}