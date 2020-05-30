using UnityEngine;
using UnityEditor;

public class EffectManager : BaseManager
{
    public GameObjectDictionary listEffectPS;

    /// <summary>
    /// 通过名字获取特效
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetEffectPSByName(string name)
    {
        return GetGameObjectByName(name, listEffectPS);
    }

}