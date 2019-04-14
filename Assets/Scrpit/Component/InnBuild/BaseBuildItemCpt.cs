using UnityEngine;
using UnityEditor;

public class BaseBuildItemCpt : BaseMonoBehaviour
{
    //建筑ID
    public long buildId;
    public Direction2DEnum direction = Direction2DEnum.Left;

    public GameObject leftObj;
    public GameObject rightObj;
    public GameObject upObj;
    public GameObject downOj;

    /// <summary>
    /// 逆时针旋转
    /// </summary>
    public void RotateLet()
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
    public void RotateRight()
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

    public virtual void SetDirection(Direction2DEnum direction)
    {
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