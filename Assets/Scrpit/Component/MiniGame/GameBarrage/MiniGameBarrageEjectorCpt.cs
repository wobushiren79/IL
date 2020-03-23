using UnityEngine;
using UnityEditor;

public class MiniGameBarrageEjectorCpt : BaseMonoBehaviour
{
    protected AudioHandler audioHandler;
    protected MiniGameBarrageBuilder miniGameBarrageBuilder;

    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        miniGameBarrageBuilder = FindInChildren < MiniGameBarrageBuilder > (ImportantTypeEnum.MiniGameBuilder);
    }

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
    public void StartLaunch(LaunchTypeEnum launchType, MiniGameBarrageBulletTypeEnum bulletType, Vector3 targetPositon, float launchSpeed)
    {
        audioHandler.PlaySound(AudioSoundEnum.Shot);
        animEjector.SetTrigger("Launch");
        switch (launchType)
        {
            case LaunchTypeEnum.Single:
                LaunchSingle(bulletType, targetPositon, launchSpeed);
                break;
            case LaunchTypeEnum.Double:
                LaunchDouble(bulletType, targetPositon, launchSpeed);
                break;
            case LaunchTypeEnum.Triple:
                LaunchTriple(bulletType, targetPositon, launchSpeed);
                break;
        }
    }

    /// <summary>
    ///  单发模式
    /// </summary>
    /// <param name="targetPositon">目标位置</param>
    /// <param name="shotSpeed">发射速度</param>
    public void LaunchSingle(MiniGameBarrageBulletTypeEnum bulletType, Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(bulletType, targetPositon, shotSpeed);
    }

    /// <summary>
    /// 双发
    /// </summary>
    /// <param name="targetPositon"></param>
    /// <param name="shotSpeed"></param>
    public void LaunchDouble(MiniGameBarrageBulletTypeEnum bulletType, Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(bulletType, targetPositon + new Vector3(2, 2, 1), shotSpeed);
        CreateBullet(bulletType, targetPositon + new Vector3(-2, -2, 1), shotSpeed);
    }

    /// <summary>
    /// 三连发
    /// </summary>
    /// <param name="targetPositon"></param>
    /// <param name="shotSpeed"></param>
    public void LaunchTriple(MiniGameBarrageBulletTypeEnum bulletType, Vector3 targetPositon, float shotSpeed)
    {
        CreateBullet(bulletType, targetPositon + new Vector3(4, 4, 1), shotSpeed);
        CreateBullet(bulletType, targetPositon + new Vector3(-4, -4, 1), shotSpeed);
        CreateBullet(bulletType, targetPositon, shotSpeed);
    }

    /// <summary>
    ///  创建子弹
    /// </summary>
    /// <param name="launchPositon"></param>
    /// <param name="force"></param>
    private void CreateBullet(MiniGameBarrageBulletTypeEnum bulletType, Vector3 targetPositon, float force)
    {
        Sprite spBullet = null;
        RuntimeAnimatorController animatorController = null;
        switch (bulletType)
        {
            case MiniGameBarrageBulletTypeEnum.Stone:
                spBullet = miniGameBarrageBuilder.spStone;
                animatorController = miniGameBarrageBuilder.animatorControllerForStone;
                break;
            case MiniGameBarrageBulletTypeEnum.Arrow:
                spBullet = miniGameBarrageBuilder.spArrow;
                animatorController = miniGameBarrageBuilder.animatorControllerForArrow;
                break;
        }
        //设置发射器朝向角度
        mAngelsTarget = VectorUtil.GetAngle(objEjector.transform.position, targetPositon);
        //创建子弹
        GameObject objBullet = Instantiate(objBulletModel, objBulletContainer.transform);
        objBullet.SetActive(true);
        objBullet.transform.position = objEjector.transform.position;
        MiniGameBarrageBulletCpt bulletCpt = objBullet.GetComponent<MiniGameBarrageBulletCpt>();
        bulletCpt.SetBulletData(bulletType, spBullet, animatorController);
        //发射子弹
        bulletCpt.LaunchBullet(targetPositon, force);
    }
}