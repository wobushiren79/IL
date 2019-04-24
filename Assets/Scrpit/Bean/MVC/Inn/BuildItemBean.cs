using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class BuildItemBean : BaseBean
{
    //类型 1地板 2墙 3桌子 4柜台 5厨房 6装饰
    public enum BuildType
    {
        Floor=1,
        Wall=2,
        Table=3,
        Stove = 4,
        Counter=5,
        Door=9,
    }

    public int build_type;
    public long price_s;
    public long price_m;
    public long price_l;
    public string icon_key;
    public string name;
}