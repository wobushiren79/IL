using UnityEngine;
using UnityEditor;

public class WorkerInfo
{
    public bool isWork;//是否工作 
    public int priority;//工作优先度
    public WorkerEnum worker;//工作职业


    public WorkerInfo()
    {

    }

    public WorkerInfo(WorkerEnum worker,int priority, bool isWork)
    {
        this.isWork = isWork;
        this.priority = priority;
        this.worker = worker;
    }
}