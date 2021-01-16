using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectManager : BaseManager
{
    public Dictionary<string, GameObject> listEffect = new Dictionary<string, GameObject>();

    public GameObject CreateEffect(GameObject objContainer, string name)
    {
        GameObject objModel = null;
        if (listEffect.TryGetValue(name, out objModel))
        {
            
        }
        else
        {
            objModel = CreatEffictModel(name);
        }
        if (objModel == null)
            return null;
        if (objContainer == null)
            objContainer = gameObject;
        GameObject objEffect = Instantiate(objContainer, objModel);
        return objEffect;
    }

    private GameObject CreatEffictModel(string name)
    {
        //GameObject objEffictModel = Resources.Load<GameObject>(resUrl + name);
        GameObject objEffictModel = LoadAssetUtil.SyncLoadAsset<GameObject>("effect/effect", name);
        objEffictModel.name = name;
        listEffect.Add(name, objEffictModel);
        return objEffictModel;
    }
}