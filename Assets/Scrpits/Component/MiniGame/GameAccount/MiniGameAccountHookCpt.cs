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
            AudioHandler.Instance.PlaySound(AudioSoundEnum.HitCoin);
            money.transform.SetParent(transform);
            ejectorCpt.Recycle();
            return;
        }

        MiniGameAccountWallCpt wall = collision.GetComponent<MiniGameAccountWallCpt>();
        //如果撞到墙了
        if (wall)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.HitWall);
            ejectorCpt.Recycle();
        }
    }
}