using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControlForBuildCourtyardCpt : BaseControlForBuild
{

    public override void InitCameraRange(int layer)
    {
        base.InitCameraRange(layer);

        //定义镜头的移动范围
        cameraMove.minMoveX = -8;
        cameraMove.maxMoveX =  8;
        cameraMove.minMoveY = -25;
        cameraMove.maxMoveY = -1;
        //定义镜头的初始位置
        SetFollowPosition(new Vector3(0, -3));
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
                else if (buildItemCpt.buildItemData.build_type == (int)BuildItemTypeEnum.Seed)
                {
                    BuildItemForSeed(buildPosition);
                }
                //刷新一下建筑占地
                InitBuildingExist();
                //刷新UI
                //里面有移除选中功能
                UIGameBuildCourtyard uiGameBuild = UIHandler.Instance.GetUI<UIGameBuildCourtyard>();
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
        //tile坐标需要从左下角计算 所以需要x-1
        buildPosition -= new Vector3(1, 0, 0);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //获取该点 种子数据
        InnResBean seedData = gameData.GetInnCourtyardData().GetSeedData(buildPosition);
        Vector3Int changePosition = Vector3Int.RoundToInt(buildPosition);

        if (seedData != null)
        {
            gameData.GetInnCourtyardData().listSeedData.Remove(seedData);
        }
        else
        {
            return;
        }
        InnBuildHandler.Instance.builderForCourtyard.ChangeSeed(changePosition, null);

        //更新一下数据
        InitBuildingExist();
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
        //种子
        if (buildType == BuildItemTypeEnum.Seed)
        {
            return false;
        }
        //拆除
        else
        {
            return true;
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

        InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
        int level = innCourtyardData.courtyardLevel + 1;

        int sizeX = level;
        int sizeY = level;

        if (buildType == BuildItemTypeEnum.Seed)
        {
            if (position.x > -sizeX && position.x <= sizeX + 1 && position.y >=(-sizeY * 2 - 3) && position.y <= -3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}