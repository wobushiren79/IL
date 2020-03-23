using UnityEngine;
using UnityEditor;

public class MiniGameBarrageBulletCpt : BaseMonoBehaviour
{
    public Animator animBullet;
    public Rigidbody2D rbBullet;
    public SpriteRenderer srBullet;

    public int bulletDamage = 10;
    public bool mIsDestroy = false;

    public bool hasDestroyAnim= false;
    public bool hasMoveAnim = false;

    public MiniGameBarrageBuilder barrageBuilder;

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
                //摧毁处理
                if (hasDestroyAnim)
                {
                    animBullet.SetBool("IsDestroy", true);
                }
                else
                {
                    DestroyBullet();
                }
                //如果是NPC 扣血
                if (npcCpt)
                {
                    npcCpt.UnderAttack(bulletDamage);
                }
            }
        }
    }

    /// <summary>
    /// 设置子弹数据
    /// </summary>
    public void SetBulletData(
        MiniGameBarrageBulletTypeEnum miniGameBarrageBulletType, 
        Sprite spIcon,
        RuntimeAnimatorController animatorController)
    {
        animBullet.runtimeAnimatorController = animatorController;
        switch (miniGameBarrageBulletType)
        {
            case MiniGameBarrageBulletTypeEnum.Stone:
                hasDestroyAnim = true;
                bulletDamage = 10;
                srBullet.sprite = spIcon;
                break;
            case MiniGameBarrageBulletTypeEnum.Arrow:
                bulletDamage = 10;
                srBullet.sprite = spIcon;
                break;
        }
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    public void LaunchBullet(Vector3 targetPositon, float force)
    {
        float angle= VectorUtil.GetAngle(transform.position, targetPositon);
        transform.Rotate(new Vector3(0,0,1),-90 + angle);
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