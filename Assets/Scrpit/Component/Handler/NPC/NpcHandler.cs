using UnityEditor;
using UnityEngine;

public class NpcHandler : BaseHandler<NpcHandler,NpcManager>
{
    //NPC创建
    protected NpcWorkerBuilder _builderForWorker;
    public NpcWorkerBuilder builderForWorker 
    {
        get
        {
            if (_builderForWorker == null)
            {
                _builderForWorker = FindWithTag<NpcWorkerBuilder>(TagInfo.Tag_NpcBuilder);
            }
            return _builderForWorker;
        }
    }

    protected NpcCustomerBuilder _builderForCustomer;
    public NpcCustomerBuilder builderForCustomer
    {
        get
        {
            if (_builderForCustomer == null)
            {
                _builderForCustomer = FindWithTag<NpcCustomerBuilder>(TagInfo.Tag_NpcBuilder);
            }
            return _builderForCustomer;
        }
    }

    protected NpcEventBuilder _builderForEvent;
    public NpcEventBuilder builderForEvent
    {
        get
        {
            if (_builderForEvent == null)
            {
                _builderForEvent = FindWithTag<NpcEventBuilder>(TagInfo.Tag_NpcBuilder);
            }
            return _builderForEvent;
        }
    }

    protected NpcImportantBuilder _buildForImportant;

    public NpcImportantBuilder buildForImportant
    {
        get
        {
            if (_buildForImportant == null)
            {
                _buildForImportant = FindWithTag<NpcImportantBuilder>(TagInfo.Tag_NpcBuilder);
            }
            return _buildForImportant;
        }
    }

    protected NpcPasserBuilder _buildForPasser;

    public NpcPasserBuilder buildForPasser
    {
        get
        {
            if (_buildForPasser == null)
            {
                _buildForPasser = FindWithTag<NpcPasserBuilder>(TagInfo.Tag_NpcBuilder);
            }
            return _buildForPasser;
        }
    }
}