using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[ExecuteInEditMode]
public class MiniGameAccountEjectorCpt : BaseMonoBehaviour
{
    public SpriteRenderer srHook;
    public SpriteRenderer srHookPlatform;
    public SpriteRenderer srRope;

    private bool mIsRotating = false;
    private int mLaunchStatus = 0;//0等待发射，1发射中，2回收中
    private float mRotateSpeed = 45f;//每秒
    private float mLaunchSpeed = 5f;//每秒
    private float mRecycleSpeed = 1f;//每秒
    private float mRotatingDirection = 1;
    public ICallBack mCallBack;

    protected AudioHandler audioHandler;

    protected void Awake()
    {
        audioHandler = Find<AudioHandler>( ImportantTypeEnum.AudioHandler);
    }

    public void SetCallBack(ICallBack mCallBack)
    {
        this.mCallBack = mCallBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(float recycleSpeed)
    {
        mRecycleSpeed = recycleSpeed;
        if (mRecycleSpeed < 1)
        {
            mRecycleSpeed = 1;
        }
    }


    void Update()
    {
        if (srHook && srHookPlatform && srRope)
        {
            srRope.transform.position = (srHook.transform.position + srHookPlatform.transform.position) / 2f;
            float angle = VectorUtil.GetAngle(srHook.transform.position, srHookPlatform.transform.position);
            float localScale = Vector3.Distance(srHook.transform.position, srHookPlatform.transform.position);
            srRope.transform.localEulerAngles = new Vector3(0, 0, angle + 90);
            srRope.size = new Vector2(0.25f, localScale * 4);
        }
        if (mIsRotating)
        {
            if (srHook.transform.eulerAngles.z > 60 && srHook.transform.eulerAngles.z <= 180)
                mRotatingDirection = -1;
            else if (srHook.transform.eulerAngles.z < 300 && srHook.transform.eulerAngles.z > 180)
                mRotatingDirection = 1;
            srHook.transform.Rotate(new Vector3(0, 0, mRotatingDirection * mRotateSpeed * Time.deltaTime));
        }
        if (mLaunchStatus == 1)
        {
            float angles = srHook.transform.eulerAngles.z;
            float positionY = Mathf.Cos(Mathf.PI * angles / 180);
            float positionX = -Mathf.Sin(Mathf.PI * angles / 180);
            srHook.transform.Translate(positionX * mLaunchSpeed * Time.deltaTime, positionY * mLaunchSpeed * Time.deltaTime, 0, Space.World);
            //如果超过上限 则回收
            if (srRope.size.y >= 50)
            {
                Recycle();
            }
        }
    }

    /// <summary>
    /// 开始旋转
    /// </summary>
    public void StartRotate()
    {
        mIsRotating = true;
    }

    /// <summary>
    /// 停止旋转
    /// </summary>
    public void StopRotate()
    {
        mIsRotating = false;
    }

    /// <summary>
    /// 发射
    /// </summary>
    public void Launch()
    {
        //如果正在发射中 则不能再次发射
        if (mLaunchStatus == 1 || mLaunchStatus == 2)
            return;
        //停止旋转
        StopRotate();
        mLaunchStatus = 1;
        audioHandler.PlaySound( AudioSoundEnum.Shot);
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Recycle()
    {
        mLaunchStatus = 2;
        //计算回收距离
        float distance = Vector3.Distance(srHook.transform.position,transform.position);
        LogUtil.Log("distance:"+distance);
        srHook.transform
            .DOLocalMove(new Vector3(0, 0, 0), distance / mRecycleSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(
            delegate ()
                {
                    Settlement();
                });
    }

    /// <summary>
    /// 结算
    /// </summary>
    public void Settlement()
    {
        mLaunchStatus = 0;
        MiniGameAccountMoneyCpt[] moneyList = srHook.GetComponentsInChildren<MiniGameAccountMoneyCpt>();
        if (mCallBack != null)
        {
            int moneyL = 0;
            int moneyM = 0;
            int moneyS = 0;
            if (moneyList != null)
                foreach (MiniGameAccountMoneyCpt money in moneyList)
                {
                    if (money.moneyType == MoneyEnum.L)
                    {
                        moneyL += money.money;
                    }
                    else if (money.moneyType == MoneyEnum.M)
                    {
                        moneyM += money.money;
                    }
                    else if (money.moneyType == MoneyEnum.S)
                    {
                        moneyS += money.money;
                    }
                }
            mCallBack.AccountEjectorSettlement(this, moneyL, moneyM, moneyS);
            //结算完毕删除
            if (moneyList != null)
            {
                foreach (MiniGameAccountMoneyCpt money in moneyList)
                {
                    Destroy(money.gameObject);
                }
            }
        }
        StartRotate();
    }

    public interface ICallBack
    {
        void AccountEjectorSettlement(MiniGameAccountEjectorCpt ejector, int moneyL, int moneyM, int moneyS);
    }
}
