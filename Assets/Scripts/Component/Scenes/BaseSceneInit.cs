using UnityEngine;
using UnityEditor;
using System.Collections;

public class BaseSceneInit : BaseMonoBehaviour
{
    public virtual void Awake()
    {
        int randSeed =  GameCommonInfo.RandomSeed;
        //应用存档中的游戏配置（语言/窗口/音量/帧数等），需在打开任何UI之前，保证文本按存档语言显示
        GameDataHandler.Instance.ApplyGameConfig();
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