using UnityEngine;
using UnityEditor;
using Cinemachine;

public class ControlForBuildCpt : BaseControl
{
    public CharacterMoveCpt cameraMove;
    public GameObject buildItemObj;
    //屏幕
    public RectTransform screenRTF;

    public override void StartControl()
    {
        base.StartControl();
        cameraFollowObj.transform.position = new Vector3(5, 5);
    }

    private void FixedUpdate()
    {
        if (cameraMove == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if (hMove == 0 && vMove == 0)
        {

            cameraMove.Stop();
        }
        else
        {
            cameraMove.Move(hMove, vMove);
        }
        if (Input.GetButtonDown("Confirm"))
        {

        }
        if (Input.GetButtonDown("Cancel"))
        {
          //  buildItemObj = null;
        }
        //屏幕坐标转换为UI坐标
        //Vector3 mousePosition;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out mousePosition);
        //buildItemObj.transform.position = mousePosition;
    }
}