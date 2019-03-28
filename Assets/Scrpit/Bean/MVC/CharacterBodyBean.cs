using UnityEngine;
using UnityEditor;

public class CharacterBodyBean : ScriptableObject
{
    public int sex;//性别 0未知，1男，2女，3中性
    public ColorBean skinColor;//皮肤颜色

    public string hair;//发型
    public ColorBean hairColor;//发型颜色

    public string eye;//眼睛
    public ColorBean eyeColor;//眼睛颜色

    public string mouth;//嘴巴
    public ColorBean mouthColor;//嘴巴颜色
}