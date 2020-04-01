using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;

public class ControlForBuildCpt : BaseControl
{
    //屏幕
    public RectTransform screenRTF;
    //镜头对象
    public CharacterMoveCpt cameraMove;
    //建筑临时存放容器
    public GameObject buildItemTempContainer;

    public UIGameBuild uiGameBuild;

    //地形模型
    public GameObject listBuildSpaceContent;
    public GameObject itemBuildSpaceModel;
    //图标
    public Sprite spRed;
    public Sprite spGreen;
    public Sprite spYellow;

    //创建的临时建筑
    public BaseBuildItemCpt buildItemCpt;

    //提示区域
    public List<SpriteRenderer> listBuildSpaceSR = new List<SpriteRenderer>();
    //已有建筑区域
    public HashSet<Vector3> listBuildingExist = new HashSet<Vector3>();
    public HashSet<Vector3> listBuildingExistForWall = new HashSet<Vector3>();
    //数据管理
    protected GameDataManager gameDataManager;
    //提示框
    protected ToastManager toastManager;
    //建筑
    protected InnBuildManager innBuildManager;
    //建造者
    protected InnFurnitureBuilder innFurnitureBuilder;
    protected InnWallBuilder innWallBuilder;
    protected InnFloorBuilder innFloorBuilder;

    protected AudioHandler audioHandler;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);

        innFurnitureBuilder = Find<InnFurnitureBuilder>(ImportantTypeEnum.InnBuilder);
        innWallBuilder = Find<InnWallBuilder>(ImportantTypeEnum.InnBuilder);
        innFloorBuilder = Find<InnFloorBuilder>(ImportantTypeEnum.InnBuilder);

        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    private void Update()
    {
        //检测是否控制镜头移动
        HandleForCameraMove();
        //如果没有选中则不进行以下检测
        if (buildItemCpt == null)
            return;

        //检测是否控制建筑旋转
        HandleForBuildRotate();
        //检测是否控制取消展示建筑
        HandleForBuildCancel();
        //设置建筑坐标和鼠标坐标一样 并输出建造点
        HandleForBuildItemPosition(out Vector3 buildPosition);
        //是否能建造
        bool isCanBuild = CheckCanBuild();
        //检测是否控制建造
        HandleForBuildConfirm(isCanBuild, buildPosition);
    }

    public override void StartControl()
    {
        base.StartControl();
        //定义镜头的初始位置
        SetFollowPosition(new Vector3(5, 5));
        //定义镜头的移动范围
        //cameraMove.minMoveX = -1;
        //cameraMove.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 1;
        //cameraMove.minMoveY = -1;
        //cameraMove.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 1;
        //初始化建筑占地坐标
        InitBuildingExist();
    }

    /// <summary>
    /// 初始化已经存在的建筑位置
    /// </summary>
    public void InitBuildingExist()
    {
        listBuildingExist.Clear();
        List<InnResBean> listFurniture = gameDataManager.gameData.GetInnBuildData().GetFurnitureList();
        if (listFurniture != null)
        {
            foreach (InnResBean itemBuilding in listFurniture)
            {
                foreach (Vector3Bean itemPosition in itemBuilding.listPosition)
                {
                    listBuildingExist.Add(TypeConversionUtil.Vector3BeanToVector3(itemPosition));
                }
            }
        }
        listBuildingExistForWall.Clear();
        List<InnResBean> listWall = gameDataManager.gameData.GetInnBuildData().GetWallList();
        if (listWall != null)
        {
            foreach (InnResBean itemBuilding in listWall)
            {
                //因为保存的tile是从0,0坐标开始的。而实际判断是以1,0开始所以需要+1
                listBuildingExistForWall.Add(itemBuilding.GetStartPosition() + new Vector3(1, 0, 0));
            }
        }
    }

    /// <summary>
    /// 启用拆除模式
    /// </summary>
    public void SetDismantleMode()
    {
        ShowBuildItem(-1);
    }

    /// <summary>
    /// 根据ID展示建筑
    /// </summary>
    /// <param name="id"></param>
    public void ShowBuildItem(long id)
    {
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        //先删除原有可能已经展示的建筑
        ClearBuildItem();
        //建造建筑
        GameObject buildItemObj = innFurnitureBuilder.BuildFurniture(id);
        //物体先在建筑控件上显示
        buildItemObj.transform.parent = buildItemTempContainer.transform;
        if (buildItemObj == null)
            return;
        buildItemCpt = buildItemObj.GetComponent<BaseBuildItemCpt>();

        //设置展示建筑和鼠标指针一样
        //Vector3 mousePosition = SetPositionWithMouse(buildItemCpt.transform);

        //设置展示位置提示坐标和鼠标指针一样
        //listBuildSpaceContent.transform.position = mousePosition;
        //创建展示位置提示
        ShowBuildSpace();
    }

    /// <summary>
    /// 修建建筑占地提示
    /// </summary>
    public void ShowBuildSpace()
    {
        //清空原有的占地提示
        CptUtil.RemoveChildsByActive(listBuildSpaceContent.transform);
        listBuildSpaceSR.Clear();
        if (buildItemCpt == null)
            return;
        List<Vector3> listPosition = buildItemCpt.GetBuildPosition();
        for (int i = 0; i < listPosition.Count; i++)
        {
            //创建单个的占地提示
            GameObject buildSpaceObj = Instantiate(itemBuildSpaceModel, listBuildSpaceContent.transform);
            buildSpaceObj.transform.localPosition = listPosition[i];
            buildSpaceObj.SetActive(true);
            SpriteRenderer srSpace = buildSpaceObj.GetComponent<SpriteRenderer>();
            listBuildSpaceSR.Add(srSpace);
        }
    }

    /// <summary>
    /// 删除选中的建筑
    /// </summary>
    public void ClearBuildItem()
    {
        if (buildItemCpt != null)
            Destroy(buildItemCpt.gameObject);
        ClearSelectBuildItem();
    }

    /// <summary>
    /// 清空选中的建筑
    /// </summary>
    public void ClearSelectBuildItem()
    {
        buildItemCpt = null;
        CptUtil.RemoveChildsByActive(listBuildSpaceContent.transform);
        listBuildSpaceSR.Clear();
    }

    /// <summary>
    /// 处理-镜头移动
    /// </summary>
    protected void HandleForCameraMove()
    {
        if (cameraMove == null)
            return;
        float hMove = Input.GetAxis(InputInfo.Horizontal);
        float vMove = Input.GetAxis(InputInfo.Vertical);
        if (hMove == 0 && vMove == 0)
        {
            //cameraMove.StopAnim();
        }
        else
        {
            cameraMove.Move(hMove, vMove);
        }
    }

    /// <summary>
    ///  处理-建筑旋转
    /// </summary>
    protected void HandleForBuildRotate()
    {
        if (buildItemCpt == null)
            return;
        //监控是否左旋
        if (Input.GetButtonDown(InputInfo.Rotate_Left))
        {
            buildItemCpt.RotateLet();
            ShowBuildSpace();
        }
        //监控是否右旋
        if (Input.GetButtonDown(InputInfo.Rotate_Right))
        {
            buildItemCpt.RotateRight();
            ShowBuildSpace();
        }
    }

    /// <summary>
    /// 处理-检测是否取消展示建筑
    /// </summary>
    protected void HandleForBuildCancel()
    {
        if (Input.GetButtonDown(InputInfo.Cancel))
        {
            ClearBuildItem();
        }
    }

    /// <summary>
    /// 处理-建筑物品的位置
    /// </summary>
    protected void HandleForBuildItemPosition(out Vector3 buildPosition)
    {
        //屏幕坐标转换为UI坐标(获取鼠标位置)
        RectTransformUtility.ScreenPointToWorldPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out Vector3 mousePosition);
        if (buildItemCpt == null)
        {
            buildPosition = mousePosition;
            return;
        }
        //设置建造物品坐标和鼠标位置一样
        buildItemCpt.transform.position = mousePosition;

        //计算建筑点(小于0 取整需要进1位)
        int xTemp = mousePosition.x >= 0 ? (int)(mousePosition.x) + 1 : (int)(mousePosition.x);
        int yTemp = mousePosition.y >= 0 ? (int)mousePosition.y : (int)mousePosition.y - 1;
        buildPosition = new Vector3Int(xTemp, yTemp, 0);
        //设置提示区域坐标和建造点一样
        listBuildSpaceContent.transform.position = buildPosition;
    }

    /// <summary>
    /// 检测是否操作建造
    /// </summary>
    /// <param name="isCanBuild"></param>
    protected void HandleForBuildConfirm(bool isCanBuild, Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        if (Input.GetButtonDown(InputInfo.Confirm))
        {
            //防止误触右边的UI
            if (UnityEngine.Screen.width - Input.mousePosition.x - 400 < 0)
                return;
            //能建造
            if (isCanBuild)
            {
                //镜头正对建造点
                //SetFollowPosition(buildPosition);
                //建筑物位置设置
                buildItemCpt.transform.position = buildPosition + new Vector3(-0.5f, 0.5f);
                //获取提示区域所占点
                List<Vector3> listBuildPosition = new List<Vector3>();
                for (int i = 0; i < listBuildSpaceSR.Count; i++)
                {
                    //精度修正
                    listBuildPosition.Add(Vector3Int.CeilToInt(listBuildSpaceSR[i].transform.position));
                }

                //如果是拆除
                if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Other && buildItemCpt.buildItemData.id == -1)
                {
                    BuildItemForDismantle(buildPosition, listBuildPosition[0]);
                }
                //如果是地板
                else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Floor)
                {
                    BuildItemForFloor(buildPosition);
                }
                else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Wall)
                {
                    BuildItemForWall(buildPosition);
                }
                //如果是家具
                else
                {
                    BuildItemForFurniture(listBuildPosition);
                }
                //刷新一下建筑占地
                InitBuildingExist();
                //刷新UI
                //里面有移除选中功能
                uiGameBuild.RefreshUI();
            }
            //不能建造 相关提示
            else
            {
                //不能建造的原因
                if (buildItemCpt.buildItemData.id == -1)
                {
                    //如果是拆除模式提示
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1003));
                }
                else
                {
                    //如果是正常模式提示
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1002));
                }
            }
        }
    }

    /// <summary>
    /// 拆除
    /// </summary>
    /// <param name="buildPosition">家具的位置</param>
    /// <param name="sceneFurniturePosition">家具在场景中的位置</param>
    protected void BuildItemForDismantle(Vector3 buildPosition, Vector3 sceneFurniturePosition)
    {
        //获取拆除位置的家具数据
        InnBuildBean buildData = gameDataManager.gameData.GetInnBuildData();
        InnResBean itemFurnitureData = buildData.GetFurnitureByPosition(buildPosition );
        //因为在保存tile时坐标减过1 所以查询是也要-1
        InnResBean itemWallData = buildData.GetWallByPosition(buildPosition - new Vector3(1, 0, 0));
        //如果拆除的是家具
        if (itemFurnitureData != null)
        {
            BuildItemBean buildItemData = innBuildManager.GetBuildDataById(itemFurnitureData.id);
            //如果是最后一扇门则不能删除
            if (buildItemData.build_type == (int)BuildItemTypeEnum.Door && buildData.GetDoorList(innBuildManager).Count <= 1)
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1004));
            }
            else
            {
                //移除数据
                buildData.GetFurnitureList().Remove(itemFurnitureData);
                //移除场景中的建筑物
                innFurnitureBuilder.DestroyFurnitureByPosition(sceneFurniturePosition);
                //如果是门。需要重置一下墙体
                if (buildItemData.build_type == (int)BuildItemTypeEnum.Door)
                {
                    //gameDataManager.gameData.GetInnBuildData().InitWall(innBuildManager);
                    //innWallBuilder.StartBuild();
                }
                //背包里添加一个
                gameDataManager.gameData.AddBuildNumber(itemFurnitureData.id, 1);
            }
        }
        //如果拆除的是墙壁
        if (itemWallData != null)
        {
            //判断是否是最外的墙壁
            Vector3 startPosition = itemWallData.GetStartPosition();
            int innWidth = gameDataManager.gameData.GetInnBuildData().innWidth;
            int innHeight = gameDataManager.gameData.GetInnBuildData().innHeight;
            if ((startPosition.x==0|| startPosition.x== (innWidth - 1))||  startPosition.y == (innHeight - 1))
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1023));
            }
            else
            {
                //移除数据
                buildData.GetWallList().Remove(itemWallData);
                //移除场景中的建筑物
                innWallBuilder.ClearWall(Vector3Int.CeilToInt(startPosition));
                //背包里添加一个
                gameDataManager.gameData.AddBuildNumber(itemWallData.id, 1);
            }
        }
        //更新一下数据
        InitBuildingExist();
    }

    /// <summary>
    /// 建造家具
    /// </summary>
    /// <param name="listBuildPosition"></param>
    protected void BuildItemForFurniture(List<Vector3> listBuildPosition)
    {
        //将家具添加到家具容器中
        buildItemCpt.transform.parent = innFurnitureBuilder.buildContainer.transform;
        //增加一个家具
        InnResBean addData = new InnResBean(buildItemCpt.buildItemData.id, buildItemCpt.transform.position, listBuildPosition, buildItemCpt.direction);
        gameDataManager.gameData.GetInnBuildData().AddFurniture(addData);
        //如果是门。需要重置一下墙体
        if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Door)
        {
            //gameDataManager.gameData.GetInnBuildData().InitWall(innBuildManager);
            //innWallBuilder.StartBuild();
        }
        //背包里删除一个
        gameDataManager.gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //动画
        buildItemCpt.transform
            .DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f)
            .From()
            .SetEase(Ease.OutBack);
        ClearSelectBuildItem();
    }

    /// <summary>
    /// 建造地板
    /// </summary>
    /// <param name="buildPosition"></param>
    protected void BuildItemForFloor(Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        //获取该点地板数据
        InnResBean floorData = gameDataManager.gameData.GetInnBuildData().GetFloorByPosition(buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有地板则直接在建造点上建造
        if (floorData == null)
        {
            changePosition = Vector3Int.CeilToInt(buildPosition);
            floorData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameDataManager.gameData.GetInnBuildData().listFloor.Add(floorData);
        }
        //如果有地板则替换地板
        else
        {
            //背包里添加一个
            gameDataManager.gameData.AddBuildNumber(floorData.id, 1);
            changePosition = Vector3Int.CeilToInt(floorData.GetStartPosition());
            floorData.id = buildItemCpt.buildItemData.id;
        }
        innFloorBuilder.ChangeFloor(changePosition, buildItemCpt.buildItemData.tile_name);
        //背包里删除一个
        ItemBean itemData = gameDataManager.gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //如果没有了，则不能继续建造
        if (itemData.itemNumber <= 0)
        {
            ClearBuildItem();
        }
    }

    /// <summary>
    /// 建造墙壁
    /// </summary>
    /// <param name="buildPosition"></param>
    protected void BuildItemForWall(Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        //获取该点地板数据
        InnResBean wallData = gameDataManager.gameData.GetInnBuildData().GetWallByPosition(buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有墙壁则直接在建造点上建造
        if (wallData == null)
        {
            changePosition = Vector3Int.CeilToInt(buildPosition);
            wallData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameDataManager.gameData.GetInnBuildData().listWall.Add(wallData);
        }
        //如果有墙壁则替换墙壁
        else
        {
            //背包里添加一个
            gameDataManager.gameData.AddBuildNumber(wallData.id, 1);
            changePosition = Vector3Int.CeilToInt(wallData.GetStartPosition());
            wallData.id = buildItemCpt.buildItemData.id;
        }
        innWallBuilder.ChangeWall(changePosition, buildItemCpt.buildItemData.tile_name);
        //背包里删除一个
        ItemBean itemData = gameDataManager.gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //如果没有了，则不能继续建造
        if (itemData.itemNumber <= 0)
        {
            ClearBuildItem();
        }

    }

    /// <summary>
    /// 检测是否能建造
    /// </summary>
    /// <returns></returns>
    protected bool CheckCanBuild()
    {
        bool isCanBuild = true;

        foreach (SpriteRenderer itemRenderer in listBuildSpaceSR)
        {
            Vector3 srPosition = itemRenderer.transform.position;
            srPosition = Vector3Int.CeilToInt(srPosition);
            //检测是否超出建造范围
            bool isOutBuild = CheckOutOfRange(srPosition);
            if (isOutBuild)
            {
                itemRenderer.sprite = spRed;
                isCanBuild = false;
                continue;
            }
            //检测该点是否有建筑
            bool hasBuilding = CheckHasBuild(srPosition);
            //设置显示的提示颜色
            //如果是拆除模式
            if (buildItemCpt.buildItemData.id == -1)
            {
                if (hasBuilding)
                    itemRenderer.sprite = spYellow;
                else
                {
                    itemRenderer.sprite = spRed;
                    isCanBuild = false;
                }
            }
            else
            {
                if (hasBuilding)
                {
                    itemRenderer.sprite = spRed;
                    isCanBuild = false;
                }
                else
                    itemRenderer.sprite = spGreen;
            }
        }
        return isCanBuild;
    }

    /// <summary>
    /// 检测指定点是否有建筑
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    protected bool CheckHasBuild(Vector3 position)
    {
        BuildItemTypeEnum buildType = (BuildItemTypeEnum)buildItemCpt.buildItemData.build_type;
        if (buildType == BuildItemTypeEnum.Floor)
        {
            return false;
        }
        else if (buildType == BuildItemTypeEnum.Wall)
        {
            bool hasFurniture = listBuildingExist.Contains(position);
            if (hasFurniture)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            bool hasFurniture = listBuildingExist.Contains(position);
            bool hasWall = listBuildingExistForWall.Contains(position);
            if (hasFurniture || hasWall)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 检测是否超出建筑范围
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    protected bool CheckOutOfRange(Vector3 position)
    {
        BuildItemTypeEnum buildType = (BuildItemTypeEnum)buildItemCpt.buildItemData.build_type;
        int innWidth = gameDataManager.gameData.GetInnBuildData().innWidth;
        int innHeight = gameDataManager.gameData.GetInnBuildData().innHeight;
        if (buildType == BuildItemTypeEnum.Door)
        {
            //门的范围为y=0 x :2- width-1
            if (position.y == 0 && position.x > 2 && position.x < innWidth - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (buildType == BuildItemTypeEnum.Floor)
        {
            //地板
            if (position.x >= 1 && position.x <= innWidth && position.y >= 0 && position.y <= innHeight - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (buildType == BuildItemTypeEnum.Wall)
        {
            //墙体
            if (position.x >= 1 && position.x <= innWidth && position.y >= 0 && position.y <= innHeight - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (buildType == BuildItemTypeEnum.Other && buildItemCpt.buildItemData.id == -1)
        {
            //拆除模式
            return false;
        }
        else
        {
            //其他则在客栈范围内 判断是否超出可修建范围
            if (position.x > 1 && position.x < innWidth && position.y > 0 && position.y < innHeight - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}