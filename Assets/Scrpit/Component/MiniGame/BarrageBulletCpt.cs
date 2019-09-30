using UnityEngine;
using UnityEditor;

public class BarrageBulletCpt : BaseMonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BarrageWallCpt>())
        {
            Destroy(gameObject);
        }
    }
}