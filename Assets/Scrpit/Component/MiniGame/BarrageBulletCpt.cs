using UnityEngine;
using UnityEditor;

public class BarrageBulletCpt : BaseMonoBehaviour
{
    public Animator animBullet;
    public Rigidbody2D rbBullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BarrageWallCpt>())
        {
            StopMove();
            animBullet.SetBool("IsDestroy", true);
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