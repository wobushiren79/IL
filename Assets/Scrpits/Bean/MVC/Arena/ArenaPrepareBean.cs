using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ArenaPrepareBean
{
    //游戏数据
    public MiniGameBaseBean miniGameData;

    public ArenaPrepareBean(MiniGameBaseBean miniGameData)
    {
        this.miniGameData = miniGameData;
    }
}