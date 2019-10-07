using UnityEngine;
using UnityEditor;

public class SightForMiniGameBarrageCpt : BaseMonoBehaviour
{
    private ICallBack callBack;

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BarrageBulletCpt barrageBullet = collision.GetComponent<BarrageBulletCpt>();
        if(barrageBullet)
        {
            if (callBack != null)
                callBack.SeeBullet(barrageBullet);
        }
    }

    public interface ICallBack
    {
        /// <summary>
        /// 看见子弹
        /// </summary>
        /// <param name="barrageBullet"></param>
        void SeeBullet(BarrageBulletCpt barrageBullet);
    }
}