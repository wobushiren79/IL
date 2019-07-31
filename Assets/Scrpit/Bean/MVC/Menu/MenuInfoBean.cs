using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MenuInfoBean : BaseBean
{
    public long menu_id;
    public string icon_key;//图标
    public int cook_time;//烹饪时间

    public int ing_oilsalt;//油盐类
    public int ing_meat;//肉类
    public int ing_riverfresh;//河鲜类
    public int ing_seafood;//海鲜类
    public int ing_vegetables;//蔬菜类
    public int ing_melonfruit;//瓜果类
    public int ing_waterwine;//水酒类
    public int ing_flour;//面粉类

    public string name;
    public string content;

    public long price_s;
    public long price_m;
    public long price_l;

}