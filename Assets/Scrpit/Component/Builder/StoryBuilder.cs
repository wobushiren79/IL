﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class StoryBuilder : BaseMonoBehaviour
{
    //剧情
    public StoryInfoBean storyInfo;
    //剧情详情
    public List<StoryInfoDetailsBean> listStoryDetails;
    //剧情NPC列表
    public List<GameObject> listNpcObj;
    //剧情道具列表
    public List<GameObject> listPropObj;

    //剧情点
    private int mStoryOrder = 1;
    //迷你游戏数据
    private MiniGameCookingBean mGameCookingData;


    public virtual void Awake()
    {
        listStoryDetails = new List<StoryInfoDetailsBean>();
        listNpcObj = new List<GameObject>();
        listPropObj = new List<GameObject>();
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        StoryInfoHandler.Instance.manager.GetStoryDetailsById(storyInfo.id, SetStoryDetailsData);
    }

    /// <summary>
    /// 创建迷你烹饪游戏的故事
    /// </summary>
    /// <param name="storyInfo"></param>
    /// <param name="gameCookingData"></param>
    public void BuildStoryForMiniGameCooking(StoryInfoBean storyInfo, MiniGameCookingBean gameCookingData)
    {
        mGameCookingData = gameCookingData;
        BuildStory(storyInfo);
    }

    /// <summary>
    /// 根据顺序获取故事
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<StoryInfoDetailsBean> GetStoryDetailsByOrder(int order)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        if (listStoryDetails == null)
            return listData;

        foreach (StoryInfoDetailsBean itemData in listStoryDetails)
        {
            if (itemData.story_order == order)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建故事场景
    /// </summary>
    public void CreateStoryScene(List<StoryInfoDetailsBean> listData)
    {
        if (storyInfo == null)
            return;
        //设置剧情发生坐标
        gameObject.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        bool isNext = true;
        BaseControl baseControl = null;
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            switch (itemData.GetStoryInfoDetailsType())
            {
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                    //Npc站位
                    GameObject objNpc = GetNpcByNpcNum(itemData.num);
                    BaseNpcAI npcAI = null;
                    if (objNpc == null)
                    {
                        npcAI = CreateNpc(itemData);
                        objNpc = npcAI.gameObject;
                    }
                    else
                    {
                        npcAI = objNpc.GetComponent<BaseNpcAI>();
                        npcAI.characterMoveCpt.SetDestinationLocal(transform, new Vector3(itemData.position_x, itemData.position_y));
                    }
                    npcAI.SetCharacterFace(itemData.face);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                    //删除角色
                    int[] npcNum = StringUtil.SplitBySubstringForArrayInt(itemData.npc_destroy, ',');
                    float destroyTime = itemData.wait_time;
                    foreach (int itemNpcNum in npcNum)
                    {
                        objNpc = GetNpcByNpcNum(itemNpcNum);
                            //延迟删除
                        StartCoroutine(CoroutineForDelayDestoryNpc(destroyTime, objNpc));
                    }
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Expression:
                    //表情
                    SetCharacterExpression(itemData.num, itemData.expression);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                    try
                    {
                        //场景物体互动
                        GameObject objFind = GameObject.Find(itemData.scene_intobj_name);
                        //参数
                        List<string> listparameter = StringUtil.SplitBySubstringForListStr(itemData.scene_intcomponent_parameters, ',');
                        //通过反射调取方法
                        ReflexUtil.GetInvokeMethod(objFind, itemData.scene_intcomponent_name, itemData.scene_intcomponent_method, listparameter);
                    }
                    catch
                    {

                    }

                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                    //进入对话
                    isNext = false;
                    UIGameText uiComponent = UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameText>(UIEnum.GameText);
                    uiComponent.SetData(TextEnum.Story, itemData.text_mark_id);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                    //剧情自动跳转
                    isNext = false;
                    StartCoroutine(StoryAutoNext(itemData.wait_time));
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
                    //道具位置
                    GameObject objProp =  GetPropByNpcNum(itemData.num);
                    if (objProp == null)
                    {
                        CreateProp(itemData);
                    }
                    else
                    {
                        CharacterMoveCpt characterMove = objProp.GetComponent<CharacterMoveCpt>();
                        //如果可以移动
                        if (characterMove)
                        {
                            characterMove.SetDestinationLocal(transform, new Vector3(itemData.position_x, itemData.position_y));
                        }
                        //如果不能移动则直接设置坐标
                        else
                        {
                            objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
                        }
                    }      
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                    //设置摄像头位置
                    baseControl = GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
                    Vector3 cameraWorldPosition = transform.TransformPoint(new Vector3(itemData.position_x, itemData.position_y));
                    baseControl.SetCameraFollowObj(null);
                    baseControl.SetFollowPosition(cameraWorldPosition);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                    //设置摄像头位置
                    objNpc = GetNpcByNpcNum(itemData.camera_follow_character);
                    baseControl = GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
                    baseControl.SetCameraFollowObj(objNpc);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                    //播放音效
                    AudioHandler.Instance.PlaySound(itemData.GetAudioSound());
                    break;
            }
        }
        if (isNext)
            NextStoryOrder();
    }

    /// <summary>
    /// 下一个剧情点
    /// </summary>
    public void NextStoryOrder()
    {
        mStoryOrder++;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(mStoryOrder);
        if (CheckUtil.ListIsNull(listOrderData))
        {
            //清理故事场景
            ClearStoryScene();
            //没有剧情。完结
            GameEventHandler.Instance.SetEventStatus(GameEventHandler.EventStatusEnum.EventEnd);
        }
        else
            CreateStoryScene(listOrderData);
    }

    public IEnumerator StoryAutoNext(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextStoryOrder();
    }

    /// <summary>
    /// 设置人物表情
    /// </summary>
    /// <param name="npcNum"></param>
    /// <param name="expression"></param>
    public void SetCharacterExpression(int npcNum, int expression)
    {
        GameObject objItem = GetNpcByNpcNum(npcNum);
        if (objItem != null)
        {
            NpcAIStoryCpt npcAIStory = objItem.GetComponent<NpcAIStoryCpt>();
            if (npcAIStory != null)
                npcAIStory.SetExpression(expression);
        }
    }

    /// <summary>
    /// 通过Npc编号获取NPC
    /// </summary>
    /// <param name="npcNum"></param>
    /// <returns></returns>
    public GameObject GetNpcByNpcNum(int npcNum)
    {
        if (listNpcObj != null)
        {
            foreach (GameObject itemNpc in listNpcObj)
            {
                if (itemNpc.name.Equals("character_"+ npcNum))
                {
                    return itemNpc;
                }
            }
        }
        return null;
    }

    public GameObject GetPropByNpcNum(int propNum)
    {
        if (listPropObj != null)
        {
            foreach (GameObject itemProp in listPropObj)
            {
                if (itemProp.name.Equals("prop_" + propNum))
                {
                    return itemProp;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 生成NPC
    /// </summary>
    /// <param name="itemData"></param>
    public BaseNpcAI CreateNpc(StoryInfoDetailsBean itemData)
    {
        GameObject objNpcModel = StoryInfoHandler.Instance.manager.objNpcModel;
        GameObject objNpc = Instantiate(transform.gameObject, objNpcModel);
        objNpc.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
        listNpcObj.Add(objNpc);
        NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
        CharacterBean characterData;
        if (itemData.npc_id == 0)
        {
            GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story).SetCameraFollowObj(objNpc);
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            characterData = gameData.userCharacter;
        }
        else
            characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemData.npc_id);
        //设置编号
        objNpc.name = "character_" + itemData.num;
        aiNpc.SetCharacterData(characterData);
        //默认设置NPC速度为1
        aiNpc.characterMoveCpt.SetMoveSpeed(1);
        return aiNpc;
    }

    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateProp(StoryInfoDetailsBean itemData)
    {
        GameObject objPropModel = StoryInfoHandler.Instance.manager.GetStoryPropModelByName(itemData.key_name);
        GameObject objProp = Instantiate(transform.gameObject, objPropModel);

        listPropObj.Add(objProp);
        objProp.name = "prop_" + itemData.num;

        //设置位置和朝向
        objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
        Vector3 bodyScale = objProp.transform.localScale;
        switch (itemData.face)
        {
            case 1:
                bodyScale.x = -1;
                break;

            case 2:
                bodyScale.x = 1;
                break;
        }
        objProp.transform.localScale = bodyScale;

        //如果有移动控件
        CharacterMoveCpt characterMove = objProp.GetComponent<CharacterMoveCpt>();
        if (characterMove != null)
        {
            characterMove.SetMoveSpeed(1);
        }
    }

    /// <summary>
    /// 清理剧情场景
    /// </summary>
    public void ClearStoryScene()
    {
        mGameCookingData = null;
        CptUtil.RemoveChildsByActive(transform);
        gameObject.transform.position = Vector3.zero;
        listNpcObj.Clear();
        listPropObj.Clear();
        mStoryOrder = 1;
    }

    /// <summary>
    /// 合并烹饪游戏数据
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="gameCookingData"></param>
    private void MergeDataForGameCooking(List<StoryInfoDetailsBean> listData, MiniGameCookingBean gameCookingData)
    {

    }

    #region  获取故事详情回调
    public void SetStoryDetailsData(List<StoryInfoDetailsBean> listData)
    {
        ClearStoryScene();
        listStoryDetails = listData;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(mStoryOrder);
        //如果是迷你烹饪游戏的剧情 则需要合并一下数据
        if (mGameCookingData != null)
        {
            MergeDataForGameCooking(listOrderData, mGameCookingData);
        }
        CreateStoryScene(listOrderData);

    }
    #endregion

    /// <summary>
    /// 协程 延迟删除NPC
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="objNpc"></param>
    /// <returns></returns>
    protected IEnumerator CoroutineForDelayDestoryNpc(float waitTime, GameObject objNpc)
    {
        yield return new WaitForSeconds(waitTime);
        if (objNpc != null)
        {
            listNpcObj.Remove(objNpc);
            Destroy(objNpc);      
        }
 
    }
}