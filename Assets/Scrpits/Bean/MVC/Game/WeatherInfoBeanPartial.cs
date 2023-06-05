using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WeatherInfoBean
{
    public Vector3 GetVector3(string data)
    {
        if (data.IsNull())
            return Vector3.zero;
        float[] position = data.SplitForArrayFloat(',');
        return new Vector3(position[0], position[1], position[2]);
    }
}
public partial class WeatherInfoCfg
{
}
