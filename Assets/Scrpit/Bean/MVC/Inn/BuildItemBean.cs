using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class BuildItemBean : BaseBean
{
    public int build_type;
    public long model_id;
    public string icon_key;
    public string name;
    public string content;
    public int aesthetics;//美观值
}