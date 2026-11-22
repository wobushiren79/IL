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
        //各场景打开UI前，先按已保存配置同步运行时语言
        TextHandler.Instance.InitData();
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