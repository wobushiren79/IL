using UnityEngine;
using UnityEditor;

public class MoveByGameTimeCpt : BaseMonoBehaviour
{
    protected GameTimeHandler gameTimeHandler;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float startTimeHour;
    public float endTimeHour;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
    }

    private void Update()
    {
        if (gameTimeHandler == null)
            return;
        gameTimeHandler.GetTime(out float hour, out float min);
        if (hour >= startTimeHour && hour < endTimeHour)
        {
            float lerp = ((hour - startTimeHour) * 60 + min) / ((endTimeHour - startTimeHour) * 60);
            transform.position = Vector3.Lerp(startPosition, endPosition, lerp);
        }
    }
}