using UnityEngine;
using UnityEditor;

public class MiniGameBarrageBulletCpt : BaseMonoBehaviour
{
    public Animator animBullet;
    public Rigidbody2D rbBullet;

    //石头伤害
    public int bulletDamage = 10;
    //石头是否摧毁
    public bool mIsDestroy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!mIsDestroy)
        {
            MiniGameBarrageWallCpt wallCpt = collision.GetComponent<MiniGameBarrageWallCpt>();
            NpcAIMiniGameBarrageCpt npcCpt = collision.GetComponent<NpcAIMiniGameBarrageCpt>();
            if (wallCpt || npcCpt)
            {
                mIsDestroy = true;
                StopMove();
                animBullet.SetBool("IsDestroy", true);
                //如果是NPC 扣血
                if (npcCpt)
                {
                    npcCpt.UnderAttack(bulletDamage);
                }
            }
        }
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    public void LaunchBullet(Vector3 targetPositon, float force)
    {
        if (rbBullet != null)
            rbBullet.velocity = (targetPositon - transform.position).normalized * force;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        if (rbBullet != null)
            rbBullet.velocity = Vector3.zero;
    }

    /// <summary>
    /// 摧毁石头
    /// </summary>
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}