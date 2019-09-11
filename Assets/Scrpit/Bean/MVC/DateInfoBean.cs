using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class DateInfoBean : BaseBean 
{
    public long date_id;
    public int month;
    public int day;
    public string content;
}