﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class StoreInfoBean : BaseBean
{
    public int type;//类型 9市场
    public string mark;
    public long mark_id;
    public int mark_type;
    public int mark_x;
    public int mark_y;
    public long price_l;
    public long price_m;
    public long price_s;
    public string icon_key;//图标KEY
    public string name;
    public string content;
}