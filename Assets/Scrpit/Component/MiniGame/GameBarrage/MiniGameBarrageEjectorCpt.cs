using UnityEngine;
using UnityEditor;

public class MiniGameBarrageEjectorCpt : BaseMonoBehaviour
{
    /// <summary>
    /// 发射类型
    /// </summary>
    public enum LaunchTypeEnum
    {
        Single = 1,//单发
        Double = 2,//双发、
        Triple = 3,//三连发

    }

    //发射动画
    public Animator animEjector;
    //发射的工具
    public GameObject objEjector;
    //子弹容器
    public GameObject objBulletContainer;
    //子弹模板
    public GameObject objBulletModel;

    //发射器朝向角度
    private float mAngelsTarget = 90;

    private void Update()
    {
        //旋转发射器朝向 
        objEjector.transform.localRotation = Quaternion.Slerp
            (objEjector.transform.rotation, Quaternion.Euler(0, 0, mAngelsTarget), 10 * Time.deltaTime);
    }

    /// <summary>
    /// 开始发射
    /// </summary>
    /// <param name="launchType"></param>
    public void StartLaunch(LaunchTypeEnum launchType, Vector3 targetPositon, float launchSpeed)
    {
        animEjector.SetTrigger("Launch");
        switch (launchType)
        {
            case LaunchTypeEnum.Single:
                LaunchSingle(targetPositon, launchSpeed);
                break;
            case LaunchTypeEnum.Double:
                LaunchDouble(targetPositon, launchSpeed);
                break;
            case LaunchTypeEnum.Triple:
                LaunchTriple(targetPositon, launchSpeed);
                break;
        }
    }

    /// <summary>
    ///  单发模式
    /// </summary>
    /// <param name="targetPositon">目标位置</param>
    /// <param name="shotSpeed">发射速度</param>
    public void LaunchSingle(Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(targetPositon, shotSpeed);
    }

    /// <summary>
    /// 双发
    /// </summary>
    /// <param name="targetPositon"></param>
    /// <param name="shotSpeed"></param>
    public void LaunchDouble(Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(targetPositon + new Vector3(2, 2, 1), shotSpeed);
        CreateBullet(targetPositon + new Vector3(-2, -2, 1), shotSpeed);
    }

    /// <summary>
    /// 三连发
    /// </summary>
    /// <param name="targetPositon"></param>
    /// <param name="shotSpeed"></param>
    public void LaunchTriple(Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(targetPositon + new Vector3(4, 4, 1), shotSpeed);
        CreateBullet(targetPositon + new Vector3(-4, -4, 1), shotSpeed);
        CreateBullet(targetPositon, shotSpeed);
    }

    /// <summary>
    ///  创建子弹
    /// </summary>
    /// <param name="launchPositon"></param>
    /// <param name="force"></param>
    private void CreateBullet(Vector3 targetPositon, float force)
    {
        //设置发射器朝向角度
        mAngelsTarget = VectorUtil.GetAngle(objEjector.transform.position, targetPositon);
        //创建子弹
        GameObject objBullet = Instantiate(objBulletModel, objBulletContainer.transform);
        objBullet.SetActive(true);
        objBullet.transform.position = objEjector.transform.position;
        MiniGameBarrageBulletCpt bulletCpt = objBullet.GetComponent<MiniGameBarrageBulletCpt>();
        //发射子弹
        bulletCpt.LaunchBullet(targetPositon, force);
    }
}