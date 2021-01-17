using UnityEditor;
using UnityEngine;

public class MiniGameBirthBean : MiniGameBaseBean
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public int life;

    public override void InitForMiniGame()
    {

    }

    public void AddLife(int addLife)
    {
        life += addLife;
        if (life < 0)
            life = 0;
    }

    public void SetLife(int life)
    {
        this.life = life;
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