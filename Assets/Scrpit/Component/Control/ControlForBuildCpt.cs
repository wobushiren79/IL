using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.Collections.Generic;

public class ControlForBuildCpt : BaseControl
{
    public CharacterMoveCpt cameraMove;
    public GameObject buildContainer;
    //屏幕
    public RectTransform screenRTF;
    //数据管理
    public InnBuildManager innBuildManager;
    public GameDataManager gameDataManager;
    //地形模型
    public GameObject listBuildSpaceContent;
    public GameObject itemBuildSpaceModel;
    //图标
    public Sprite spHasBuild;
    public Sprite spNoBuild;

    public ToastView toastView;

    public GameObject buildItemObj;
    public BaseBuildItemCpt buildItemCpt;
    public List<SpriteRenderer> listBuildSpaceSR = new List<SpriteRenderer>();


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
            buildItemObj.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            //真实的建设地点
            int xTemp = mousePosition.x >= 0 ? (int)(mousePosition.x) + 1 : (int)(mousePosition.x);
            int yTemp = mousePosition.y >= 0 ? (int)mousePosition.y : (int)mousePosition.y - 1;
            Vector3Int truePosition = new Vector3Int(xTemp, yTemp, 0);
            //设置提示区域
            listBuildSpaceContent.transform.position = truePosition;
            //设置区域是否有障碍物
            List<InnResBean> listFurniture = gameDataManager.gameData.GetInnBuildData().GetFurnitureList();
            //是否能建造
            bool canBuild = true;
            foreach(SpriteRenderer itemRenderer in listBuildSpaceSR)
            {
                bool hasBuild = false;
                Vector3 srPosition = itemRenderer.transform.position;
                foreach (InnResBean itemData in listFurniture)
                {
                    foreach (Vector3Bean itemPosition in itemData.GetListPosition())
                    {
                        if(itemPosition.x == srPosition.x && itemPosition.y == srPosition.y)
                        {
                            hasBuild = true;
                            break;
                        }
                    }
                    if (hasBuild)
                        break;
                }
                if (hasBuild)
                {
                    itemRenderer.sprite = spHasBuild;
                    canBuild = false;
                }
                else
                {
                    itemRenderer.sprite = spNoBuild;
                }
            }


            if (Input.GetButtonDown("Confirm"))
            {
                //能建造
                if (canBuild)
                {
                    transform.position = buildItemObj.transform.position;
                    buildItemObj.transform.position = truePosition;
                    //获取提示区域所占点
                    List<Vector3> buildPosition = new List<Vector3>();
                    for (int i = 0; i < listBuildSpaceSR.Count; i++)
                    {
                        buildPosition.Add(listBuildSpaceSR[i].transform.position);
                    }
                    InnResBean addData = new InnResBean(buildItemCpt.buildId, truePosition, buildPosition, buildItemCpt.direction);
                    gameDataManager.gameData.GetInnBuildData().AddFurniture(addData);
                    ClearBuild();
                }
                //不能建造
                else
                {
                    toastView.ToastHint("不能建造");
                }

            }
            if (Input.GetButtonDown("Rotate_Left"))
            {
                buildItemCpt.RotateLet();
                BuildSpace();
            }
            if (Input.GetButtonDown("Rotate_Right"))
            {
                buildItemCpt.RotateRight();
                BuildSpace();
            }
        }

    }

    public void SetBuildItem(long id)
    {
        DestoryBuild();
        buildItemObj = innBuildManager.GetFurnitureObjById(id);
        if (buildItemObj == null)
            return;
        buildItemObj.transform.SetParent(buildContainer.transform);
        buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();
        //屏幕坐标转换为UI坐标
        Vector3 mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out mousePosition);
        listBuildSpaceContent.transform.position = mousePosition;
        BuildSpace();
    }

    /// <summary>
    /// 修建建筑占地提示
    /// </summary>
    public void BuildSpace()
    {
        CptUtil.RemoveChildsByActive(listBuildSpaceContent.transform);
        listBuildSpaceSR.Clear();
        if (buildItemCpt == null)
            return;

        List<Vector3> listPosition = buildItemCpt.GetBuildPosition();
        for (int i = 0; i < listPosition.Count; i++)
        {
            GameObject buildSpaceObj = Instantiate(itemBuildSpaceModel, itemBuildSpaceModel.transform);
            buildSpaceObj.transform.SetParent(listBuildSpaceContent.transform);
            buildSpaceObj.transform.localPosition = listPosition[i];
            buildSpaceObj.SetActive(true);
            listBuildSpaceSR.Add(buildSpaceObj.GetComponent<SpriteRenderer>());
        }
    }

    public void DestoryBuild()
    {
        Destroy(buildItemObj);
        buildItemObj = null;
        buildItemCpt = null;
        CptUtil.RemoveChildsByActive(listBuildSpaceContent.transform);
        listBuildSpaceSR.Clear();
    }

    public void ClearBuild()
    {
        buildItemObj = null;
        buildItemCpt = null;
        CptUtil.RemoveChildsByActive(listBuildSpaceContent.transform);
        listBuildSpaceSR.Clear();
    }
}