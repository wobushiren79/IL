using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildBedBean : BaseBean
{
    //床的名字
    public string bedName;

    //床的数据
    public int bedSize = 1;
    public long bedBase;
    public long bedBar;
    public long bedSheets;
    public long bedPillow;

    //备注ID
    public string remarkId;
}