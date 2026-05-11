using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class SightForMiniGameBarrageCpt : BaseMonoBehaviour
{
    private ICallBack callBack;

    //视线内的所有石头
    public List<MiniGameBarrageBulletCpt> listSightBullet = new List<MiniGameBarrageBulletCpt>();

    private void Start()
    {
        StartCoroutine(GetBullet());
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    BarrageBulletCpt barrageBullet = collision.GetComponent<BarrageBulletCpt>();
    //    if (barrageBullet)
    //    {
    //        listSightBullet.Add(barrageBullet);
    //        if (callBack != null)
    //            callBack.SeeBullet(barrageBullet);
    //    }
    //}


    public IEnumerator GetBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
            List<MiniGameBarrageBulletCpt> barrageBulletList = new List<MiniGameBarrageBulletCpt>();
            foreach (Collider2D itemCollider in colliders)
            {
                MiniGameBarrageBulletCpt itemBullet = itemCollider.GetComponent<MiniGameBarrageBulletCpt>();
                if (itemBullet)
                {
                    barrageBulletList.Add(itemBullet);
                }
            }
            if (callBack != null && barrageBulletList.Count > 0)
                callBack.SeeBullet(barrageBulletList);
        }
    }

    public interface ICallBack
    {
        /// <summary>
        /// 看见子弹
        /// </summary>
        /// <param name="barrageBullet"></param>
        void SeeBullet(List<MiniGameBarrageBulletCpt> barrageBulletList);
    }
}