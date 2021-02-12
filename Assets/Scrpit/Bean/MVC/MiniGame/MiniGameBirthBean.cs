using UnityEditor;
using UnityEngine;

public class MiniGameBirthBean : MiniGameBaseBean
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public int fireNumber;
    public float enmeySpeed;
    public float enmeyBuildInterval;
    public float playSpeed;

    public override void InitForMiniGame()
    {
        gameType = MiniGameEnum.Birth;
        this.winFireNumber = fireNumber;
    }

    public void AddFireNumber(int addNumber)
    {
        fireNumber += addNumber;
        if (fireNumber < 0)
            fireNumber = 0;
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }

}