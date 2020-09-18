using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

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

    //建筑层数
    public int buildLayer = 1;

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

    private void FixedUpdate()
    {
        HandleForZoom();
        HandleForMouseMove();
        //检测是否控制镜头移动
        HandleForCameraMove();
    }

    private void Update()
    {
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

    private void OnDisable()
    {
        SetCameraOrthographicSize();
    }

    public override void StartControl()
    {
        base.StartControl();
        SetLayer(1);
    }

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetLayer(int layer)
    {
        this.buildLayer = layer;
        ClearBuildItem();
        InitCameraRange(layer);
        InitBuildingExist();
    }

    /// <summary>
    /// 设置摄像头范围
    /// </summary>
    /// <param name="layer"></param>
    protected void InitCameraRange(int layer)
    {
        InnBuildBean innBuild = gameDataManager.gameData.GetInnBuildData();
        if (layer == 1)
        {
            //定义镜头的移动范围
            cameraMove.minMoveX = -0.1f;
            cameraMove.maxMoveX = innBuild.innWidth + 1;
            cameraMove.minMoveY = -0.1f;
            cameraMove.maxMoveY = innBuild.innHeight + 1;
            //定义镜头的初始位置
            SetFollowPosition(new Vector3(innBuild.innWidth / 2f, innBuild.innHeight / 2f, 0));
        }
        else if (layer == 2)
        {
            //定义镜头的移动范围
            cameraMove.minMoveX = -0.1f;
            cameraMove.maxMoveX = innBuild.innSecondWidth + 1;
            cameraMove.minMoveY = -0.1f + 100;
            cameraMove.maxMoveY = innBuild.innSecondHeight + 1 + 100;
            //定义镜头的初始位置
            SetFollowPosition(new Vector3(innBuild.innSecondWidth / 2f, 100 + innBuild.innSecondHeight / 2f, 0));
        }
    }

    /// <summary>
    /// 初始化已经存在的建筑位置
    /// </summary>
    public void InitBuildingExist()
    {
        listBuildingExist.Clear();
        List<InnResBean> listFurniture = gameDataManager.gameData.GetInnBuildData().GetFurnitureList(buildLayer);
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
        List<InnResBean> listWall = gameDataManager.gameData.GetInnBuildData().GetWallList(buildLayer);
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
    /// <param name="buildBedData"></param>
    public void ShowBuildItem(long id,BuildBedBean buildBedData)
    {
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        //先删除原有可能已经展示的建筑
        ClearBuildItem();
        //建造建筑
        GameObject buildItemObj = innFurnitureBuilder.BuildFurniture(id, buildBedData);
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
    /// 根据ID展示建筑
    /// </summary>
    /// <param name="id"></param>
    public void ShowBuildItem(long id)
    {
        ShowBuildItem(id, null);
    }

    /// <summary>
    /// 修建建筑占地提示
    /// </summary>
    protected void ShowBuildSpace()
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
    /// 处理-镜头移动
    /// </summary>
    protected void HandleForMouseMove()
    {
        if (cameraMove == null)
            return;
        base.HandleForMouseMove(out float moveX, out float moveY);
        cameraMove.Move(moveX, moveY);
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
            audioHandler.PlaySound(AudioSoundEnum.Set);
        }
        if (Input.GetButtonDown(InputInfo.Confirm) || Input.GetButton(InputInfo.Confirm))
        {
            //防止误触右边的UI
            if (UnityEngine.Screen.width - Input.mousePosition.x - 400 < 0)
                return;
            //能建造
            if (isCanBuild)
            {
                //audioHandler.PlaySound(AudioSoundEnum.Set);
                //镜头正对建造点
                //SetFollowPosition(buildPosition);
                //建筑物位置设置
                buildItemCpt.transform.position = buildPosition + new Vector3(-0.5f, 0.5f);
                //获取提示区域所占点
                List<Vector3> listBuildPosition = new List<Vector3>();
                for (int i = 0; i < listBuildSpaceSR.Count; i++)
                {
                    //精度修正
                    listBuildPosition.Add(Vector3Int.RoundToInt(listBuildSpaceSR[i].transform.position));
                }
                //如果是拆除
                if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Other && buildItemCpt.buildItemData.id == -1)
                {
                    BuildItemForDismantle(buildLayer, buildPosition, listBuildPosition[0]);
                }
                //如果是地板
                else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Floor)
                {
                    BuildItemForFloor(buildPosition);
                }
                //如果是墙壁
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
                    //只有一次点击时才出提示
                    if (Input.GetButtonDown(InputInfo.Confirm))
                    {
                        //toastManager.ToastHint(GameCommonInfo.GetUITextById(1003));
                    }
                }
                else
                {
                    //如果是正常模式提示
                    //只有一次点击时才出提示
                    if (Input.GetButtonDown(InputInfo.Confirm))
                    {
                        toastManager.ToastHint(GameCommonInfo.GetUITextById(1002));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 拆除
    /// </summary>
    /// <param name="buildPosition">家具的位置</param>
    /// <param name="sceneFurniturePosition">家具在场景中的位置</param>
    protected void BuildItemForDismantle(int dismantleLayer, Vector3 buildPosition, Vector3 sceneFurniturePosition)
    {
        //获取拆除位置的家具数据
        InnBuildBean buildData = gameDataManager.gameData.GetInnBuildData();
        InnResBean itemFurnitureData = buildData.GetFurnitureByPosition(dismantleLayer, buildPosition);
        //因为在保存tile时坐标减过1 所以查询是也要-1
        InnResBean itemWallData = buildData.GetWallByPosition(dismantleLayer, buildPosition - new Vector3(1, 0, 0));
        //如果拆除的是家具
        if (itemFurnitureData != null)
        {
            BuildItemBean buildItemData = innBuildManager.GetBuildDataById(itemFurnitureData.id);
            //如果是最后一扇门则不能删除
            if (buildItemData.build_type == (int)BuildItemTypeEnum.Door && buildData.GetDoorList(innBuildManager).Count <= 1)
            {
                //只有一次点击时才出提示
                if (Input.GetButtonDown(InputInfo.Confirm))
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1004));
                }
            }
            else
            {
                //移除数据
                buildData.GetFurnitureList(dismantleLayer).Remove(itemFurnitureData);
                //移除场景中的建筑物
                innFurnitureBuilder.DestroyFurnitureByPosition(sceneFurniturePosition);
                if (buildItemData.build_type == (int)BuildItemTypeEnum.Bed)
                {
                    //如果是床
                    BuildBedBean buildBedData= gameDataManager.gameData.GetBedByRemarkId(itemFurnitureData.remarkId);
                    buildBedData.isSet = false;
                }
                else if (buildItemData.build_type == (int)BuildItemTypeEnum.Stairs)
                {
                    //背包里添加一个
                    gameDataManager.gameData.AddBuildNumber(itemFurnitureData.id, 1);
                    InnResBean itemStarisData = null;
                    if (dismantleLayer == 1)
                    {
                        itemStarisData = buildData.GetFurnitureByPosition(2, buildPosition + new Vector3(0,100));
                        buildData.GetFurnitureList(2).Remove(itemStarisData);
                        //移除场景中的建筑物
                        innFurnitureBuilder.DestroyFurnitureByPosition(sceneFurniturePosition + new Vector3(0, 100));
                    }
                    else if (dismantleLayer == 2)
                    {
                        itemStarisData = buildData.GetFurnitureByPosition(1, buildPosition - new Vector3(0, 100));
                        buildData.GetFurnitureList(1).Remove(itemStarisData);
                        //移除场景中的建筑物
                        innFurnitureBuilder.DestroyFurnitureByPosition(sceneFurniturePosition - new Vector3(0, 100));
                    }
                }
                else
                {
                    //背包里添加一个
                    gameDataManager.gameData.AddBuildNumber(itemFurnitureData.id, 1);
                }
        
            }
        }
        //如果拆除的是墙壁
        if (itemWallData != null)
        {
            //判断是否是最外的墙壁
            Vector3 startPosition = itemWallData.GetStartPosition();
            gameDataManager.gameData.GetInnBuildData().GetInnSize(buildLayer, out int innWidth, out int innHeight, out int offsetHeight);        
            bool isOutWall = false;
            if (buildLayer==1)
            {
                isOutWall = (startPosition.x == 0 || startPosition.x == (innWidth - 1)) || startPosition.y == (innHeight - 1) + offsetHeight;
            }
            else if (buildLayer == 2)
            {
                isOutWall = (startPosition.x == 0 || startPosition.x == (innWidth - 1)) || startPosition.y == 0 + offsetHeight || startPosition.y == (innHeight - 1) + offsetHeight;
            }
            if (isOutWall)
            {
                //只有一次点击时才出提示
                if (Input.GetButtonDown(InputInfo.Confirm))
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1023));
                }
            }
            else
            {
                //移除数据
                buildData.GetWallList(buildLayer).Remove(itemWallData);
                //移除场景中的建筑物
                innWallBuilder.ClearWall(Vector3Int.RoundToInt(startPosition));
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
        //如果是门。需要重置一下墙体
        if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Door)
        {
            //gameDataManager.gameData.GetInnBuildData().InitWall(innBuildManager);
            //innWallBuilder.StartBuild();
        }
        else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Bed)
        {
            //如果是床。需要加上备注ID
            BuildBedCpt buildBedCpt = (BuildBedCpt)buildItemCpt;
            buildBedCpt.buildBedData.isSet = true;
            addData.remarkId = buildBedCpt.buildBedData.remarkId;
        }
        else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Stairs)
        {
            BuildStairsCpt buildStairsCpt = (BuildStairsCpt)buildItemCpt;
            //所有坐标下移
            List<Vector3> listFirstBuildPosition = new List<Vector3>();
            foreach (Vector3 itemPosition in listBuildPosition)
            {
                Vector3 firstPostion = itemPosition + new Vector3(0, -100, 0);
                listFirstBuildPosition.Add(firstPostion);
                //删除当前坐标下的建筑
                BuildItemForDismantle(buildLayer - 1, firstPostion, firstPostion);
            }

            //楼下也要添加同样的数据
            GameObject objFirstStairs = Instantiate(innFurnitureBuilder.buildContainer, buildItemCpt.gameObject);
            objFirstStairs.transform.position += new Vector3(0,-100,0);
            BuildStairsCpt firstStairs= objFirstStairs.GetComponent<BuildStairsCpt>();
            firstStairs.SetLayer(buildLayer - 1);
            InnResBean addFirstData = new InnResBean(firstStairs.buildItemData.id, objFirstStairs.transform.position, listFirstBuildPosition, firstStairs.direction);
            //设置相同的备注ID
            addData.remarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
            addData.remark = "2";
            addFirstData.remarkId = addData.remarkId;
            addFirstData.remark = "1";
            gameDataManager.gameData.GetInnBuildData().AddFurniture(buildLayer - 1, addFirstData);
            firstStairs.SetRemarkId(addData.remarkId);
            buildStairsCpt.SetRemarkId(addData.remarkId);
        }
        gameDataManager.gameData.GetInnBuildData().AddFurniture(buildLayer, addData);
        //背包里删除一个
        ItemBean itemData = gameDataManager.gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //动画
        buildItemCpt.transform
            .DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f)
            .From()
            .SetEase(Ease.OutBack);
           
        if ( buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Bed|| itemData.itemNumber <= 0 )
        {
            //如果没有了，则不能继续建造
            ClearSelectBuildItem();
        }
        else
        {
            //如果还有则实例化一个新的
            //物体先在建筑控件上显示   
            GameObject objCopy = Instantiate(buildItemTempContainer, buildItemCpt.gameObject);
            objCopy.transform.localScale = new Vector3(1, 1, 1);
            buildItemCpt = objCopy.GetComponent<BaseBuildItemCpt>();
        }
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
        InnResBean floorData = gameDataManager.gameData.GetInnBuildData().GetFloorByPosition(buildLayer, buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有地板则直接在建造点上建造
        if (floorData == null)
        {
            changePosition = Vector3Int.RoundToInt(buildPosition);
            floorData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameDataManager.gameData.GetInnBuildData().GetFloorList(buildLayer).Add(floorData);
        }
        //如果有地板则替换地板
        else
        {
            //背包里添加一个
            gameDataManager.gameData.AddBuildNumber(floorData.id, 1);
            changePosition = Vector3Int.RoundToInt(floorData.GetStartPosition());
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
        InnResBean wallData = gameDataManager.gameData.GetInnBuildData().GetWallByPosition(buildLayer, buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有墙壁则直接在建造点上建造
        if (wallData == null)
        {
            changePosition = Vector3Int.RoundToInt(buildPosition);
            wallData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameDataManager.gameData.GetInnBuildData().GetWallList(buildLayer).Add(wallData);
        }
        //如果有墙壁则替换墙壁
        else
        {
            //背包里添加一个
            gameDataManager.gameData.AddBuildNumber(wallData.id, 1);
            changePosition = Vector3Int.RoundToInt(wallData.GetStartPosition());
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
            srPosition = Vector3Int.RoundToInt(srPosition);
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
        gameDataManager.gameData.GetInnBuildData().GetInnSize(buildLayer, out int innWidth, out int innHeight, out int offsetHeight);

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
            if (position.x >= 1 && position.x <= innWidth && position.y >= 0 + offsetHeight && position.y <= innHeight - 1 + offsetHeight)
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
            if (position.x >= 1 && position.x <= innWidth && position.y >= 0 + offsetHeight && position.y <= innHeight - 1 + offsetHeight)
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
            if (position.x > 1 && position.x < innWidth && position.y > 0 + offsetHeight && position.y < innHeight - 1 + offsetHeight)
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