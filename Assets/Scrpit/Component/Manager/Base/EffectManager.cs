using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectManager : BaseManager
{
    public Dictionary<string, GameObject> listEffect = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> listOtherEffect = new Dictionary<string, GameObject>();

    public GameObject CreateEffect(GameObject objContainer, string name)
    {
        GameObject objModel =  GetModel(listEffect, "effect/effect", name);
        GameObject objEffect = Instantiate(objContainer, objModel);
        return objEffect;
    }

    public GameObject CreateOtherEffect(GameObject objContainer, string name)
    {
        GameObject objModel = GetModel(listEffect, "effect/effect_other", name);
        GameObject objEffect = Instantiate(objContainer, objModel);
        return objEffect;
    }
}