using UnityEngine;
using UnityEditor;

public class BaseMiniGameHandler : BaseHandler
{
    public enum NotifyMiniGameEnum
    {
        GameStart = 1,
        GameEnd = 2,
        GameClose = 3,
    }
}