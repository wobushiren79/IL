using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseBuildItemCpt : BaseMonoBehaviour
{
    //建筑ID
    public long buildId;
    public Direction2DEnum direction = Direction2DEnum.Left;

    public GameObject leftObj;
    public List<Vector3> leftPosition;

    public GameObject rightObj;
    public List<Vector3> rightPosition;

    public GameObject upObj;
    public List<Vector3> upPosition;

    public GameObject downOj;
    public List<Vector3> downPosition;
    //是否能旋转
    public bool canRotated=true;
    /// <summary>
    /// 获取建筑位置
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetBuildPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftPosition;
            case Direction2DEnum.Right:
                return rightPosition;
            case Direction2DEnum.UP:
                return upPosition;
            case Direction2DEnum.Down:
                return downPosition;
        }
        return null;
    }

    public List<Vector3> GetBuildWorldPosition()
    {
        List<Vector3> worldPositionList = new List<Vector3>();
        foreach (Vector3 itemData in GetBuildPosition())
        {
           Vector3 worldPos = transform.TransformPoint(itemData);
            worldPositionList.Add(worldPos);
        }
        return worldPositionList;
    }
    /// <summary>
    /// 获取建筑物体
    /// </summary>
    /// <returns></returns>
    public GameObject GetBuilObj()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftObj;
            case Direction2DEnum.Right:
                return rightObj;
            case Direction2DEnum.UP:
                return upObj;
            case Direction2DEnum.Down:
                return downOj;
        }
        return null;
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

    public virtual void SetDirection(Direction2DEnum direction)
    {
        if (!canRotated)
        {
            return;
        }
        this.direction = direction;
        switch (direction)
        {
            case Direction2DEnum.Left:
                leftObj.SetActive(true);
                rightObj.SetActive(false);
                upObj.SetActive(false);
                downOj.SetActive(false);
                break;
            case Direction2DEnum.Right:
                leftObj.SetActive(false);
                rightObj.SetActive(true);
                upObj.SetActive(false);
                downOj.SetActive(false);
                break;
            case Direction2DEnum.UP:
                leftObj.SetActive(false);
                rightObj.SetActive(false);
                upObj.SetActive(true);
                downOj.SetActive(false);
                break;
            case Direction2DEnum.Down:
                leftObj.SetActive(false);
                rightObj.SetActive(false);
                upObj.SetActive(false);
                downOj.SetActive(true);
                break;
        }
    }
}