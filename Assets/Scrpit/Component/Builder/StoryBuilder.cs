﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class StoryBuilder : BaseMonoBehaviour, StoryInfoManager.CallBack
{
    [Header("控件")]
    public GameObject objNpcModel;

    [Header("数据")]
    protected GameDataManager gameDataManager;
    protected StoryInfoManager storyInfoManager;
    protected NpcInfoManager npcInfoManager;
    protected BaseUIManager uiManager;

    protected EventHandler eventHandler;
    protected ControlHandler controlHandler;
    protected AudioHandler audioHandler;

    //剧情
    public StoryInfoBean storyInfo;
    //剧情详情
    public List<StoryInfoDetailsBean> listStoryDetails;
    //剧情NPC列表
    public List<GameObject> listNpcObj;

    //剧情点
    private int mStoryOrder = 1;
    //迷你游戏数据
    private MiniGameCookingBean mGameCookingData;

    public virtual void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        storyInfoManager = Find<StoryInfoManager>(ImportantTypeEnum.StoryManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        uiManager = Find<BaseUIManager>(ImportantTypeEnum.GameUI);

        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);

        listStoryDetails = new List<StoryInfoDetailsBean>();
        listNpcObj = new List<GameObject>();
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        storyInfoManager.GetStoryDetailsById(storyInfo.id, this);
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
            switch ((StoryInfoDetailsBean.StoryInfoDetailsTypeEnum)itemData.type)
            {
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                    //Npc站位
                    GameObject objNpc = GetNpcByNpcNum(itemData.npc_num);
                    BaseNpcAI npcAI = null;
                    if (objNpc == null)
                    {
                        npcAI = CreateNpc(itemData);
                        objNpc = npcAI.gameObject;
                    }
                    else
                    {
                        npcAI = objNpc.GetComponent<BaseNpcAI>();
                        npcAI.characterMoveCpt.SetDestinationLocal(transform, new Vector3(itemData.npc_position_x, itemData.npc_position_y));
                    }
                    npcAI.SetCharacterFace(itemData.npc_face);
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
                    SetCharacterExpression(itemData.npc_num, itemData.expression);
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
                    UIGameText uiComponent = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
                    uiComponent.SetData(TextEnum.Story, itemData.text_mark_id);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                    //剧情自动跳转
                    isNext = false;
                    StartCoroutine(StoryAutoNext(itemData.wait_time));
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                    //设置摄像头位置
                    baseControl = controlHandler.GetControl();
                    Vector3 cameraWorldPosition = transform.TransformPoint(new Vector3(itemData.camera_position_x, itemData.camera_position_y));
                    baseControl.SetCameraFollowObj(null);
                    baseControl.SetFollowPosition(cameraWorldPosition);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                    //设置摄像头位置
                    objNpc = GetNpcByNpcNum(itemData.camera_follow_character);
                    baseControl = controlHandler.GetControl();
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
            eventHandler.SetEventStatus(EventHandler.EventStatusEnum.EventEnd);
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
                if (itemNpc.name.Equals(npcNum + ""))
                {
                    return itemNpc;
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
        GameObject objNpc = Instantiate(transform.gameObject, objNpcModel);
        objNpc.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
        listNpcObj.Add(objNpc);
        NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
        CharacterBean characterData;
        if (itemData.npc_id == 0)
        {
            ((ControlForStoryCpt)controlHandler.GetControl()).SetCameraFollowObj(objNpc);
            characterData = gameDataManager.gameData.userCharacter;
        }
        else
            characterData = npcInfoManager.GetCharacterDataById(itemData.npc_id);
        //设置编号
        objNpc.name = itemData.npc_num + "";
        aiNpc.SetCharacterData(characterData);
        //默认设置NPC速度为1
        aiNpc.characterMoveCpt.SetMoveSpeed(1);
        return aiNpc;
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
    public void GetStoryDetailsSuccess(List<StoryInfoDetailsBean> listData)
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