using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[ExecuteInEditMode]
public class MiniGameAccountEjectorCpt : MonoBehaviour
{
    public SpriteRenderer srHook;
    public SpriteRenderer srHookPlatform;
    public SpriteRenderer srRope;

    private bool mIsRotating = false;
    private int mLaunchStatus = 0;//0等待发射，1发射中，2回收中
    private float mRotateSpeed = 45f;//每秒
    private float mLaunchSpeed = 5f;//每秒

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
                mRotateSpeed = -mRotateSpeed;
            else if (srHook.transform.eulerAngles.z < 300 && srHook.transform.eulerAngles.z > 180)
                mRotateSpeed = -mRotateSpeed;
            srHook.transform.Rotate(new Vector3(0, 0, mRotateSpeed * Time.deltaTime));
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
        if (mLaunchStatus==1|| mLaunchStatus==2)
            return;
        //停止旋转
        StopRotate();
        mLaunchStatus = 1;
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Recycle()
    {
        mLaunchStatus = 2;
        srHook.transform.DOLocalMove(new Vector3(0, 0, 0), 3).OnComplete(delegate(){
            Settlement();
        });
    }

    /// <summary>
    /// 结算
    /// </summary>
    public void Settlement()
    {
        mLaunchStatus = 0;
        StartRotate();
    }
}
