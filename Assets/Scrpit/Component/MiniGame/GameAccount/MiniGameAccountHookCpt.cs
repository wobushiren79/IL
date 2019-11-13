using UnityEngine;
using UnityEditor;

public class MiniGameAccountHookCpt : BaseMonoBehaviour
{
    public MiniGameAccountEjectorCpt ejectorCpt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MiniGameAccountMoneyCpt money = collision.GetComponent<MiniGameAccountMoneyCpt>();
        //如果撞到钱了
        if (money)
        {
            ejectorCpt.Recycle();
            return;
        }

        MiniGameAccountWallCpt wall = collision.GetComponent<MiniGameAccountWallCpt>();
        //如果撞到墙了
        if (wall)
        {
            ejectorCpt.Recycle();
        }
    }
}