using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

public class BaseControlForBuild : BaseControl
{
    //镜头对象
    public CharacterMoveCpt cameraMove;

    //地形模型
    protected GameObject _listBuildSpaceContent;

    public GameObject listBuildSpaceContent
    {
        get
        {
            if (_listBuildSpaceContent == null)
            {
                _listBuildSpaceContent = new GameObject();
                _listBuildSpaceContent.name = "BuildSpace";
                _listBuildSpaceContent.transform.SetParent(GameControlHandler.Instance.transform);
                _listBuildSpaceContent.transform.position = Vector3.zero;
            }
            return _listBuildSpaceContent;
        }
    }

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

    //建筑层数
    public int buildLayer = 1;


    public void FixedUpdate()
    {
        HandleForZoom();
        HandleForMouseMove();
        //检测是否控制镜头移动
        HandleForCameraMove();
    }

    public void Update()
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

    public void OnDisable()
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
    public virtual void InitCameraRange(int layer)
    {

    }

    /// <summary>
    /// 初始化已经存在的建筑位置
    /// </summary>
    public virtual void InitBuildingExist()
    {
        listBuildingExist.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<InnResBean> listFurniture = gameData.GetInnBuildData().GetFurnitureList(buildLayer);
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
    public void ShowBuildItem(long id, ItemBean itemData = null)
    {
        ShowBuildItem(id, null, itemData);
    }

    /// <summary>
    /// 根据ID展示建筑
    /// </summary>
    /// <param name="id"></param>
    /// <param name="buildBedData"></param>
    public void ShowBuildItem(long id, BuildBedBean buildBedData,ItemBean itemData = null)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //先删除原有可能已经展示的建筑
        ClearBuildItem();
        //建造建筑
        GameObject buildItemObj = InnBuildHandler.Instance.builderForFurniture.BuildFurniture(id, buildBedData, itemData);
        //物体先在建筑控件上显示
        buildItemObj.transform.SetParent(GameControlHandler.Instance.gameObject.transform);
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
        base.HandleForMouseButtonMove(out float moveButtonX, out float moveButtonY);
        cameraMove.Move(moveX + moveButtonX, moveY + moveButtonY);
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
        RectTransform screenRTF = (RectTransform)UIHandler.Instance.manager.GetUITypeContainer(UITypeEnum.UIBase);
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
    public  virtual void HandleForBuildConfirm(bool isCanBuild, Vector3 buildPosition)
    {
       
    }

    /// <summary>
    /// 拆除
    /// </summary>
    /// <param name="buildPosition">家具的位置</param>
    /// <param name="sceneFurniturePosition">家具在场景中的位置</param>
    protected virtual void BuildItemForDismantle(int dismantleLayer, Vector3 buildPosition, Vector3 sceneFurniturePosition)
    {

    }

    /// <summary>
    /// 建造家具
    /// </summary>
    /// <param name="listBuildPosition"></param>
    protected void BuildItemForFurniture(List<Vector3> listBuildPosition)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //将家具添加到家具容器中
        buildItemCpt.transform.parent = InnBuildHandler.Instance.builderForFurniture.buildContainer.transform;
        //增加一个家具
        InnResBean addData = new InnResBean(buildItemCpt.buildItemData.id, buildItemCpt.transform.position, listBuildPosition, buildItemCpt.direction);
        //如果是门。需要重置一下墙体
        if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Door)
        {
            //gameData.GetInnBuildData().InitWall(innBuildManager);
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
            GameObject objFirstStairs = Instantiate(InnBuildHandler.Instance.builderForFurniture.buildContainer, buildItemCpt.gameObject);
            objFirstStairs.transform.position += new Vector3(0, -100, 0);
            BuildStairsCpt firstStairs = objFirstStairs.GetComponent<BuildStairsCpt>();
            firstStairs.SetLayer(buildLayer - 1);
            InnResBean addFirstData = new InnResBean(firstStairs.buildItemData.id, objFirstStairs.transform.position, listFirstBuildPosition, firstStairs.direction);
            //设置相同的备注ID
            addData.remarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
            addData.remark = "2";
            addFirstData.remarkId = addData.remarkId;
            addFirstData.remark = "1";
            gameData.GetInnBuildData().AddFurniture(buildLayer - 1, addFirstData);
            firstStairs.SetRemarkId(addData.remarkId);
            buildStairsCpt.SetRemarkId(addData.remarkId);
        }
        gameData.GetInnBuildData().AddFurniture(buildLayer, addData);
        //背包里删除一个
        ItemBean itemData = gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //动画
        buildItemCpt.transform
            .DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f)
            .From()
            .SetEase(Ease.OutBack);

        if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Bed || itemData.itemNumber <= 0)
        {
            //如果没有了，则不能继续建造
            ClearSelectBuildItem();
        }
        else
        {
            //如果还有则实例化一个新的
            //物体先在建筑控件上显示   
            GameObject objCopy = Instantiate(GameControlHandler.Instance.gameObject, buildItemCpt.gameObject);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        //获取该点地板数据
        InnResBean floorData = gameData.GetInnBuildData().GetFloorByPosition(buildLayer, buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有地板则直接在建造点上建造
        if (floorData == null)
        {
            changePosition = Vector3Int.RoundToInt(buildPosition);
            floorData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameData.GetInnBuildData().GetFloorList(buildLayer).Add(floorData);
        }
        //如果有地板则替换地板
        else
        {
            //背包里添加一个
            gameData.AddBuildNumber(floorData.id, 1);
            changePosition = Vector3Int.RoundToInt(floorData.GetStartPosition());
            floorData.id = buildItemCpt.buildItemData.id;
        }
        InnBuildHandler.Instance.builderForFloor.ChangeFloor(changePosition, buildItemCpt.buildItemData.tile_name);
        //背包里删除一个
        ItemBean itemData = gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //如果没有了，则不能继续建造
        if (itemData.itemNumber <= 0)
        {
            ClearBuildItem();
        }
    }
    
    protected void BuildItemForSeed(Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        //获取该点 种子数据
        InnResBean seedData = gameData.GetInnCourtyardData().GetSeedData(buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //设置种子数据
        InnCourtyardSeedBean innCourtyardSeedData = new InnCourtyardSeedBean();
        innCourtyardSeedData.growDay = 0;
        //如果没有种子则直接在建造点上建造
        if (seedData == null)
        {
            changePosition = Vector3Int.RoundToInt(buildPosition);
            seedData = new InnResBean(buildItemCpt.itemData.itemId, changePosition, null, Direction2DEnum.Left);
            seedData.remark = JsonUtil.ToJson(innCourtyardSeedData);

            gameData.GetInnCourtyardData().listSeedData.Add(seedData);
        }
        //如果有植物 则无法种植
        else
        {
            return; 
        }
        InnBuildHandler.Instance.builderForCourtyard.ChangeSeed(changePosition, seedData);
        //背包里删除一个
        ItemBean itemData = gameData.AddItemsNumber(buildItemCpt.itemData.itemId, -1);
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
    public virtual bool CheckHasBuild(Vector3 position)
    {
        return true;
    }

    /// <summary>
    /// 检测是否超出建筑范围
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual bool CheckOutOfRange(Vector3 position)
    {
        return true;
    }
}