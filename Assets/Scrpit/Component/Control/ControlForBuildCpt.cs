using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ControlForBuildCpt : BaseControl
{
    public CharacterMoveCpt cameraMove;
    public GameObject buildContainer;
    //屏幕
    public RectTransform screenRTF;
    //数据管理
    public GameDataManager gameDataManager;
    //建造者
    public InnFurnitureBuilder innFurnitureBuilder;
    public InnWallBuilder innWallBuilder;
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
        cameraMove.minMoveX = -1;
        cameraMove.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 1;
        cameraMove.minMoveY = -1;
        cameraMove.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 1;
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
            foreach (SpriteRenderer itemRenderer in listBuildSpaceSR)
            {
                bool hasBuild = false;
                Vector3 srPosition = itemRenderer.transform.position;
                foreach (InnResBean itemData in listFurniture)
                {
                    foreach (Vector3Bean itemPosition in itemData.GetListPosition())
                    {
                        //判断当前位置是否有物体
                        if (itemPosition.x == srPosition.x && itemPosition.y == srPosition.y)
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
                    if (buildItemCpt.buildId > 90000 && buildItemCpt.buildId < 100000)
                    {
                        // 门的单独处理
                        if (srPosition.y == 0 && srPosition.x > 2 && srPosition.x < gameDataManager.gameData.GetInnBuildData().innWidth-1)
                        {
                            itemRenderer.sprite = spNoBuild;
                        }
                        else
                        {
                            itemRenderer.sprite = spHasBuild;
                            canBuild = false;
                        }
                    }
                    else
                    {
                        //判断是否超出可修建范围
                        if (srPosition.x > 1 && srPosition.x < gameDataManager.gameData.GetInnBuildData().innWidth
                                 && srPosition.y > 0 && srPosition.y < gameDataManager.gameData.GetInnBuildData().innHeight - 1)
                        {
                            itemRenderer.sprite = spNoBuild;
                        }
                        else
                        {
                            itemRenderer.sprite = spHasBuild;
                            canBuild = false;
                        }
                    }


                }
            }


            if (Input.GetButtonDown("Confirm"))
            {
                //如果在不在UI范围内才处理
                if (UnityEngine.Screen.width - Input.mousePosition.x - 300 > 0)
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
                        //如果是门。需要重置一下墙体
                        if (buildItemCpt.buildId > 90000 && buildItemCpt.buildId < 100000)
                        {
                            gameDataManager.gameData.GetInnBuildData().InitWall();
                            innWallBuilder.StartBuild();
                        }
                        ClearBuild();
                    }
                    //不能建造
                    else
                    {
                        toastView.ToastHint("不能建造");
                    }
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
        buildItemObj = innFurnitureBuilder.BuildFurniture(id);
        if (buildItemObj == null)
            return;
        buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();
        //屏幕坐标转换为UI坐标
        Vector3 mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out mousePosition);
        listBuildSpaceContent.transform.position = mousePosition;
        buildItemCpt.transform.position = mousePosition;
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