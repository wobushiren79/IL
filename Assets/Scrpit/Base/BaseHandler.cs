using UnityEngine;
using UnityEditor;

public class BaseHandler<T,M> : BaseSingletonMonoBehaviour<T> 
    where M : BaseManager
    where T : BaseMonoBehaviour
{
    private M mManager;
    protected virtual void Awake()
    {

    }

    public M manager
    {
        get
        {
            if (mManager == null)
            {
                mManager = CptUtil.AddCpt<M>(gameObject);
            }
            return mManager;
        }
    }
}