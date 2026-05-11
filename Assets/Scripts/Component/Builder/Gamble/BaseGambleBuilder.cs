using UnityEngine;
using UnityEditor;

public class BaseGambleBuilder : BaseMonoBehaviour
{
    protected virtual void Awake()
    {
        tag = TagInfo.Tag_Gamble;
    }
}