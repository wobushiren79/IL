using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

public class ControlForBuildCpt : BaseControlForBuild
{

    public override void InitCameraRange(int layer)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnBuildBean innBuild = gameData.GetInnBuildData();
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

    public override void HandleForBuildConfirm(bool isCanBuild, Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        if (Input.GetButtonDown(InputInfo.Confirm))
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Set);
        }
        if (Input.GetButtonDown(InputInfo.Confirm) || Input.GetButton(InputInfo.Confirm))
        {
            //防止误触右边的UI
            if (UnityEngine.Screen.width - Input.mousePosition.x - 400 < 0)
                return;
            //能建造
            if (isCanBuild)
            {
                //AudioHandler.Instance.PlaySound(AudioSoundEnum.Set);
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
                UIGameBuild uiGameBuild = UIHandler.Instance.GetUI<UIGameBuild>();
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
                        //UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1003));
                    }
                }
                else
                {
                    //如果是正常模式提示
                    //只有一次点击时才出提示
                    if (Input.GetButtonDown(InputInfo.Confirm))
                    {
                        UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1002));
                    }
                }
            }
        }
    }

    protected override void BuildItemForDismantle(int dismantleLayer, Vector3 buildPosition, Vector3 sceneFurniturePosition)
    {
        base.BuildItemForDismantle(dismantleLayer, buildPosition, sceneFurniturePosition);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //获取拆除位置的家具数据
        InnBuildBean buildData = gameData.GetInnBuildData();
        InnResBean itemFurnitureData = buildData.GetFurnitureByPosition(dismantleLayer, buildPosition);
        //因为在保存tile时坐标减过1 所以查询是也要-1
        InnResBean itemWallData = buildData.GetWallByPosition(dismantleLayer, buildPosition - new Vector3(1, 0, 0));
        //如果拆除的是家具
        if (itemFurnitureData != null)
        {
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemFurnitureData.id);
            //如果是最后一扇门则不能删除
            if (buildItemData.build_type == (int)BuildItemTypeEnum.Door && buildData.GetDoorList().Count <= 1)
            {
                //只有一次点击时才出提示
                if (Input.GetButtonDown(InputInfo.Confirm))
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1004));
                }
            }
            else
            {
                //移除数据
                buildData.GetFurnitureList(dismantleLayer).Remove(itemFurnitureData);
                //移除场景中的建筑物
                InnBuildHandler.Instance.builderForFurniture.DestroyFurnitureByPosition(sceneFurniturePosition);
                if (buildItemData.build_type == (int)BuildItemTypeEnum.Bed)
                {
                    //如果是床
                    BuildBedBean buildBedData = gameData.GetBedByRemarkId(itemFurnitureData.remarkId);
                    buildBedData.isSet = false;
                }
                else if (buildItemData.build_type == (int)BuildItemTypeEnum.Stairs)
                {
                    //背包里添加一个
                    gameData.AddBuildNumber(itemFurnitureData.id, 1);
                    InnResBean itemStarisData = null;
                    if (dismantleLayer == 1)
                    {
                        itemStarisData = buildData.GetFurnitureByPosition(2, buildPosition + new Vector3(0, 100));
                        buildData.GetFurnitureList(2).Remove(itemStarisData);
                        //移除场景中的建筑物
                        InnBuildHandler.Instance.builderForFurniture.DestroyFurnitureByPosition(sceneFurniturePosition + new Vector3(0, 100));
                    }
                    else if (dismantleLayer == 2)
                    {
                        itemStarisData = buildData.GetFurnitureByPosition(1, buildPosition - new Vector3(0, 100));
                        buildData.GetFurnitureList(1).Remove(itemStarisData);
                        //移除场景中的建筑物
                        InnBuildHandler.Instance.builderForFurniture.DestroyFurnitureByPosition(sceneFurniturePosition - new Vector3(0, 100));
                    }
                }
                else
                {
                    //背包里添加一个
                    gameData.AddBuildNumber(itemFurnitureData.id, 1);
                }

            }
        }
        //如果拆除的是墙壁
        if (itemWallData != null)
        {
            //判断是否是最外的墙壁
            Vector3 startPosition = itemWallData.GetStartPosition();
            gameData.GetInnBuildData().GetInnSize(buildLayer, out int innWidth, out int innHeight, out int offsetHeight);
            bool isOutWall = false;
            if (buildLayer == 1)
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
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1023));
                }
            }
            else
            {
                //移除数据
                buildData.GetWallList(buildLayer).Remove(itemWallData);
                //移除场景中的建筑物
                InnBuildHandler.Instance.builderForWall.ClearWall(Vector3Int.RoundToInt(startPosition));
                //背包里添加一个
                gameData.AddBuildNumber(itemWallData.id, 1);
            }
        }
        //更新一下数据
        InitBuildingExist();
    }

    /// <summary>
    /// 建造墙壁
    /// </summary>
    /// <param name="buildPosition"></param>
    protected void BuildItemForWall(Vector3 buildPosition)
    {
        if (buildItemCpt == null)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        //获取该点地板数据
        InnResBean wallData = gameData.GetInnBuildData().GetWallByPosition(buildLayer, buildPosition);
        Vector3Int changePosition = Vector3Int.zero;
        //如果没有墙壁则直接在建造点上建造
        if (wallData == null)
        {
            changePosition = Vector3Int.RoundToInt(buildPosition);
            wallData = new InnResBean(buildItemCpt.buildItemData.id, changePosition, null, Direction2DEnum.Left);
            gameData.GetInnBuildData().GetWallList(buildLayer).Add(wallData);
        }
        //如果有墙壁则替换墙壁
        else
        {
            //背包里添加一个
            gameData.AddBuildNumber(wallData.id, 1);
            changePosition = Vector3Int.RoundToInt(wallData.GetStartPosition());
            wallData.id = buildItemCpt.buildItemData.id;
        }
        InnBuildHandler.Instance.builderForWall.ChangeWall(changePosition, buildItemCpt.buildItemData.tile_name);
        //背包里删除一个
        ItemBean itemData = gameData.AddBuildNumber(buildItemCpt.buildItemData.id, -1);
        //如果没有了，则不能继续建造
        if (itemData.itemNumber <= 0)
        {
            ClearBuildItem();
        }
    }

    public override void InitBuildingExist()
    {
        base.InitBuildingExist();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listBuildingExistForWall.Clear();
        List<InnResBean> listWall = gameData.GetInnBuildData().GetWallList(buildLayer);
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
    /// 检测指定点是否有建筑
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override bool CheckHasBuild(Vector3 position)
    {
        base.CheckHasBuild(position);
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
    public override bool CheckOutOfRange(Vector3 position)
    {
        base.CheckOutOfRange(position);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        BuildItemTypeEnum buildType = (BuildItemTypeEnum)buildItemCpt.buildItemData.build_type;
        gameData.GetInnBuildData().GetInnSize(buildLayer, out int innWidth, out int innHeight, out int offsetHeight);

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