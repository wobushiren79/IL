﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseBuildItemCpt : BaseMonoBehaviour
{
    //建筑数据
    public BuildItemBean buildItemData;
    public ItemBean itemData;

    public Direction2DEnum direction = Direction2DEnum.Left;

    //不同方向的贴图
    public Sprite spLeft;
    public Sprite spRight;
    public Sprite spDown;
    public Sprite spUp;

    //用户使用面朝方向
    public int leftUserFace = 2;
    public int rightUserFace = 2;
    public int upUserFace = 2;
    public int downUserFace = 2;

    //体积
    public GameObject objBox;
    //占地
    public GameObject objBuildPositionList;

    //主要的建筑物图标
    public SpriteRenderer srMainBuild;
    //阴影
    public SpriteRenderer srShadow;
    //是否能旋转
    public int rotatedFace = 1;



    /// <summary>
    /// 设置建筑数据
    /// </summary>
    /// <param name="buildItemData"></param>
    public virtual void SetData(BuildItemBean buildItemData, ItemBean itemData = null)
    {
        this.buildItemData = buildItemData;
        this.itemData = itemData;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="buildItemData"></param>
    /// <param name="spIcon"></param>
    public virtual void SetData(BuildItemBean buildItemData, Sprite spIcon, ItemBean itemData = null)
    {
        SetData(buildItemData, itemData);
        SetSprite(spIcon, spIcon, spIcon, spIcon);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="buildItemData"></param>
    /// <param name="spLeft"></param>
    /// <param name="spRight"></param>
    /// <param name="spDown"></param>
    /// <param name="spUp"></param>
    public void SetData(BuildItemBean buildItemData, Sprite spLeft, Sprite spRight, Sprite spDown, Sprite spUp, ItemBean itemData = null)
    {
        SetData(buildItemData, itemData);
        SetSprite(spLeft, spRight, spDown, spUp);
    }

    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="spLeft"></param>
    /// <param name="spRight"></param>
    /// <param name="spDown"></param>
    /// <param name="spUp"></param>
    public void SetSprite(Sprite spLeft, Sprite spRight, Sprite spDown, Sprite spUp)
    {
        if (srMainBuild != null)
            srMainBuild.sprite = spLeft;
        if (srShadow != null && spLeft != null)
            srShadow.sprite = spLeft;
        this.spLeft = spLeft;
        this.spRight = spRight;
        this.spDown = spDown;
        this.spUp = spUp;
    }

    public void SetSprite()
    {
        Sprite spDirection = null;
        switch (direction)
        {
            case Direction2DEnum.Left:
                spDirection = spLeft;
                break;
            case Direction2DEnum.Right:
                spDirection = spRight;
                break;
            case Direction2DEnum.UP:
                spDirection = spUp;
                break;
            case Direction2DEnum.Down:
                spDirection = spDown;
                break;
        }
        if (srMainBuild != null)
            srMainBuild.sprite = spDirection;
        if (srShadow != null && spLeft != null)
            srShadow.sprite = spDirection;
    }

    /// <summary>
    /// 获取建筑位置
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetBuildPosition()
    {
        List<Vector3> listPosition = new List<Vector3>();
        List<Transform> buildPositionList = CptUtil.GetAllCptInChildrenByContainName<Transform>(objBuildPositionList, "Position_");
        foreach (Transform itemTF in buildPositionList)
        {
            Vector3 position = transform.InverseTransformPoint(itemTF.position);
            listPosition.Add(position);
        }
        return listPosition;
    }

    /// <summary>
    /// 获取建筑物的世界坐标
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetBuildWorldPosition()
    {
        List<Vector3> worldPositionList = new List<Vector3>();
        List<Transform> buildPositionList = CptUtil.GetAllCptInChildrenByContainName<Transform>(objBuildPositionList, "Position_");
        foreach (Transform itemTF in buildPositionList)
        {
            worldPositionList.Add(itemTF.position);
        }
        return worldPositionList;
    }

    /// <summary>
    /// 获取使用者朝向
    /// </summary>
    /// <returns></returns>
    public int GetUserFace()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftUserFace;
            case Direction2DEnum.Right:
                return rightUserFace;
            case Direction2DEnum.UP:
                return upUserFace;
            case Direction2DEnum.Down:
                return downUserFace;
        }
        return 2;
    }

    /// <summary>
    /// 逆时针旋转
    /// </summary>
    public virtual void RotateLet()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                SetDirection(Direction2DEnum.Down);
                break;
            case Direction2DEnum.Right:
                SetDirection(Direction2DEnum.UP);
                break;
            case Direction2DEnum.UP:
                SetDirection(Direction2DEnum.Left);
                break;
            case Direction2DEnum.Down:
                SetDirection(Direction2DEnum.Right);
                break;
        }
    }

    /// <summary>
    /// 顺时针旋转
    /// </summary>
    public virtual void RotateRight()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                SetDirection(Direction2DEnum.UP);
                break;
            case Direction2DEnum.Right:
                SetDirection(Direction2DEnum.Down);
                break;
            case Direction2DEnum.UP:
                SetDirection(Direction2DEnum.Right);
                break;
            case Direction2DEnum.Down:
                SetDirection(Direction2DEnum.Left);
                break;
        }
    }

    public virtual void SetDirection(int direction)
    {
        SetDirection((Direction2DEnum)direction);
    }

    public Direction2DEnum GetDirection()
    {
        return direction;
    } 

    public virtual void SetDirection(Direction2DEnum direction)
    {
        if (rotatedFace != 4)
        {
            return;
        }
        this.direction = direction;
        Sprite spShadow = null;
        Sprite spMianBuild = null;
        switch (direction)
        {
            case Direction2DEnum.Left:
                spShadow = spLeft;
                spMianBuild = spLeft;
                objBox.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case Direction2DEnum.Right:
                spShadow = spRight;
                spMianBuild = spRight;
                objBox.transform.localEulerAngles = new Vector3(0, 0, 180);
                break;
            case Direction2DEnum.Down:
                spShadow = spDown;
                spMianBuild = spDown;
                objBox.transform.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Direction2DEnum.UP:
                spShadow = spUp;
                spMianBuild = spUp;
                objBox.transform.localEulerAngles = new Vector3(0, 0, -90);
                break;
        }

        if (srMainBuild != null)
            srMainBuild.sprite = spMianBuild;
        if (srShadow != null && spShadow != null)
            srShadow.sprite = spShadow;
    }
}