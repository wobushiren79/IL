using UnityEngine;
using UnityEditor;
using Cinemachine;

public class ControlForBuildCpt : BaseControl
{
    public CharacterMoveCpt cameraMove;
    public GameObject buildContainer;
    //屏幕
    public RectTransform screenRTF;
    //数据管理
    public InnBuildManager innBuildManager;


    public GameObject buildItemObj;
    public BaseBuildItemCpt buildItemCpt;
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
        if (Input.GetButtonDown("Cancel"))
        {
            DestoryBuild();
        }
        if (buildItemObj != null)
        {
            //屏幕坐标转换为UI坐标
            Vector3 mousePosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out mousePosition);
            buildItemObj.transform.position = mousePosition;
            if (Input.GetButtonDown("Confirm"))
            {
                transform.position = mousePosition;
                buildItemObj.transform.position = new Vector3Int((int)mousePosition.x+1,(int)mousePosition.y,0);
                ClearBuild();
            }
            if (Input.GetButtonDown("Rotate_Left"))
            {
                buildItemCpt.RotateLet();
            }
            if (Input.GetButtonDown("Rotate_Right"))
            {
                buildItemCpt.RotateRight();
            }
        }

    }

    public void SetBuildItem(long id)
    {
        DestoryBuild();
        buildItemObj = innBuildManager.GetFurnitureObjById(id);
        buildItemObj.transform.SetParent(buildContainer.transform);
        buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();
    }

    public void DestoryBuild()
    {
        Destroy(buildItemObj);
        buildItemObj = null;
        buildItemCpt = null;
    }

    public void ClearBuild()
    {
        buildItemObj = null;
        buildItemCpt = null;
    }
}