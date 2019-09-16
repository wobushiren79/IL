using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseBuildItemCpt : BaseMonoBehaviour
{
    //建筑ID
    public long buildId;
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
    public bool canRotated = true;

    /// <summary>
    /// 获取建筑位置
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetBuildPosition()
    {
        List<Vector3> listPosition = new List<Vector3>();
        List<Transform> buildPositionList= CptUtil.GetAllCptInChildrenByContainName<Transform>(objBuildPositionList,"Position_");
        foreach (Transform itemTF in buildPositionList)
        {
            Vector3 position= transform.InverseTransformPoint(itemTF.position);
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

    public virtual void SetDirection(Direction2DEnum direction)
    {
        if (!canRotated)
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
                objBox.transform.localEulerAngles = new Vector3(0,0,180);
                break;
            case Direction2DEnum.Down:
                spShadow = spDown;
                spMianBuild = spDown;
                objBox.transform.localEulerAngles = new Vector3(0,0,90);
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