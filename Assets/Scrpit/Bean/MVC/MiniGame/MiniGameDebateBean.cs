using UnityEngine;
using UnityEditor;

public class MiniGameDebateBean : MiniGameBaseBean
{
    public Vector3 debatePosition;

    public MiniGameDebateBean() {
        gameType = MiniGameEnum.Debate;
    }
}