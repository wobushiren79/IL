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
    private bool mIsLaunch = false;
    private float mRotateSpeed = 1;
    private float mLaunchSpeed = 0.1f;
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

    }


    private void FixedUpdate()
    {
        // 
        if (mIsRotating)
        {
            if (srHook.transform.eulerAngles.z > 60 && srHook.transform.eulerAngles.z <= 180)
                mRotateSpeed = -1;
            else if (srHook.transform.eulerAngles.z < 300 && srHook.transform.eulerAngles.z > 180)
                mRotateSpeed = 1;
            srHook.transform.Rotate(new Vector3(0, 0, mRotateSpeed));
        }
        if (mIsLaunch)
        {
            float angles = srHook.transform.eulerAngles.z;
            float positionY = Mathf.Cos(Mathf.PI * angles / 180) ;
            float positionX = -Mathf.Sin(Mathf.PI * angles / 180) ;
            srHook.transform.Translate(positionX* mLaunchSpeed, positionY* mLaunchSpeed, 0, Space.World);
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
        if (mIsLaunch)
            return;
        //停止旋转
        StopRotate();
        mIsLaunch = true;
    }
}
