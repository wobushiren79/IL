using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;

[Serializable]
public class InnCourtyardBean
{
    public int courtyardLevel = 1;

    public List<InnResBean> listSeedData = new List<InnResBean>();


    public InnResBean GetSeedData(Vector3 buildPosition)
    {
        foreach (InnResBean itemData in listSeedData)
        {
            if (itemData.GetStartPosition() == buildPosition)
            {
                return itemData;
            }
        }
        return null;
    }
}

[Serializable]
public class InnCourtyardSeedBean
{
    public int growDay = 0;
}