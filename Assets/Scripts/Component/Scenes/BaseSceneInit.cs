using UnityEngine;
using UnityEditor;
using System.Collections;

public class BaseSceneInit : BaseMonoBehaviour
{
    public virtual void Awake()
    {
        int randSeed =  GameCommonInfo.RandomSeed;
    }

    public virtual void Start()
    {
        GameTimeHandler.Instance.SetTimeStop();
        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        if (AstarPath.active != null)
            AstarPath.active.Scan();
    }

    public virtual void RefreshScene()
    {

    }

}